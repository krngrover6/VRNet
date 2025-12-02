using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class DisableRemoteCamera : NetworkBehaviour
{
    public Camera vrCamera;
    public AudioListener audioListener;
    public GameObject[] xrRigObjects;

    public override void OnStartClient()
    {
        if (!IsOwner)
        {
            if (vrCamera != null) vrCamera.enabled = false;
            if (audioListener != null) audioListener.enabled = false;
            foreach (var obj in xrRigObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}
