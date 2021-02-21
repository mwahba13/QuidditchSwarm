using System;
using UnityEngine;
using System.Collections;
using ScriptableObjs;

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
        _bruiserLevel = GaussHouse.GenerateGaussianFloat(slythTraits.bruiserLevel.x, slythTraits.bruiserLevel.y);
    
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
                newVec += SpeedExhaustReg.NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position),
                              playerConstants.maxSteeringForce) * playerConstants.neighbourAvoidanceWeight;

            else if (neigh.gameObject.CompareTag("Gryffindor"))
            {
                //will try to ram gryffindor depending how much of a brute this agent is
                newVec += SpeedExhaustReg.NormalizeSteeringForce(
                    (neigh.gameObject.transform.position - transform.position), playerConstants.maxSteeringForce)
                          *CalculateBruiserRate();
          
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
    private float CalculateBruiserRate()
    {
        return ((_bruiserLevel + aggro) / 100) * slythTraits.bruiserWeighting;
    }
}


