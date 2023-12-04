using UnityEditor;
using UnityEngine;

namespace Yoziya
{
    public class UnitTest : ScriptableObject
    {
        [MenuItem("Yoziya/单元测试")]
        static void DoIt()
        {
            PlayerPrefsData<string> a = new PlayerPrefsData<string>("",new StringSerializer());
            a.Data = "adawdwdaw";
            Debug.Log(a.Data);
        }
    }
}