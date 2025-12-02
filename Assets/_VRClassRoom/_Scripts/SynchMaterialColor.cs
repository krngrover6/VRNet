using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;


public class SynchMaterialColor : NetworkBehaviour
{
    public readonly SyncVar<Color> color = new SyncVar<Color>();
    [SerializeField] MeshRenderer meshRenderer;

    void Awake()
    {
        color.OnChange += OnColorChanged;
    }

    private void OnColorChanged(Color previous, Color next, bool asServer)
    {
        meshRenderer.material.color = color.Value;
    }
}