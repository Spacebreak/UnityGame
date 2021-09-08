using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LocalizedTextEditor : EditorWindow
{
    private Vector2 scrollPosition;             //Scrollbar object for the editor window if item count exceeds window height.
    public LocalizationData localizationData;   //Storing localizationData class to ensure it isn't null.

    //Menu window needs to be [serialized] to work with other Unity functions.
    [MenuItem ("Window/Localized Text Editor")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LocalizedTextEditor)).Show();
    }

    //Draws the window when activated.
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        //If the class does contain the right data, creates a
        if(localizationData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Save Localization File"))
            {
                SaveGameData();
            }
        }

        if(GUILayout.Button("Load Localization File"))
        {
            LoadGameData();
        }

        if (GUILayout.Button("Create New Localization File"))
        {
            CreateNewData();
        }

        EditorGUILayout.EndScrollView();
    }

    //Loads the localization file that the user chooses in the windows file explorer. 
    private void LoadGameData()
    {
        string filePath = EditorUtility.OpenFilePanel("Select Localization File", Application.streamingAssetsPath, "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        }
    }

    private void SaveGameData()
    {
        string filePath = EditorUtility.SaveFilePanel("Save localization data file.", Application.streamingAssetsPath, "", "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            string dataAsJson = JsonUtility.ToJson(localizationData);

            File.WriteAllText(filePath, dataAsJson);
        }
    }

    private void CreateNewData()
    {
        localizationData = new LocalizationData();
    }
}
