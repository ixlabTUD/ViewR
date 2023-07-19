using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.Core.UI.Visuals.Reward
{
    /// <summary>
    /// Invokes the Particle system once
    /// </summary>
    public class FireParticleBurst : MonoBehaviour
    {
        [SerializeField]
        private int defaultNumberOfParticles = 100;
        [SerializeField,Tooltip("We child the particle system to ease the rotation of it, such that it does not fire \"towards the user\".")]
        private ParticleSystem burstParticleSystem;
        
        private Vector3 _initialLocalPosition;
        private Vector3 _initialLocalScale;
        private Quaternion _initialLocalRotation;

        private void Awake()
        {
            if(!burstParticleSystem)
                burstParticleSystem = GetComponentInChildren<ParticleSystem>();
            
            // Fetch Initial Values
            var thisTransform = transform;
            _initialLocalPosition = thisTransform.localPosition;
            _initialLocalScale = thisTransform.localScale;
            _initialLocalRotation = thisTransform.localRotation;
        }

        /// <summary>
        /// Restores the initial pose and scale.
        /// </summary>
        public void RestoreInitialPose()
        {
            var thisTransform = transform;
            thisTransform.localPosition = _initialLocalPosition;
            thisTransform.localScale = _initialLocalScale;
            thisTransform.localRotation = _initialLocalRotation;
        }

        /// <summary>
        /// Allows us to fire the particle system
        /// </summary>
        /// <remarks>
        /// Sets the localPosition to Vector3.zero.
        /// </remarks>
        /// <param name="numberOfParticles">Number of Particles. If negative, the default value is going to be used.</param>
        public void EmitOnce(int numberOfParticles = -1)
        {
            // Reposition
            transform.localPosition = Vector3.zero;
            
            burstParticleSystem.Emit(numberOfParticles > 0 ? numberOfParticles : defaultNumberOfParticles);
        } 
        
        /// <summary>
        /// Allows us to fire the particle system
        /// </summary>
        /// <param name="position">The world position where to position this particle system.</param>
        /// <param name="numberOfParticles">Number of Particles. If negative, the default value is going to be used.</param>
        public void EmitOnce(Vector3 position, int numberOfParticles = -1)
        {
            // Update position
            this.transform.position = position;
            // Fire
            burstParticleSystem.Emit(numberOfParticles > 0 ? numberOfParticles : defaultNumberOfParticles);
        }

        /// <summary>
        /// Allows us to fire the particle system
        /// </summary>
        /// <param name="position">The world position where to position this particle system.</param>
        /// <param name="rotation">The world rotation to apply to this particle system</param>
        /// <param name="numberOfParticles">Number of Particles. If negative, the default value is going to be used.</param>
        public void EmitOnce(Vector3 position, Quaternion rotation, int numberOfParticles = -1)
        {
            // Update position
            var thisTransform = transform;
            thisTransform.position = position;
            thisTransform.rotation = rotation;
            // Fire
            burstParticleSystem.Emit(numberOfParticles > 0 ? numberOfParticles : defaultNumberOfParticles);
        }

        /// <summary>
        /// Tunnels <see cref="EmitOnce"/> with its <see cref="defaultNumberOfParticles"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="EmitOnce"/> directly if you want to set the number of particles.
        /// </remarks>
        [ExposeMethodInEditor]
        public void FireBurst() => EmitOnce();

    }
}