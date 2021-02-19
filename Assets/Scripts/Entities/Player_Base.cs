﻿using UnityEngine;
using System.Collections;
using System.Numerics;
using ScriptableObjs;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent( typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class PlayerBase : MonoBehaviour
{

    //This class handles behaviours common across players on both teams (i.e. movement etc.)

    public enum PlayerState : uint
    {

        Unconscious, Conscious, Waiting

    }

    public enum Team : uint
    {
        Gryffindor,Slytherin, Null
    }

    //Player traits, all between 0.0 and 1.0
    [FormerlySerializedAs("_aggro")] [SerializeField]
    protected float aggro;
    [FormerlySerializedAs("_maxExhaust")] [SerializeField]
    protected float maxExhaust;
    [FormerlySerializedAs("_currentExhaust")] [SerializeField]
    protected float currentExhaust = 0;
    [FormerlySerializedAs("_maxVelocity")] [SerializeField]
    protected float maxVelocity;
    [FormerlySerializedAs("_weight")] [SerializeField]
    protected float weight;


    
    [FormerlySerializedAs("_state")] [SerializeField]
    protected PlayerState state;
    protected Rigidbody _rb;
    [SerializeField]
    protected GameObject _snitchObj;

    

    public PlayerConstants playerConstants;

    private float _exhaustTimer;
    private float _waitingTimer = 0 ;

    private Transform _spawnTransform;

    //if true, player is on ground, at spawn,  waiting to be active again
    
    public void Start()
    {
        TryGetComponent<Rigidbody>(out _rb);
        
        GenerateTraitValues();
        SetState(PlayerState.Conscious);

        _exhaustTimer = playerConstants.exhastionTickFreq;
        _rb.mass = weight;
        
        //give a lil push
        _rb.AddForce(Vector3.forward);
    }


    public void FixedUpdate()
    {
 
            switch (GetState())
            {
                case PlayerState.Conscious:
                    ConsciousState();
                    break;

                case PlayerState.Unconscious:
                    UnconsciousState();
                    break;
                
                case PlayerState.Waiting:
                    WaitingState();
                    break;
            }
    
            if(playerConstants.showAgentVelocity)
                Debug.DrawRay(transform.position,_rb.velocity,Color.white);
    }

    //MOVEMENT RELATED METHODS//
    //*SEPERATION NEEDS WORK
    //this function only changes direction
    //Some of this code adapted from Omar Addam (https://github.com/omaddam/Boids-Simulation)
    private void _generalBoidBehavior()
    {
        Vector3 accel = new Vector3();

        Vector3 snitchPos = _snitchObj.transform.position;
        Vector3 thisPos = transform.position;
        
        //add force towards snitch
        accel += (snitchPos - transform.position).normalized;
        accel += SpeedExhaustReg.NormalizeSteeringForce(snitchPos - thisPos,playerConstants.maxSteeringForce) *
                 playerConstants.snitchFollowWeight;
        
        if(playerConstants.showVectorTowardSnitch)
            Debug.DrawRay(transform.position,accel,Color.red);
        
        //add force away from neighbours and environment - seperation
        Collider[] neighbours = Physics.OverlapSphere(transform.position, playerConstants.neighbourDetctionRadius);
        if (neighbours.Length > 0)
            accel += TeamSpecificSeperation(neighbours);

        accel += SpeedExhaustReg.NormalizeSteeringForce(AvoidCollisions(), playerConstants.maxSteeringForce)
                 * playerConstants.environmentAvoidanceWeight;

        
        //create new velocity based on calculated acceleration
        Vector3 newVel = _rb.velocity;
        newVel += accel * Time.deltaTime;
        
        //clamp velocity
        newVel = newVel.normalized * Mathf.Clamp(newVel.magnitude, 0.0f, maxVelocity);
        
        //apply velocity
        _rb.velocity = newVel;
        transform.forward = _rb.velocity.normalized;


    }
    


    
 
    //STATE FUNCTIONS//
    public void ConsciousState() 
    {
        //handle exhaustion lovic
        _handleExhaustionTick();
        _generalBoidBehavior();
        
    }

    public void UnconsciousState()
    {
        

    }

    //in this state
    public void WaitingState()
    {
        _waitingTimer += Time.deltaTime;
        if (_waitingTimer >= playerConstants.inactiveTime)
        {
            TransitionState(PlayerState.Conscious);
        }
    }

    public void TransitionState(PlayerState newState)
    {
        //conscious -> unconscious
        if (state.Equals(PlayerState.Conscious) && newState.Equals(PlayerState.Unconscious))
        {
            //transport back to spawn and set a timer
            //transform.SetPositionAndRotation(_spawnTransform.position,Quaternion.identity);
            //_unconsciousTimer = 0;
            _rb.useGravity = true;
        }
        
        //unconscious -> waiting
        else if (state.Equals(PlayerState.Unconscious) && newState.Equals(PlayerState.Waiting))
        {
            transform.SetPositionAndRotation(_spawnTransform.position,Quaternion.identity);
            _waitingTimer = 0;
        }
        
        //waiting -> conscious
        else if (state.Equals(PlayerState.Waiting) && newState.Equals(PlayerState.Conscious))
        {
            _rb.useGravity = false;
            currentExhaust = 0;
        }
        
        state = newState;

    }


    private void _handleExhaustionTick()
    {
        //tick exhaustion if timer has elapsed
        _exhaustTimer += Time.deltaTime;
        if (_exhaustTimer > playerConstants.exhastionTickFreq)
        {
            //calculate new exhaust
            currentExhaust = SpeedExhaustReg.TickExhaust(currentExhaust, _rb.velocity.magnitude,
                maxVelocity,playerConstants);
            
            
            if(currentExhaust <= 0)
                //setState(PlayerState.Unconscious);

                _exhaustTimer = 0;
        }
    }
    private Vector3 AvoidCollisions()
    {
        Vector3 output = Vector3.zero;
        
        //https://github.com/omaddam/Boids-Simulation
        if(!Physics.SphereCast(transform.position,
            playerConstants.environmentAvoidanceRadius,
            transform.forward,
            out RaycastHit hitInfo,
            playerConstants.environmentAvoidanceRadius))
            return Vector3.zero;

        return transform.position - hitInfo.point;
        
        
        
        /*
        //ignore gryff/slyth players
        //LayerMask ignoreLayers = LayerMask.GetMask("Player");
        Collider[] hits = Physics.OverlapSphere(transform.position, snitchSettings.collisionRadiusDetection);
        if (hits.Length > 0)
        {
            foreach(Collider hit in hits)
            {
                if (hit.gameObject.CompareTag("Environment"))
                {
                    Debug.Log("avoid collision");
                    output += SpeedExhaustReg.NormalizeSteeringForce((transform.position - hit.transform.position),
                        snitchSettings.maxSteeringForce) * snitchSettings.environmentAvoidanceWeighting; 
                }

            }
            
            //give timer 2 extra seconds to let snitch get back roughly to middle 
            _timer = -2.0f;
        }
        
        if(snitchSettings.showEnvironmentAvoidVector)
            Debug.DrawRay(transform.position,output,Color.red);
            
            return output;
            */
        
        

    }
    
    //GET/SET///
    public void SetSnitchObj(GameObject newSnitch)
    {
        _snitchObj = newSnitch;
        
    }
    public void SetState(PlayerState newState) { state = newState; }
   
    //need this one for teleporting once unconscious
    public void setSpawnPoint(Transform newTran) {_spawnTransform = newTran;}
    
    public PlayerState GetState() { return state; }

    public float GetAggro()
    {
        return aggro;
        
    }
    
    public float GetCurrentExhaust() {return currentExhaust;}
    public float GetMaxExhaust(){return maxExhaust;}
    
    

    //ABSTRACT METHODS//
    public abstract void GenerateTraitValues();


    public abstract Vector3 TeamSpecificSeperation(Collider[] neighbours);
    
    
    //UTILITILES//
    public static void ResolveCollision(PlayerBase playerOne, PlayerBase playerTwo)
    {
        
    }
}
