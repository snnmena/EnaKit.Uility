using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Yoziya
{
    public class ResourceManager
    {
        static public async Task<T> Load<T>(string path)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError("Failed to load " + typeof(T).Name + ":" + path);
                return default;
            }
        }
    }
}
