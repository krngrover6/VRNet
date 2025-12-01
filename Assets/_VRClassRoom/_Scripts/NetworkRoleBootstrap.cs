using UnityEngine;
using System.Collections;
using FishNet.Managing;

public class NetworkRoleBootstrap : MonoBehaviour
{
    public NetworkManager networkManager;

    private const ushort PORT = 7770;

    private void Awake()
    {
        if (networkManager == null)
            networkManager = FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {
        StartCoroutine(StartNetworkAfterXR());
    }

    private IEnumerator StartNetworkAfterXR()
    {
        // Give XR time to initialize (required for VR builds)
        yield return new WaitForSeconds(1.0f);

        var handler = SignInHandler.Instance;

        if (handler == null)
        {
            Debug.LogWarning("No SignInHandler found. Starting as HOST by default.");
            StartHost();
            yield break;
        }

        if (handler.selectedPlayer == SignInHandler.PlayerType.Instructor)
        {
            Debug.Log("[Bootstrap] Instructor → HOST");
            StartHost();
        }
        else
        {
            Debug.Log("[Bootstrap] Trainee → CLIENT");
            StartClient();
        }
    }

    // ===========================
    //      HOST = SERVER + CLIENT
    // ===========================
    private void StartHost()
    {
        // Start the server on port 7770
        networkManager.ServerManager.StartConnection(PORT);

        // Connect local client to server
        networkManager.ClientManager.StartConnection("localhost", PORT);

        Debug.Log("[FishNet] HOST started on port " + PORT);
    }

    // ===========================
    //          CLIENT ONLY
    // ===========================
    private void StartClient()
    {
        networkManager.ClientManager.StartConnection("localhost", PORT);

        Debug.Log("[FishNet] CLIENT ONLY started on port " + PORT);
    }
}
