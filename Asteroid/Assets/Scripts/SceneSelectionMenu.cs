using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectionMenu : MonoBehaviour
{
    [Header("UI References")]
    private TMP_Dropdown list_box;
    private string selectedScene;
    private float lastClickTime;
    private const float doubleClickThreshold = 0.3f;

    private void Start()
    {
        list_box = GetComponentInChildren<TMP_Dropdown>();
        PopulateSceneList();
    }

    private void PopulateSceneList()
    {
        if (list_box == null)
        {
            Debug.LogError("Dropdown component not found!");
            return;
        }

        // Remove old scenes, if exist
        list_box.options.Clear();

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(
                SceneUtility.GetScenePathByBuildIndex(i));
            list_box.options.Add(new TMP_Dropdown.OptionData(sceneName));
        }
        list_box.RefreshShownValue();
    }
    public void OnLoadScene()
    {
        selectedScene = list_box.options[list_box.value].text;
        if (!string.IsNullOrEmpty(selectedScene))
            SceneManager.LoadScene(selectedScene);
    }
}
