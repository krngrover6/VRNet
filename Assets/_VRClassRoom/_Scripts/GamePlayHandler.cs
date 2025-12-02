using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections;
using UnityEngine;

public class GamePlayHandler : MonoBehaviour
{
    [Header("References")]
    public NetworkManager networkManager;
    public NetworkObject playerPrefab;
    public Transform[] spawnPoints;

    private void Awake()
    {
        if (networkManager == null)
            networkManager = InstanceFinder.NetworkManager;

        // Server events
        networkManager.ServerManager.OnServerConnectionState += OnServerState;
        networkManager.ServerManager.OnRemoteConnectionState += OnRemoteState;

        // Local client events (host included)
       // networkManager.ClientManager.OnClientConnectionState += OnLocalClientState;
    }

    private void OnDestroy()
    {
        // Server events
        networkManager.ServerManager.OnServerConnectionState -= OnServerState;
        networkManager.ServerManager.OnRemoteConnectionState -= OnRemoteState;

        // Local client events (host included)
        //networkManager.ClientManager.OnClientConnectionState -= OnLocalClientState;
    }

    private void Start()
    {
        if (SignInHandler.Instance == null)
        {
            Debug.LogError("SignInHandler not found!");
            return;
        }

        switch (SignInHandler.Instance.selectedPlayer)
        {
            case SignInHandler.PlayerType.Instructor:
                StartHost();
                break;

            case SignInHandler.PlayerType.Trainee:
                StartClient();
                break;

            default:
                Debug.Log("Player type not selected.");
                break;
        }
    }

    // -----------------------------
    // CONNECTION START METHODS
    // -----------------------------

    public void StartHost()
    {
        Debug.Log("Starting Host (Server + Client)...");
        networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
    }

    public void StartClient()
    {
        Debug.Log("Starting Client...");
        networkManager.ClientManager.StartConnection();
    }

    // -----------------------------
    // SERVER CALLBACKS
    // -----------------------------

    private void OnServerState(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState != LocalConnectionState.Started)
            return;

        Debug.Log("Server Started !!");
    }

   

    // ----------------------------
    // HOST CLIENT CONNECTED
    // ----------------------------
    private void OnLocalClientState(ClientConnectionStateArgs args)
    {
        if (!networkManager.IsServerStarted)
            return; // Only spawn host on server mode

        if (args.ConnectionState == LocalConnectionState.Started)
        {
            Debug.Log("HOST client connected → Spawning host");

            NetworkConnection hostConn = networkManager.ServerManager.Clients[0];
            SpawnPlayer(hostConn);
        }
    }

    // ----------------------------
    // REMOTE CLIENT CONNECTED
    // ----------------------------
    private void OnRemoteState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            Debug.Log($"Remote client connected: {conn.ClientId}");
            StartCoroutine(SpawnHostDelayed(conn));
        }
    }

    private IEnumerator SpawnHostDelayed(NetworkConnection con)
    {
        // Wait a few frames for host connection
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(5f);

        if (networkManager.ServerManager.Clients.Count > 0)
        {
            SpawnPlayer(con);
        }
    }



    // -----------------------------
    // PLAYER SPAWNING
    // -----------------------------

    private void SpawnPlayer(NetworkConnection conn)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab Missing!");
            return;
        }
        Debug.Log(conn.IsHost);
        //Transform point = spawnPoints[conn.IsHost ? 0:1];
        Transform point = spawnPoints[conn.ClientId == 0? 0:1];

        NetworkObject player = Instantiate(playerPrefab, point.position, point.rotation);
        var script = player.GetComponent<SynchMaterialColor>();
        if (script != null)
        {
            //if(conn.IsHost)
            if(conn.ClientId == 0)
            {
                player.name = "Instructor";
                script.color.Value = Color.red;
            }
            else
            {
                player.name = "client";
                script.color.Value = Color.blue;

            }
        }
        networkManager.ServerManager.Spawn(player, conn);

        Debug.Log($"Spawned player for ClientId {conn.ClientId}");
    }
}
