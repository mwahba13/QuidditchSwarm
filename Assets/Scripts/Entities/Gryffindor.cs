using UnityEngine;
using System.Collections;

public class Gryffindor : Player_Base
{
    
    //INIT//
    public override void generateTraitValues()
    {
        _weight = GaussHouse.generateGaussianFloat(75, 12);
        _maxVelocity = GaussHouse.generateGaussianFloat(18, 2);
        _aggro = GaussHouse.generateGaussianFloat(22, 3);
        _maxExhaust = GaussHouse.generateGaussianFloat(65, 13);
    }


    public override void teamSpecificBehavior()
    {
       
    }
}