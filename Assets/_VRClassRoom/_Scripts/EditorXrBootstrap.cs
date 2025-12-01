#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class EditorXrBootstrap : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Only start XR if instructor (host)
        if (SignInHandler.Instance != null &&
            SignInHandler.Instance.selectedPlayer != SignInHandler.PlayerType.Instructor)
        {
            Debug.Log("[XRBootstrap] Editor running as TRAINEE so XR is disabled.");
            yield break;
        }

        Debug.Log("[XRBootstrap] Starting XR in Editor (Instructor).");

        var settings = XRGeneralSettings.Instance;
        if (settings == null) yield break;

        var xrManager = settings.Manager;
        if (xrManager == null) yield break;

        yield return xrManager.InitializeLoader();

        if (xrManager.activeLoader == null)
        {
            Debug.LogWarning("Failed to initialize XR loader.");
            yield break;
        }

        xrManager.StartSubsystems();
    }

    private void OnDisable()
    {
        var settings = XRGeneralSettings.Instance;
        if (settings == null) return;

        var xrManager = settings.Manager;
        if (xrManager == null) return;

        xrManager.StopSubsystems();
        xrManager.DeinitializeLoader();
    }
}
#endif
