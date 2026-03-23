// 1/5/2026 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(0)]
public class GameInfoDisplay: MonoBehaviour
{
    public static GameInfoDisplay instance;

    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI versionText;
    public TextMeshProUGUI sceneText;

    private float deltaTime = 0.0f;
    private string scene_name = "";

    private void Awake()
    {
        // Singleton + persist
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        // Display the application version
        versionText.text = $"Version: {Application.version}";
        LogToFile.Log($"GameInfoDisplay start. sceneText.text: {sceneText.text}");
    }

    private void Update()
    {
        // Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
    }

    public void update_scene_name(string sceneName)
    {
        LogToFile.Log($"GameInfoDisplay before update_scene_name. sceneName:      {sceneName}");
        LogToFile.Log($"GameInfoDisplay before update_scene_name. sceneText.text: {sceneText.text}");
        scene_name = sceneName;
        sceneText.text = scene_name;
        LogToFile.Log($"GameInfoDisplay after  update_scene_name. sceneText.text: {sceneText.text}");
    }
}
