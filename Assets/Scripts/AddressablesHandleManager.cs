using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace T_Addressables.Manager
{
    // Manage Addressable asset handles
    // TODO :
    // 1. Set Handle for key
    // 2. Get Handle's object for key
    // 3. Release Handle for Request

    public class AddressablesHandleManager : MonoBehaviour
    {
        public AddressablesHandleManager()
        {
            handles = new();
        }

        private class AddressableHandleData
        {
            public string label;
            public string objectName;
            public List<object> instances;
            public AsyncOperationHandle handle;

            public bool Release()
            {
                try
                {
                    foreach(var instance in this.instances)
                        Addressables.ReleaseInstance(instance as GameObject);
                        
                    Addressables.Release(handle);
                }
                catch(Exception ex)
                {
                    Debug.LogException(ex);
                    return false;
                }
                
                return true;
            }
        }

        private Dictionary<string, AddressableHandleData> handles;



#region [Private Method]
        private void SetObjectForKey(string key)
        {
            var handle = Addressables.LoadAssetAsync<object>(key);

            handle.Completed += inputHandle => 
            {
                var loadedObject = inputHandle.Result;
                if(handles.ContainsKey(key) == false)
                {
                    handles[key] = new AddressableHandleData()
                    {
                        label = key,
                        objectName = nameof(loadedObject).Split('_')[0],
                        instances = new(),
                        handle = inputHandle,
                    };
                }

                InstantiateWithHandle(handles[key]);
            };
        }

        private void InstantiateWithHandle(AddressableHandleData handleData)
        {
            var instancingHandle = Addressables.InstantiateAsync(handleData.label);

            instancingHandle.Completed += handle =>
            {
                handleData.instances.Add(handle.Result);
            };
        }
#endregion

#region [Public Method]
        public void GetObjectForKeyAsync(string key)
        {
            var isExist = handles.TryGetValue(key, out AddressableHandleData handleData);
        
            if(isExist == false)
            {
                SetObjectForKey(key);
            }
            else
            {
                InstantiateWithHandle(handles[key]);
            }
        }

        public void GetObjectsForPathAsync(string groupName)
        {
            
        }

        public void ReleaseLoadAsset(string targetObjectName)
        {
            if(handles.ContainsKey(targetObjectName) == false)
                return;

            handles[targetObjectName].Release();

            handles.Remove(targetObjectName);
        }
#endregion
    }
}
