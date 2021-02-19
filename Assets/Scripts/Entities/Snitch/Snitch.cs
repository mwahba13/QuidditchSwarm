using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using ScriptableObjs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class Snitch : MonoBehaviour
{
    //Snitch Movement script

    //GOLDEN snitch will always just be flying around randomly

    private float _timer = 0.0f;
    private Rigidbody _rb;
    private ScoreEvent _scoreEvent;
    
    public SnitchScriptable snitchSettings;
    

    private void Start()
    {
        _timer = snitchSettings.directionTimer;
        
        
        _scoreEvent = new ScoreEvent();
        _scoreEvent.AddListener(ScoreManager.IncrementScore);
        

        
        TryGetComponent<Rigidbody>(out _rb);
        //give it a lil push
        //_rb.AddForce(Vector3.forward);
        
    }   


    //TODO: fix snitches movement
    private void FixedUpdate()
    {

        Vector3 accel = new Vector3();

        _timer += Time.deltaTime;
        if (_timer >= snitchSettings.directionTimer)
        {
            Vector3 randDir = (Random.onUnitSphere - transform.position) * snitchSettings.snitchSpeed;
            accel += randDir;
            accel += SpeedExhaustReg.NormalizeSteeringForce(randDir, snitchSettings.maxSteeringForce);
            _timer = 0.0f;
        }
        
        accel += SpeedExhaustReg.NormalizeSteeringForce(AvoidCollisions(), snitchSettings.maxSteeringForce)
         *snitchSettings.environmentAvoidanceWeighting;

        Vector3 newVelocity = _rb.velocity;
        newVelocity += accel * Time.deltaTime;
        
        //clamp velocity
        newVelocity = newVelocity.normalized * Mathf.Clamp(newVelocity.magnitude, snitchSettings.minVelocity,
            snitchSettings.maxVelocity);

        _rb.velocity = newVelocity;

        transform.forward = _rb.velocity.normalized;

        /*
        Vector3 accel = new Vector3();

        _timer += Time.deltaTime;
        if (_timer >= snitchSettings.directionTimer)
        {

            Vector3 randDir = (Random.onUnitSphere - transform.position) * snitchSettings.snitchSpeed;

            accel += randDir;
            accel += SpeedExhaustReg.NormalizeSteeringForce(randDir, snitchSettings.maxSteeringForce);
            _timer = 0.0f;
        }


        //accel += SpeedExhaustReg.NormalizeSteeringForce(AvoidCollisions(), snitchSettings.maxSteeringForce)
           // *snitchSettings.environmentAvoidanceWeighting;

        Vector3 newVel = _rb.velocity;
        newVel += accel * Time.deltaTime;
    
        //clamp velocity
        newVel = newVel.normalized * Mathf.Clamp(newVel.magnitude, snitchSettings.minVelocity,
            snitchSettings.maxVelocity);

        _rb.velocity = newVel;

        if (snitchSettings.showDirectionVector)
            Debug.DrawRay(transform.position,newVel,Color.cyan);
    
        transform.forward = _rb.velocity.normalized;
        */


    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Gryffindor"))
            _scoreEvent.Invoke(PlayerBase.Team.Gryffindor);
        else if(other.collider.CompareTag("Slytherin"))
            _scoreEvent.Invoke(PlayerBase.Team.Slytherin);
        else if (other.collider.CompareTag("Environment"))
            _rb.velocity = -_rb.velocity;
            
    }


    //avoid collisions with environment
    private Vector3 AvoidCollisions()
    {
        Vector3 output = Vector3.zero;
        
        //https://github.com/omaddam/Boids-Simulation
        if(!Physics.SphereCast(transform.position,
                snitchSettings.collisionRadiusDetection,
                transform.forward,
                out RaycastHit hitInfo,
                snitchSettings.collisionRadiusDetection))
                return Vector3.zero;

        return transform.position - hitInfo.point;

        
        

    }
    


}
