using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Snitch_Move : MonoBehaviour
{
    //Snitch Movement script

    //GOLDEN snitch will always just be flying around randomly

    public bool DEBUG_SNITCH;
    //time after which snitch will choose a new random direction to fly
    [SerializeField]
    private float directionTimer;
    [SerializeField]
    private float speed;

    private float _timer = 0.0f;
    private Rigidbody _rb;



    private void Start()
    {
        _timer = directionTimer;

        TryGetComponent<Rigidbody>(out _rb);
        
    }


    private void Update()
    {
        
        _timer += Time.deltaTime;
        if (_timer >= directionTimer)
        {
            Redirect(Random.insideUnitSphere*speed);
            _timer = 0.0f;
        }
          
         
    }


    //when the snitch is about to leave the bounds, send it towards the center
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaving Boundaries");

        //vector which points towards origin
        Vector3 dirTowardsOrigin = new Vector3(0, 0, 0) - transform.position;
        dirTowardsOrigin = dirTowardsOrigin.normalized;

        Redirect(dirTowardsOrigin);
               

        //give timer 2 extra seconds to let snitch get back roughly to middle 
        _timer = -2.0f;
        
    }



    //changes direction of snitch once timer is up
    private void Redirect(Vector3 newDir)
    {

        if(DEBUG_SNITCH)
            Debug.DrawRay(transform.position, newDir);

        Debug.Log("Redirect new Vector: " + newDir);
        _rb.AddForce(newDir * speed, ForceMode.Force);

        //transform.Translate(newDir*speed*Time.deltaTime);
        
    }

}
