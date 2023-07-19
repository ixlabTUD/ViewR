using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ViewR.Core.Calibration.AlignWorlds;
using ViewR.HelpersLib.Extensions.AlignmentHelpers;

namespace ViewR.Core.Calibration.Aligner
{
    public class Aligner : MonoBehaviour
    {
        public delegate void CalibrationDelegate(float distanceBetween, float angleBetween, float endDistanceKabsch,
            float endAngleKabsch, float endDistanceTwoPoint, float endAngleTwoPoint, string calibrationMethod);

        public delegate void EndDelegate(Vector3 endPosition, Vector3 endRotation);


        public delegate void KabschDelegate(Vector3 positionKabsch, Vector3 rotationKabsch);

        public delegate void KabschPerformedDelegate();

        public delegate void StartDelegate(Vector3 position, Vector3 rotation);


        public delegate void TwoPointDelegate(Vector3 positionTwoPoint, Vector3 rotationTwoPoint);

        public delegate void TwoPointPerformedDelegate();

        private static bool kabschFinished;
        public float kabschTranslationalThreshold = 0.1f;
        public float kabschRotationalThreshold = 10;
        public static event KabschPerformedDelegate kabschPerformedEvent;
        public static event TwoPointPerformedDelegate twoPointPerformedEvent;
        public static event StartDelegate Started;
        public static event TwoPointDelegate TwoPointPerformed;
        public static event KabschDelegate KabschPerformed;
        public static event CalibrationDelegate CalibrationPerformed;
        public static event EndDelegate CalibrationEnd;

        public static void DoAlignChargedOverTime(Transform objectToMove,
            Vector3[] targetPositionsL,
            Vector3[] targetPositionsR,
            List<Vector3> sourcePositionsL,
            List<Vector3> sourcePositionsR)
        {
            var filter = new KalmanFilter();

            var filterSourcePositionsL = filter.Filter(sourcePositionsL.ToArray());
            var filterSourcePositionsR = filter.Filter(sourcePositionsR.ToArray());


            var sourcePositions = filterSourcePositionsL.Concat(filterSourcePositionsR).ToArray();
            var targetPositions = targetPositionsL.Concat(targetPositionsR).ToArray();

            // First move center upon center.
            AlignmentHelpers.AlignPositions(objectToMove, sourcePositions, targetPositions);

            // Next: Rotate
            AlignmentHelpers.AlignRotations(objectToMove,
                sourcePositionsL.ToArray(),
                sourcePositionsR.ToArray(),
                targetPositionsL,
                targetPositionsR);
        }


        public void DoAlignChargedOverTime(Transform objectToMove,
            Vector3 targetPositionsL,
            Vector3 targetPositionsR,
            Vector3 PositionsL,
            Vector3 PositionsR
        )
        {
            var sourcePositions = new Vector3[2];
            var targetPositions = new Vector3[2];

            sourcePositions[0] = PositionsL;
            sourcePositions[1] = PositionsR;

            targetPositions[0] = targetPositionsL;
            targetPositions[1] = targetPositionsR;


            //ADD NEW TARGETS AND SOURCES AND MOVE OLD BEHIND
            //AND SET Y POSITION TO ZERO
            CalibrationManager.Instance.targetTransforms[0].position = new Vector3(
                CalibrationManager.Instance.targetTransforms[2].position.x, 0,
                CalibrationManager.Instance.targetTransforms[2].position.z);
            CalibrationManager.Instance.targetTransforms[1].position = new Vector3(
                CalibrationManager.Instance.targetTransforms[3].position.x, 0,
                CalibrationManager.Instance.targetTransforms[3].position.z);
            CalibrationManager.Instance.targetTransforms[2].position =
                new Vector3(targetPositionsL.x, 0, targetPositionsL.z);
            CalibrationManager.Instance.targetTransforms[3].position =
                new Vector3(targetPositionsR.x, 0, targetPositionsR.z);

            CalibrationManager.Instance.sourceTransforms[0].position = new Vector3(
                CalibrationManager.Instance.sourceTransforms[2].position.x, 0,
                CalibrationManager.Instance.sourceTransforms[2].position.z);
            CalibrationManager.Instance.sourceTransforms[1].position = new Vector3(
                CalibrationManager.Instance.sourceTransforms[3].position.x, 0,
                CalibrationManager.Instance.sourceTransforms[3].position.z);
            CalibrationManager.Instance.sourceTransforms[2].position = new Vector3(PositionsL.x, 0, PositionsL.z);
            CalibrationManager.Instance.sourceTransforms[3].position = new Vector3(PositionsR.x, 0, PositionsR.z);

            var kabschAligner = CalibrationManager.Instance.kabschAligner;
            var twoPointAligner = CalibrationManager.Instance.twoPointAligner;

            //WRITE START DATA
            Started?.Invoke(objectToMove.position, objectToMove.eulerAngles);

            //PERFORM TWO POINT WITH DUPLICATE
            // First move center upon center.
            AlignmentHelpers.AlignPositions(twoPointAligner, sourcePositions, targetPositions);

            // Next: Rotate
            AlignmentHelpers.AlignRotations(twoPointAligner,
                PositionsL,
                PositionsR,
                targetPositionsL,
                targetPositionsR);

            TwoPointPerformed?.Invoke(twoPointAligner.position, twoPointAligner.eulerAngles);


            //PERFORM KABSCH WITH DUPLICATE
            var kabsch = CalibrationManager.Instance.GetComponentInChildren<IxKabsch>();
            kabsch.objectToAlign = kabschAligner;


            kabsch.targetTransforms = CalibrationManager.Instance.targetTransforms.ToArray();
            kabsch.sourceTransforms = CalibrationManager.Instance.sourceTransforms.ToArray();

            kabsch.translateObject = true;
            kabsch.rotateObject = true;

            kabsch.alignObject = true;

            IxKabsch.KabschConverged += kabschfinished;
            //MOVE ALIGNER
            kabsch.KabschEnabled = true;


            //WAIT FOR KABSCH TO FINISH
            StartCoroutine(FinishCalibration(objectToMove, targetPositionsL, targetPositionsR, sourcePositions));
        }

        public IEnumerator FinishCalibration(Transform objectToMove, Vector3 targetPositionsL, Vector3 targetPositionsR,
            Vector3[] sourcePositions)
        {
            Debug.Log("Waiting for princess to be rescued...");
            yield return new WaitUntil(() => kabschFinished);

            var targetPositions = new Vector3[2];

            targetPositions[0] = targetPositionsL;
            targetPositions[1] = targetPositionsR;


            IxKabsch.KabschConverged -= kabschfinished;
            kabschFinished = false;

            var kabschAligner = CalibrationManager.Instance.kabschAligner;
            var twoPointAligner = CalibrationManager.Instance.twoPointAligner;

            KabschPerformed?.Invoke(kabschAligner.position, kabschAligner.eulerAngles);

            Debug.Log("Kabsch Position: " + kabschAligner.position);
            Debug.Log("Kabsch Rotation: " + kabschAligner.eulerAngles);

            Debug.Log("------------------------------------");


            Debug.Log("TwoPoint Position: " + twoPointAligner.position);
            Debug.Log("TwoPoint Rotation: " + twoPointAligner.eulerAngles);

            var distBetweenKabschAndTwoPoint = Vector3.Distance(kabschAligner.position, twoPointAligner.position);
            var angleBetweenKabschAndTwoPoint = Quaternion.Angle(kabschAligner.rotation, twoPointAligner.rotation);

            var objectToMoveVec2 = new Vector2(objectToMove.position.x, objectToMove.position.z);
            var kabsch = new Vector2(kabschAligner.position.x, kabschAligner.position.z);
            var twoPoint = new Vector2(twoPointAligner.position.x, twoPointAligner.position.z);

            var distanceFloat2 = Vector2.Distance(kabsch, twoPoint);

            var endDistanceKabsch = Vector2.Distance(kabsch, objectToMoveVec2);
            var endAngleKabsch = Quaternion.Angle(objectToMove.rotation, kabschAligner.rotation);
            var endDistanceTwoPoint = Vector2.Distance(twoPoint, objectToMoveVec2);
            var endAngleTwoPoint = Quaternion.Angle(objectToMove.rotation, twoPointAligner.rotation);

            var kabschBuffer = kabschAligner.parent;
            var twoPointBuffer = twoPointAligner.parent;
            kabschAligner.parent = null;
            twoPointAligner.parent = null;

            //CHECK IF DISTANCE AND ANGLE IS SIMILAR TO TWO POINT

            var sameCalibrationStation = CalibrationManager.Instance.targetTransforms[1].position ==
                                         CalibrationManager.Instance.targetTransforms[3].position;

            Debug.Log("Same Station: " + sameCalibrationStation);

            if (distanceFloat2 < kabschTranslationalThreshold &&
                angleBetweenKabschAndTwoPoint <= kabschRotationalThreshold && !sameCalibrationStation)
                //if (false)
            {
                Debug.Log("KABSCH PERFORMED");
                //MovedBy?.Invoke(kabschAligner.position - objectToMove.position);
                //RotatedBy?.Invoke(Quaternion.Angle(kabschAligner.rotation,objectToMove.rotation));

                objectToMove.SetPositionAndRotation(kabschAligner.position, kabschAligner.rotation);
                kabschPerformedEvent?.Invoke();
                CalibrationPerformed?.Invoke(distanceFloat2, angleBetweenKabschAndTwoPoint, endDistanceKabsch,
                    endAngleKabsch, endDistanceTwoPoint, endAngleTwoPoint, "Kabsch");
            }
            else
            {
                Debug.Log("TWOPOINT PERFORMED");


                //MovedBy?.Invoke(twoPointAligner.position - objectToMove.position);
                //RotatedBy?.Invoke(Quaternion.Angle(twoPointAligner.rotation,objectToMove.rotation));
                objectToMove.SetPositionAndRotation(twoPointAligner.position, twoPointAligner.rotation);
                twoPointPerformedEvent?.Invoke();
                CalibrationPerformed?.Invoke(distanceFloat2, angleBetweenKabschAndTwoPoint, endDistanceKabsch,
                    endAngleKabsch, endDistanceTwoPoint, endAngleTwoPoint, "TwoPoint");
            }

            kabschAligner.parent = kabschBuffer;
            twoPointAligner.parent = twoPointBuffer;

            //RESET ALIGNERS
            kabschAligner.localPosition = Vector3.zero;
            kabschAligner.localRotation = Quaternion.identity;

            twoPointAligner.localPosition = Vector3.zero;
            twoPointAligner.localRotation = Quaternion.identity;

            CalibrationManager.Instance.sourceTransforms[2].position = targetPositionsL;
            CalibrationManager.Instance.sourceTransforms[3].position = targetPositionsR;


            StartCoroutine(DoTwoPointAgain(objectToMove, targetPositionsL, targetPositionsR, sourcePositions));
        }

        public IEnumerator DoTwoPointAgain(Transform objectToMove, Vector3 targetPositionsL, Vector3 targetPositionsR,
            Vector3[] sourcePositions)
        {
            yield return new WaitForSeconds(0.01f);

            var targetPositions = new Vector3[2];

            targetPositions[0] = targetPositionsL;
            targetPositions[1] = targetPositionsR;

            var leftWrist = CalibrationManager.Instance.leftWrist.gameObject;
            var rightWrist = CalibrationManager.Instance.rightWrist.gameObject;

            sourcePositions[0] = leftWrist.transform.position;
            sourcePositions[1] = rightWrist.transform.position;


            AlignmentHelpers.AlignPositions(objectToMove, sourcePositions, targetPositions);

            CalibrationEnd?.Invoke(objectToMove.position, objectToMove.eulerAngles);
        }


        public static void kabschfinished()
        {
            kabschFinished = true;
        }
    }
}