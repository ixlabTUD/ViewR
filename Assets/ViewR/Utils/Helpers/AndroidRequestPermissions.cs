using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.Utils.Helpers
{
    /// <summary>
    /// Ensures we have the needed rights
    /// </summary>
    public class AndroidRequestPermissions : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private bool askForWritingExternalData = true;

        [SerializeField]
        private bool askForReadingExternalData = true;

        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private readonly List<string> _listOfPermissions = new List<string>();

        private void Start()
        {
            _listOfPermissions.Add(Permission.Microphone);
            if (askForWritingExternalData) _listOfPermissions.Add(Permission.ExternalStorageWrite);
            if (askForReadingExternalData) _listOfPermissions.Add(Permission.ExternalStorageRead);

            // Request permission recursively
            StartCoroutine(RequestPermissionsRecursively(_listOfPermissions[0]));
        }

        private IEnumerator RequestPermissionsRecursively(string requestedPermission)
        {
            if (debugging)
                Debug.Log(
                    $"AccessRequest: number of requested rights: {_listOfPermissions.Count}".StartWithFrom(GetType()),
                    this);

            // Pop permission.
            _listOfPermissions.Remove(requestedPermission);

            // Request permission
            if (!Permission.HasUserAuthorizedPermission(requestedPermission))
                Permission.RequestUserPermission(requestedPermission);

            // Wait until agreed.
            yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(requestedPermission));

            // Recurse if not all requested yet.
            if (_listOfPermissions.Count != 0)
                StartCoroutine(RequestPermissionsRecursively(_listOfPermissions[0]));
            else
            {
                if (debugging)
                    Debug.Log("AccessRequest: All requested permissions acquired.".StartWithFrom(GetType()),
                        this);
            }

            // Disable this.
            this.enabled = false;
        }
    }
}