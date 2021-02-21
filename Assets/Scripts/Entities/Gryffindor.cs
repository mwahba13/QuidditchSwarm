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

    //TODO:TEAM TRAITS X2
    public override Vector3 TeamSpecificSeperation(Collider[] neighbours)
    {
        Vector3 newVec = Vector3.zero;

        foreach (Collider neigh in neighbours)
        {
            //avoid teammates, 
            if (neigh.gameObject.CompareTag("Gryffindor"))
                newVec += SpeedExhaustReg.NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position)
                    ,playerConstants.maxSteeringForce)* playerConstants.neighbourAvoidanceWeight;
            
            else if (neigh.gameObject.CompareTag("Slytherin"))
            {
                
            }
            if(playerConstants.showNeighbourLineTraces)
                Debug.DrawLine(transform.position,neigh.transform.position,Color.magenta,duration:2.0f);
            
        }
        
        if(playerConstants.showSeperationVector)
            Debug.DrawRay(transform.position,newVec,Color.green);
        
        return newVec;  
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Slytherin"))
            CollisionHandler.ResolveDiffTeamCollision(this.GetComponent<PlayerBase>(),other.gameObject.GetComponent<PlayerBase>());
        else if(other.gameObject.CompareTag("Gryffindor"))
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
}















