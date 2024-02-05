using System.Dynamic;
using UnityEngine;

namespace T_Addressables.Manager
{
    // Manage Addressable Assets
    // TODO :
    // Instantiate loaded Addressable asset
    // Release loaded Addressable asset

    public class AddressablesManager
    {
        AddressablesHandleManager handleManager;
        public AddressablesManager(GameObject parent)
            => handleManager = parent.AddComponent<AddressablesHandleManager>();

        public void GetObject(string key)
            => handleManager.GetObjectForKeyAsync(key);

        public void GetObjects(string path)
            => handleManager.GetObjectForKeyAsync(path);

        public void ClearAllData(string key)
        {
            handleManager.ReleaseLoadAsset(key);
        }
    }
}
