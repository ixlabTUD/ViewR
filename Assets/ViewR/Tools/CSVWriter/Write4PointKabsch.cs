using System;
using UnityEngine;
using Random = UnityEngine.Random;
using ViewR.Core.Calibration.Aligner;

namespace ViewR.Tools.CSVWriter
{
    public class Write4PointKabsch : CsvWriter
    {
        public Transform head;

        private Vector3 _previousPosition;
        private Vector3 _positionDelta;
        private Vector3 _angularVelocityDelta;

        private void OnEnable()
        {
            
            //Start data
            //TIME, Position
            //Postion Kabsch, Rotation Kabsch, Position Two Point, Rotation Two Point
            //Distance, Angle, Calibration Method, End Position and Rotation
            Aligner.Started += WriteStartData;
            Aligner.TwoPointPerformed += WriteTwoPoint;
            Aligner.KabschPerformed += WriteKabsch;
            Aligner.CalibrationPerformed += WriteCalibration;
            Aligner.CalibrationEnd += WriteEnd;
            
            

        }

        private void OnDisable()
        {
            Aligner.Started -= WriteStartData;
            Aligner.TwoPointPerformed -= WriteTwoPoint;
            Aligner.KabschPerformed -= WriteKabsch;
            Aligner.CalibrationPerformed -= WriteCalibration;
            Aligner.CalibrationEnd -= WriteEnd;
        }
        
        private void WriteTime()
        {
            var timeSinceStartup = Time.time;
            WriteToFile(new []{timeSinceStartup.ToString(TimeFormat, CultInfo)}, false);
        }

        private void WriteStartData(Vector3 positionBefore, Vector3 rotationBefore)
        {
            // Write time
            WriteTime();
            
            var res1 = Vector3ToCsvString(positionBefore);
            WriteToFile(new []{res1}, false);
            
            var res2 = Vector3ToCsvString(rotationBefore);
            WriteToFile(new []{res2}, false);
        }
        
        private void WriteTargetsAndSources(Vector3[] targetPositions, Vector3[] sourcePositions)
        {

            foreach (var VARIABLE in targetPositions)
            {
                var res = Vector3ToCsvString(VARIABLE);
                WriteToFile(new []{res}, false);
            }
            foreach (var VARIABLE in sourcePositions)
            {
                var res = Vector3ToCsvString(VARIABLE);
                WriteToFile(new []{res}, false);
            }
        }

        private void WriteKabsch(Vector3 positionKabsch, Vector3 rotationKabsch)
        {
            // Change Vector3 to string and write
            var res1 = Vector3ToCsvString(positionKabsch);
            WriteToFile(new []{res1}, false);
            
            var res2 = Vector3ToCsvString(rotationKabsch);
            WriteToFile(new []{res2}, false);
        }
        
        private void WriteTwoPoint(Vector3 positionTwoPoint, Vector3 rotationTwoPoint)
        {
            // Change Vector3 to string and write
            var res1 = Vector3ToCsvString(positionTwoPoint);
            WriteToFile(new []{res1}, false);
            
            var res2 = Vector3ToCsvString(rotationTwoPoint);
            WriteToFile(new []{res2}, false);
        }
        
        private void WriteCalibration(float distanceBetween, float angleBetween, float endDistanceKabsch, float endAngleKabsch, float endDistanceTwoPoint, float endAngleTwoPoint, string calibrationMethod)
        {
            WriteToFile(new []{distanceBetween.ToString()}, false);
            
            WriteToFile(new []{angleBetween.ToString()}, false);
            
            WriteToFile(new []{endDistanceKabsch.ToString()}, false);
            
            WriteToFile(new []{endAngleKabsch.ToString()}, false);
            
            WriteToFile(new []{endDistanceTwoPoint.ToString()}, false);
            
            WriteToFile(new []{endAngleTwoPoint.ToString()}, false);
            
            WriteToFile(new []{calibrationMethod}, false);
            // // Change Vector3 to string and write
            // var res1 = Vector3ToCsvString(head.position);
            // WriteToFile(new []{res1}, false);
            //
            // var res2 = Vector3ToCsvString(head.eulerAngles);
            // WriteToFile(new []{res2}, false);
            //
            // WriteToFile(new []{endDistance.ToString()}, false);
            //
            // WriteToFile(new []{endAngle.ToString()}, true);
            
            
        }
        
        private void WriteEnd(Vector3 endPosition, Vector3 endRotation)
        {

            // Change Vector3 to string and write
            var res1 = Vector3ToCsvString(endPosition);
            WriteToFile(new []{res1}, false);
            
            var res2 = Vector3ToCsvString(endRotation);
            WriteToFile(new []{res2}, true);
        }
        

        

        private static string Vector3ToCsvString(Vector3 arg0, string format = PositionFormat)
        {
            return arg0.x.ToString(format, CultInfo) + "," +
                      arg0.y.ToString(format, CultInfo) + "," +
                      arg0.z.ToString(format, CultInfo);
        }


        [ContextMenu("TestRotation")]
        private void TestRotation()
        {
            //WriteRotatedByKabsch(Random.Range(0.0f,95.2f));
        }
        [ContextMenu("Test Position")]
        private void TestPosition()
        {
            var vector = new Vector3(Random.Range(0.0f, 15.2f), Random.Range(0.0f, 15.2f), Random.Range(0.0f, 15.2f));
           // WriteMovedByKabsch(vector);
        }
    }
}