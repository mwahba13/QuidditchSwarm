using UnityEngine;
using System.Collections;

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

    //how often the player's exhaustion increment
    public float exhaustTickFreq;

    //Player traits, all between 0.0 and 1.0
    [SerializeField]
    protected float _aggro;
    protected float _maxExhaust;
    protected float _currentExhaust;
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

    private float _currentSpeed;


    private Player_SpeedReg _speedReg;


    public void Start()
    {

        _snitchObj = GameObject.FindGameObjectWithTag("Snitch");
        TryGetComponent<Rigidbody>(out _rb);
        generateTraitValues();
        setState(PlayerState.Conscious);

        _speedReg = gameObject.AddComponent<Player_SpeedReg>() as Player_SpeedReg;
        _speedReg.setSpeedExhaustValues(exhaustTickFreq, _maxExhaust, _maxVelocity);
         
    }


    public void FixedUpdate()
    {

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
                    Debug.DrawLine(transform.position, col.transform.position);
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

    public void setState(PlayerState newState) { _state = newState; }
    public PlayerState getState() { return _state; }

    

    public void consciousState() 
    {
        float distToSnitch = (_snitchObj.transform.position - transform.position).magnitude;

        _currentSpeed = _speedReg.regulateSpeed(_currentSpeed,distToSnitch,_aggro);
        _generalBoidBehavior();
        teamSpecificBehavior();
        
    
    }

    public void unconsciousState()
    {

    }

    //ABSTRACT METHODS//
    public abstract void generateTraitValues();

    public abstract void teamSpecificBehavior();
}
