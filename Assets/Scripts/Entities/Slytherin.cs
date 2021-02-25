using System;
using UnityEngine;
using System.Collections;
using ScriptableObjs;
using Random = UnityEngine.Random;

public class Slytherin : PlayerBase
{

    private float _bruiserLevel;
    
    //the current Gryffindor player this agent is trying to chase down
    private GameObject _currentTarget;
    
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
        
        
    }
    
    public override Vector3 TeamSpecificBehavior()
    {
        Vector3 newVec = Vector3.zero;
        
        

        return newVec;
    }

    //todo team traits
    public override Vector3 TeamSpecificSeperation(Collider[] neighbours)
    {
        Vector3 newVec = Vector3.zero;

        foreach (Collider neigh in neighbours)
        {
            
            //avoid teammates, ground and environment
            if (neigh.gameObject.CompareTag("Slytherin") || neigh.gameObject.CompareTag("Environment")
                                                         || neigh.gameObject.CompareTag("Ground"))
            {
                if ((neigh.transform.position - transform.position).magnitude <=
                    playerConstants.neighbourAvoidanceRadius)
                {
                    newVec += SpeedExhaustReg.NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position),
                        playerConstants.maxSteeringForce) * playerConstants.neighbourAvoidanceWeight;
                }
                
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


    //calculates weighting which dictates how hard this agent willl chase down gryffindors
    private float CalculateBruiserLevel()
    {
        float bruiserLevel = GaussHouse.GenerateGaussianFloat(slythTraits.bruiserLevel.x, slythTraits.bruiserLevel.y);
        
        return ((bruiserLevel + aggro) / 100);
    }
}


