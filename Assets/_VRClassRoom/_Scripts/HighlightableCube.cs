using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class HighlightableCube : NetworkBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material highlightedMaterial;

    private Renderer _renderer;

    // This bool only lives on the server; we push changes via RPC.
    private bool _isHighlighted;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // When a client joins late, make sure it sees the correct state.
        ApplyMaterial(_isHighlighted);
    }

    [Server]  // Only the server is allowed to change highlight state
    public void SetHighlighted(bool highlighted)
    {
        if (_isHighlighted == highlighted)
            return;

        _isHighlighted = highlighted;

        // Update visuals on the server
        ApplyMaterial(highlighted);

        // Tell all observers (all clients) to update visuals too
        RpcSetHighlighted(highlighted);
    }

    [ObserversRpc(BufferLast = true)]
    private void RpcSetHighlighted(bool highlighted)
    {
        // Server already updated its renderer; ignore there
        if (IsServer)
            return;

        _isHighlighted = highlighted;
        ApplyMaterial(highlighted);
    }

    private void ApplyMaterial(bool highlighted)
    {
        if (_renderer == null)
            return;

        _renderer.material = highlighted ? highlightedMaterial : normalMaterial;
    }
}