#define ENABLE_UNITASK

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace T_Addressables.Manager.Scenes
{
    using System;
    using UnityEngine.AddressableAssets;
#if ENABLE_UNITASK
    using Cysharp.Threading.Tasks;
#else
    using System.Threading.Tasks;
#endif

    // Manage Scenes With Addressables
    // TODO :
    // 1. Load Scene
    // 2. Unload Scene

    public partial class SceneSwitchManager
    {
#region [ Private Property ]
        private Dictionary<string, AsyncOperationHandle?> sceneDataList;
#endregion

#region [ Public Mehtod ]
        public SceneSwitchManager()
        {
            sceneDataList = new Dictionary<string, AsyncOperationHandle?>();
        }
    #if ENABLE_UNITASK
        public async UniTask<bool> LoadSceneAsyncWithAddressables(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            bool isExist = sceneDataList.TryGetValue(sceneName, out var sceneData);

            if(isExist)
            {
                Debug.LogWarning($"[{sceneName}] : Already Exist Scene");
                return false;
            }

            try
            {
                var operationHandle = Addressables.LoadSceneAsync(sceneName, loadSceneMode);
                await operationHandle.ToUniTask();

                if(operationHandle.Status != AsyncOperationStatus.Succeeded)
                    return false;
                
                sceneDataList.Add(sceneName, operationHandle);
            }
            catch(Exception ex)
            {
                Debug.LogError($"[{sceneName}] : Fail to Load Scene \n{ex}");
            }

            return true;
        }

        public async UniTask UnloadSceneAsyncWithAddressables(string sceneName)
        {
            if(sceneDataList.ContainsKey(sceneName) == false)
                return;
            else if(sceneDataList[sceneName] == null)
            {
                // Unload Scene With UnityEngine.SceneManager
                UnloadSceneAsync(sceneName);
                return;
            }

            await Addressables.UnloadSceneAsync((AsyncOperationHandle)sceneDataList[sceneName]).ToUniTask();

            sceneDataList.Remove(sceneName);
        }
    #else
        public async Task<bool> LoadSceneAsyncWithAddressables(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            bool isExist = sceneDataList.TryGetValue(sceneName, out var sceneData);

            if(isExist)
            {
                Debug.LogWarning($"[{sceneName}] : Already Exist Scene");
                return false;
            }

            try
            {
                var operationHandle = Addressables.LoadSceneAsync(sceneName, loadSceneMode);
                await operationHandle.Task;

                if(operationHandle.Status != AsyncOperationStatus.Succeeded)
                    return false;
                
                sceneDataList.Add(sceneName, operationHandle);
            }
            catch(Exception ex)
            {
                Debug.LogError($"[{sceneName}] : Fail to Load Scene \n{ex}");
            }

            return true;
        }

        public async Task UnloadSceneAsyncWithAddressables(string sceneName)
        {
            if(sceneDataList.ContainsKey(sceneName) == false)
                return;
            else if(sceneDataList[sceneName] == null)
            {
                // Unload Scene With UnityEngine.SceneManager
                UnloadSceneAsync(sceneName);
                return;
            }

            await Addressables.UnloadSceneAsync((AsyncOperationHandle)sceneDataList[sceneName]).Task;

            sceneDataList.Remove(sceneName);
        }
    #endif
#endregion
    }
}