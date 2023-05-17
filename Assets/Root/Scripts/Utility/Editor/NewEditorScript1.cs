using UnityEngine;
using UnityEditor;
using System.IO;

public class CodeGenerator : MonoBehaviour
{
    [MenuItem("Yoziya/生成代码")]
    static void CreateNewScript()
    {
        // 设置要生成的代码
        string scriptCode =
@"using UnityEngine;

public class NewScript : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}";
        // 设置生成的脚本路径
        string path = Application.dataPath + "/Scripts/NewScript.cs";

        // 如果脚本已存在，不进行覆盖
        if (File.Exists(path))
        {
            Debug.LogError("The file already exists.");
            return;
        }

        // 写入新的脚本文件
        File.WriteAllText(path, scriptCode);

        // 刷新Unity编辑器，使新脚本在编辑器中可见
        AssetDatabase.Refresh();
    }
}
