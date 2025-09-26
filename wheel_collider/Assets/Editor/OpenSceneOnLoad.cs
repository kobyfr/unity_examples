using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class OpenOnLoad
{
    static OpenOnLoad()
    {
        // Path to your scene (relative to Assets folder)
        string scenePath = "Assets/Scenes/SampleScene.unity";

        // Check if scene exists
        if (System.IO.File.Exists(scenePath))
        {
            // Prevent reopening during playmode domain reloads
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
        else
        {
            Debug.LogWarning($"Scene not found: {scenePath}");
        }
    }
}
