﻿using System;
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


    private void FixedUpdate()
    {

        Vector3 accel = new Vector3();

        _timer += Time.deltaTime;
        if (_timer >= snitchSettings.directionTimer)
        {
            Vector3 randPoint = Random.onUnitSphere;
            
            Vector3 randDir = (randPoint - transform.position) * snitchSettings.snitchSpeed;
            accel += randDir;
            accel += SpeedExhaustReg.NormalizeSteeringForce(randDir, snitchSettings.maxSteeringForce);
            _timer = 0.0f;
            
            if(snitchSettings.showRandomVector)
                Debug.DrawRay(transform.position,randPoint*5.0f,Color.yellow);
        }
        
        accel += SpeedExhaustReg.NormalizeSteeringForce(AvoidCollisions(), snitchSettings.maxSteeringForce)
         *snitchSettings.environmentAvoidanceWeighting;

        Vector3 newVelocity = _rb.velocity;
        newVelocity += accel * Time.deltaTime;
        
        //clamp velocity
        newVelocity = newVelocity.normalized * Mathf.Clamp(newVelocity.magnitude, snitchSettings.minVelocity,
            snitchSettings.maxVelocity);

        _rb.velocity = newVelocity;
        
        if(snitchSettings.showDirectionVector)
            Debug.DrawRay(transform.position,newVelocity,Color.white);

        transform.forward = _rb.velocity.normalized;



    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Gryffindor"))
            _scoreEvent.Invoke(PlayerBase.Team.Gryffindor);
        else if(other.collider.CompareTag("Slytherin"))
            _scoreEvent.Invoke(PlayerBase.Team.Slytherin);
        else if (other.collider.CompareTag("Boundary") || other.collider.CompareTag("Ground"))
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

        if(snitchSettings.showEnvironmentAvoidVector)
            Debug.DrawLine(transform.position,transform.position - hitInfo.point,Color.red);
        
        return transform.position - hitInfo.point;

        
        

    }
    


}
