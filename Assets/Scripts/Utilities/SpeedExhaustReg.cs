using UnityEngine;
using System.Collections;
using ScriptableObjs;

public class SpeedExhaustReg : MonoBehaviour
{
    //methods related to speed regulation and exhaustion management

    



    //returns new exhaustion level as a function of ratio of current speed to max speed
    public static float TickExhaust(float currentExhaust, float currentSpeed, float maxSpeed,
        PlayerConstants playerConstants)
    {
        
        float speedRatio = currentSpeed / maxSpeed;
        float randFloat = Random.Range(0.5f, 1.0f);

        //if player is below half their max speed, regain some energy
        if (speedRatio <= 0.5f)
            return currentExhaust + (10*-speedRatio)*randFloat;

        return currentExhaust + (10*speedRatio)*randFloat;
    
    }
    
    
    
    //From Omar Addam https://github.com/omaddam/Boids-Simulation
    public static Vector3 NormalizeSteeringForce(Vector3 force,float maxSteerForce)
    {
        return force.normalized * Mathf.Clamp(force.magnitude, 0, maxSteerForce);
    }
    
}
