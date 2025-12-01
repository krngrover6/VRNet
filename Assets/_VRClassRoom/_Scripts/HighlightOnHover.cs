using FishNet.Object;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HighlightWithNearFar : NetworkBehaviour
{
    private XRBaseInteractable interactable;
    private HighlightableCube cube;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        cube = GetComponent<HighlightableCube>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!IsServer) return;   // Only host controls highlight
        cube.SetHighlighted(true);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (!IsServer) return;
        cube.SetHighlighted(false);
    }
}
