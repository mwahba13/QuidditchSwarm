using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using ScriptableObjs;
using Random = System.Random;

public class Gryffindor : PlayerBase
{

    public GryffindorTraits gryffTraits;

    private float _crunknessLevel;
    
    //INIT//
    public override void GenerateTraitValues()
    {
        weight = GaussHouse.GenerateGaussianFloat(gryffTraits.weight.x,gryffTraits.weight.y);
        maxVelocity = GaussHouse.GenerateGaussianFloat(gryffTraits.maxVelocity.x, gryffTraits.maxVelocity.y);
        aggro = GaussHouse.GenerateGaussianFloat(gryffTraits.aggression.x, gryffTraits.aggression.y);
        maxExhaust = GaussHouse.GenerateGaussianFloat(gryffTraits.maxExhaustion.x,gryffTraits.maxExhaustion.y);
        
        //gryffindor specific traits
        _crunknessLevel = CalculateCrunkLevel();

    }

    //todo: team specific tick behaviors
    
    public override Vector3 TeamSpecificBehavior()
    {
        Vector3 newVec = Vector3.zero;
        
        //calculate crunkness vector
        newVec += SpeedExhaustReg.NormalizeSteeringForce(CalculateCrunkVector(),
            playerConstants.maxSteeringForce)*gryffTraits.crunknessWeighting;


        return newVec;
    }

    //TODO:TEAM TRAITS X2
    //called only when overlapped sphere with neighbours
    public override Vector3 TeamSpecificSeperation(Collider[] neighbours)
    {
        Vector3 newVec = Vector3.zero;

        foreach (Collider neigh in neighbours)
        {
            //avoid teammates, 
            if (neigh.gameObject.CompareTag("Gryffindor") || neigh.gameObject.CompareTag("Environment")
                                                          || neigh.gameObject.CompareTag("Ground"))
            {
                if ((transform.position - neigh.transform.position).magnitude <=
                    playerConstants.neighbourAvoidanceRadius)
                {
                    newVec += SpeedExhaustReg.NormalizeSteeringForce((transform.position - neigh.gameObject.transform.position)
                        ,playerConstants.maxSteeringForce)* playerConstants.neighbourAvoidanceWeight;
                }
                if(playerConstants.showNeighbourLineTraces)
                    Debug.DrawLine(transform.position,neigh.transform.position,Color.magenta,duration:2.0f);
            }
                
            /*
            else if (neigh.gameObject.CompareTag("Slytherin"))
            {
                
            }
            */

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

    
    
    //CRUNKNESS//

    private Vector3 CalculateCrunkVector()
    {
        Vector3 crunkVec = Vector3.zero;
        
        //intermittently toggle an up/right or down/left vector
        float sinVal = Mathf.Abs(Mathf.Sin(Time.time));
        float crunkPower = gryffTraits.crunkPower * gryffTraits.crunknessWeighting;
        
        if (sinVal < 0.25f || (sinVal >= 0.5f && sinVal < 0.75))
        {
            crunkVec += Vector3.up*crunkPower;
            crunkVec += Vector3.right*crunkPower;
            _rb.AddForce(Vector3.up*crunkPower,ForceMode.Impulse);
        }
            
        else if ((sinVal >= 0.25f && sinVal < 0.5f) || sinVal >= 0.75)
        {
           crunkVec += Vector3.down*crunkPower;
           crunkVec += Vector3.left*crunkPower;
           _rb.AddForce(Vector3.down*crunkPower,ForceMode.Impulse);
        }
            
        
        //add a random vector
        
        if(gryffTraits.showCrunknessVector)
            Debug.DrawRay(transform.position,crunkVec,Color.yellow);

        return crunkVec;

    }
    

    //calculates a float value based on crunk level
    private float CalculateCrunkLevel()
    {
        float crunkLevel = GaussHouse.GenerateGaussianFloat(gryffTraits.crunkness.x, gryffTraits.crunkness.y);

        return ((crunkLevel) / 100);

    }
    
    
}















