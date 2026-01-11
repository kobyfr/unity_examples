// 1/11/2026 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GlobalMainSceneLoader : MonoBehaviour
{
    [Header("Scene Name")]
    [SerializeField] private string sceneToLoad = "scene_selection";

    private static GlobalMainSceneLoader instance;
    private InputAction escapeAction;

    private void Awake()
    {
        // Singleton + persist
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Create an input action for Escape
            escapeAction = new InputAction(
                name: "OpenSceneSelectionScene",
                binding: "<Keyboard>/escape"
            );
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        if (escapeAction != null)
        {
            escapeAction.performed += OnEscapePressed;
            escapeAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (escapeAction != null)
        {
            escapeAction.performed -= OnEscapePressed;
            escapeAction.Disable();
        }
    }

    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        LoadSceneSelection();
    }

    private void LoadSceneSelection()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}