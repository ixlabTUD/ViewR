using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace AnimationRecorder.Editor
{
    /// <summary>
    /// A simple class to record all transform changes in the gameobject and all its children that this class lives on.
    /// Modified, based on https://pastebin.com/Y2k4AWdp
    /// </summary>
    public class AnimationRecorder : MonoBehaviour
    {
        private GameObjectRecorder _recorder;

        #region Inspector Variables

        [FormerlySerializedAs("_saveFolderLocation")]
        [Tooltip("Must start with Assets/")]
        [SerializeField]
        private string saveFolderLocation = "Assets/";

        [SerializeField]
        [FormerlySerializedAs("_clipName")] 
        private string clipName;
        [SerializeField]
        [FormerlySerializedAs("_frameRate")] 
        private float frameRate = 15f;

        [Header("Key Bindings")] 
        [SerializeField]
        [FormerlySerializedAs("_startRecKey")]
        private string startRecKey = "i";
        [SerializeField] 
        [FormerlySerializedAs("_stopRecKey")] 
        private string stopRecKey = "o";
        [SerializeField] 
        [FormerlySerializedAs("_deleteRecKey")]
        private string deleteRecKey = "p";
        [SerializeField]
        [Tooltip("ONLY USE WHEN ALL RELATED ASSETS ARE DELETED FROM ASSETS FOLDER")]
        [FormerlySerializedAs("_deleteIndexKey")]
        private string deleteIndexKey = "l";

        #endregion 

        private AnimationClip _clip;
        private AnimationClip _currentClip;
        private bool _canRecord = true;
        private int _index;
        private string _currentClipName;

        private void OnEnable()
        {
            if(_clip == null)
            {
                CreateNewClip();
            }

            var savedIndex = PlayerPrefs.GetInt(gameObject.name + "Index");

            if ( savedIndex != 0)
            {
                _index = savedIndex;
            }
        }

        private void Start()
        {
            _recorder = new GameObjectRecorder(gameObject);
            _recorder.BindComponentsOfType<Transform>(gameObject, true);

            if (clipName == "")
            {
                clipName = gameObject.name + "_animation";
            }

        }

        private void Update()
        {
            ControllerInputs();
        }

        private void ControllerInputs()
        {
            if (Input.GetKeyDown(startRecKey))
            {
                StartRecording();
            }

            if (Input.GetKeyDown(stopRecKey))
            {
                StopRecording();
            }

            if (Input.GetKeyDown(deleteRecKey))
            {
                DeleteRecording();
            }

            //reset index if all clips have been deleted from the assets
            //ONLY USE IF ALL ASSETS HAVE BEEN DELETED
            if (Input.GetKey(deleteIndexKey))
            {
                PlayerPrefs.DeleteKey(gameObject.name + "Index");
                Debug.LogWarning("Clip name indexing has been reset");
                _index = 0;
            }
        }

        private void StartRecording()
        {
            _canRecord = true;
            CreateNewClip();
            Debug.Log("Animation Recording for " + gameObject.name + " has STARTED");
        }

        private void StopRecording()
        {
            Debug.Log("Animation Recording for " + gameObject.name + " has STOPPED");

            _canRecord = false;

            _recorder.SaveToClip(_currentClip);

            AssetDatabase.CreateAsset(_currentClip, saveFolderLocation + _currentClipName + ".anim");

            AssetDatabase.SaveAssets();
        }

        private void DeleteRecording()
        {
            if (_canRecord)
            {
                Debug.LogWarning("Cannot delete when recording!");
                return;
            }

            if (!AssetDatabase.Contains(_currentClip))
            {
                Debug.LogWarning("Clip Has not been saved yet.");
                return;
            }
            AssetDatabase.DeleteAsset(saveFolderLocation + _currentClipName + ".anim");
            Debug.Log("Clip has been DELETED");
    
        }

        private void LateUpdate()
        {
            if (_clip == null) return;

            if(_canRecord)
            {
                _recorder.TakeSnapshot(Time.deltaTime);
            }
        }

        private void CreateNewClip()
        {
            _clip = new AnimationClip();

            if (_clip.name.Contains(_clip.name))
            {
                _clip.name = clipName + " " + (_index++);
                _currentClipName = _clip.name;
            }

            _clip.frameRate = frameRate;

            _currentClip = _clip;
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt(gameObject.name + "Index", _index);
        }

    }
}