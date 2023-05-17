using UnityEditor;
using UnityEngine;

public class YoziyaDebugger : EditorWindow
{
    string debugText = "Hello, Debug!";

    [MenuItem("Yoziya/Custom Debugger")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(YoziyaDebugger));
    }

    void OnGUI()
    {
        debugText = EditorGUILayout.TextField("Debug Text", debugText);
        debugText = EditorGUILayout.TextField(typeof(YoziyaDebugger).ToString(), debugText);

        if (GUILayout.Button("Print Debug Text"))
        {
            Debug.Log(debugText);
            Debug.Log(typeof(YoziyaDebugger));
        }
    }
}
