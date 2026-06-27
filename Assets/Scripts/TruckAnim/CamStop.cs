using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CamStop : MonoBehaviour
{
    [SerializeField]GameObject ConstantCam;
    [SerializeField]private TrucksManagment trucksManagment;
    void OnTriggerEnter(Collider other)
    {
        int targetIndex = 0;
        switch (GameManager.instance.Level)
        {
            case GameManager.diffLevel.Begginer:
            targetIndex = 0;
            break;
            case GameManager.diffLevel.Intermidiate:
            targetIndex = 1;
            break;
            case GameManager.diffLevel.Expert:
            targetIndex = 2;
            break;

        }

        if (other.gameObject.tag == trucksManagment.trucks[trucksManagment.lastTruckIndex].tag)
        {
            ConstantCam.SetActive(true);
            Debug.Log("targetIndex"+targetIndex);
            Debug.Log("trucksManagment.lastTruckIndex"+trucksManagment.lastTruckIndex);
        }
    }
}
