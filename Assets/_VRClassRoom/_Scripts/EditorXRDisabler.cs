using UnityEngine;
using UnityEngine.XR.Management;

public class EditorXRDisabler : MonoBehaviour
{
    void Awake()
    {
#if UNITY_EDITOR
        // Disable XR completely inside Unity Editor
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("[EditorXRDisabler] XR disabled in Editor.");
#else
        // Enable XR in build
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
#endif
    }
}
