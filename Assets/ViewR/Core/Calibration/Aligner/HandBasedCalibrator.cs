using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ViewR.Core.Calibration;
using ViewR.Core.Calibration.Aligner;
using ViewR.HelpersLib.Extensions.AlignmentHelpers;

//[RequireComponent(typeof(ProgressBar))]
public class HandBasedCalibrator : MonoBehaviour
{
    [SerializeField] private float timeElapsedThreshhold = 1.0f;
    private const int NUM_SAMPLES = 50;
    [SerializeField] private float distTolerance = 0.02f;
    [SerializeField] public float velocityThreshold = 0.2f; //Check the velocity Threshold if it is too low.
    [SerializeField] public float calibratorColliderOffset = 0.4f;
    [SerializeField] private bool useVelocityJumpDetection;
    [SerializeField] public float velocityHeadThreshold = 5.0f; // This is a safety feature to turn on passthrough if the person is moving too fast. 
    
    public Transform headCameraTransform;
    public GameObject leftWrist;
    public GameObject rightWrist;
    
    [SerializeField] public Vector3 leftHandPoint;
    [SerializeField] public Vector3 rightHandPoint;

    [SerializeField] private float distanceBetweenFingerPoints;
    public bool UserInCalibrationMode;

    public bool UserStayInCalibrationZone; // This variable is used to avoid continous calibration in the event of user staying near the calibration zone.

    [SerializeField] private float timeElapsed;
    [SerializeField] private float recordingTimeElapsed;

    [SerializeField] private float distanceBetweenHeadAndHand;
    
    [SerializeField] private float leftHandVelocity;
    [SerializeField] private float rightHandVelocity;
    [SerializeField] private float avgVelocityL;
    [SerializeField] private float avgVelocityR;
    [SerializeField] private Vector3 kalmanVelocityL;
    [SerializeField] private Vector3 kalmanVelocityR;

    public CalibrationStation currentCalibrationStation;
    public List<CalibrationStation> HandCalibrationStations;
    [SerializeField] private HandCalibrationConfiguration currentCalibrationStationConfiguration;
   
    public GameObject debugCanvas;
    private DebugDisplay DebugDisplayComponent;

    private float maxDistance;
    private float minDistance;
    
    private Vector3 headCameraPosition_prev;
    private Vector3 leftFingerPoint_prev;
    private Vector3 rightFingerPoint_prev;
   
    private int sampleIndex;

    private Vector3 targetDirectionL;
    private Vector3 targetDirectionR;
    private Vector3 targetPosition;

    private Vector3 targetPositionL;
    private Vector3 targetPositionR;
    
    
    [SerializeField] 
    private Vector3 velocityH;
    private Vector3 velocityL;
    private Vector3 velocityR;
    
    private float speedL;
    private float speedR;

    private KalmanFilter positionKFilter;
    private KalmanFilter velocityKFilter;

    private Vector3[] velocitySamplesL;
    private Vector3[] velocitySamplesR;
    
    private Vector3[] positionSamplesL;
    private Vector3[] positionSamplesR;
    
    [SerializeField] private bool IsDebugMode = false;  
    
    [Header("Record Data")]
    public bool recordDelta;

    private ProgressBar progressBar;

    [SerializeField] private GameObject handBasedCalibratorColliderGameObject;
    [SerializeField] private Collider handBasedCalibratorCollider;


    [SerializeField] private bool TrackingOff;
    private bool isPassthroughOverlayEnabled = false;


    private void Awake()
    {
        progressBar = GetComponent<ProgressBar>();
        progressBar.chargeTime = timeElapsedThreshhold;
    }

    private void Start()
    {
        headCameraTransform = Camera.main.transform;
        leftWrist = CalibrationManager.Instance.leftWrist.gameObject;
        rightWrist = CalibrationManager.Instance.rightWrist.gameObject;

        HandCalibrationStations = CalibrationManager.Instance.HandCalibrationStations;
        
        //handBasedCalibratorComponent = transform.parent.GetComponent<HandBasedCalibrator>();
        handBasedCalibratorColliderGameObject = transform.Find("CalibrationTriggerCollider").gameObject;
        handBasedCalibratorCollider = handBasedCalibratorColliderGameObject.GetComponent<Collider>();
        
        velocitySamplesL = new Vector3[NUM_SAMPLES];
        velocitySamplesR = new Vector3[NUM_SAMPLES];
        
        positionSamplesL = new Vector3[NUM_SAMPLES];
        positionSamplesR = new Vector3[NUM_SAMPLES];
        
        if(debugCanvas&&IsDebugMode)
       {   
            debugCanvas.transform.GetChild(0).gameObject.SetActive(true);
            debugCanvas.transform.GetChild(1).gameObject.SetActive(true);
            DebugDisplayComponent = debugCanvas.transform.Find("DebugUI").gameObject.GetComponent<DebugDisplay>();
        }

        positionKFilter = new KalmanFilter();
        velocityKFilter = new KalmanFilter();
        
        maxDistance = CalibrationManager.Instance.GetMaxDistanceBetweenCalibTargets()+distTolerance;
        minDistance =CalibrationManager.Instance.GetMinDistanceBetweenCalibTargets()-distTolerance;
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(useVelocityJumpDetection)
        {

            // Simple check to see if the person is moving too fast and turning on full passthrough mode.
            velocityH = headCameraTransform.position - headCameraPosition_prev;
            velocityH = velocityH / Time.deltaTime;
            headCameraPosition_prev = headCameraTransform.position;

            if ((velocityH.magnitude > velocityHeadThreshold))
            {
                TrackingOff = true;
            }

            if (TrackingOff && !isPassthroughOverlayEnabled)
            {
                EnablePassthroughSafetyOverlay();
                isPassthroughOverlayEnabled = true;
            }
            else if (!TrackingOff && isPassthroughOverlayEnabled)
            {
                DisablePassthroughSafetyOverlay();
                isPassthroughOverlayEnabled = false;
            }
        }

        //TODO Check if hand tracking is used
        // if (OvrReferenceManager.Instance.LeftOvrHand.HandConfidence != OVRHand.TrackingConfidence.High ||
        //     OvrReferenceManager.Instance.RightOvrHand.HandConfidence != OVRHand.TrackingConfidence.High)
        //     return;

        handBasedCalibratorColliderGameObject.transform.position = headCameraTransform.position;
        handBasedCalibratorColliderGameObject.transform.rotation = headCameraTransform.rotation;
        handBasedCalibratorColliderGameObject.transform.rotation = Quaternion.Euler(0f, handBasedCalibratorColliderGameObject.transform.rotation.eulerAngles.y, handBasedCalibratorColliderGameObject.transform.rotation.eulerAngles.z);
        handBasedCalibratorColliderGameObject.transform.Translate(0,0,calibratorColliderOffset);
      
      
        
        leftHandPoint = leftWrist.transform.position;
        rightHandPoint = rightWrist.transform.position;
        
        positionSamplesL[sampleIndex] = leftWrist.transform.position;
        positionSamplesR[sampleIndex] = rightWrist.transform.position;
    
        leftHandPoint = PerformKalmanFiltering(positionKFilter, positionSamplesL);
        rightHandPoint = PerformKalmanFiltering(positionKFilter, positionSamplesR);
        
        distanceBetweenFingerPoints = Vector3.Distance(rightHandPoint, leftHandPoint);

        distanceBetweenHeadAndHand = Vector3.Distance((leftHandPoint + rightHandPoint) / 2,
            CalibrationManager.Instance.centerEye.transform.position);
     
        velocityL = leftHandPoint - leftFingerPoint_prev ;
        velocityR = rightHandPoint - rightFingerPoint_prev ;
        
        velocityL = velocityL / Time.deltaTime;
        velocityR = velocityR / Time.deltaTime;

        avgVelocityL = 0f;
        avgVelocityR = 0f;
        
        kalmanVelocityL = Vector3.zero;
        kalmanVelocityR = Vector3.zero;
        
        leftFingerPoint_prev = leftHandPoint;
        rightFingerPoint_prev = rightHandPoint;
        
        velocitySamplesL[sampleIndex] = velocityL;
        velocitySamplesR[sampleIndex] = velocityR;

        sampleIndex = (sampleIndex + 1) % NUM_SAMPLES;

        kalmanVelocityL = PerformKalmanFiltering(velocityKFilter, velocitySamplesL);
        kalmanVelocityR = PerformKalmanFiltering(velocityKFilter, velocitySamplesR);
        speedL = kalmanVelocityL.magnitude;
        speedR = kalmanVelocityR.magnitude;
        
        UserStayInCalibrationZone = IsPointWithinCollider(handBasedCalibratorCollider,leftHandPoint) && 
                                    IsPointWithinCollider(handBasedCalibratorCollider,rightHandPoint);
            
        if (IsDebugMode)
        {   DebugDisplayComponent.recordingTimeElapsed = recordingTimeElapsed;
            DebugDisplayComponent.timeElapsed = timeElapsed;
            DebugDisplayComponent.LVelocity = leftHandVelocity;
            DebugDisplayComponent.RVelocity = rightHandVelocity;
            DebugDisplayComponent.LAvgVelocity = avgVelocityL;
            DebugDisplayComponent.RAvgVelocity = avgVelocityR;
            DebugDisplayComponent.LKalmanVelocity = speedL;
            DebugDisplayComponent.RKalmanVelocity = speedR;
            DebugDisplayComponent.distanceBetweenFingerPoints = distanceBetweenFingerPoints;
            DebugDisplayComponent.distanceBetweenHeadAndHand = distanceBetweenHeadAndHand;
        }
        if (!UserStayInCalibrationZone||(speedL > velocityThreshold) ||
            (speedR > velocityThreshold) ||
            (distanceBetweenFingerPoints < minDistance)  ||
            (distanceBetweenFingerPoints > maxDistance)
           )
        {
            ResetValues();
            return;
        }
        
        timeElapsed += Time.deltaTime;
        UserInCalibrationMode = true;
        TrackingOff = false;
        
        progressBar.EnableProgressBar();
        progressBar.IncreaseProgressBar();

        if (timeElapsed < timeElapsedThreshhold)
        {
            UserInCalibrationMode = false;
            currentCalibrationStation = null;
            DisablePassthroughSafetyOverlay();
            return;
        }
         
        // BEGIN CALIBRATION!
        currentCalibrationStationConfiguration =
            CalibrationManager.Instance.getHandCalibrationConfiguration(leftHandPoint, rightHandPoint);
        currentCalibrationStation = GetCalibrationStation(distanceBetweenFingerPoints,
            currentCalibrationStationConfiguration, currentCalibrationStation);

        if (currentCalibrationStation == null)
        {
            ResetValues();
            return;
        }
               
        // Success! Calibrating...
        // if (recordDelta)
        //     RequestWritingEvents.DoWrite();
        
        CalibrationManager.Instance.GetComponentInChildren<Aligner>().DoAlignChargedOverTime(CalibrationManager.Instance.transformToMove.transform,currentCalibrationStation.TargetL.position,
            currentCalibrationStation.TargetR.position, leftHandPoint, rightHandPoint);
        
       
        // if (recordDelta)
        //     RequestWritingEvents.DoWrite();

       // UserStayInCalibrationZone = true;
        ResetValues();
    }

    void OnDrawGizmos()
    {
        if (currentCalibrationStation == null) return;
        
        {
        
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        
        
        Gizmos.DrawSphere(currentCalibrationStation.TargetL.position, 0.02f);
        Gizmos.DrawSphere(currentCalibrationStation.TargetR.position, 0.02f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftHandPoint, 0.02f);
        Gizmos.DrawSphere(rightHandPoint, 0.02f);
        }
    }
    
    private void ResetValues()
    {
        //UserStayInCalibrationZone = false;
        //PassthroughManager.Instance.passthroughOverlayManager.SetOverlayOpacity(0.0f);
        DisablePassthroughSafetyOverlay();
        recordingTimeElapsed = 0;
        timeElapsed = 0;


        if (!GetComponent<ControllerBasedCalibrator>().Running)
        {
            progressBar.DisableProgressBar();
        }
        
    }
    
    private void EnablePassthroughSafetyOverlay()
    {
        //PassthroughManager.Instance.passthroughOverlayManager.SetOverlayOpacity(0.8f);
       
    }
    
    private void DisablePassthroughSafetyOverlay()
    {
        //PassthroughManager.Instance.passthroughOverlayManager.SetOverlayOpacity(0.0f);
      
    }



    private CalibrationStation GetCalibrationStation(float distanceBetweenFingerPoints,
        HandCalibrationConfiguration currentCalibrationStationConfiguration, CalibrationStation calibrationStation)
    {
        if (currentCalibrationStation == null)
        {
            if (HandCalibrationStations.Count == 1)
            {
                currentCalibrationStation = HandCalibrationStations[0];
            }
            else if (!CalibrationManager.Instance.TryGetMatchingHandCalibrationStation(distanceBetweenFingerPoints,
                         currentCalibrationStationConfiguration, out currentCalibrationStation))
            {
                // Debug.LogError(
                //     $"Could not find a matching {nameof(CalibrationStation)}. The {nameof(CalibrationManager)} holds currently {HandCalibrationStations.Count} entries. Maybe you have none present?",
                //     this);
                //
                // return currentCalibrationStation;
            }
        }
        else
        {
            CalibrationManager.Instance.TryGetMatchingHandCalibrationStation(distanceBetweenFingerPoints,
                currentCalibrationStationConfiguration, out currentCalibrationStation);
        }

        return currentCalibrationStation;
    }
 
    public static Vector3 PerformKalmanFiltering(KalmanFilter filter,
        Vector3[] Positions)
    {  
        var filteredPositions = filter.Filter(Positions.ToArray());
        var avgOfFilteredPosition = AlignmentHelpers.AveragePosition(filteredPositions);
        return avgOfFilteredPosition;
    }

    public static bool IsPointWithinCollider(Collider collider, Vector3 point)
    {
        return collider.ClosestPoint(point) == point;
    }
}