using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ScriptableObjs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;


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
        
    }   


    //TODO: fix snitches movement
    //TODO: SCORING
    private void FixedUpdate()
    {
        Vector3 accel = new Vector3();
        
        //redirect timer
        _timer += Time.deltaTime;
        if (_timer >= snitchSettings.directionTimer)
        {
            //Redirect(Random.insideUnitSphere*snitchSettings.snitchSpeed);
            Vector3 randomDir = (Random.onUnitSphere - transform.position)*snitchSettings.snitchSpeed;
            
            accel += randomDir;
            accel += SpeedExhaustReg.NormalizeSteeringForce(randomDir, snitchSettings.maxSteeringForce);
            _timer = 0.0f;
        }

        accel += AvoidCollisions();

        Vector3 newVel = _rb.velocity;
        newVel += accel * Time.deltaTime;
        
        //clamp velocity
        newVel = newVel.normalized * Mathf.Clamp(newVel.magnitude, 0.0f, snitchSettings.maxVelocity);

        _rb.velocity = newVel;

        if (snitchSettings.showDirectionVector)
            Debug.DrawRay(transform.position,newVel,Color.cyan);
        
        transform.forward = _rb.velocity.normalized;


    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Gryffindor"))
            _scoreEvent.Invoke(PlayerBase.Team.Gryffindor);
        else if(other.collider.CompareTag("Slytherin"))
            _scoreEvent.Invoke(PlayerBase.Team.Slytherin);
            
    }


    //avoid collisions with environment
    private Vector3 AvoidCollisions()
    {
        Vector3 output = Vector3.zero;
        
        //ignore gryff/slyth players
        LayerMask ignoreLayers = LayerMask.GetMask("Player");
        Collider[] hits = Physics.OverlapSphere(transform.position, snitchSettings.collisionRadiusDetection,ignoreLayers);
        if (hits.Length > 0)
        {
            foreach(Collider hit in hits)
            {
                output += SpeedExhaustReg.NormalizeSteeringForce((transform.position - hit.transform.position),
                    snitchSettings.maxSteeringForce) * snitchSettings.environmentAvoidanceWeighting;
            }
            
            //give timer 2 extra seconds to let snitch get back roughly to middle 
            _timer = -2.0f;
        }
        
        if(snitchSettings.showEnvironmentAvoidVector)
            Debug.DrawRay(transform.position,output,Color.red);
        
        return output;

    }
    


}
