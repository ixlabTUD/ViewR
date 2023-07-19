using UnityEngine;
using ViewR.Core.OVR.Passthrough.ConfigurePassthroughLevel.ReactToVisibility;
using ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Core.OVR.Passthrough.Safety
{
    /// <summary>
    /// A class to show a notification window whenever the user enters colliders.
    /// This notification allows the user to toggle the material on the stairs.
    /// 
    /// This is controlled via <see cref="StairsMaterialSwapperCollisionDetector"/>. This allows us to have multiple colliders without having the class react multiple times.
    /// The user needs to close this window.
    /// </summary>
    public class StairsMaterialSwapperWarning : NotificationPanelCaller
    {
        [Header("References")]
        [SerializeField]
        private EnvironmentPassthroughMaterialsSwapper[] materialsChangerEnvironment;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;
        
        private int _countOfCurrentInsideCollisions;
        private int CountOfCurrentInsideCollisions
        {
            get => _countOfCurrentInsideCollisions;
            set
            {
                if (value <= 0)
                {
                    if (debugging)
                        Debug.Log($"Value: {value}. Processing exit.".StartWithFrom(GetType()), this);

                    // We just exited the last collider!
                    ProcessExit();
                    
                    // Set value to 0
                    _countOfCurrentInsideCollisions = 0;
                }
                else
                {
                    // If previously were at 0, we just entered!
                    if (_countOfCurrentInsideCollisions == 0)
                        ProcessEntry();
                    
                    // Set value
                    _countOfCurrentInsideCollisions = value;
                }
            }
        }

        #region Public Methods

        #region Trigger calls

        /// <summary>
        /// Increases the <see cref="CountOfCurrentInsideCollisions"/>
        /// </summary>
        public void RegisterCollisionEnter()
        {
            CountOfCurrentInsideCollisions += 1;
        }

        /// <summary>
        /// Decreases the <see cref="CountOfCurrentInsideCollisions"/>
        /// </summary>
        public void RegisterCollisionExit()
        {
            CountOfCurrentInsideCollisions -= 1;
        }

        #endregion

        #region MaterialsChangerEnvironment tunnles

        public void ShowPassthrough()
        {
            foreach (var changerEnvironment in materialsChangerEnvironment)
                changerEnvironment.BecamePassthrough();
        }
        
        public void ShowVirtual()
        {
            foreach (var changerEnvironment in materialsChangerEnvironment)
                changerEnvironment.BecameVirtual();
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Show notification if not open yet.
        /// </summary>
        private void ProcessEntry()
        {
            // If we are using the manager: Check if this notification is still open.
            if (!localNotificationPanel && NotificationPanelManager.IsInstanceRegistered)
            {
                // Is this notification still open?
                if(NotificationPanelManager.Instance.ContainsCaller((NotificationPanelCaller) this))
                {
                    if (debugging)
                        Debug.Log($"Notification is still open. Bailing.".StartWithFrom(GetType()), this);
                    
                    // Bail
                    return;
                }
            }
            
            ShowWindow();
        }

        private void ProcessExit()
        {
            // Nothing yet...
        }

        #endregion
    }
}