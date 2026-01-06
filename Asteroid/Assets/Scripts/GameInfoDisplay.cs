// 1/5/2026 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using TMPro;

public class GameInfoDisplay: MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI versionText;

    private float deltaTime = 0.0f;

    private void Start()
    {
        // Display the application version
        versionText.text = $"Version: {Application.version}";
    }

    private void Update()
    {
        // Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
    }
}
