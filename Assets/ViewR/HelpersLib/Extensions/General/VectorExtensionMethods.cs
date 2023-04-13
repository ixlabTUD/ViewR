using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
	/// <summary>
	/// Extension methods for Vectors.
	/// 
	/// Heavily modified, from Andrew Perry @ https://gist.github.com/omgwtfgames/f917ca28581761b8100f/564dddacb77d63d0e8084f7cdc0cd16f86e82f9c - Thanks!
	/// </summary>
	
	public static class VectorExtensionMethods {

		// ReSharper disable once InconsistentNaming
		/// <summary>
		/// Takes X and Y of Vector3 and turns it into Vector2 
		/// </summary>
		public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.y);

		/// <summary>
		/// Overwrites x of Vector3
		/// </summary>
		public static Vector3 SetX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);

		/// <summary>
		/// Overwrites y of Vector3
		/// </summary>
		public static Vector3 SetY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);

		/// <summary>
		/// Overwrites z of Vector3
		/// </summary>
		public static Vector3 SetZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);

		/// <summary>
		/// Overwrites x of Vector2
		/// </summary>
		public static Vector2 SetX(this Vector2 v, float x) => new Vector2(x, v.y);

		/// <summary>
		/// Overwrites y of Vector2
		/// </summary>
		public static Vector2 SetY(this Vector2 v, float y) => new Vector2(v.x, y);

		/// <summary>
		/// Extends Vector2 by z to Vector3
		/// </summary>
		public static Vector3 WithZ(this Vector2 v, float z) => new Vector3(v.x, v.y, z);

		/// <summary>
		/// Adds x to the vector v's x value
		/// </summary>
		public static Vector3 AddToX(this Vector3 v, float x) => new Vector3(v.x + x, v.y, v.z);

		/// <summary>
		/// Adds y to the vector v's y value
		/// </summary>
		public static Vector3 AddToY(this Vector3 v, float y) => new Vector3(v.x, v.y + y, v.z);

		/// <summary>
		/// Adds z to vector v's z value
		/// </summary>
		public static Vector3 AddToZ(this Vector3 v, float z) => new Vector3(v.x, v.y, v.z + z);

		/// <summary>
		/// Adds x to vector v's x value
		/// </summary>
		public static Vector2 AddToX(this Vector2 v, float x) => new Vector2(v.x + x, v.y);

		/// <summary>
		/// Adds y to vector v's y value
		/// </summary>
		public static Vector2 AddToY(this Vector2 v, float y) => new Vector2(v.x, v.y + y);

		/// <param name="axisDirection">unit vector in direction of an axis (eg, defines a line that passes through zero)</param>
		/// <param name="point">the point to find nearest on line for</param>
		/// <param name="isNormalized">Normalize <see cref="axisDirection"/></param>
		public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
		{
			if (!isNormalized) axisDirection.Normalize();
			var d = Vector3.Dot(point, axisDirection);
			return axisDirection * d;
		}
		
		/// <param name="lineDirection">unit vector in direction of line</param>
		/// <param name="point">the point to find nearest on line for</param>
		/// <param name="pointOnLine">a point on the line (allowing us to define an actual line in space)</param>
		/// <param name="isNormalized">Normalize <see cref="lineDirection"/></param>
		public static Vector3 NearestPointOnLine(
			this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
		{
			if (!isNormalized) lineDirection.Normalize();
			var d = Vector3.Dot(point - pointOnLine, lineDirection);
			return pointOnLine + (lineDirection * d);
		}
	}
}
