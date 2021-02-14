using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using ScriptableObjs;

public class Gryffindor : PlayerBase
{
    private float _avoidance;

    //Slytherin player that this agent is trying to run away from
    private GameObject _currentChaser;
    
    public GryffindorTraits gryffTraits;
    
    //INIT//
    public override void GenerateTraitValues()
    {
        weight = GaussHouse.GenerateGaussianFloat(gryffTraits.weight.x,gryffTraits.weight.y);
        maxVelocity = GaussHouse.GenerateGaussianFloat(gryffTraits.maxVelocity.x, gryffTraits.maxVelocity.y);
        aggro = GaussHouse.GenerateGaussianFloat(gryffTraits.aggression.x, gryffTraits.aggression.y);
        maxExhaust = GaussHouse.GenerateGaussianFloat(gryffTraits.maxExhaustion.x,gryffTraits.maxExhaustion.y);
        
        //gryffindor specific traits

    }


    public override Vector3 TeamSpecificSeperation(Collider[] neighbours)
    {
        Vector3 newVec = Vector3.zero;

        foreach (Collider neigh in neighbours)
        {
            if (neigh.gameObject.CompareTag("Gryffindor"))
                newVec += NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position)
                                                 * playerConstants.neighbourAvoidanceWeight);
            
            else if (neigh.gameObject.CompareTag("Slytherin"))
            {
                
            }
            
        }

        return newVec;  
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Slytherin"))
            CollisionHandler.ResolveDiffTeamCollision(this.GetComponent<PlayerBase>(),other.gameObject.GetComponent<PlayerBase>());
        else if(other.gameObject.CompareTag("Gryffindor"))
            CollisionHandler.ResolveSameTeamCollision(this.gameObject,other.gameObject);
       

    }
}















