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
    public interface IResourceLoader
    {
        Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object;
        Task UnloadAssetAsync<T>(T asset) where T : UnityEngine.Object;
    }

    [Serializable]
    public sealed class ResourceManager
    {
        public ResourceManager() { }
        AsyncOperationHandle<GameObject> handle;

        // 加载资源
        public void LoadAsset()
        {
            handle = Addressables.LoadAssetAsync<GameObject>("myAssetAddress");
            handle.Completed += OnLoadCompleted;
        }

        void OnLoadCompleted(AsyncOperationHandle<GameObject> obj)
        {
            // 资源加载完成后的处理
            GameObject loadedObject = obj.Result;
            //Instantiate(loadedObject);
        }

        // 卸载资源
        public void UnloadAsset()
        {
            Addressables.Release(handle);
        }
        public async Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
        {
            var operation = Addressables.LoadAssetAsync<T>(address);
            await operation.Task;
            return operation.Result;
        }

        public async Task UnloadAssetAsync<T>(T asset) where T : UnityEngine.Object
        {
            //await Addressables.ReleaseAsync(asset);
        }
        public void UnloadAsset<T>(T asset) where T : UnityEngine.Object
        {
            Addressables.Release(asset);
        }

    }
}
