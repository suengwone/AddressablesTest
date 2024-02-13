using Cysharp.Threading.Tasks;
using UnityEngine;

namespace T_Addressables.Manager.Assets
{
    public partial class ResourceManager
    {
#region [ Public Mehotd ]
        public async UniTask<T> LoadAssetAsync<T> (string assetName)
            where T : UnityEngine.Object
            => await Resources.LoadAsync<T>(assetName).ToUniTask() as T;
#endregion
    }
}
