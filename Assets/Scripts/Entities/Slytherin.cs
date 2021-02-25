using System;
using UnityEngine;
using System.Collections;
using System.Numerics;
using ScriptableObjs;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Slytherin : PlayerBase
{

    private float _bruiserLevel;
    private float _vortexLevel;
    
    //the current Gryffindor player this agent is trying to chase down
    private GameObject _currentTarget;
    
    //vortex center
    private Vector3 _vortexMiddle;
    
    public SlytherinTraitsScriptable slythTraits;
    
    //INIT//
    public override void GenerateTraitValues()
    {
        weight = GaussHouse.GenerateGaussianFloat(slythTraits.weight.x,slythTraits.weight.y);
        maxVelocity = GaussHouse.GenerateGaussianFloat(slythTraits.maxVelocity.x,slythTraits.maxVelocity.y);
        aggro = GaussHouse.GenerateGaussianFloat(slythTraits.aggression.x,slythTraits.aggression.y);
        maxExhaust = GaussHouse.GenerateGaussianFloat(slythTraits.maxExhaustion.x,slythTraits.maxExhaustion.y);
        
        //slytherin specific traits
        _bruiserLevel = CalculateBruiserLevel();

        _vortexLevel = GaussHouse.GenerateGaussianFloat(slythTraits.vortexLevel.x, slythTraits.vortexLevel.y);
        _vortexLevel /= 100;
        _vortexLevel *= slythTraits.vortexingPower;
    }
    
    public override Vector3 TeamSpecificBehavior()
    {
        Vector3 newVec = Vector3.zero;
        
        //vortexing behavior
        /*
        if(slythTraits.showVortexingVector)
            Debug.DrawLine(transform.position,_vortexMiddle,Color.magenta);
*/
        newVec += SpeedExhaustReg.NormalizeSteeringForce(CalculateVortexingVelocity(), playerConstants.maxSteeringForce)
                  * slythTraits.vortexWeighting;
        
        return newVec;
    }

    //todo team traits
    public override Vector3 TeamSpecificSeperation(Collider[] neighbours)
    {
        Vector3 newVec = Vector3.zero;
        
        //for calculating vortex position
        int numTeammates = 1;
        Vector3 centrePos = transform.position;

        foreach (Collider neigh in neighbours)
        {
            
            //avoid teammates, ground and environment
            if (neigh.gameObject.CompareTag("Environment")|| neigh.gameObject.CompareTag("Ground"))
            {
                if ((neigh.transform.position - transform.position).magnitude <=
                    playerConstants.neighbourAvoidanceRadius)
                {
                    newVec += SpeedExhaustReg.NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position),
                        playerConstants.maxSteeringForce) * playerConstants.neighbourAvoidanceWeight;
                }
                
            }

            if (neigh.gameObject.CompareTag("Slytherin"))
            {
                //seperation
                if ((neigh.transform.position - transform.position).magnitude <=
                    playerConstants.neighbourAvoidanceRadius)
                {
                    newVec += SpeedExhaustReg.NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position),
                        playerConstants.maxSteeringForce) * playerConstants.neighbourAvoidanceWeight;
                }
                
                //determine center of local vortex
                numTeammates++;
                centrePos += neigh.gameObject.transform.position;


            }
            
            
            else if (neigh.gameObject.CompareTag("Gryffindor"))
            {
                if ((neigh.transform.position - transform.position).magnitude <= slythTraits.bruiserBludgeonRadius)
                    //will try to ram gryffindor depending how much of a brute this agent is
                {
                    float tempBruiserWeight = _bruiserLevel * slythTraits.bruiserWeighting;
                
                    newVec += SpeedExhaustReg.NormalizeSteeringForce(
                                  (neigh.gameObject.transform.position - transform.position), playerConstants.maxSteeringForce)
                              *tempBruiserWeight;
                }
                
                if(slythTraits.showBruiserVector)
                    Debug.DrawLine(transform.position,neigh.gameObject.transform.position,Color.yellow);
          
            }
            

            
            if(playerConstants.showNeighbourLineTraces)
                Debug.DrawLine(transform.position,neigh.transform.position,Color.magenta,duration:2.0f);
        }
        
        if(playerConstants.showSeperationVector)
            Debug.DrawRay(transform.position,newVec,Color.green);

        
        _vortexMiddle = centrePos / numTeammates;
        
        return newVec;
    }


    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Gryffindor"))
            CollisionHandler.ResolveDiffTeamCollision(this.GetComponent<PlayerBase>(),other.gameObject.GetComponent<PlayerBase>());
        else if(other.gameObject.CompareTag("Slytherin"))
            CollisionHandler.ResolveSameTeamCollision(this.gameObject,other.gameObject);
        
        else if (other.gameObject.CompareTag("Ground"))
        {
            //player is already unconscious and falling
            if (state.Equals(PlayerState.Unconscious))
                TransitionState(PlayerState.Waiting);
            
        }

        else if (other.gameObject.CompareTag("Environment"))
        {
            TransitionState(PlayerState.Unconscious);
        }
        
        else if (other.gameObject.CompareTag("Boundary"))
            _rb.AddForce(-other.gameObject.transform.position);

        
        
    }


    //calculates a vector that is facing forward but propduces an angular velocity around the middle of the swarm
    private Vector3 CalculateVortexingVelocity()
    {
        Vector3 _vecFromMiddle = transform.position - _vortexMiddle;
        Vector3 output = Vector3.Cross(_vecFromMiddle.normalized, _rb.velocity.normalized)*_vortexLevel;
        
        if(slythTraits.showVortexingVector)
            Debug.DrawRay(transform.position,output,Color.magenta);
        return output;
    }


    //calculates weighting which dictates how hard this agent willl chase down gryffindors
    private float CalculateBruiserLevel()
    {
        float bruiserLevel = GaussHouse.GenerateGaussianFloat(slythTraits.bruiserLevel.x, slythTraits.bruiserLevel.y);
        
        return ((bruiserLevel + aggro) / 100);
    }
}


