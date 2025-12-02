using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    private Vector2 moveInput;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Called by PlayerInput -> "Move"
    public void OnMove(InputValue value)
    {
        if (!IsOwner)
            return;

        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if (controller == null || !controller.enabled)
            return;

        // Convert input to world-space movement
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDir.sqrMagnitude > 1f)
            moveDir.Normalize();

        // Apply movement through CharacterController
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }
}
