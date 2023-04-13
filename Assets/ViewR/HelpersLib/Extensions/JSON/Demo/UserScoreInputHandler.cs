using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ViewR.HelpersLib.Extensions.JSON.Demo
{
    public class UserScoreInputHandler : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField userNameInputField;
        [SerializeField]
        private string filePathInPersistent;

        private List<UserScoreEntry> _userScoreList = new List<UserScoreEntry>();

        // Fetch current score list
        private void Start()
        {
            _userScoreList = FileHandler.ReadArrayFromJSON<UserScoreEntry>(filePathInPersistent).ToList();
        }

        /// <summary>
        /// Add a user to the list and the json score board
        /// </summary>
        public void AddUserScore()
        {
            // Add to list.
            _userScoreList.Add(new UserScoreEntry(userNameInputField.text, Random.Range(0, 1000)));

            // Clear field
            userNameInputField.text = string.Empty;

            // Save it.
            FileHandler.SaveToJSON<UserScoreEntry>(_userScoreList.ToArray(), filePathInPersistent);
        }

        public void ReadUserScores()
        {
            _userScoreList = FileHandler.ReadArrayFromJSON<UserScoreEntry>(filePathInPersistent).ToList();
        }

        public void ResetLocalList()
        {
            _userScoreList =  new List<UserScoreEntry>();
        }
    }
}