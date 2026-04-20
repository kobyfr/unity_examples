using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorController))]
public class DoorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DoorController controller = (DoorController)target;

        if (GUILayout.Button("Next State"))
        {
            Undo.RecordObject(controller, "Next Door State");
            controller.NextState();
            EditorUtility.SetDirty(controller);
        }

        if (GUILayout.Button("Play Closed Sound"))
        {
            var audioSource = controller.GetComponent<AudioSource>();
            var closedClip = controller.audioSettings != null ? controller.audioSettings.door_closed_audio : null;
            if (audioSource != null && closedClip != null)
            {
                audioSource.PlayOneShot(closedClip);
            }
            else
            {
                Debug.LogWarning("Missing AudioSource or Closed AudioClip on DoorController.");
            }
        }
    }
}
