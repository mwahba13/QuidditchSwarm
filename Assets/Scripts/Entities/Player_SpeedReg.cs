using UnityEngine;
using System.Collections;

public class Player_SpeedReg : MonoBehaviour
{
    //this class handles everything related to speed and exhaustion

    private float _exhaustTickFreq;
    private float _currentExhaust;
    private float _maxExhaust;
    private float _maxVelocity;
    private float _currentSpeed;

    //metric that is function of distance to snitch and aggression
    //players who are very hungry will work to exhaustion to catch teh snitch
    private float _hunger;

    public float _exhaustTimer = 0;


    public void setSpeedExhaustValues(float timer, float maxExhaust,float maxVel)
    {
        _exhaustTickFreq = timer;
        _maxExhaust = maxExhaust;
        _maxVelocity = maxVel;
    }

    public void Update()
    {
        //increment exhaustion as function of currentspeed/maxspeed ratio
        _exhaustTimer += (Time.deltaTime*getSpeedRatio());
        if (_exhaustTimer > _exhaustTickFreq)
        {
            _currentExhaust += 1;
            _exhaustTimer = 0;
        }

        //change speed depending on how exhausted person is

    }

    //returns the players updated speed
    public float regulateSpeed(float currentSpeed,float distToSnitch,float aggro)
    {
        float newSpeed = 0.0f;
        float exhaustRatio = getExhaustRatio();
        
        _currentSpeed = currentSpeed;
        _hunger = getHunger(distToSnitch,aggro);


        



        return newSpeed;
    }

    //UTILITILES//

    //generates exhaustion level as ratio of currentexhaust/maxExhasut
    private float getExhaustRatio() { return (_currentExhaust / _maxExhaust); }
    private float getSpeedRatio() { return (_currentSpeed / _maxVelocity); }

    private float getHunger(float distToSnitch,float aggro)
    {

    }
}
