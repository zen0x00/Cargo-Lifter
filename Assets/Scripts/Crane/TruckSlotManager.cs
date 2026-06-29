using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckSlotManager : MonoBehaviour
{
    [SerializeField] public List<Transform> truckSlots = new List<Transform>();
    public CraneAudio craneAudio;

    public int currentSlotIndex = 0;

    public Transform GetNextSlot()
    {
        if(currentSlotIndex >= truckSlots.Count)
        {
            return null;
        }

        Transform slot = truckSlots[currentSlotIndex];
        currentSlotIndex++;
        //Debug.Log($"Slot Recevied: {slot}");
        return slot;
    }


    public IEnumerator MoveCargoToSlot(Transform cargo, Transform truckSlot)
    {
        //Debug.Log($"Move Cargo: {cargo} to slot:{truckSlot}");
        Rigidbody cargoRb = cargo.GetComponent<Rigidbody>();

        Vector3 startPos = cargo.position;
        Vector3 endPos = truckSlot.position;

        float time = 0f;
        float duration = 0.5f;

        while(time < duration)
        {
            cargo.position = Vector3.Lerp(startPos, endPos, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        
        cargo.position = endPos;
        cargo.localRotation = truckSlot.localRotation;
        cargo.transform.SetParent(truckSlot);

        yield return new WaitForSeconds(3);
        cargoRb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
