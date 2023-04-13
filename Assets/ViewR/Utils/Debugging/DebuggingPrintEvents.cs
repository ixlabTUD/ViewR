using UnityEngine;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Utils.Debugging
{
    public class DebuggingPrintEvents : MonoBehaviour
    {
        public void PrintDebug(string printSting)
        {
            Debug.Log($"Printing from {this}: {printSting}".StartWithFrom(GetType()), this);
        }
    }
}
