using System.Text;
using UnityEditor;
using UnityEngine;

namespace Yoziya
{
    public class DebugWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private static StringBuilder logMessages = new StringBuilder();

        [MenuItem("Yoziya/DebugWindow")]
        public static void ShowWindow()
        {
            GetWindow<DebugWindow>("Debug");
            Application.logMessageReceived += LogMessageReceived;
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.LabelField(logMessages.ToString(), GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }

        private static void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            logMessages.AppendLine(condition);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }
    }
}
