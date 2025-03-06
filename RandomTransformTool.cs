using UnityEngine;
using UnityEditor;

public class RandomTransformTool : EditorWindow
{
    private float minRotationY = 0f, maxRotationY = 360f;
    private float minScale = 0.5f, maxScale = 2f;

    [MenuItem("Tools/Random Transform Tool")]
    public static void ShowWindow()
    {
        GetWindow<RandomTransformTool>("Random Transform Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Random Y Rotation", EditorStyles.boldLabel);
        minRotationY = EditorGUILayout.FloatField("Min Y Rotation", minRotationY);
        maxRotationY = EditorGUILayout.FloatField("Max Y Rotation", maxRotationY);

        if (GUILayout.Button("Apply Random Y Rotation"))
        {
            ApplyRandomYRotation();
        }

        GUILayout.Space(10);

        GUILayout.Label("Random Scale", EditorStyles.boldLabel);
        minScale = EditorGUILayout.FloatField("Min Scale", minScale);
        maxScale = EditorGUILayout.FloatField("Max Scale", maxScale);

        if (GUILayout.Button("Apply Random Scale"))
        {
            ApplyRandomScale();
        }
    }

    private void ApplyRandomYRotation()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Random Y Rotation");
            float randomY = Random.Range(minRotationY, maxRotationY);
            obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.eulerAngles.x, randomY, obj.transform.rotation.eulerAngles.z);
        }
    }

    private void ApplyRandomScale()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj.transform, "Random Scale");
            float randomScale = Random.Range(minScale, maxScale);
            obj.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }
    }
}
