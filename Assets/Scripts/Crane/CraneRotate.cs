using UnityEngine;

public class CraneRotate : MonoBehaviour
{
    public float startAngle = -90f;
    public float endAngle = 90f;
    public float rotationSpeed = 10f;

    public float currentAngle;

    public bool isRotating;
    public bool isCollided = false;
    public bool isAtDropPoint = false;

    [SerializeField] private Hook hook;

    void Start()
    {
        ResetRotation();
    }

    void Update()
    {
        if (!isRotating)
            return;

        currentAngle += rotationSpeed * Time.deltaTime;


        currentAngle = Mathf.Clamp(currentAngle, startAngle, endAngle);


        if (currentAngle >= endAngle)
        {
            currentAngle = endAngle;

            if (hook.cargoStack.Count > 0)
            {

                isAtDropPoint = true;
                StopRotation();
            }
            else
            {
                rotationSpeed = -Mathf.Abs(rotationSpeed);
            }
        }


        if (currentAngle <= startAngle)
        {
            currentAngle = startAngle;
            rotationSpeed = Mathf.Abs(rotationSpeed);
        }

        transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }

    public void ResetRotation()
    {
        currentAngle = startAngle;
        rotationSpeed = Mathf.Abs(rotationSpeed);
        transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);

        isRotating = false;
        isAtDropPoint = false;
        isCollided = false;
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    public void RotateReverse()
    {
        rotationSpeed = -rotationSpeed;
    }
}