using UnityEngine;
using UnityEngine.Serialization;

namespace ViewR.Core.Calibration.AlignWorlds
{
	/// <summary>
	/// Kabsch algorithm from Matthew McGinity.
	/// Slightly modified.
	/// </summary>
	public class IxKabsch: MonoBehaviour
	{
		public Transform objectToAlign = null;
		public bool alignObject = false;
		public bool translateObject = false;
		public bool rotateObject = false;
		public bool scaleObject = false;
		public bool alignSourceTransforms = true;
		public int numIterations = 20;  // num of Kabsch iterations per frame
		public bool stopWhenConverged = false;
//	public bool invertTransform = false;
		/// <summary>
		/// Only for Getter/Setter 
		/// </summary>
		private bool _kabschEnabled = false;
		public bool KabschEnabled
		{
			get => _kabschEnabled;
			set
			{
				// Reset counter if set to true.
				if(value)
					_counterIfNoConvergence = 0;
				_kabschEnabled = value;
			}
		}
		[FormerlySerializedAs("debug")]
		[Header("Debugging")]
		public bool debugKabsch = false;
		public bool debuggingFeedbackAndUI = false;

		// public static UnityEvent KabschConvergedOld;
		public delegate void KabschChanged();
		public static KabschChanged KabschConverged;

		// List of Transforms to be aligned with reference Transforms
		public Transform[] sourceTransforms;
		// List of Transforms 
		public Transform[] targetTransforms;

		// Copy of positions of real and virtual positions...
		private Vector3[] _sourcePoints;
		private Vector4[] _targetPoints;

		private KabschSolver _solver = new KabschSolver();
		private GameObject _centroid;

		private Vector3 _origPosition;
		private Quaternion _origRotation;
		private Vector3 _origScale;
		
		// Catch in case of horrible alignment
		private int _counterIfNoConvergence = 0;
		private const int CounterThresholdNonConverging = 20;

		public Vector3 worldPosition;

		public void SaveOriginalPosition()
		{
			if (objectToAlign)
			{
				_origPosition = objectToAlign.transform.position;
				_origRotation = objectToAlign.transform.rotation;
				_origScale = objectToAlign.transform.lossyScale;
			}
			else
			{
				//Debug.LogWarning("There is no \"objectToAlign\" defined. This may be on purpose. If this was called when loading the scene, you probably don't have to worry about it.", this);
			}
		}

		//Set up the Input Points
		private void Start ()
		{
			_sourcePoints = new Vector3[sourceTransforms.Length];
			_targetPoints = new Vector4[targetTransforms.Length];

			// An object representing the centroid of the ...
			_centroid = new GameObject();
			_centroid.name = name + ".centroid";

			SaveOriginalPosition();
		}

		//Calculate the Kabsch Transform and Apply it to the input points
		private void Update()
		{
			if (!KabschEnabled)
				return;
			
			if (_sourcePoints.Length != sourceTransforms.Length)
				_sourcePoints = new Vector3[sourceTransforms.Length];

			if (_targetPoints.Length != targetTransforms.Length)
				_targetPoints = new Vector4[targetTransforms.Length];

			var minCount = Mathf.Min(sourceTransforms.Length, targetTransforms.Length);

			// Copy world-positions from transforms into array of vectors. Scale not currently supported correctly
			for (var i = 0; i < minCount; i++)
			{
				_sourcePoints[i] = sourceTransforms[i].position;
				var p = targetTransforms[i].position;
				_targetPoints[i].Set(p.x, p.y, p.z, 1); // targetTransforms[i].localScale.x);

				if (debugKabsch)
					Debug.DrawLine(_sourcePoints[i], _targetPoints[i], Color.red);
			}

			var bConverged = _solver.SolveKabsch(_sourcePoints, _targetPoints, translateObject, rotateObject, scaleObject, numIterations); // Calculate optimal transform from sourcePoints -> targetPoints

			// Disable if desired...
			if (stopWhenConverged && bConverged)
			{
				KabschConverged?.Invoke();
				KabschEnabled = false;
			}

			// Quick fix to handle non-converging cases:
			_counterIfNoConvergence++;
			if (_counterIfNoConvergence >= CounterThresholdNonConverging && KabschEnabled)
			{
				KabschConverged?.Invoke();
				KabschEnabled = false;
			}

			if (alignSourceTransforms) // transform the source reference points
			{
				for (var i = 0; i < minCount; i++)
				{
					var p = _solver.matrix.MultiplyPoint(_sourcePoints[i]); // transform 

					if (debugKabsch)
						Debug.DrawLine(_sourcePoints[i], p, Color.green);

					sourceTransforms[i].position = _solver.matrix.MultiplyPoint(sourceTransforms[i].position); // transform 
				}
			}

			//		objectToAlign.SetPositionAndRotation(origPosition, origRotation);

			if (debugKabsch)
			{
				Debug.DrawLine(_solver.targetCentroid, _solver.inCentroid, Color.yellow);
				Debug.DrawLine(_solver.targetCentroid, Vector3.zero, Color.yellow);
			}

			if (alignObject) // Transform the object to be aligned...
			{
				if (!alignSourceTransforms)
					objectToAlign.transform.SetPositionAndRotation(_origPosition, _origRotation);

				var translate = _solver.targetCentroid - _solver.inCentroid;
				_centroid.transform.position = _solver.targetCentroid;
				_centroid.transform.rotation = Quaternion.identity;
				_centroid.transform.localScale.Set(1, 1, 1);
				var oldParent = objectToAlign.parent;

				objectToAlign.parent = _centroid.transform;

				if (rotateObject)
					_centroid.transform.rotation = _solver.OptimalRotation;

				objectToAlign.localPosition += translate;

				objectToAlign.parent = oldParent;

				if (scaleObject)
					objectToAlign.localScale *= _solver.scaleRatio;

				if (alignSourceTransforms)
					SaveOriginalPosition();
				
				//Debug.Log($"Scale: {_solver.scaleRatio}".Blue().StartWithFrom(GetType()), this);
			}

//		if (invertTransform)
			{
				// TO BE COMPLETED....
				//			objectToAlign.position = solver.matrix.inverse.MultiplyPoint3x4(objectToAlign.position);
				//			objectToAlign.rotation = objectToAlign.rotation * Quaternion.Inverse(solver.matrix.rotation);
			}
		}
	}


//Kabsch Implementation-----------------------------------------------------------
	public class KabschSolver
	{
		private Vector3[] _quatBasis = new Vector3[3];
		private Vector3[] _dataCovariance = new Vector3[3];
		public Quaternion OptimalRotation = Quaternion.identity;
		public float scaleRatio = 1f;
		public Vector3 targetCentroid = Vector3.zero;
		public Vector3 inCentroid = Vector3.zero;
		public Matrix4x4 matrix;
	
		// Returns true if converged, other returns false;
		public bool SolveKabsch(Vector3[] inPoints, Vector4[] targetPoints, bool solveTranslation = true, bool solveRotation = true, bool solveScale = false, int numIterations = 9)
		{
			if (inPoints.Length != targetPoints.Length)
			{
				matrix = Matrix4x4.identity;
				return true;
			}

			//Calculate the centroid offset and construct the centroid-shifted point matrices
			inCentroid = Vector3.zero;
			targetCentroid = Vector3.zero;
			float inTotal = 0f, targetTotal = 0f;
			for (var i = 0; i < inPoints.Length; i++)
			{
				inCentroid += new Vector3(inPoints[i].x, inPoints[i].y, inPoints[i].z) * targetPoints[i].w;
				inTotal += targetPoints[i].w;
				targetCentroid += new Vector3(targetPoints[i].x, targetPoints[i].y, targetPoints[i].z) * targetPoints[i].w;
				targetTotal += targetPoints[i].w;
			}
			inCentroid /= inTotal;
			targetCentroid /= targetTotal;

			//Calculate the scale ratio
			if (solveScale)
			{
				float inScale = 0f, targetScale = 0f;
				for (var i = 0; i < inPoints.Length; i++)
				{
					inScale += (new Vector3(inPoints[i].x, inPoints[i].y, inPoints[i].z) - inCentroid).magnitude;
					targetScale += (new Vector3(targetPoints[i].x, targetPoints[i].y, targetPoints[i].z) - targetCentroid).magnitude;
				}
				scaleRatio = (targetScale / inScale);
			}

			if (!solveTranslation)
				targetCentroid = inCentroid;

			//Calculate the 3x3 covariance matrix, and the optimal rotation
			if (solveRotation)
			{
//			Profiler.BeginSample ("Solve Optimal Rotation");
				ExtractRotation(TransposeMultSubtract(inPoints, targetPoints, inCentroid, targetCentroid, _dataCovariance), ref OptimalRotation, numIterations);
//			Profiler.EndSample ();
			}


			matrix =  Matrix4x4.TRS(targetCentroid, Quaternion.identity, Vector3.one * scaleRatio) *
			          Matrix4x4.TRS(Vector3.zero, OptimalRotation, Vector3.one) *
			          Matrix4x4.TRS(-inCentroid, Quaternion.identity, Vector3.one);

			return (matrix.isIdentity);
		}

		//https://animation.rwth-aachen.de/media/papers/2016-MIG-StableRotation.pdf
		//Iteratively apply torque to the basis using Cross products (in place of SVD)
		private void ExtractRotation (Vector3[] a, ref Quaternion q, int numIterations)
		{
			for (var iter = 0; iter < numIterations; iter++) {
//			Profiler.BeginSample ("Iterate Quaternion");
				q.FillMatrixFromQuaternion(ref _quatBasis);
				var omega = (Vector3.Cross (_quatBasis [0], a [0]) +
				             Vector3.Cross (_quatBasis [1], a [1]) +
				             Vector3.Cross (_quatBasis [2], a [2])) *
				            (1f / Mathf.Abs (Vector3.Dot (_quatBasis [0], a [0]) +
				                             Vector3.Dot (_quatBasis [1], a [1]) +
				                             Vector3.Dot (_quatBasis [2], a [2]) + 0.000000001f));

				var w = omega.magnitude;
				if (w < 0.000000001f)
					break;
				q = Quaternion.AngleAxis(w * Mathf.Rad2Deg, omega / w) * q;
				q = Quaternion.Lerp(q, q, 0f); //Normalizes the Quaternion; critical for error suppression
//			Profiler.EndSample ();
			}
		}

		//Calculate Covariance Matrices --------------------------------------------------
		public static Vector3[] TransposeMultSubtract (Vector3[] vec1, Vector4[] vec2, Vector3 vec1Centroid, Vector3 vec2Centroid, Vector3[] covariance)
		{
//		Profiler.BeginSample ("Calculate Covariance Matrix");
			for (var i = 0; i < 3; i++) { //i is the row in this matrix
				covariance [i] = Vector3.zero;
			}

			for (var k = 0; k < vec1.Length; k++) {//k is the column in this matrix
				var left = (vec1 [k] - vec1Centroid) * vec2 [k].w;
				var right = (new Vector3 (vec2 [k].x, vec2 [k].y, vec2 [k].z) - vec2Centroid) * Mathf.Abs (vec2 [k].w);

				covariance [0] [0] += left [0] * right [0];
				covariance [1] [0] += left [1] * right [0];
				covariance [2] [0] += left [2] * right [0];
				covariance [0] [1] += left [0] * right [1];
				covariance [1] [1] += left [1] * right [1];
				covariance [2] [1] += left [2] * right [1];
				covariance [0] [2] += left [0] * right [2];
				covariance [1] [2] += left [1] * right [2];
				covariance [2] [2] += left [2] * right [2];
			}

//		Profiler.EndSample ();
			return covariance;
		}
	}

	public static class FromMatrixExtension
	{
		public static Vector3 GetVector3(this Matrix4x4 m)
		{
			return m.GetColumn(3);
		}

		public static Quaternion GetQuaternion(this Matrix4x4 m)
		{
			if (m.GetColumn(2) == m.GetColumn(1))
				return Quaternion.identity;

			return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
		}

		public static void FillMatrixFromQuaternion (this Quaternion q, ref Vector3[] covariance)
		{
			covariance [0] = q * Vector3.right;
			covariance [1] = q * Vector3.up;
			covariance [2] = q * Vector3.forward;
		}

		public static Matrix4x4 Lerp(Matrix4x4 a, Matrix4x4 b, float alpha)
		{
			return Matrix4x4.TRS(Vector3.Lerp(a.GetVector3(), b.GetVector3(), alpha), Quaternion.Slerp (a.GetQuaternion(), b.GetQuaternion(), alpha), Vector3.one);
		}

	}
}