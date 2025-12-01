using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
   

    public Renderer cubeRenderer;
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("View (assign in Inspector)")]
    [SerializeField] private PlayerInput playerInput;    // PlayerInput on Player

    private Vector2 moveInput;

   

    //new
    private void Start()
    {
        if (SignInHandler.Instance.selectedPlayer == SignInHandler.PlayerType.Trainee)
        {
            ChangeColorServerRpc(Color.blue);
            Debug.Log("Color change to blue");
        }


        else
        {
            ChangeColorServerRpc(Color.red);
            Debug.Log("Color change to red");
        }
    }

    

    [ServerRpc]
    public void ChangeColorServerRpc(Color newColor)
    {
        // Server sends to all clients
        UpdateColorObserversRpc(newColor);
        Debug.Log("1");
    }

    // Server → All Clients
    [ObserversRpc]
    void UpdateColorObserversRpc(Color newColor)
    {
        cubeRenderer.material.color = newColor;
        Debug.Log("2");
    }


    

    

    // Called by PlayerInput -> "Move" action (Send Messages)
    public void OnMove(InputValue value)
    {
        // Only the local trainee should drive movement
        if (!IsOwner)
            return;

        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        // Only the local trainee moves this object
        if (!IsOwner)
            return;

        Vector3 dir = new Vector3(moveInput.x, 0f, moveInput.y);
        if (dir.sqrMagnitude > 1f)
            dir.Normalize();

        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}