using UnityEngine;

public class StartSceneCameraController : MonoBehaviour
{
    public Camera editorCamera;
    public Camera xrCamera;

    void Awake()
    {
        bool isVRBuild =
#if UNITY_EDITOR
            false;
#else
            true;
#endif

        if (isVRBuild)
        {
            // Build → VR mode
            if (editorCamera) editorCamera.enabled = false;
            if (xrCamera) xrCamera.enabled = true;
        }
        else
        {
            // Editor → Desktop mode
            if (editorCamera) editorCamera.enabled = true;
            if (xrCamera) xrCamera.enabled = false;
        }
    }
}
