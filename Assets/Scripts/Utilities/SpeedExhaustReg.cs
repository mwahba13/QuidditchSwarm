using UnityEngine;
using System.Collections;
using ScriptableObjs;

public class SpeedExhaustReg : MonoBehaviour
{
    //methods related to speed regulation and exhaustion management

    //returns the new speed of the agent
    public static float RegulateSpeed(float currentSpeed, float maxSpeed, float distToSnitch, 
        float aggro, float currentExhaust)
    {
        return 0;
    }


    //returns new exhaustion level as a function of ratio of current speed to max speed
    public static float TickExhaust(float currentExhaust, float currentSpeed, float maxSpeed,
        PlayerConstants playerConstants)
    {
        float speedRatio = currentSpeed / maxSpeed;

        //if they are not above the exhaustion threshold, they regain some energy
        if ((speedRatio < playerConstants.exhaustThreshold) && currentExhaust != 0)
            return (currentExhaust - playerConstants.exhaustRegainValue);
            
        
        //else they gain exhaustion proportional to speed and maxExhaustDepletion value
        return (currentExhaust + (playerConstants.maxExhaustionDepletion * speedRatio));
    }
    
    //From Omar Addam https://github.com/omaddam/Boids-Simulation
    public static Vector3 NormalizeSteeringForce(Vector3 force,float maxSteerForce)
    {
        return force.normalized * Mathf.Clamp(force.magnitude, 0, maxSteerForce);
    }
    
}
