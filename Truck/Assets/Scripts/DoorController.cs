using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [System.Serializable]
    public class MovementSettings
    {
        public Transform hinge_to_rotate_about;
        public float open_angle = 90;
        public float closed_angle = 0;
        public float rotation_speed = 50;
        public bool initially_closed = true;
    }

    [System.Serializable]
    public class AudioSettings
    {
        public AudioClip door_open_audio;
        public AudioClip door_closed_audio;
    }

    [SerializeField] MovementSettings movement = new MovementSettings();
    [SerializeField] public AudioSettings audioSettings = new AudioSettings();

    DoorState state = DoorState.Closed;
    float initial_y_rotation = 0;
    AudioSource audio_source;

    enum DoorState
    {
        Open,
        Closed,
        Opening,
        Closing
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        if (audio_source == null)
            audio_source = gameObject.AddComponent<AudioSource>();

        if (movement.initially_closed)
            initial_y_rotation = transform.eulerAngles.y;
        else
            initial_y_rotation = transform.eulerAngles.y + movement.open_angle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Depending on the state of the door, we will rotate it towards the open or closed position.
        switch (state)
        {

            case DoorState.Open:
                break;
            case DoorState.Closed:
                break;
            case DoorState.Opening:
                // read actual Y rotation of the door from the Transform
                if ((transform.eulerAngles.y - initial_y_rotation) >= movement.open_angle)
                    state = DoorState.Open;
                else
                    transform.RotateAround(movement.hinge_to_rotate_about.position, Vector3.up, movement.rotation_speed * Time.deltaTime);
                break;
            case DoorState.Closing:
                // read actual Y rotation of the door from the Transform
                if ((transform.eulerAngles.y - initial_y_rotation) <= movement.closed_angle)
                {
                    state = DoorState.Closed;
                    PlaySound();
                }
                else
                    transform.RotateAround(movement.hinge_to_rotate_about.position, Vector3.up, -movement.rotation_speed * Time.deltaTime);
                break;
        }
    }

    private void PlaySound()
    {
        if (audio_source == null) { return;  }

        switch (state)
        {  
            case DoorState.Opening:
                if (audioSettings.door_open_audio)
                    audio_source.PlayOneShot(audioSettings.door_open_audio);
                break;
           case DoorState.Closed:
                if (audioSettings.door_closed_audio)
                    audio_source.PlayOneShot(audioSettings.door_closed_audio);
                break;
        }
    }

    // cycle to the next DoorState (callable from editor button)
    public void NextState()
    {
        DoorState previous_state = state;
        switch (state)
        {
            case DoorState.Open:
                state = DoorState.Closing;
                break;
            case DoorState.Closed:
                state = DoorState.Opening;
                PlaySound();
                break;
            case DoorState.Opening:
                state = DoorState.Closing;
                break;
            case DoorState.Closing:
                state = DoorState.Opening;
                break;
        }
    }
}
