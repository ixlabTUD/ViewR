using System.Text;
using Normal.Realtime;
using UnityEngine;
using ViewR.HelpersLib.Extensions.EditorExtensions.ExposeMethodInEditor;

namespace ViewR.Core.Networking.Tests
{
    public class StringSync : RealtimeComponent<StringSyncModel>
    {
        public int numberOfChars = 500;
        
        private const string Glyphs = "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
        
        protected override void OnRealtimeModelReplaced(StringSyncModel previousModel, StringSyncModel currentModel)
        {
            if (previousModel != null)
            {
                // Unregister from events
                previousModel.aVeryLongStringDidChange -= AVeryLongStringDidChange;
            }

            if (currentModel != null)
            {
                // Register for events 
                currentModel.aVeryLongStringDidChange += AVeryLongStringDidChange;
            }
        }

        private void AVeryLongStringDidChange(StringSyncModel stringSyncModel, string value)
        {
            Debug.Log("Received new String: " + value);
        }

        public void SetNewString(string newString)
        {
            model.aVeryLongString = newString;
        }

        [ExposeMethodInEditor]
        private void SetRandomNewString()
        {
            var chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
            var r = new System.Random();
            
            var sb = new StringBuilder();
            for (var i = 0; i < numberOfChars; i++)
            {
                var j = r.Next(chars.Length);
                sb.Append(chars[j]);
            }
            
            SetNewString( sb.ToString());
        }
    }
}