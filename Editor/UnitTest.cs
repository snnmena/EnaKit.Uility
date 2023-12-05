using EnaKit.Utility;
using UnityEditor;
using UnityEngine;

namespace EnaKit.Uility
{
    public class UnitTest : ScriptableObject
    {
        [MenuItem("EnaKit/Uility/Run")]
        private static void Run()
        {
            Debug.Log(100400010.ToChinese());
        }
    }
}