using UnityEngine;

public class CanvasCameraSwitcher : MonoBehaviour
{
    public Camera editorCamera;
    public Camera xrCamera;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

#if UNITY_EDITOR
        // Editor ⇒ Mouse mode ⇒ use editor camera
        if (editorCamera != null)
        {
            canvas.worldCamera = editorCamera;
            Debug.Log("[CanvasCameraSwitcher] Using EDITOR camera for Canvas.");
        }
#else
        // Build ⇒ VR mode ⇒ use XR camera
        if (xrCamera != null)
        {
            canvas.worldCamera = xrCamera;
            Debug.Log("[CanvasCameraSwitcher] Using XR camera for Canvas.");
        }
#endif
    }
}
