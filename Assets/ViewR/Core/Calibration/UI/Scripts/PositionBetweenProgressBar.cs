using System;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.Core.Calibration;

public class PositionBetweenProgressBar : MonoBehaviour
    {
        private Transform objectA;
        
        private Transform objectB;
        
        public Vector3 worldCoordinateOffset;

        public Vector3 localCoordinateOffset;

        public bool Active
        {
            get => this.enabled;
            set => this.enabled = value;
        }

        private void Start()
        {
            objectA = CalibrationManager.Instance.leftHandAnchor;
            objectB = CalibrationManager.Instance.rightHandAnchor;
        }

        private void Update()
        {
            var objectAPosition = objectA.position;
            var localTransform = transform;
            localTransform.position = objectAPosition +  (objectB.position - objectAPosition) / 2 + worldCoordinateOffset + (localTransform = transform).TransformVector(localCoordinateOffset);
        }
    }