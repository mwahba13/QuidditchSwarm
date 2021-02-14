using UnityEngine;
using System.Collections;
using ScriptableObjs;
using UnityEngine.Serialization;

[RequireComponent( typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class PlayerBase : MonoBehaviour
{

    //This class handles behaviours common across players on both teams (i.e. movement etc.)

    public enum PlayerState : uint
    {

        Unconscious, Conscious

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

    private float _currentSpeed;
    private float _exhaustTimer;
    
    public void Start()
    {
        TryGetComponent<Rigidbody>(out _rb);
        
        GenerateTraitValues();
        SetState(PlayerState.Conscious);

        _exhaustTimer = playerConstants.exhastionTickFreq;
        _rb.mass = weight;
    }


    public void FixedUpdate()
    {
 
            switch (GETState())
            {
                case PlayerState.Conscious:
                    ConsciousState();
                    break;

                case PlayerState.Unconscious:
                    UnconsciousState();
                    break;
            }

    }

    //MOVEMENT RELATED METHODS//
    //*SEPERATION NEEDS WORK
    //this function only changes direction
    //Some of this code adapted from Omar Addam (https://github.com/omaddam/Boids-Simulation)
    private void _generalBoidBehavior()
    {
        Vector3 accel = new Vector3();
        
        //add force towards snitch
        accel += (_snitchObj.transform.position - transform.position).normalized;
        accel += NormalizeSteeringForce(_snitchObj.transform.position - transform.position) *
                 playerConstants.snitchFollowWeight;
        Debug.DrawRay(transform.position,accel,Color.red);
        
        
        //add force away from neighbours - seperation
        Collider[] neighbours = Physics.OverlapSphere(transform.position, playerConstants.neighbourDetctionRadius);
        if (neighbours.Length > 0)
            accel += TeamSpecificSeperation(neighbours);
        
        //need to add force away from environment

        
        //create new velocity based on calculated acceleration
        Vector3 newVel = _rb.velocity;
        newVel += accel * Time.deltaTime;
        
        //clamp velocity
        newVel = newVel.normalized * Mathf.Clamp(newVel.magnitude, 0.0f, maxVelocity);
        
        //apply velocity
        _rb.velocity = newVel;
        transform.forward = _rb.velocity.normalized;


    }
    
    //From Omar Addam https://github.com/omaddam/Boids-Simulation
    protected Vector3 NormalizeSteeringForce(Vector3 force)
    {
        return force.normalized * Mathf.Clamp(force.magnitude, 0, playerConstants.maxSteeringForce);
    }

    
 
    //SPEED REGULATION//
    public void ConsciousState() 
    {
        //handle exhaustion lovic
        _handleExhaustionTick();
        
        _generalBoidBehavior();
        
    }

    public void UnconsciousState()
    {

    }

    private void _handleExhaustionTick()
    {
        //tick exhaustion if timer has elapsed
        _exhaustTimer += Time.deltaTime;
        if (_exhaustTimer > playerConstants.exhastionTickFreq)
        {
            //calculate new exhaust
            currentExhaust = SpeedExhaustReg.TickExhaust(currentExhaust, _currentSpeed,
                maxVelocity,playerConstants);
            
            
            if(currentExhaust <= 0)
                //setState(PlayerState.Unconscious);

                _exhaustTimer = 0;
        }
    }
    
    //GET/SET///
    public void SetSnitchObj(GameObject newSnitch)
    {
        _snitchObj = newSnitch;
        
    }
    public void SetState(PlayerState newState) { state = newState; }
    public PlayerState GETState() { return state; }

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
