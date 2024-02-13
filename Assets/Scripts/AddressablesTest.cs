using UnityEngine;
using T_Addressables.Manager.Assets;

public class AddressablesTest : MonoBehaviour
{
    public ResourceManager resourceManager;

    private void Awake()
    {
        resourceManager = new ResourceManager();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
        }
    }
}
