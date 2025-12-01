using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Management;

public class UIInputSwitcher : MonoBehaviour
{
    public BaseInputModule xrModule;
    public BaseInputModule mouseModule;

    void Awake()
    {
#if UNITY_EDITOR
        // Editor = Trainee → use mouse
        EnableMouseInput();
#else
        // Build (VR) → use XR
        EnableXRInput();
#endif
    }

    void EnableMouseInput()
    {
        if (xrModule) xrModule.enabled = false;
        if (mouseModule) mouseModule.enabled = true;
        Debug.Log("[UIInputSwitcher] Mouse input enabled.");
    }

    void EnableXRInput()
    {
        if (mouseModule) mouseModule.enabled = false;
        if (xrModule) xrModule.enabled = true;
        Debug.Log("[UIInputSwitcher] XR UI input enabled.");
    }
}
