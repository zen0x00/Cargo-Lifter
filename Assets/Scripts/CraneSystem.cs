using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(LineRenderer))]
public class CraneSystem : MonoBehaviour
{
    [Header("Setup")]
    public Transform trolley;        // Drag your Trolley object here
    public string targetTag = "Pickable";

    [Header("Winch Settings")]
    public float winchSpeed = 2.0f;
    public float minLength = 1.0f;
    public float maxLength = 15.0f;

    [Header("Hook Settings")]
    public float grabRadius = 1.5f;

    // Internal variables
    private ConfigurableJoint joint;
    private LineRenderer lineRenderer;
    private FixedJoint grabJoint;
    private Rigidbody currentLoad;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        lineRenderer = GetComponent<LineRenderer>();

        // Ensure the joint is set up correctly automatically
        joint.connectedBody = trolley.GetComponent<Rigidbody>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
    }

    void Update()
    {
        HandleWinch();
        HandleVisuals();
        HandleGrabInput();
    }

    // 1. Controls the cable length (W/S or Arrow Keys)
    void HandleWinch()
    {
        float input = Input.GetAxis("Vertical"); // W = 1, S = -1

        // We modify the SoftJointLimit
        SoftJointLimit limit = joint.linearLimit;

        // If pressing W (up), decrease limit. If S (down), increase limit.
        // Note: We use -= because smaller limit means shorter rope.
        limit.limit -= input * winchSpeed * Time.deltaTime;

        // Clamp values so we don't break physics
        limit.limit = Mathf.Clamp(limit.limit, minLength, maxLength);

        // Apply back to joint
        joint.linearLimit = limit;
    }

    // 2. Draws the line from Trolley to Hook
    void HandleVisuals()
    {
        lineRenderer.SetPosition(0, trolley.position); // Start at Trolley
        lineRenderer.SetPosition(1, transform.position); // End at Hook
    }

    // 3. Logic for Q (Grab) and E (Release)
    void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) TryGrab();
        if (Input.GetKeyDown(KeyCode.E)) Release();
    }

    void TryGrab()
    {
        if (grabJoint != null) return; // Already holding something

        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(targetTag) && col.GetComponent<Rigidbody>())
            {
                // Grab the object
                currentLoad = col.GetComponent<Rigidbody>();

                // Create a FixedJoint to lock it to the hook
                grabJoint = gameObject.AddComponent<FixedJoint>();
                grabJoint.connectedBody = currentLoad;

                Debug.Log("Grabbed: " + col.name);
                break;
            }
        }
    }

    void Release()
    {
        if (grabJoint == null) return;

        Destroy(grabJoint);
        grabJoint = null;
        currentLoad = null;
        Debug.Log("Released");
    }

    // Visualize grab radius in Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}