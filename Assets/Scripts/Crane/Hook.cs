using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GameObject cargoContainer;
    [SerializeField] private TrucksManagment trucksManagment;

    BoxCollider cargoContainerCollider;
    public List<GameObject> cargoStack = new List<GameObject>();

    public float cargoHeight = 1f;
    public float ropeSpeed = 2.0f;
    public float minLength = 1.0f;
    public float maxLength = 20.0f;
    private LineRenderer lineRenderer;
    public Transform trolley;
    public int totalCargoReleased = 0;
    public bool isGameStarted = false;
    public int ObstacleCollisionCount = 0;
    //public int highestCargoStack = 0;
    public int repCount = 0;
    public float holdTimer = 0f;
    public int postureBreaks = 0;
    float input;


    public bool isReleasing;
    private Rigidbody rb;

    [SerializeField] private CraneRotate crane;
    [SerializeField] private CraneAudio craneAudio;

    private void Start()
    {
        cargoContainerCollider = cargoContainer.GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        isReleasing = false;
    }

    private void Update()
    {
        input = Input.GetAxis("Vertical");

        if (crane.isAtDropPoint && cargoStack.Count > 0 && !isReleasing)
        {
            if(cargoStack.Count > 0)
            {
                ReleaseCargo();
            }
            else
            {
                crane.isAtDropPoint = false;
            }
        }

        RopeControl();
        PlayCraneAudio();
        
    }

    void PlayCraneAudio()
    {
        if (input > 0.01f)
        {
            craneAudio.MoveUp();
        }
        else if (input < -0.01f)
        {
            craneAudio.MoveDown();
        }
        else if(crane.isRotating)
        {
            craneAudio.Move();
        }
        else
        {
            craneAudio.Stop();
        }

    }

void RopeControl()
{
    if (isReleasing)
    {
        crane.StopRotation();
        return;
    }

    // Calculate rope limits
    maxLength = (trolley.position.y - cargoContainerCollider.size.y) - 0.5f;

    float minY = trolley.position.y - maxLength;
    float maxY = trolley.position.y - minLength;

    // Is hook fully raised?
    bool hookAtTop = transform.position.y >= maxY - 0.05f;

    // Rotate ONLY when hook is at the top
    if (Mathf.Abs(input) < 0.01f &&
        !crane.isCollided &&
        isGameStarted &&
        !crane.isAtDropPoint &&
        hookAtTop)
    {
        crane.StartRotation();

        repCount++;
        holdTimer += Time.deltaTime;
    }
    else
    {
        crane.StopRotation();

        if (holdTimer < 2f)
            postureBreaks++;
    }

    float moveAmount = input * ropeSpeed * Time.deltaTime;

    if (moveAmount != 0f)
    {
        Vector3 moveDir = moveAmount > 0 ? Vector3.up : Vector3.down;
        float moveDist = Mathf.Abs(moveAmount);

        Vector3 halfExtents = cargoContainerCollider.size * 0.5f;

        LayerMask mask = ~(LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("Hook"));

        bool blocked = Physics.BoxCast(
            transform.position,
            halfExtents,
            moveDir,
            out RaycastHit hit,
            Quaternion.identity,
            moveDist + 0.05f,
            mask
        );

        if (blocked)
        {
            float safeMove = Mathf.Max(0f, hit.distance - 0.05f);
            moveAmount = moveDir == Vector3.up ? safeMove : -safeMove;
        }
    }

    transform.Translate(0, moveAmount, 0);

    Vector3 pos = transform.position;
    pos.y = Mathf.Clamp(pos.y, minY, maxY);
    transform.position = pos;

    lineRenderer.SetPosition(0, trolley.position);
    lineRenderer.SetPosition(1, transform.position);
}

    public void StackCargo(GameObject cargo)
    {
        Rigidbody cargoRb = cargo.GetComponent<Rigidbody>();

        if (cargoRb)
        {
            cargoRb.velocity = Vector3.zero;
            cargoRb.angularVelocity = Vector3.zero;
            cargoRb.isKinematic = true;
            cargoRb.constraints = RigidbodyConstraints.None;
        }
        cargo.GetComponent<Collider>().isTrigger = false;
        cargo.transform.SetParent(cargoContainer.transform);

        int index = cargoStack.Count;

        Vector3 localPos = Vector3.down * (index * cargoHeight + 0.5f * cargoHeight);
        cargo.transform.localPosition = localPos;

        cargoStack.Add(cargo);


        GrowTrigger(cargo.transform);
    }

    void GrowTrigger(Transform cargoTf)
    {
        Vector3 size = cargoContainerCollider.size;
        size.y = cargoStack.Count * cargoHeight;
        cargoContainerCollider.size = size;

        Vector3 center = cargoContainerCollider.center;
        center.y = -size.y * 0.5f + 0.1f;
        cargoContainerCollider.center = center;

    }

    public void ReleaseCargo()
    {
        isReleasing = true;
        cargoContainerCollider.enabled = false;


        totalCargoReleased += cargoStack.Count;

        int truckIndex = (totalCargoReleased - 1) / 5;
        truckIndex = Mathf.Clamp(truckIndex, 0, trucksManagment.trucks.Length - 1);
        trucksManagment.AssignSlotToCargo(trucksManagment.trucks[truckIndex], truckIndex); 


        //if(cargoStack.Count > highestCargoStack)
        //{
        //    highestCargoStack = cargoStack.Count;
        //}

        foreach (GameObject c in cargoStack)
        {
            c.transform.SetParent(null, true);
            Rigidbody rb = c.GetComponent<Rigidbody>();
            c.tag = "ReleasedCargo";


            if (rb)
                rb.isKinematic = false;

        }

        cargoStack.Clear();

        cargoContainerCollider.enabled = true;


        cargoContainerCollider.size = new Vector3(cargoContainerCollider.size.x, 0.1f, cargoContainerCollider.size.z);
        cargoContainerCollider.center = Vector3.zero;
        crane.isAtDropPoint = false;
        Invoke("ReleaseComplete", 2f);
    }


    void ReleaseComplete()
    {
        isReleasing = false;
    }

}
