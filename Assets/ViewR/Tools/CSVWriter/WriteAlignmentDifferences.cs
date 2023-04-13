using System;
using UnityEngine;
using ViewR.HelpersLib.Extensions.General;
using Random = UnityEngine.Random;

namespace ViewR.Tools.CSVWriter
{
    public class WriteAlignmentDifferences : CsvWriter
    {
        [SerializeField] private bool logOnUpdate;

        [SerializeField] private Transform head;

        [SerializeField] private Rigidbody headRigidbody;

        private Vector3 _angularVelocityDelta;
        private Vector3 _positionDelta;

        private Vector3 _previousPosition;

        private void Awake()
        {
            if (!logOnUpdate) return;

            csvHeaders = csvHeaders.Append("posDeltaX").Append("posDeltaY").Append("posDeltaZ");
            csvHeaders = csvHeaders.Append("rotVelocityDeltaX").Append("rotVelocityDeltaY").Append("rotVelocityDeltaZ");
        }

        private void Update()
        {
            if (!logOnUpdate)
                return;

            var positionDelta = head.position - _previousPosition;
            _positionDelta += new Vector3(Math.Abs(positionDelta.x), Math.Abs(positionDelta.y),
                Math.Abs(positionDelta.z));
            _previousPosition = head.position;

            var angularVelocity = headRigidbody.angularVelocity;
            _angularVelocityDelta += new Vector3(Math.Abs(angularVelocity.x), Math.Abs(angularVelocity.y),
                Math.Abs(angularVelocity.z));
        }

        private void OnEnable()
        {
            // AlignmentHelpers.RotatedBy += WriteRotatedBy;
            // AlignmentHelpers.MovedBy += WriteMovedBy;
            //
            //Aligner.RotatedBy += WriteRotatedBy;
            //Aligner.MovedBy += WriteMovedBy;
        }

        private void OnDisable()
        {
            //     AlignmentHelpers.RotatedBy -= WriteRotatedBy;
            //     AlignmentHelpers.MovedBy -= WriteMovedBy;
            //Aligner.RotatedBy -= WriteRotatedBy;
            //Aligner.MovedBy -= WriteMovedBy;
        }


        private void WriteMovedBy(Vector3 arg0)
        {
            // Write time
            WriteTime();

            // Change Vector3 to string and write
            var res = Vector3ToCsvString(arg0);
            WriteToFile(new[] { res }, false);
        }

        private void WriteRotatedBy(float arg0)
        {
            if (!logOnUpdate)
            {
                WriteToFile(new[] { arg0.ToString(RotationFormat, CultInfo) }, true);
            }
            else
            {
                WriteToFile(new[] { arg0.ToString(RotationFormat, CultInfo) }, false);
                WriteDeltas();
            }
        }

        private void WriteTime()
        {
            var timeSinceStartup = Time.time;
            WriteToFile(new[] { timeSinceStartup.ToString(TimeFormat, CultInfo) }, false);
        }

        private static string Vector3ToCsvString(Vector3 arg0, string format = PositionFormat)
        {
            return arg0.x.ToString(format, CultInfo) + "," +
                   arg0.y.ToString(format, CultInfo) + "," +
                   arg0.z.ToString(format, CultInfo);
        }

        private void WriteDeltas()
        {
            var pos = Vector3ToCsvString(_positionDelta);
            var rot = Vector3ToCsvString(_angularVelocityDelta);

            WriteToFile(new[] { pos }, false);
            WriteToFile(new[] { rot }, true);

            _positionDelta = Vector3.zero;
            _angularVelocityDelta = Vector3.zero;
        }

        [ContextMenu("TestRotation")]
        private void TestRotation()
        {
            WriteRotatedBy(Random.Range(0.0f, 95.2f));
        }

        [ContextMenu("Test Position")]
        private void TestPosition()
        {
            var vector = new Vector3(Random.Range(0.0f, 15.2f), Random.Range(0.0f, 15.2f), Random.Range(0.0f, 15.2f));
            WriteMovedBy(vector);
        }
    }
}