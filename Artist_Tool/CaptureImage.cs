using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TMP_InputField
using System.IO;

public class CaptureImage : MonoBehaviour
{
    [Header("Capture Settings")]
    public Camera captureCamera;
    public RenderTexture renderTexture;

    [Header("File Settings")]
    [Tooltip("Directory where screenshots will be saved.")]
    public string screenshotsDirectory = @"F:\Screenshot_unity";

    [Tooltip("TMP InputField for entering the custom file name.")]
    public TMP_InputField fileNameInputField;

    public Button saveButton;

    void Start()
    {
        // Assign the Button's onClick event
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(CaptureAndSaveImage);
        }
    }

    public void CaptureAndSaveImage()
    {
        // Get the file name from the InputField
        string customFileName = fileNameInputField != null ? fileNameInputField.text : "";

        // Set the render texture
        captureCamera.targetTexture = renderTexture;
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        captureCamera.Render();

        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();

        // Ensure the directory exists
        if (!Directory.Exists(screenshotsDirectory))
        {
            Directory.CreateDirectory(screenshotsDirectory);
        }

        // Determine the file name _{System.DateTime.Now:_HH-mm-ss}
        string fileName = string.IsNullOrEmpty(customFileName)
            ? $"icon_{System.DateTime.Now:yyyyMMdd_HHmmss}.png"
            : $"{customFileName}.png";

        // Combine the directory and file name
        string filePath = Path.Combine(screenshotsDirectory, fileName);

        // Convert to PNG and save
        byte[] byteArray = screenshot.EncodeToPNG();
        File.WriteAllBytes(filePath, byteArray);

        Debug.Log($"Image Saved to {filePath}!");

        // Clean up
        RenderTexture.active = null;
        captureCamera.targetTexture = null;

        // Refresh the AssetDatabase if you're in the Editor
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }

    void OnDestroy()
    {
        // Clean up the event listener when the script is destroyed
        if (saveButton != null)
        {
            saveButton.onClick.RemoveListener(CaptureAndSaveImage);
        }
    }
}
