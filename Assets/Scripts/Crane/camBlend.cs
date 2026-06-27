using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camBlend : MonoBehaviour
{
    // [SerializeField] GameObject rotationCamOne;
    // [SerializeField] GameObject rotationCamTwo;
    [SerializeField] GameObject hookCam;

    [SerializeField] private Transform rotatingRod;
    [SerializeField] CraneRotate crane;
    [SerializeField] Hook hook;
    public float camChangePoint = -40f;
    public bool stopCameraSwitch=false;


    private void Start()
    {
        // rotationCamOne.SetActive(true);
        // rotationCamTwo.SetActive(false);
        hookCam.SetActive(false);
    }


    private void Update()
    {
        if (stopCameraSwitch)
        {
            return;
        }

        if (!crane.isRotating && !hook.isReleasing)
        {
            if (crane.isCollided)
            {
                return;
            }
            hookCam.SetActive(true);
        }
        else
        {
            hookCam.SetActive(false);
        }



        // float yAngle = rotatingRod.localEulerAngles.y;

        // if (yAngle > 180) yAngle -= 360;

        // if (yAngle <= camChangePoint && crane.isRotating)
        // {
        //     rotationCamOne.SetActive(true);
        //     rotationCamTwo.SetActive(false);
        // }
        // else if (yAngle > camChangePoint && crane.isRotating)
        // {
        //     rotationCamTwo.SetActive(true);
        //     rotationCamOne.SetActive(false);
        // }
    }
}
