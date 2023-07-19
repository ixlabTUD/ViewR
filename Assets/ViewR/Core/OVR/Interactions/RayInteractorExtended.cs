using Oculus.Interaction;
using UnityEngine;

namespace ViewR.Core.OVR.Interactions
{
    public class RayInteractorExtended : RayInteractor
    {
        [Header("Config")]
        [SerializeField]
        private float defaultScale = 1;
        [SerializeField]
        private float nearFieldScale = 0.3f;
        [SerializeField]
        private AnimationCurve animationCurve;

        [Header("References")]
        [SerializeField]
        private Transform cursorVisual;

        /// <summary>
        /// Runs the expected <see cref="ComputeCandidate"/>, but interjects and scales the cursor before forwarding the resulting value.
        /// </summary>
        /// <returns></returns>
        protected override RayInteractable ComputeCandidate()
        {
            var closestRayInteractable = base.ComputeCandidate();

            // Keep it simple if there is nothing to do here.
            if (closestRayInteractable == null)
                return null;

            // Get ray distance and calculate the local scale of the selector
            cursorVisual.localScale = Vector3.one *
                                  Mathf.Lerp(nearFieldScale, defaultScale,
                                      // Clamp
                                      Mathf.Clamp01(
                                          // Evaluate our curve for the distance
                                          animationCurve.Evaluate(
                                              // Calculate the distance between origin and end point.
                                              Vector3.Distance(Origin, End) / MaxRayLength
                                          )
                                      )
                                  );

            return closestRayInteractable;
        }
    }
}