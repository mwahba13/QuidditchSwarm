using System;
using UnityEngine;
using System.Collections;
using System.Numerics;
using ScriptableObjs;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
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

    protected Vector3 _agentVector;
    

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
        Collider[] neighbours = Physics.OverlapSphere(transform.position, playerConstants.neighbourDetectionRadius);
        if (neighbours.Length > 0)
            accel += TeamSpecificSeperation(neighbours);

        //avoid environemnt
        accel += SpeedExhaustReg.NormalizeSteeringForce(AvoidCollisions(), playerConstants.maxSteeringForce)
                 * playerConstants.environmentAvoidanceWeight;
        
        //adjust speed based on exhaustion
        float tempMaxVel = maxVelocity;
        if ((maxExhaust - currentExhaust) <= 8)
        {
            tempMaxVel = maxVelocity * Random.Range(0.3f,0.8f);
        }
            
        //add force based on team-specific traits (i.e. crunkness)
        accel += TeamSpecificBehavior();
        
        //create new velocity based on calculated acceleration
        Vector3 newVel = _rb.velocity;
        newVel += accel * Time.deltaTime;
        
        //clamp velocity
        newVel = newVel.normalized * Mathf.Clamp(newVel.magnitude, 0.0f, tempMaxVel);
        
        //apply velocity
        _rb.velocity = newVel;
        transform.forward = _rb.velocity.normalized;

        _agentVector = newVel;

    }

    //STATE FUNCTIONS//
    public void ConsciousState() 
    {

        
        _generalBoidBehavior();
        _handleExhaustionTick();
        
    }

    public void UnconsciousState()
    {
        if(_rb.velocity.y < .5 && _rb.velocity.y > -0.5)
            TransitionState(PlayerState.Waiting);

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
            _rb.velocity = new Vector3(0, -1, 0);
        }
        
        //unconscious -> waiting
        else if (state.Equals(PlayerState.Unconscious) && newState.Equals(PlayerState.Waiting))
        {
            //_rb.useGravity = false;
            Transform spawnTrans = _spawnTransform;
            spawnTrans.position.Set(spawnTrans.position.x + Random.Range(-10.0f,10.0f),
                spawnTrans.position.y,spawnTrans.position.z);
            transform.SetPositionAndRotation(spawnTrans.position, Quaternion.identity);
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

            Mathf.Clamp(currentExhaust, 0.0f, maxExhaust);

            if (currentExhaust >= maxExhaust)
            {
                TransitionState(PlayerState.Unconscious);
            }
            
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

    //called every tick during conscious state
    public abstract Vector3 TeamSpecificBehavior();
    
    //UTILITILES//
    public static void ResolveCollision(PlayerBase playerOne, PlayerBase playerTwo)
    {
        
    }
}
