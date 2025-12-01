using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCubeCreator : NetworkBehaviour
{
    public NetworkObject cubePrefab;

    public override void OnStartClient()
    {
        if (IsOwner)
            GetComponent<PlayerInput>().enabled = true;
    }
    
    // this is for dealing with Unity Events
    /*public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
            SpawnCube();
    }*/
    
    public void OnFire(InputValue value)
    {
        if (!value.isPressed)
            return;
        
        SpawnCube();
    }

    // We are using a ServerRpc here because the Server needs to do all network object spawning.
    [ServerRpc]
    private void SpawnCube()
    {
        NetworkObject obj = Instantiate(cubePrefab, transform.position, Quaternion.identity);
        Spawn(obj); // NetworkBehaviour shortcut for ServerManager.Spawn(obj);
    }
}