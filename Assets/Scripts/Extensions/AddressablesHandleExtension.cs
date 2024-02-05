using System;
using System.Collections;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
    public static class AddressablesHandleExtension
    {
        public static void GetResultLazy<T>(this AsyncOperationHandle<T> handle, MonoBehaviour context, Action<T> onComplete)
        {
            IEnumerator GetWaiter(Action<T> onComplete)
            {
                while(handle.IsDone != true)
                {
                    yield return null;
                }

                onComplete((T)handle.Result);
            }

            context.StartCoroutine(GetWaiter(onComplete));
        }
    }
}
