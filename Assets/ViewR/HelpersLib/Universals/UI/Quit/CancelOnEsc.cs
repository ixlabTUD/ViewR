using UnityEngine;

namespace ViewR.HelpersLib.Universals.UI.Quit
{
     /// <summary>
     /// Should be on <see cref="ManageQuitUI"/>s "quitYesNo" GameObject, such that it does not take up unnecessary performance. 
     /// </summary>
     public class CancelOnEsc : MonoBehaviour
     {
          [SerializeField] private ManageQuitUI manageQuitUI;

          private void Update()
          {
               if (Input.GetKeyDown(KeyCode.Escape))
               {
                    manageQuitUI.CancelQuitting();
               }
          }
     }
}
