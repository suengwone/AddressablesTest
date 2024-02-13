using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace T_Addressables.Manager.Assets
{
    // Manage Addressable asset handles
    // TODO :
    // 1. Set Handle for key
    // 2. Get Handle's object for key
    // 3. Release Handle for Request

    public class AddressablesHandleManager
    {
#region [ Public Property ]
        public readonly string Label;
        public readonly string ObjectName;
        public readonly Type ObjectType;
        public readonly AsyncOperationHandle Handle;
        public List<GameObject> instances;
#endregion

#region [ Public Method ]
        public AddressablesHandleManager(string label, string objectName, Type type, AsyncOperationHandle handle)
        {
            Label = label;
            ObjectName = objectName;
            ObjectType = type;
            Handle = handle;

            if(ObjectType == typeof(GameObject))
            {
                instances = new List<GameObject>();
            }
        }
        
        public void Release(bool isReleaseHandle)
        {
            if(instances != null && instances.Count > 0)
            {
                foreach(var instance in instances)
                    Release(instance);
            }

            if(isReleaseHandle == true)
                Addressables.Release(Handle);
        }

        public void Release(GameObject targetObject = null)
        {
            // Check Empty List
            if(instances.Count == 0)
            {
                Debug.Log("Instancing List is Empty");
                return;
            }

            // For Default Option
            targetObject ??= instances.Last();

            // Defend for Unexpected Issue
            try
            {
                if(instances.Remove(targetObject))
                    Addressables.ReleaseInstance(targetObject);
                else
                    Debug.LogWarning($"No Target Object({targetObject} in Instancing List)");
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
        }
#endregion
    }
}
