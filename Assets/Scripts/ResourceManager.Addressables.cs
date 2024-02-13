#define ENABLE_UNITASK

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace T_Addressables.Manager.Assets
{
#if ENABLE_UNITASK
    using Cysharp.Threading.Tasks;
#else
    using System.Threading.Tasks;
#endif

    // Manage Addressable Assets
    // TODO :
    // Load Addressable asset
    // Instantiate loaded Addressable asset
    // Release loaded Addressable asset

    public partial class ResourceManager
    {
#region [ Private Property ]
        private Dictionary<string, AddressablesHandleManager> addressablesAssetList;
#endregion

#region [ Public Method ]
        public ResourceManager()
        {
            addressablesAssetList = new Dictionary<string,AddressablesHandleManager>();
        }
    #if ENABLE_UNITASK
        public async UniTask<(bool isSuccess, T output)> LoadAssetAsyncWithAddressables<T>(string assetName, bool isInstancing = true)
            where T : UnityEngine.Object
        {
            var isExist = addressablesAssetList.TryGetValue(assetName, out var handleData);

            (bool isSuccess, T output) result = (false, null);

            if(isExist == false)
            {
                var operationHandle = Addressables.LoadAssetAsync<T>(assetName);

                var output = await operationHandle.ToUniTask();
                
                if(operationHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    handleData = new AddressablesHandleManager(
                        label : assetName,
                        objectName : output.name,
                        type : output.GetType(),
                        handle : operationHandle
                    );

                    addressablesAssetList.Add(assetName, handleData);
                }
                else
                {
                    Debug.LogWarning($"Asset Loading Fail ({assetName})");
                    return result;
                }
            }
            else
            {
                result = (true, handleData.Handle.Result as T);
            }

            if(handleData.ObjectType == typeof(GameObject) && isInstancing == true)
            {
                var instancingResult = await InstantiateAsyncWithAddressables(handleData);
                result = (instancingResult.isSuccess, instancingResult.output as T);
            }

            return result;
        }
    #else
        public async Task<(bool isSuccess, T output)> LoadAssetAsyncWithAddressables<T>(string assetName, bool isInstancing = true)
            where T : UnityEngine.Object
        {
            var isExist = addressablesAssetList.TryGetValue(assetName, out var handleData);

            (bool isSuccess, T output) result = (false, null);

            if(isExist == false)
            {
                var operationHandle = Addressables.LoadAssetAsync<T>(assetName);

                var output = await operationHandle.Task;
                
                if(operationHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    handleData = new AddressablesHandleManager(
                        label : assetName,
                        objectName : output.name,
                        type : output.GetType(),
                        handle : operationHandle
                    );

                    addressablesAssetList.Add(assetName, handleData);
                }
                else
                {
                    Debug.LogWarning($"Asset Loading Fail ({assetName})");
                    return result;
                }
            }
            else
            {
                result = (true, handleData.Handle.Result as T);
            }

            if(handleData.ObjectType == typeof(GameObject) && isInstancing == true)
            {
                var instancingResult = await InstantiateAsyncWithAddressables(handleData);
                result = (instancingResult.isSuccess, instancingResult.output as T);
            }

            return result;
        }
    #endif

        public void Release(string assetName)
            => addressablesAssetList[assetName]?.Release();
        public void Release(string assetName, GameObject targetInstance)
            => addressablesAssetList[assetName]?.Release(targetInstance);
        public void ReleaseAll(string assetName)
            => addressablesAssetList[assetName]?.Release(true);
#endregion

#region [ Private Method ]
    #if ENABLE_UNITASK
        private async UniTask<(bool isSuccess, GameObject output)> InstantiateAsyncWithAddressables(AddressablesHandleManager handleData)
        {
            (bool isSuccess, GameObject output) result = (false, null);

            if(handleData != null)
            {
                var instancingHandle = Addressables.InstantiateAsync(handleData.Label);
                var output = await instancingHandle.ToUniTask();

                if(instancingHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    output.name = output.name.Replace("(Clone)", "");
                    handleData.instances.Add(output);
                    result = (true, output);
                }
                else
                {
                    Debug.LogWarning($"Asset Instancing Fail ({handleData.Label})");
                }
            }

            return result;
        }
    #else
        private async Task<(bool isSuccess, GameObject output)> InstantiateAsyncWithAddressables(AddressablesHandleManager handleData)
        {
            (bool isSuccess, GameObject output) result = (false, null);

            if(handleData != null)
            {
                var instancingHandle = Addressables.InstantiateAsync(handleData.Label);
                var output = await instancingHandle.Task;

                if(instancingHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    output.name = output.name.Replace("(Clone)", "");
                    handleData.instances.Add(output);
                    result = (true, output);
                }
                else
                {
                    Debug.LogWarning($"Asset Instancing Fail ({handleData.Label})");
                }
            }

            return result;
        }
    #endif
#endregion
    }
}
