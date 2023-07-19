using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using ViewR.Core.Networking.Normcore.Avatar;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.Managers;

namespace ViewR.Core.Networking.Normcore.ActiveUsers
{
    /// <summary>
    /// Lists active users on the board. Currently will overflow after > 6 users
    /// </summary>
    [DisallowMultipleComponent]
    public class ListActiveUsers : MonoBehaviour
    {
        [SerializeField, Help("If left empty, this gameObjects transform will be used.")]
        private Transform listActiveUsersParent;

        [SerializeField, Tooltip("If left empty, the first child of listActiveUsersParent will be used."), Optional]
        private GameObject userEntryPrefab;

        private RealtimeAvatarManager _realtimeAvatarManager;
        private Realtime _realtime;

        private readonly Dictionary<int, Transform> _markersOfActiveUsers = new Dictionary<int, Transform>();

        private bool _initialized;
        private bool _subscribed;

        private void Start()
        {
            // Get references
            _realtimeAvatarManager = NetworkManager.Instance.RealtimeAvatarManager;

            if (!listActiveUsersParent)
                listActiveUsersParent = transform;

            if (!_realtime)
                _realtime = NetworkManager.Instance.MainRealtimeInstance;
            Debug.Log($"ListActiveUsers.Start: Listening to connection to room.");
            if (_realtime.connected)
                Initialize(_realtime, true);
            _realtime.didConnectToRoom += Initialize;
            _subscribed = true;

            // Deactivate all current markers
            foreach (Transform marker in listActiveUsersParent)
                marker.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UnInitialize();

            if (_subscribed)
            {
                _realtime.didConnectToRoom -= Initialize;
                _subscribed = false;
            }
        }

        #region Initialize / Un-Initialize

        private void Initialize(Realtime realtime)
        {
            Initialize(realtime, false);
        }

        private void Initialize(Realtime realtime, bool overwriteInitialization = false)
        {
            switch (overwriteInitialization)
            {
                // Bail if already initialized and we won't overwrite it.
                case false when _initialized:
                    return;
                case true when _initialized:
                    UnInitialize();
                    break;
            }

            Debug.Log($"{nameof(ListActiveUsers)}.{nameof(Initialize)}: Connected to room. Initializing");

            _realtimeAvatarManager.avatarCreated += HandleAvatarCreated;
            _realtimeAvatarManager.avatarDestroyed += HandleAvatarDestroyed;

            _initialized = true;
        }

        private void UnInitialize()
        {
            if (!_initialized)
                return;

            _realtimeAvatarManager.avatarCreated -= HandleAvatarCreated;
            _realtimeAvatarManager.avatarDestroyed -= HandleAvatarDestroyed;

            _initialized = false;
        }

        #endregion

        #region Subscribed methods

        private void HandleAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
        {
            try
            {
                // Initial run. Might cause errors as no realtime model is present yet.
                UpdateUserBoardName(avatar.ownerIDInHierarchy);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Running {nameof(UpdateUserBoardName)} with {nameof(UpdateUserBoardNameDelayed)}, because {nameof(UpdateUserBoardName)} failed with:\n" + e);
                
                StartCoroutine(UpdateUserBoardNameDelayed(avatar.ownerIDInHierarchy));
            }
        }

        private void HandleAvatarDestroyed(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar,
            bool isLocalAvatar)
        {
            RemoveUsersMarker(avatar.ownerIDInHierarchy);
        }

        #endregion

        #region Core logic

        private IEnumerator UpdateUserBoardNameDelayed(int ownerID)
        {
            yield return new WaitForSeconds(2f);
            UpdateUserBoardName(ownerID);
        }

        private void UpdateUserBoardName(int ownerID)
        {
            // Abort if not connected User ID
            if (ownerID == -1) return;

            //! Update Board
            // If not present
            if (!_markersOfActiveUsers.TryGetValue(ownerID, out var marker))
            {
                AddUserMarker(ownerID);
                // If still not present
                if (!_markersOfActiveUsers.TryGetValue(ownerID, out marker))
                    throw new Exception($"There seems to be no marker for this user {ownerID}.");
            }

            _realtimeAvatarManager.avatars.TryGetValue(ownerID, out var avatar);

            if (avatar == null)
            {
                return;
            }

            // Get name
            var syncedPlayerPropertiesSync = avatar.GetComponent<AvatarAccessHelper>().SyncedPlayerPropertiesSync;
            if (syncedPlayerPropertiesSync == null)
                //! If no syncedPlayerPropertiesSync present (invisible desktop avatar), skip it.
                return;

            // Set Name
            var tmp = marker.GetComponentInChildren<TMP_Text>();
            tmp.text = syncedPlayerPropertiesSync.GetCurrentUserName();

            // Show marker
            marker.gameObject.SetActive(true);
        }


        private void AddUserMarker(int ownerID)
        {
            // If not yet present
            if (!_markersOfActiveUsers.TryGetValue(ownerID, out var marker))
            {
                // Find unused (=inactive) marker:
                foreach (Transform child in listActiveUsersParent)
                    if (!child.gameObject.activeSelf)
                    {
                        marker = child;
                        break;
                    }

                // Error prevention
                if (marker == null)
                {
                    Debug.LogWarning(
                        "Could not find available inactive marker. It seems there are too few marker instances available.");
                    // Instantiate objToInstantiate or clone first child.
                    var objToInstantiate = userEntryPrefab != null
                        ? userEntryPrefab
                        : listActiveUsersParent.GetChild(0).gameObject;
                    marker = Instantiate(objToInstantiate, listActiveUsersParent).transform;
                }

                // Update dict
                _markersOfActiveUsers.Add(ownerID, marker);
            }

            // Set Name
            var tmp = marker.GetComponentInChildren<TMP_Text>();
            tmp.text = "";

            marker.gameObject.SetActive(true);
        }

        private void RemoveUsersMarker(int ownerID)
        {
            // If not present
            if (!_markersOfActiveUsers.TryGetValue(ownerID, out var marker))
                throw new Exception($"There seems to be no marker for this user {ownerID}.");

            marker.gameObject.SetActive(false);

            // Reset dict entry
            _markersOfActiveUsers.Remove(ownerID);
        }

        #endregion
    }
}