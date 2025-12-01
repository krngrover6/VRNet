using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using System;
using UnityEngine;

public class GamePlayHandler : MonoBehaviour
{
    public NetworkObject Playerprefab;
    public Transform[] spawnPoint;
   

    [SerializeField] NetworkManager _networkManager;
    private LocalConnectionState _serverState = LocalConnectionState.Stopped;
    private LocalConnectionState _clientState = LocalConnectionState.Stopped;



    private void Awake()
    {
        _networkManager.ServerManager.OnServerConnectionState += HandleServerState;

        //_networkManager.ServerManager.OnRemoteConnectionState += HandleClientState;
         _networkManager.ClientManager.OnClientConnectionState += HandleClientState;

       


    }

    

    private void HandleClientState(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            Debug.Log("REMOTE CLIENT CONNECTED: " + connection.ClientId);
            SpawnPlayer(connection);  // conn is VALID here
        }
    }

    private void OnDestroy()
    {
        _networkManager.ServerManager.OnServerConnectionState -= HandleServerState;
        //_networkManager.ServerManager.OnRemoteConnectionState -= HandleClientState;
        _networkManager.ClientManager.OnClientConnectionState -= HandleClientState;
    }

    private void HandleClientState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            Debug.Log("Client connected: ");

            SpawnPlayer(null);
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            Debug.Log("Client DISconnected: ");
        }
    }

    private void HandleServerState(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            Debug.Log("Server started successfully!");
            SpawnPlayer(null);
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            Debug.Log("Server stopped.");
        }
    }

    private void Start()
    {
        if (SignInHandler.Instance != null)
        {
            if(SignInHandler.Instance.selectedPlayer == SignInHandler.PlayerType.Instructor)
            {
                OnClick_Server();
            }

            else if (SignInHandler.Instance.selectedPlayer == SignInHandler.PlayerType.Trainee)
            {
                OnClick_Client();
            }

            else
            {
                Debug.Log(" PlayerType not selected");
            }

        }
        else
        {
            Debug.Log(" SignInHandler Instance not found");
        }
    }

    private void OnEnable()
    {
       
       
       
    }

    

    

    


    public void OnClick_Server()
    {

        Debug.Log(" OnclickServer");
        if (_networkManager == null)
            return;
        Debug.Log(" OnclickServer1");

        if (_serverState != LocalConnectionState.Stopped)
            _networkManager.ServerManager.StopConnection(true);
        else
        { 
            _networkManager.ServerManager.StartConnection();
           
        }




        
    }

    
    public void OnClick_Client()
    {
        Debug.Log(" OnclickClient");
        if (_networkManager == null)
            return;
        Debug.Log(" OnclickClient1");
        if (_clientState != LocalConnectionState.Stopped)
            _networkManager.ClientManager.StopConnection();
        else
        { 
            _networkManager.ClientManager.StartConnection();
           
        }
            

        
    }

    

    private void SpawnPlayer(NetworkConnection conn)
    {
        Debug.Log("Spawning the player)");
       
        if (Playerprefab == null)
        {
            NetworkManagerExtensions.LogWarning($"Player prefab is empty and cannot be spawned for connection {conn.ClientId}.");
            return;
        }
        Transform tran = spawnPoint[UnityEngine.Random.Range(0, spawnPoint.Length)];
        
        

        //NetworkObject nob = _networkManager.GetPooledInstantiated(Playerprefab, tran.position, tran.rotation, true);
        NetworkObject nob = Instantiate(Playerprefab, tran.position, tran.rotation);
        _networkManager.ServerManager.Spawn(nob, conn);

       
    }

    


}
