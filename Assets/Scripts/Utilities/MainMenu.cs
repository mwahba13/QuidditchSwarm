using System.Collections;
using System.Collections.Generic;
using ScriptableObjs;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    public GryffindorTraitsScriptable gryffSerial;
    public SlytherinTraitsScriptable slythSerial;

    public InputField gryffNumPlayers;
    public InputField slythNumPlayers;
    
    //DEFINATELY A BETTER WAY TO DO THIS BUT YOOOLOOOO
    public InputField gryffWeight_mean;
    public InputField gryffWeight_std;
    public InputField slythWeight_mean;
    public InputField slythWeight_std;

    public InputField gryffMaxVel_mean;
    public InputField gryffMaxVel_std;
    public InputField slythMaxVel_mean;
    public InputField slythMaxVel_std;

    public InputField gryffAggro_mean;
    public InputField gryffAggro_std;
    public InputField slythAggro_mean;
    public InputField slythAggro_std;
    
    public InputField gryffMaxExhaust_mean;
    public InputField gryffMaxExhaust_std;
    public InputField slythMaxExhaust_mean;
    public InputField slythMaxExhaust_std;

    public InputField gryffCrunk_mean;
    public InputField gryffCrunk_std;
    public InputField slythBruiser_mean;
    public InputField slythBruiser_std;
    
    public InputField gryffTrait2_mean;
    public InputField gryffTrait2_std;
    public InputField slythTrait2_mean;
    public InputField slythTrait2_std;

    
   
    //copy serializable into fields into text UI
    public void UpdateTextFields()
    {   
        //gryffindor
        gryffNumPlayers.text = gryffSerial.NumberOfPlayers.ToString();
        
        gryffWeight_mean.text = gryffSerial.weight.x.ToString();
        gryffWeight_std.text = gryffSerial.weight.y.ToString();
        
        gryffAggro_mean.text = gryffSerial.aggression.x.ToString();
        gryffAggro_std.text = gryffSerial.aggression.y.ToString();

        gryffMaxExhaust_mean.text = gryffSerial.maxExhaustion.x.ToString();
        gryffMaxExhaust_std.text = gryffSerial.maxExhaustion.y.ToString();

        gryffMaxVel_mean.text = gryffSerial.maxVelocity.x.ToString();
        gryffMaxVel_std.text = gryffSerial.maxVelocity.y.ToString();

        gryffCrunk_mean.text = gryffSerial.crunkness.x.ToString();
        gryffCrunk_std.text = gryffSerial.crunkness.y.ToString();

        gryffTrait2_mean.text = gryffSerial.buddySystem.x.ToString();
        gryffTrait2_std.text = gryffSerial.buddySystem.y.ToString();
        
        
        
        //slytherin
        slythNumPlayers.text = slythSerial.NumberOfPlayers.ToString();
        
        slythWeight_mean.text = slythSerial.weight.x.ToString();
        slythWeight_std.text = slythSerial.weight.y.ToString();
        
        slythAggro_mean.text = slythSerial.aggression.x.ToString();
        slythAggro_std.text = slythSerial.aggression.y.ToString();

        slythMaxExhaust_mean.text = slythSerial.maxExhaustion.x.ToString();
        slythMaxExhaust_std.text = slythSerial.maxExhaustion.y.ToString();

        slythMaxVel_mean.text = slythSerial.maxVelocity.x.ToString();
        slythMaxVel_std.text = slythSerial.maxVelocity.y.ToString();

        slythBruiser_mean.text = slythSerial.bruiserLevel.x.ToString();
        slythBruiser_std.text = slythSerial.bruiserLevel.y.ToString();

        slythTrait2_mean.text = slythSerial.vortexLevel.x.ToString();
        slythTrait2_std.text = slythSerial.vortexLevel.y.ToString();

    }

 
    //writes the contents of each text field into the serializable obj used during runtime
    public void WriteTextFieldsToSerializable()
    {
        //gryffindor
        gryffSerial.NumberOfPlayers = int.Parse(gryffNumPlayers.text);
        
        gryffSerial.weight.x = float.Parse(gryffWeight_mean.text);
        gryffSerial.weight.y = float.Parse(gryffWeight_std.text);

        gryffSerial.aggression.x = float.Parse(gryffAggro_mean.text);
        gryffSerial.aggression.y = float.Parse(gryffAggro_std.text);

        gryffSerial.maxExhaustion.x = float.Parse(gryffMaxExhaust_mean.text);
        gryffSerial.maxExhaustion.y = float.Parse(gryffMaxExhaust_std.text);

        gryffSerial.maxVelocity.x = float.Parse(gryffMaxVel_mean.text);
        gryffSerial.maxVelocity.y = float.Parse(gryffMaxVel_std.text);

        gryffSerial.crunkness.x = float.Parse(gryffCrunk_mean.text);
        gryffSerial.crunkness.y = float.Parse(gryffCrunk_std.text);

        gryffSerial.buddySystem.x = float.Parse(gryffTrait2_mean.text);
        gryffSerial.buddySystem.y = float.Parse(gryffTrait2_std.text);
        
        
        //slytherin
        slythSerial.NumberOfPlayers = int.Parse(slythNumPlayers.text);
        
        slythSerial.weight.x = float.Parse(slythWeight_mean.text);
        slythSerial.weight.y = float.Parse(slythWeight_std.text);

        slythSerial.aggression.x = float.Parse(slythAggro_mean.text);
        slythSerial.aggression.y = float.Parse(slythAggro_std.text);

        slythSerial.maxExhaustion.x = float.Parse(slythMaxExhaust_mean.text);
        slythSerial.maxExhaustion.y = float.Parse(slythMaxExhaust_std.text);

        slythSerial.maxVelocity.x = float.Parse(slythMaxVel_mean.text);
        slythSerial.maxVelocity.y = float.Parse(slythMaxVel_std.text);

        slythSerial.bruiserLevel.x = float.Parse(slythBruiser_mean.text);
        slythSerial.bruiserLevel.y = float.Parse(slythBruiser_std.text);

        slythSerial.vortexLevel.x = float.Parse(slythTrait2_mean.text);
        slythSerial.vortexLevel.y = float.Parse(slythTrait2_std.text);

    }
}
