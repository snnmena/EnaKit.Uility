using UnityEditor;
using UnityEngine;
using System.IO;

namespace Yoziya
{
    public class Auto : EditorWindow
    {
        string className = "MyClass";
        string directory = "Assets/";

        [MenuItem("Yoziya/Auto/自动生成新脚本")]
        static void Init()
        {
            Auto window = (Auto)GetWindow(typeof(Auto));
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("新脚本设置", EditorStyles.boldLabel);
            className = EditorGUILayout.TextField("类名", className);
            directory = EditorGUILayout.TextField("路径", directory);

            if (GUILayout.Button("生成新脚本"))
            {
                string scriptContent =
    @"using UnityEngine;
public class " + className + @" : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }
}";

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(directory + "/" + className + ".cs", scriptContent);
                AssetDatabase.Refresh();

                Debug.Log("成功创建新脚本: " + className + ".cs");
            }
        }
    }
}