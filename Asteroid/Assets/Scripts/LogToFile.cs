using System;
using System.IO;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEngine.Rendering.GPUSort;

// Simple singleton runtime logger that writes to a file in Application.persistentDataPath
public class LogToFile : MonoBehaviour
{
    private static LogToFile _instance;
    public static LogToFile Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find existing instance using the newer API to avoid obsolete warnings.
                _instance = UnityEngine.Object.FindAnyObjectByType<LogToFile>();
                if (_instance == null)
                {
                    var go = new GameObject("_RuntimeLogger");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<LogToFile>();
                }
            }
            return _instance;
        }
    }

    private StreamWriter writer;
    private string output_file_path;
    private readonly object writeLock = new object();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        // If running in the Editor and the game is playing (game view), route logs to the IDE/Console
        // and do not write to the persistent file. In all other cases enable file logging.
        bool routeToEditorConsole = Application.isEditor && Application.isPlaying;

        if (!routeToEditorConsole)
        {
            try
            {
                // Prefer executable folder for standalone builds so the log sits next to the .exe.
                // Application.dataPath points to "<exe>_Data"; the exe folder is its parent.
                string output_dir;
                try
                {
                    var dataPath = Application.dataPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    output_dir = Path.GetDirectoryName(dataPath);
                    if (string.IsNullOrEmpty(output_dir))
                        output_dir = Application.persistentDataPath;
                }
                catch
                {
                    output_dir = Application.persistentDataPath;
                }

                output_file_path = Path.Combine(output_dir, "game_input_log.txt");
                Directory.CreateDirectory(output_dir);
                writer = new StreamWriter(output_file_path, true) { AutoFlush = true };

                // Subscribe to Unity log messages so all Debug.Log / Debug.LogError / etc are captured.
                Application.logMessageReceivedThreaded += HandleLogToFile;

                lock (writeLock)
                {
                    writer.WriteLine($"Session start: {DateTime.Now}");
                }
            }
            catch (Exception e)
            {
                // Avoid using Debug.Log here to prevent recursion into the handler.
                try { lock (writeLock) { writer?.WriteLine($"RuntimeLogger failed to open log file: {e.Message}"); } } catch { }
            }
        }
    }

    private void OnApplicationQuit()
    {
        try
        {
            // Unsubscribe before closing to avoid handler running while writer is closed
            Application.logMessageReceivedThreaded -= HandleLogToFile;
            lock (writeLock)
            {
                writer?.WriteLine($"Session end: {DateTime.Now}");
                writer?.Close();
                writer = null;
            }
        }
        catch (Exception) { }
    }

    public static void Log(int n)
    {
        LogFormat("{0}", n);
    }
    public static void Log(float f)
    {
        LogFormat("{0}", f);
    }

    public static void Log(Vector2 v2)
    {
        LogFormat("{0}", v2);
    }

    public static void Log(Vector3 v3)
    {
        LogFormat("{0}", v3);
    }

    public static void Log(string message)
    {
        try
        {
            // If running in the Editor in Play mode, forward to the Unity Console so IDE receives it.
            if (Application.isEditor && Application.isPlaying)
            {
                Debug.Log(message);
            }
            else
            {
                lock (Instance.writeLock) { Instance.writer?.WriteLine(message); }
            }
        }
        catch (Exception) { }
    }

    public static void LogFormat(string fmt, params object[] args)
    {
        var msg = string.Format(fmt, args);
        Log(msg);
    }

    private void HandleLogToFile(string condition, string stackTrace, LogType type)
    {
        try
        {
            string line = $"{DateTime.Now:O}\t{type}\t{condition}";
            if (type == LogType.Exception || type == LogType.Error)
                line += $"\n{stackTrace}";
            lock (writeLock)
            {
                writer?.WriteLine(line);
            }
        }
        catch { }
    }
}

