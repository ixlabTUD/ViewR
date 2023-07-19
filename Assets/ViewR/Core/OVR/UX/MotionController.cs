using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MotionController : MonoBehaviour
{
    public GameObject OVRCameraRig;

    private Transform cameraTransform;

    public float movementSpeed = 0.05f;
    
    
    [FormerlySerializedAs("leftControllerPrimaryButton")] public InputAction leftJoystick;
    [FormerlySerializedAs("rightControllerPrimaryButton")] public InputAction rightJoystick;

    // Start is called before the first frame update
    void Start()
    {
        leftJoystick.Enable();
        rightJoystick.Enable();

        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var xzDirection = leftJoystick.ReadValue<Vector2>();
        var upDownDirection = rightJoystick.ReadValue<Vector2>();
        
        Vector3 movementDirection = cameraTransform.right * xzDirection.x + cameraTransform.up * upDownDirection.y + cameraTransform.forward * xzDirection.y;
        Vector3 movement = movementDirection * movementSpeed * Time.deltaTime;
        
        
        OVRCameraRig.transform.Translate(movement);
    }
}
