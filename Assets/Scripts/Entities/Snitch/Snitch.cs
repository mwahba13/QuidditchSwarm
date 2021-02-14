using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjs;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Snitch : MonoBehaviour
{
    //Snitch Movement script

    //GOLDEN snitch will always just be flying around randomly



    private float _timer = 0.0f;
    private Rigidbody _rb;


    public SnitchScriptable snitchSettings; 

    private void Start()
    {
        _timer = snitchSettings.directionTimer;

        TryGetComponent<Rigidbody>(out _rb);
        
    }



    private void FixedUpdate()
    {
        //redirect timer
        _timer += Time.deltaTime;
        if (_timer >= snitchSettings.directionTimer)
        {
            Redirect(Random.insideUnitSphere*snitchSettings.snitchSpeed);
            _timer = 0.0f;
        }

        AvoidCollisions();
    }

    private void AvoidCollisions()
    {
        LayerMask ignoreLayers = LayerMask.GetMask("Player");
        Collider[] hits = Physics.OverlapSphere(transform.position, snitchSettings.collisionRadiusDetection,ignoreLayers);
        if (hits.Length > 0)
        {
            //redirect snitch towards origin
            Vector3 dirTowardsOrigin = new Vector3(0, 0, 0) - transform.position;
            dirTowardsOrigin = dirTowardsOrigin.normalized;
            Redirect(dirTowardsOrigin);
            
            //give timer 2 extra seconds to let snitch get back roughly to middle 
            _timer = -2.0f;
        }
        
    }
    
    


    //changes direction of snitch once timer is up
    private void Redirect(Vector3 newDir)
    {

        if (snitchSettings.showNewDirection)
        {
            Debug.DrawRay(transform.position, newDir);
        }

        _rb.AddForce(newDir * snitchSettings.snitchSpeed, ForceMode.Force);

    }

}
