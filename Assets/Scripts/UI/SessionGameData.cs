using UnityEngine;

[System.Serializable]
public class SessionGameData : MonoBehaviour
{

    public int cargoDelivered;
    public int collisionCount;

    public float rewardPerCargo;
    public float penalityPerCollision;
}
