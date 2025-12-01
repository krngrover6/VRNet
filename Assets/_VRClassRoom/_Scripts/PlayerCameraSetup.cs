using FishNet.Object;
using UnityEngine;

public class PlayerCameraSetup : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener audioListener;

    public override void OnStartClient()
    {
        base.OnStartClient();

        bool isLocal = IsOwner;   // my player on this machine?

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();

        if (audioListener == null)
            audioListener = GetComponentInChildren<AudioListener>();

        if (playerCamera != null)
            playerCamera.enabled = isLocal;

        if (audioListener != null)
            audioListener.enabled = isLocal;
    }
}