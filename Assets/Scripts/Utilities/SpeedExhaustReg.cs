using UnityEngine;
using System.Collections;
using ScriptableObjs;

public class SpeedExhaustReg : MonoBehaviour
{
    //methods related to speed regulation and exhaustion management

    //returns the new speed of the agent
    public static float regulateSpeed(float currentSpeed, float maxSpeed, float distToSnitch, 
        float aggro, float currentExhaust)
    {
        
    }


    //returns new exhaustion level as a function of ratio of current speed to max speed
    public static float tickExhaust(float currentExhaust, float currentSpeed, float maxSpeed,
        int maxExhaustDeplete,float exhaustThreshold)
    {
        float speedRatio = currentSpeed / maxSpeed;
        
        if(speedRatio < exhaustThreshold )

        return (currentExhaust - (maxExhaustDeplete * speedRatio));
    }
    
}
