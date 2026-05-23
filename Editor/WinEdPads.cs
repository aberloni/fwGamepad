using UnityEngine;
using UnityEditor;
using fwp.gamepad;
using UnityEngine.Rendering;

public class WinEdPads : EditorWindow
{
    [MenuItem("Window/Pads/(win) viewer", false, 1)]
    static public void init() => EditorWindow.GetWindow<WinEdPads>();

    void OnEnable()
    {
        fetch();
    }

    GamepadWatcher[] watchers;

    void fetch()
    {
        watchers = FindObjectsByType<GamepadWatcher>();
    }

    void OnGUI()
    {
        if (GUILayout.Button("fetch pads"))
        {
            fetch();
        }

        if (watchers == null || watchers.Length <= 0) return;

        foreach (var w in watchers)
        {
            GUILayout.Label(w.ToString());
            GUILayout.Label(w.stringify());
        }
    }

}
