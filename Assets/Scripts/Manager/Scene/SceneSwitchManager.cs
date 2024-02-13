#define ENABLE_UNITASK

using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace T_Addressables.Manager.Scenes
{
    public partial class SceneSwitchManager
    {
#region [ Public Method ]
        public async UniTask<bool> LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            bool isExist = sceneDataList.TryGetValue(sceneName, out var sceneData);

            if(isExist)
            {
                Debug.LogWarning($"[{sceneName}] : Already Exist Scene");
                return false;
            }

            await SceneManager.LoadSceneAsync(sceneName, loadSceneMode).ToUniTask();
            sceneDataList.Add(sceneName, null);
            
            return true;
        }

        public async UniTask<bool> LoadSceneAsync(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
            => await LoadSceneAsync(SceneManager.GetSceneByBuildIndex(sceneIndex).name, loadSceneMode);
    
        public async UniTask UnloadSceneAsync(string sceneName)
        {
            if(sceneDataList.ContainsKey(sceneName) == false)
                return;
            else if(sceneDataList[sceneName] != null)
            {
                UnloadSceneAsyncWithAddressables(sceneName);
                return;
            }

            await SceneManager.UnloadSceneAsync(sceneName).ToUniTask();
            sceneDataList.Remove(sceneName);
        }
#endregion
    }
}