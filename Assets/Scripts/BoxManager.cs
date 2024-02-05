using UnityEngine;
using T_Addressables.Manager;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class BoxManage : MonoBehaviour
{
    public AddressablesManager addressablesManager;

    private void Awake()
    {
        addressablesManager = new AddressablesManager(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            addressablesManager.GetObject("Box");
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            addressablesManager.GetObject("Capsule");
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            addressablesManager.GetObject("Sphere");
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            addressablesManager.ClearAllData("Box");
            addressablesManager.ClearAllData("Capsule");
            addressablesManager.ClearAllData("Sphere");
        }
    }
}
