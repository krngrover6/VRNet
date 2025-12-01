using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Management;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class CanvasRaycasterSwitcher : MonoBehaviour
{
    public GraphicRaycaster mouseRaycaster;
    public TrackedDeviceGraphicRaycaster xrRaycaster;

    void Awake()
    {
#if UNITY_EDITOR
        mouseRaycaster.enabled = true;
        xrRaycaster.enabled = false;
#else
        mouseRaycaster.enabled = false;
        xrRaycaster.enabled = true;
#endif
    }
}
