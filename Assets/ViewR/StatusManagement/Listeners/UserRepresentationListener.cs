using UnityEngine;
using ViewR.StatusManagement.States;

namespace ViewR.StatusManagement.Listeners
{
    public class UserRepresentationListener : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            UserRepresentation.CurrentUserRepresentationDidChange += HandleRepresentationChanges;
        }

        protected virtual void OnDisable()
        {
            UserRepresentation.CurrentUserRepresentationDidChange -= HandleRepresentationChanges;
        }

        protected virtual void HandleRepresentationChanges(UserRepresentationType newUserRepresentationType)
        {
        }
    }
}