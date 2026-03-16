// 1/11/2026 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class GlobalSceneLoader : MonoBehaviour
{
    [Header("Scene Name")]
    [SerializeField] private string sceneToLoad = "scene_selection";

    public static GlobalSceneLoader instance;
    private InputAction escapeAction;
    private InputAction next_scene_action;
    private InputAction prev_scene_action;
    private GameInfoDisplay gameInfoDisplay;

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

            // Create an input action for scene management
            next_scene_action = new InputAction(
                name: "NextScene",
                binding: "<keyboard>/n"
            );
            prev_scene_action = new InputAction(
                name: "PrevScene",
                binding: "<keyboard>/p"
            );

            gameInfoDisplay = FindAnyObjectByType<GameInfoDisplay>();
            gameInfoDisplay.update_scene_name(sceneToLoad);
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
        if (next_scene_action != null)
        {
            next_scene_action.performed += OnNextScenePressed;
            next_scene_action.Enable();
        }
        if (prev_scene_action != null)
        {
            prev_scene_action.performed += OnPrevScenePressed;
            prev_scene_action.Enable();
        }
    }

    private void OnDisable()
    {
        if (escapeAction != null)
        {
            escapeAction.performed -= OnEscapePressed;
            escapeAction.Disable();
        }
        if ( next_scene_action != null)
        {
            next_scene_action.performed -= OnNextScenePressed;
            next_scene_action.Disable();
        }
        if (prev_scene_action != null)
        {
            prev_scene_action.performed -= OnPrevScenePressed;
            prev_scene_action.Disable();
        }
    }

    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        LoadSceneSelectionScene();
    }
    private void OnNextScenePressed(InputAction.CallbackContext context)
    {
        LoadRelativeScene(+1);
    }

    private void OnPrevScenePressed(InputAction.CallbackContext context)
    {
        LoadRelativeScene(-1);
    }

    private void LoadSceneSelectionScene()
    {
        SceneManager.LoadScene(sceneToLoad);
        gameInfoDisplay.update_scene_name(sceneToLoad);
        ReleaseMouse();
    }

    public void LoadScene(int scene_index)
    {
        SceneManager.LoadScene(scene_index);
        string scene_name = SceneManager.GetSceneByBuildIndex(scene_index).name;
        gameInfoDisplay.update_scene_name(string.Format("{0} : {1}", scene_index, scene_name));
        if (scene_name != "scene_selection")
        {
            CaptureMouse();
        }
        else
        {
            ReleaseMouse();
        }
    }
    private void LoadRelativeScene(int offset)
    {
        int scene_to_load_index = (SceneManager.GetActiveScene().buildIndex + offset + SceneManager.sceneCountInBuildSettings) % 
                                   SceneManager.sceneCountInBuildSettings;
        LoadScene(scene_to_load_index);
    }

    void ReleaseMouse()
    {
        Debug.Log("Releasing mouse");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CaptureMouse()
    {
        Debug.Log("Capturing mouse");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}