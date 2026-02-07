using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnalyticsCalculator : MonoBehaviour
{
    public SessionGameData data;

    public int CalculateTotalCargoDelivered()
    {
        return data.cargoDelivered;
    }

    public int CalculateTotalCollisions()
    {
        return data.collisionCount;
    }


    public float CalculateTotalEarnings()
    {
        return (data.cargoDelivered * data.rewardPerCargo) - (data.collisionCount * data.penalityPerCollision);
    }

    public float CalculateTotalPenalities()
    {
        return data.collisionCount * data.penalityPerCollision;
    }




    public float CalculateEffectiveHoldTime()
    {
        //cacalate the effective hold time based on the player's performance 
        return 0f;
    }

    public float CalculateStabilityPercent()
    {
        //cacalate the stability percent based on the player's performance
        return 0f;
    }

    public int CalculatePostureBreaks()
    {
        //cacalate the number of posture breaks based on the player's performance
        return 0;
    }

    public float CalculateAvgHoldDuration()
    {
        //cacalate the average hold duration based on the player's performance
        return 0f;
    }

    public float CalculateCalories()
    {
        //cacalate the calories burned based on the player's performance
        return 0f;
    }


}
