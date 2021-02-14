using UnityEngine;
using System.Collections;
using System;

public class GaussHouse : MonoBehaviour
{
    //This class will be used for generating our random gaussian distributions

    private static System.Random _rand = new System.Random();

    //Uses Box-Muller Transform-https://stackoverflow.com/questions/218060/random-gaussian-variables
    public static float GenerateGaussianFloat (float x,float std)
    {


        double u1 = 1.0 - _rand.NextDouble();
        double u2 = 1.0 - _rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
            Math.Sin(2.0 * Math.PI * u2);

        return (float)(x + std * randStdNormal);
    }


    
    
    
}
