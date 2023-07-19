using UnityEngine;
using UnityEngine.Animations;

/// <summary>
/// Sets the first source in the <see cref="LookAtConstraint"/> to the main camera.
/// Overwrites first value if there are any existing.
/// </summary>
[RequireComponent(typeof(LookAtConstraint))]
public class LookAtMainCameraProgressBar : MonoBehaviour
{
    [SerializeField]
    private bool overwriteFirstSourceIfPresent = true;
    [SerializeField]
    private bool destroyUponSetup = true;
    
    public void Start()
    {
        Setup();
    }

    /// <summary>
    /// Forces the <see cref="LookAtConstraint"/> to set the first constraint source to the main camera.
    /// </summary>
    private void Setup()
    {
        var lookAtConstraint = GetComponent<LookAtConstraint>();
       
        // Define the source - use ReferenceManager if possible.
        var constraintSource = new ConstraintSource
        {
            sourceTransform = Camera.main.transform,
            weight = 1
        };

        if (lookAtConstraint.sourceCount == 0 || !overwriteFirstSourceIfPresent)
            lookAtConstraint.AddSource(constraintSource);
        else
            lookAtConstraint.SetSource(0, constraintSource);
        
        lookAtConstraint.constraintActive = true;
        
        // Clean up
        if (destroyUponSetup)
            Destroy(this);
    }
}
