using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;

namespace Yoziya
{
    public static class UnityExtend
    {
        public static T Get<T>()
        {
            return default;
        }
        public static void DestoryAll(this GameObject parent)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject.Destroy(parent.transform.GetChild(i));
            }
        }
        public static void Load(this GameObject parent, string address)
        {
            //Addressables.InstantiateAsync(address).Completed += handle =>
            //{
            //    if (handle.Status == AsyncOperationStatus.Succeeded)
            //    {
            //        GameObject prefab = handle.Result;
            //        prefab.transform.SetParent(parent.transform,false);
            //    }
            //    else
            //    {
            //        Debug.LogError($"没有{address}资源");
            //    }
            //};
        }
    }
}
