using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class BakeScenes : EditorWindow
{
    // Array to store an Object array of the scenes
    public Object[] scenes;

    // Lists and string array for easier management
    List<string> sceneList = new List<string>();
    private int sceneIndex = 0;
    private string[] scenePath;

    // Editor text
    string bakeButton = "Bake";
    string status = "Idle...";
    System.DateTime timeStamp;

    // Menu entry
    [MenuItem("Tools/Bake Scenes")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(BakeScenes), false, "Bake Scenes");
        window.autoRepaintOnSceneChange = true;
    }

    // Refresh the editor text when in focus
    void OnFocus()
    {
        status = "Idle...";
        if (!Lightmapping.isRunning)
        {
            bakeButton = "Bake";
        }
    }

    void OnGUI()
    {
        // "target" can be any class derrived from ScriptableObject 
        // (could be EditorWindow, MonoBehaviour, etc)
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty scenesProperty = so.FindProperty("scenes");

        EditorGUILayout.PropertyField(scenesProperty, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

        if (GUILayout.Button(bakeButton)) // Button to start bake process
        {
            InitializeBake();
        }
        EditorGUILayout.LabelField("Status: ", status);
        so.Update();
    }

    // If not baking, set delegates, set scenes, and start baking.
    // Otherwise, stop lightmapping and update editor text
    void InitializeBake()
    {
        if (!Lightmapping.isRunning)
        {
            //Lightmapping.bakeCompleted = null;
            Lightmapping.bakeCompleted += SaveScene;
            //SaveScene();
            Lightmapping.bakeCompleted += BakeNewScene;
            SetScenes();
            BakeNewScene();
        }
        else
        {
            Lightmapping.Cancel();
            UpdateBakeProgress();
        }
    }

    // Create a string array of scenes to bake
    private bool SetScenes()
    {
        // Reset values
        sceneList.Clear();
        sceneIndex = 0;

        // Get paths for scenes and store in list
        if (scenes.Length == 0)
        {
            status = "No scenes found";
            bakeButton = "Bake";
            return false;
        }
        else
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                sceneList.Add(AssetDatabase.GetAssetPath(scenes[i]));
            }

            // Sort and put scene paths in array
            scenePath = sceneList.ToArray();
            return true;
        }
    }

    // Loop through scenes to bake and update on progress
    private void BakeNewScene()
    {
        if (sceneIndex < scenes.Length)
        {
            EditorSceneManager.OpenScene(scenePath[sceneIndex]);
            //EditorApplication.OpenScene(scenePath[sceneIndex]);
            timeStamp = System.DateTime.Now;
            Lightmapping.BakeAsync();
            UpdateBakeProgress();
            sceneIndex++;
        }
        else
        {
            DoneBaking("done");
        }
    }

    // Updates baking progress
    private void UpdateBakeProgress()
    {
        if (Lightmapping.isRunning)
        {
            status = "Baking " + (sceneIndex + 1).ToString() + " of " + scenes.Length.ToString();
            bakeButton = "Cancel";
        }
        else if (!Lightmapping.isRunning)
        {
            DoneBaking("cancel");
        }
    }

    // Saves the scene at the end of each bake before starting new bake
    private void SaveScene()
    {
        System.TimeSpan bakeSpan = System.DateTime.Now - timeStamp;
        string bakeTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            bakeSpan.Hours, bakeSpan.Minutes, bakeSpan.Seconds);
        Debug.Log("(" + sceneIndex.ToString() + "/" +
            scenes.Length.ToString() + ") " + "Done baking: " +
            EditorSceneManager.GetActiveScene().name + " after " + bakeTime +
            " on " + System.DateTime.Now.ToString());
        EditorSceneManager.SaveOpenScenes();
        //EditorApplication.SaveScene();
    }

    // When done baking, update the editor text
    private void DoneBaking(string reason)
    {
        //Lightmapping.completed = null;
        Lightmapping.bakeCompleted -= BakeNewScene;
        sceneList.Clear();
        sceneIndex = 0;

        if (reason == "done")
        {
            status = "Bake is done";
            bakeButton = "Bake";
        }
        else if (reason == "cancel")
        {
            status = "Canceled";
            bakeButton = "Bake";
        }
    }
}