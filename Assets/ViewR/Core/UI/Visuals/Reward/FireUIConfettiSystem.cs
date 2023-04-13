using UnityEngine;
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.UI.Visuals.Reward
{
    public class FireUIConfettiSystem : MonoBehaviour
    {
        private FireParticleBurst _fireParticleBurst;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if(ModalWindowUIController.IsInstanceRegistered)
                _fireParticleBurst = ModalWindowUIController.Instance.FireParticleBurst;
            else
                throw new MissingReferenceException("ModalWindowUIController could not be found. Ensure it's loaded and initialized.".StartWithFrom(GetType()));
        }

        /// <summary>
        /// Fires one shot at the position and orientation of the modalWindowsUI
        /// </summary>
        public void FireOneShotModalUI()
        {
            FireOneShot(ModalWindowUIController.Instance.ModalWindowPanel.confettiSpawnPosition);
        }

        /// <summary>
        /// Fires the system.
        /// </summary>
        public void FireOneShot()
        {
            // Ensure we have references
            if(!_fireParticleBurst)
                Initialize();
            
            // Just in case
            _fireParticleBurst.gameObject.SetActive(true);
            
            // Fire!
            _fireParticleBurst.EmitOnce();
        }

        /// <summary>
        /// Fires the system.
        /// </summary>
        /// <param name="position">The world position where to position this particle system.</param>
        public void FireOneShot(Vector3 position)
        {
            // Ensure we have references
            if(!_fireParticleBurst)
                Initialize();
            
            // Just in case
            _fireParticleBurst.gameObject.SetActive(true);
            
            // Fire!
            _fireParticleBurst.EmitOnce(position);
        }

        /// <summary>
        /// Fires the system.
        /// </summary>
        /// <param name="targetPose">The transform with which to align this particle systems position with.</param>
        public void FireOneShot(Transform targetPose)
        {
            // Ensure we have references
            if(!_fireParticleBurst)
                Initialize();
            
            // Just in case
            _fireParticleBurst.gameObject.SetActive(true);
            
            // Fire!
            _fireParticleBurst.EmitOnce(targetPose.position, targetPose.rotation);
        }
    }
}