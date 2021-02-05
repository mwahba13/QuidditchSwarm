using UnityEngine;
using System.Collections;

public class Gryffindor_AI : Player_Base
{



    private Transform _snitchPos;

  
    public void Update()
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

    //STATE BEHAVIOURS//

    private void unconsciousState()
    {
        
    }


    private void consciousState()
    {

    }

    

    //INIT//
    public override void generateTraitValues()
    {
        _weight = GaussHouse.generateGaussianFloat(75, 12);
        _maxVelocity = GaussHouse.generateGaussianFloat(18, 2);
        _aggro = GaussHouse.generateGaussianFloat(22, 3);
        _maxExhaust = GaussHouse.generateGaussianFloat(65, 13);
    }

    private void _setSnitchPos(GameObject snitch) { _snitchPos = snitch.transform; }

}
