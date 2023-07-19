using TMPro;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.Core.UI.MainUI.UI.ConfigMenu
{
    /// <summary>
    /// Displays the battery status and charge.
    /// </summary>
    public class BatteryStatus : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text tmpBatteryLevel;
        [SerializeField]
        private TMP_Text tmpBatteryStatus;

        private void OnEnable()
        {
            UpdateValues();
        }

#if UNITY_EDITOR
        [ExposeMethodInEditor]
#endif
        public void UpdateValues()
        {
            tmpBatteryStatus.text = SystemInfo.batteryStatus.ToString();
            tmpBatteryLevel.text = SystemInfo.batteryLevel.ToString("P");
        }
    }
}