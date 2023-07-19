using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    public class SelectThisButtonOnStart : MonoBehaviour
    {
        [SerializeField] private StateMachine stateMachine;

        private void OnEnable()
        {
            if(stateMachine.returnToDefaultOnDisable)
                GetComponent<Button>().Select();
        }
    }
}
