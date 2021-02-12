using UnityEngine;
using System.Collections;
using ScriptableObjs;

[RequireComponent( typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Player_Base : MonoBehaviour
{

    //This class handles behaviours common across players on both teams (i.e. movement etc.)

    public enum PlayerState : uint
    {

        Unconscious, Conscious

    }

    public bool DEBUG = true;


    //Player traits, all between 0.0 and 1.0
    [SerializeField]
    protected float _aggro;
    [SerializeField]
    protected float _maxExhaust;
    [SerializeField]
    protected float _currentExhaust = 0;
    [SerializeField]
    protected float _maxVelocity;
    [SerializeField]
    protected float _weight;

    // radius where agent checks for neighbours
    [SerializeField]
    protected float _neighbourAvoidanceRadius;
    

    protected PlayerState _state;
    protected Rigidbody _rb;
    protected GameObject _snitchObj;

    

    public PlayerConstants playerConstants;

    private float _currentSpeed;
    private float _exhaustTimer;
    
    public void Start()
    {
        TryGetComponent<Rigidbody>(out _rb);
        generateTraitValues();
        setState(PlayerState.Conscious);

        _exhaustTimer = playerConstants.exhastionTickFreq;
    }


    public void FixedUpdate()
    {
        
        //tick exhaustion if timer has elapsed
        _exhaustTimer += Time.deltaTime;
        if (_exhaustTimer > playerConstants.exhastionTickFreq)
        {
            //calculate new exhaust
            _currentExhaust = SpeedExhaustReg.tickExhaust(_currentExhaust, _currentSpeed,
                _maxVelocity,playerConstants.maxExhaustionDepletion,playerConstants.exhaustThreshold);
            
            if(_currentExhaust <= 0)
                setState(PlayerState.Unconscious);

            _exhaustTimer = 0;
        }
        
        
        

            switch (getState())
            {
                case PlayerState.Conscious:
                    consciousState();
                    break;

                case PlayerState.Unconscious:
                    unconsciousState();
                    break;
            }

    }

    //MOVEMENT RELATED METHODS//
    //*SEPERATION NEEDS WORK
    protected void _generalBoidBehavior()
    {
        //point agent towards the Snitch
        transform.LookAt(_snitchObj.transform);
        Vector3 dirToSnitch = _snitchObj.transform.position - transform.position;
        dirToSnitch = dirToSnitch.normalized;
        
        //SEPERATION
        //get neighbours within radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _neighbourAvoidanceRadius);
        if(hitColliders.Length > 0)
        {
            if(DEBUG)
                Debug.Log("Num Neighbours: " + hitColliders.Length);
            
            Vector3 avgNeighbourVector = new Vector3();
            //generate avg vector of neighbours
            foreach (Collider col in hitColliders)
            {
                
                if (col.gameObject.GetComponent<Player_Base>())
                    avgNeighbourVector += col.gameObject.transform.position.normalized;

                if (DEBUG)
                    Debug.DrawLine(transform.position, col.transform.position,Color.red);
            }

            avgNeighbourVector /= hitColliders.Length;
            dirToSnitch += avgNeighbourVector;
        }

        //dirToSnitch is now normalized and seperating from neighbours
        _rb.velocity = dirToSnitch*_currentSpeed;

        if (DEBUG)
            Debug.DrawRay(transform.position, dirToSnitch*_currentSpeed);

    }


    //SPEED REGULATION//
    public void consciousState() 
    {
        float distToSnitch = (_snitchObj.transform.position - transform.position).magnitude;
        _currentSpeed = 5.0f;
        _currentSpeed = SpeedExhaustReg.regulateSpeed(_currentSpeed,_maxVelocity,distToSnitch,_aggro,_currentExhaust);
        _generalBoidBehavior();
        teamSpecificBehavior();
        
    
    }

    public void unconsciousState()
    {

    }
    
    
    //GET/SET///
    public void setSnitchObj(GameObject newSnitch)
    {
        _snitchObj = newSnitch;
        
    }
    public void setState(PlayerState newState) { _state = newState; }
    public PlayerState getState() { return _state; }

    //ABSTRACT METHODS//
    public abstract void generateTraitValues();

    public abstract void teamSpecificBehavior();
}
