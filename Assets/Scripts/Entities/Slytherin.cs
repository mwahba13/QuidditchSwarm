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


    public override Vector3 TeamSpecificSeperation(Collider[] neighbours)
    {
        Vector3 newVec = Vector3.zero;

        foreach (Collider neigh in neighbours)
        {
            //avoid teammates
            if (neigh.gameObject.CompareTag("Slytherin"))
                newVec += NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position)
                                                 * playerConstants.neighbourAvoidanceWeight);

            else if (neigh.gameObject.CompareTag("Gryffindor"))
            {
                //will try to ram gryffindor depending how much of a brute this agent is
                newVec += NormalizeSteeringForce((neigh.gameObject.transform.position - transform.position)
                                                 * CalculateBruiserRate());
            }
            

            
            if(playerConstants.showNeighbourLineTraces)
                Debug.DrawLine(transform.position,neigh.transform.position,Color.white);
        }
        

        
        return newVec;
    }


    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Gryffindor"))
            CollisionHandler.ResolveDiffTeamCollision(this.GetComponent<PlayerBase>(),other.gameObject.GetComponent<PlayerBase>());
        else if(other.gameObject.CompareTag("Slytherin"))
            CollisionHandler.ResolveSameTeamCollision(this.gameObject,other.gameObject);
    }


    //calculates weighting which dictates how hard this agent willl chase down gryffindors
    private float CalculateBruiserRate()
    {
        return ((_bruiserLevel + aggro) / 100) * slythTraits.bruiserWeighting;
    }
}


