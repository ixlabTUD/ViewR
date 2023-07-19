using Normal.Realtime;
using UnityEngine.Events;

namespace ViewR.Core.Networking.Normcore.SyncedActiveState
{
    public class BoolSync : RealtimeComponent<BoolSyncModel>
    {
        public bool applyDefaultBoolOnFreshModel = false;
        public bool defaultBool = false;

        public UnityEvent<bool> boolChanged;
        public bool CurrentValue 
        {
            get => model.syncedBool;
            set => SetBool(value);
        }

        protected override void OnRealtimeModelReplaced(BoolSyncModel previousModel, BoolSyncModel currentModel)
        {
            if (previousModel != null)
            {
                previousModel.syncedBoolDidChange -= BoolDidChange;
            }

            if (currentModel != null)
            {


                if (applyDefaultBoolOnFreshModel && currentModel.isFreshModel)
                {
                    currentModel.syncedBool = defaultBool;
                }
                
                
                BoolDidChange(currentModel, currentModel.syncedBool);
                currentModel.syncedBoolDidChange += BoolDidChange;
            }
        }

        private void BoolDidChange(BoolSyncModel model, bool value)
        {
            boolChanged?.Invoke(value);
        }

        public void SetBool(bool newBool)
        {
            if (!isOwnedLocallyInHierarchy)
                return;
            
            // Bail if no model
            if (model == null)
                return;

            model.syncedBool = newBool;
        }
    }

}