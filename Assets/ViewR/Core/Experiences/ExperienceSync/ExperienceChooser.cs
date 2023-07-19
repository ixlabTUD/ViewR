using System;
using System.Collections;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;
using ViewR.HelpersLib.Extensions.General;
using ViewR.Managers;

namespace ViewR.Core.Experiences.ExperienceSync
{
    //[RequireComponent(typeof(SyncExperience))]
    public class ExperienceChooser : MonoBehaviour
    {
        [Header("Setup")]
        public ExperienceConfig[] experiences;
        
        [Header("Initial state")]
        public int initialExperience;
        
        [Header("Debugging")]
        public bool debugging;
        
        [Header("Steer from Remote")]
        public int setValueFromRemote;
        
        
        [Header("Boolean for syncing scene across clients")]
        public bool isSynced = true;

        public bool loadDefaultScene = true;

        private SyncExperience _syncExperience;

        private int? _currentExperienceID;
        public int? CurrentExperienceID
        {
            get => _currentExperienceID;
            private set => _currentExperienceID = value;
        }

        public void Start()
        {
            // if (loadDefaultScene)
            // {
            //     
            //     var experience = experiences[initialExperience];
            //     _currentExperienceID = 0;
            //     experience.ToggleOn();
            // }
        }

        public event System.Action<int> NewSpaceExperienceWasConfigured;

        #region Remote Controll Methods

        /// <summary>
        /// Proposes new model based on <see cref="setValueFromRemote"/>
        /// </summary>
        [ExposeMethodInEditor]
        private void RemoteProposeNewModel()
        {
            ProposeNewModel(setValueFromRemote);
        }

        #endregion

        #region Public Methods

        public void ProposeNewModel(int proposedModelExperienceID)
        {
            if(debugging)
                Debug.Log($"Running {nameof(ProposeNewModel)} with proposedModelExperienceID = {proposedModelExperienceID}.".StartWithFrom(GetType()), this);

            if(Application.isPlaying)
            {
                // Catch if out of bounds!
                if (proposedModelExperienceID > experiences.Length - 1)
                {
                    Debug.LogWarning($"Proposed out of bounds value! ({nameof(proposedModelExperienceID)} > {nameof(experiences)}length-1! You may have to add more experiences!)" + " Bailing.".Bold(), this);
                    return;
                }
                
                // If online: Set values
                //TODO SKIP FOR ROBOTRON
                 if (NetworkManager.IsInstanceRegistered &&
                     NetworkManager.Instance.MainRealtimeInstance.connected && isSynced)
                 {
                     if (!_syncExperience)
                         _syncExperience = GetComponent<SyncExperience>();
                
                     if (!_syncExperience)
                     {
                         Debug.LogWarning(
                             $"No {nameof(SyncExperience)} found. Cannot set values.".StartWithFrom(
                                 GetType()), this);
                         return;
                     }
                
                     if(debugging)
                         Debug.Log($"Requesting Ownership and proposing new model.".StartWithFrom(GetType()), this);
                
                     _syncExperience.RequestOwnership();
                
                     _syncExperience.SetModel(proposedModelExperienceID);
                 }
                 else
                 {
                     //Do it offline instead
                     SetNewExperience(proposedModelExperienceID);
                 }
                     
                
                
                //SetNewExperience(proposedModelExperienceID);
            }
        }

        public void SetNewExperience(int modelExperienceID)
        {
            
            if(debugging)
                Debug.Log($"Running {nameof(SetNewExperience)}.".StartWithFrom(GetType()), this);

            // Update!
            SetExperienceModel(modelExperienceID);
        }

        #endregion

        #region Public not-synced methods

        /// <summary>
        /// Toggles ON ALL experiences
        /// </summary>
        public void ToggleOnAllExperiencesLocally()
        {
            ToggleAllExperiencesLocally(true);
        }

        /// <summary>
        /// Toggles OFF ALL experiences
        /// </summary>
        public void ToggleOffAllExperiencesLocally()
        {
            ToggleAllExperiencesLocally(false);
        }

        /// <summary>
        /// Toggles ALL experiences
        /// </summary>
        public void ToggleAllExperiencesLocally(bool toggleOn)
        {
            if (toggleOn)
                foreach (var experienceConfig in experiences)
                    experienceConfig.ToggleOn();
            else
                foreach (var experienceConfig in experiences)
                    experienceConfig.ToggleOff();
        }
        
        public void ToggleSyncButton()
        {
            isSynced = !isSynced;

            if (isSynced)
            {
                //TODO UPDATE SCENES
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator SetExperienceModelDelayed(int modelExperienceID, float waitTime)
        {
            if(debugging)
                Debug.Log($"Running {nameof(SetExperienceModelDelayed)}.".StartWithFrom(GetType()), this);

            yield return new WaitForSeconds(waitTime);
            
            if(debugging)
                Debug.Log($"Done waiting. Changing experience!.".StartWithFrom(GetType()), this);

            // Update!
            SetExperienceModel(modelExperienceID);
        }

        private void SetExperienceModel(int suggestedExperience)
        {
            if(debugging)
                Debug.Log($"Starting to run {nameof(SetExperienceModel)}.".StartWithFrom(GetType()), this);
                
            // Check
            if (CurrentExperienceID == suggestedExperience)
            {
                Debug.LogWarning($"Already at state {CurrentExperienceID}. Bailing", this);
                return;
            }
            
            if(debugging)
                Debug.Log($"Made it past catches of {nameof(SetExperienceModel)}.".StartWithFrom(GetType()), this);
            
            // Apply
            for (var i = 0; i < experiences.Length; i++)
            {
                var experience = experiences[i];

                if (i == suggestedExperience)
                {
                    if(debugging)
                        Debug.Log($"Toggling ON: experience{i}.".StartWithFrom(GetType()), this);
                    
                    experience.ToggleOn();
                }
                else
                {
                    if(debugging)
                        Debug.Log($"Toggling OFF: experience{i}.".StartWithFrom(GetType()), this);
                    experience.ToggleOff();
                }
            }

            // Cache
            CurrentExperienceID = suggestedExperience;
            
            if(debugging)
                Debug.Log($"New CurrentExperienceID: {CurrentExperienceID}.".StartWithFrom(GetType()), this);
            
            NewSpaceExperienceWasConfigured?.Invoke(suggestedExperience);
        }

        /// <summary>
        /// Convenience feature
        /// </summary>
        private void OnValidate()
        {
            // if (!_syncExperience)
            //     _syncExperience = GetComponent<SyncExperience>();

            if (initialExperience > experiences.Length - 1)
            {
                Debug.LogWarning($"the selected initial experience should be within the boundaries of \"{nameof(experiences)}\"", this);
            }
        }

        #endregion
    }
}