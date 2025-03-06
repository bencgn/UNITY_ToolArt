using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ProjectFolderSelectionTool : EditorWindow
{
    private List<string> folderPaths = new List<string>(); // List to hold folder paths

    [MenuItem("Tools/Project Folder Selection Tool")]
    public static void ShowWindow()
    {
        GetWindow<ProjectFolderSelectionTool>("Project Folder Selection Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Project Folder Selection Tool", EditorStyles.boldLabel);

        // Display UI for folder paths
        for (int i = 0; i < folderPaths.Count; i++)
        {
            GUILayout.BeginHorizontal();

            // Display folder path and select button
            GUILayout.Label($"Path {i + 1}: ", GUILayout.Width(50));
            GUILayout.TextField(folderPaths[i], GUILayout.Width(250));

            // "Select Folder" button
            if (GUILayout.Button("Select Folder", GUILayout.Width(120)))
            {
                string folderPath = SelectFolderFromProject();
                if (!string.IsNullOrEmpty(folderPath))
                {
                    folderPaths[i] = folderPath;
                }
            }

            // "Click" button for additional action
            if (GUILayout.Button("Click", GUILayout.Width(80)))
            {
                if (!string.IsNullOrEmpty(folderPaths[i]))
                {
                    OpenFolder(folderPaths[i]);
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please select a folder first!", "OK");
                }
            }

            // Remove folder button
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                folderPaths.RemoveAt(i);
            }

            GUILayout.EndHorizontal();
        }

        // Add new folder button
        if (GUILayout.Button("Add Folder", GUILayout.Width(120)))
        {
            folderPaths.Add("");
        }

        // Save configuration
        GUILayout.Space(10);
        if (GUILayout.Button("Save Configuration", GUILayout.Width(150)))
        {
            SaveConfiguration();
        }
    }

    private string SelectFolderFromProject()
    {
        string path = EditorUtility.OpenFolderPanel("Select Folder in Project", Application.dataPath, "");
        if (!string.IsNullOrEmpty(path))
        {
            // Ensure the path is within the Assets folder
            if (path.StartsWith(Application.dataPath))
            {
                // Convert to a relative project path (e.g., "Assets/MyFolder")
                return "Assets" + path.Substring(Application.dataPath.Length).Replace("\\", "/");
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid Folder", "Please select a folder inside the Assets directory.", "OK");
            }
        }
        return null;
    }

    private void OpenFolder(string folderPath)
    {
        // Check if the folder exists
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            // Highlight the folder in the Project window
            Object folderObject = AssetDatabase.LoadAssetAtPath<Object>(folderPath);
            Selection.activeObject = folderObject;
            EditorGUIUtility.PingObject(folderObject);
        }
        else
        {
            EditorUtility.DisplayDialog("Error", $"Folder '{folderPath}' does not exist in the Project!", "OK");
        }
    }

    private void SaveConfiguration()
    {
        string savePath = EditorUtility.SaveFilePanel("Save Config", "", "FolderConfig.json", "json");
        if (!string.IsNullOrEmpty(savePath))
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(new FolderConfig { paths = folderPaths }, true));
            EditorUtility.DisplayDialog("Success", "Configuration saved successfully!", "OK");
        }
    }

    [System.Serializable]
    public class FolderConfig
    {
        public List<string> paths;
    }
}
