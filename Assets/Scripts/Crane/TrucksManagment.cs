using System.Collections;
using Cinemachine;
using Cinemachine.Editor;
using UnityEngine;

public class TrucksManagment : MonoBehaviour
{
    [SerializeField] public GameObject[] trucks;

    [SerializeField] private GameObject lastTruckFollowCam;

    [SerializeField] private Hook hook;
    [SerializeField] private GameManager gameManager;
    [SerializeField]private GameObject HookCam;
    [SerializeField] private camBlend camblend;
    public int lastTruckIndex;


    private void Update()
    {
        //bool isReleaseStarted = hook.isReleasing && !wasReleasingLastFrame;
        //for (int i = 0; i < trucks.Length; i++)
        //{
        //    //if(isReleaseStarted)
        //        AssignSlotToCargo(trucks[i], i);
        //}
        ////wasReleasingLastFrame = hook.isReleasing;
    }

    public bool CheckTruckFilled(GameObject truck)
    {
        Transform slotContainer = truck.transform.GetChild(11);
        //Debug.Log($"SlotContainer: {slotContainer.name}");
        TruckSlotManager truckSlotManager = truck.GetComponent<TruckSlotManager>();

        if(slotContainer != null && truckSlotManager != null)
        {
            if(slotContainer.childCount == truckSlotManager.currentSlotIndex)
            {
                //Debug.Log($"truckSlotManager.truckSlots.Count: {truckSlotManager.currentSlotIndex + 1}  slotContainer.childCount: {slotContainer.childCount} ");

                return true;
            }
        }
        return false;
    }

    public void PlayTruckAnimation(GameObject truck, int index)
    {
        Animator truckAnim = truck.GetComponent<Animator>();

        if(!truckAnim.GetBool("IsRun" + index))
        {
            truckAnim.SetBool("IsRun" + index, true);
        }
    }

    public void AssignSlotToCargo(GameObject truck, int i)
    {
        TruckSlotManager truckSlotManager = truck.GetComponent<TruckSlotManager>();

        Transform slot = truckSlotManager.GetNextSlot();
        if (slot != null)
        {
            if (hook.cargoStack.Count == 0) return;

            StartCoroutine(truckSlotManager.MoveCargoToSlot(hook.cargoStack[0].gameObject.transform, slot));
        }
        if (CheckTruckFilled(truck))
        {
            StartCoroutine(PlayTruckAnimAfter(truck, i));

            
            switch (gameManager.Level)
            {
                case GameManager.diffLevel.Begginer: lastTruckIndex = 0; break;
                case GameManager.diffLevel.Intermidiate: lastTruckIndex = 1; break;
                case GameManager.diffLevel.Expert: lastTruckIndex = 2; break;
                default: lastTruckIndex = 0; break;
            }

            if(truck == trucks[lastTruckIndex])
            {
                camblend.stopCameraSwitch=true;
                HookCam.SetActive(false);
                lastTruckFollowCam.SetActive(true);
                lastTruckFollowCam.gameObject.GetComponent<CinemachineVirtualCamera>().Follow=truck.transform;
                lastTruckFollowCam.gameObject.GetComponent<CinemachineVirtualCamera>().LookAt=truck.transform;
            }
        }
    }

    public IEnumerator PlayTruckAnimAfter(GameObject truck, int i)
    {
        yield return new WaitForSeconds(4);
        PlayTruckAnimation(truck, i);
    }
}         
