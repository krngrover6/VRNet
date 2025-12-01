// REMOVE this line:
// using FishNet.Object;

using UnityEngine;
using UnityEngine.InputSystem;

public class InstructorLaserHighlighter : MonoBehaviour
{
    [Header("Ray Setup")]
    [SerializeField] private Transform rayOrigin;             // Right controller (drag in Inspector)
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private LayerMask interactMask = ~0;

    [Header("Input")]
    [SerializeField] private InputActionReference triggerAction; // XRI Right Interaction/Select Value

    [Header("Line Visual")]
    [SerializeField] private LineRenderer lineRenderer;       // LineVisual's LineRenderer
    [SerializeField] private float lineWidth = 0.01f;
    [SerializeField] private Color idleColor   = Color.white;
    [SerializeField] private Color hoverColor  = Color.yellow;
    [SerializeField] private Color selectColor = Color.red;

    private HighlightableCube _current;

    private void Start()
    {
        // --- Find ray origin if not assigned in Inspector ---
        if (rayOrigin == null)
        {
            GameObject rc = GameObject.FindWithTag("RightController");
            if (rc != null)
            {
                rayOrigin = rc.transform;
                Debug.Log("[Laser] rayOrigin set to RightController", this);
            }
            else
            {
                Debug.LogWarning("[Laser] No GameObject with tag 'RightController' found.", this);
            }
        }

        // --- Find LineRenderer on LineVisual if not set ---
        if (lineRenderer == null && rayOrigin != null)
        {
            Transform t = rayOrigin.Find("Near-Far Interactor/LineVisual");
            if (t != null)
                lineRenderer = t.GetComponent<LineRenderer>();

            if (lineRenderer != null)
                Debug.Log("[Laser] Found LineRenderer under RightController.", this);
            else
                Debug.LogWarning("[Laser] No LineRenderer found under RightController.", this);
        }

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.widthMultiplier = lineWidth;
        }

        if (triggerAction != null)
            triggerAction.action.Enable();
        else
            Debug.LogWarning("[Laser] Trigger Action not assigned!", this);
    }

    private void OnDisable()
    {
        if (triggerAction != null)
            triggerAction.action.Disable();
    }

    private void Update()
    {
        if (rayOrigin == null || triggerAction == null || lineRenderer == null)
            return;

        bool triggerPressedThisFrame = triggerAction.action.WasPressedThisFrame();
        bool triggerHeld             = triggerAction.action.IsPressed();

        Vector3 start = rayOrigin.position;
        Vector3 dir   = rayOrigin.forward;

        RaycastHit hit;
        bool hitCube = false;
        HighlightableCube hitHighlightable = null;
        Vector3 end = start + dir * maxDistance;

        // Physics raycast for hover + selection
        if (Physics.Raycast(start, dir, out hit, maxDistance, interactMask))
        {
            end = hit.point;

            hitHighlightable = hit.collider.GetComponent<HighlightableCube>();
            hitCube = (hitHighlightable != null);
        }

        // Line positions
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // Line color
        Color c = idleColor;
        if (hitCube)
            c = triggerHeld ? selectColor : hoverColor;

        lineRenderer.startColor = c;
        lineRenderer.endColor   = c;

        // Highlight on trigger press
        if (triggerPressedThisFrame && hitCube)
        {
            Debug.Log("[Laser] Trigger pressed on HighlightableCube", this);

            if (_current != null)
                _current.SetHighlighted(false);

            _current = hitHighlightable;
            _current.SetHighlighted(true);   // HighlightableCube is where networking happens
        }
    }
}
