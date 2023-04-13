using UnityEngine;

namespace ViewR.HelpersLib.Universals.UI.Quit
{
    public class Quit : MonoBehaviour
    {
        private static bool _debugging;
        
        public static void QuitApplication()
        {
            if(_debugging) Debug.Log($"Quitting via Quit.cs");
        
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    
        // Optional, but usually handled elsewhere to ensure it's present: 
        // private void OnApplicationQuit()
        // {
        //     PlayerPrefs.SetString("QuitTime", "The application last closed at: " + System.DateTime.Now);
        // }
    }
}
