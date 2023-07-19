using Normal.Realtime;

namespace ViewR.Core.Experiences.ExperienceSync
{
    public class SyncExperience : RealtimeComponent<SyncExperienceModel>
    {
        
        private ExperienceChooser _experienceChooser;
        
        protected override void OnRealtimeModelReplaced(SyncExperienceModel previousModel, SyncExperienceModel currentModel)
        {
            if (previousModel != null)
            {
                // Unregister from events
                previousModel.experienceIDDidChange -= ExperienceIDDidChange;
            }
            
            if (currentModel != null)
            {                
                // Ensure we see the right thing
                ExperienceIDDidChange(currentModel, model.experienceID);
            
                // Register for events so we'll know if the color changes later
                currentModel.experienceIDDidChange += ExperienceIDDidChange;
            }
        }

        private void ExperienceIDDidChange(SyncExperienceModel syncExperienceModel, int value)
        {
            if (!_experienceChooser)
                _experienceChooser = GetComponent<ExperienceChooser>();

            if (_experienceChooser.isSynced)
            {
                UpdateVisual();
            }
            
        }

        private void UpdateVisual()
        {
            if (!_experienceChooser)
                _experienceChooser = GetComponent<ExperienceChooser>();
            
            _experienceChooser.SetNewExperience(model.experienceID);
        }

        public void SetModel(int idGeometry)
        {
            // Set the id on the model
            model.experienceID = idGeometry;
        }
        
        /// <summary>
        /// Convenience feature
        /// </summary>
        private void OnValidate()
        {
            if (!_experienceChooser)
                _experienceChooser = GetComponent<ExperienceChooser>();
        }
    }
}