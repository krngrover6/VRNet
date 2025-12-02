using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class HighlightableCube : NetworkBehaviour
{
    public readonly SyncVar<Color> color = new SyncVar<Color>();
    [SerializeField] private MeshRenderer meshRenderer;

    private void Awake()
    {
        color.OnChange += OnColorChanged;
    }

    private void OnDestroy()
    {
        color.OnChange -= OnColorChanged;

    }

    private void OnColorChanged(Color previous, Color next, bool asServer)
    {
        Debug.Log("On Color  Change ");
        meshRenderer.material.color = next;
    }

    // XR → calls this when ray selects
    public void OnSelect()
    {
        Debug.Log("hOVER 1");
        if (SignInHandler.Instance.selectedPlayer == SignInHandler.PlayerType.Instructor)
        {
            Debug.Log("hOVER 2");

            CmdChangeColor(Color.red);
        }
    }

    // XR → calls this when ray deselects
    public void OnDeSelect()
    {
        Debug.Log("Un hOVER 1");
        if (SignInHandler.Instance.selectedPlayer == SignInHandler.PlayerType.Instructor)
        {
            Debug.Log("Un hOVER 2");
            CmdChangeColor(Color.white);
        }
    }

    // -----------------------------
    // SERVER SIDE COLOR UPDATE
    // -----------------------------
    [ServerRpc(RequireOwnership = false)]
    private void CmdChangeColor(Color newColor)
    {
        color.Value = newColor;
    }
}
