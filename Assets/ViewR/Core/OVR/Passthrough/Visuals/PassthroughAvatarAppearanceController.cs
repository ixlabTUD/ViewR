using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewR.Managers;

/// <summary>
/// Control the size of the passthrough bubble when the user is stationary
/// </summary>
public class PassthroughAvatarAppearanceController : MonoBehaviour
{
    [SerializeField] private Transform localUserHeadTransform; 
    [SerializeField] private Transform userHeadTransform;
    [SerializeField] private Vector3 userHeadPosition_prev;
    [SerializeField] private Vector3 localUserHeadPosition_prev;
    [SerializeField] private Vector3 deltaLocalUserPos;
    [SerializeField] private Vector3 deltaUserPos;
    [SerializeField] private float localUserHeadMovementDistance;
    [SerializeField] private float userHeadMovementDistance;
    [SerializeField] private float passthroughScale;
    [SerializeField] private float deltaThreshold = 0.001f;
    [SerializeField] private float targetScale;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private float minScale = 0.4f;
    [SerializeField] private float smoothTime = 1.0f;
    [SerializeField] private float currentScaleVelocity;
    [SerializeField] private GameObject PassthroughAvatarPlane;
    private Material PassthroughAvatarMaterial;


    // Start is called before the first frame update
    void Start()
    {
        localUserHeadTransform =
            Camera.main.transform; // Is that the right way to access the local user head transform?
        userHeadTransform = this.transform;
        PassthroughAvatarMaterial = PassthroughAvatarPlane.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        deltaLocalUserPos = localUserHeadTransform.position - localUserHeadPosition_prev;
        localUserHeadPosition_prev = localUserHeadTransform.position;
        localUserHeadMovementDistance = Vector3.Distance(Vector3.zero, deltaLocalUserPos); //TO DO: Normalize this value

        deltaUserPos = userHeadTransform.position - userHeadPosition_prev;
        userHeadPosition_prev = userHeadTransform.position;
        userHeadMovementDistance = Vector3.Distance(Vector3.zero, deltaUserPos); //TO DO: Normalize this value

        // Calculate the passthroughScale based on movement
        if (localUserHeadMovementDistance > deltaThreshold || userHeadMovementDistance > deltaThreshold)
        {
            targetScale = maxScale;
        }
        else
        {
            targetScale = minScale;
        }

        passthroughScale = Mathf.SmoothDamp(passthroughScale, targetScale, ref currentScaleVelocity, smoothTime);
        PassthroughAvatarMaterial.SetFloat("_PassthroughScale", passthroughScale);
    }
}