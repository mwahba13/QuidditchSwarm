using System.Collections;
using System.Collections.Generic;
using ScriptableObjs;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    public GryffindorTraits gryffTraits;
    public SlytherinTraitsScriptable slythTraits;

    
    //PROBABLY A BETTER WAY TO DO THIS BUT YOOOLOOOO
    public Text gryffWeight_mean;
    public Text gryffWeight_std;
    public Text slythWeight_mean;
    public Text slythWeight_std;

    public Text gryffMaxVel_mean;
    public Text gryffMaxVel_std;
    public Text slythMaxVel_mean;
    public Text slythMaxVel_std;

    public Text gryffAggro_mean;
    public Text gryffAggro_std;
    public Text slythAggro_mean;
    public Text slythAggro_std;
    
    public Text gryffMaxExhaust_mean;
    public Text gryffMaxExhaust_std;
    public Text slythMaxExhaust_mean;
    public Text slythMaxExhaust_std;

    public Text gryffTrait1_mean;
    public Text gryffTrait1_std;
    public Text slythTrait1_mean;
    public Text slythTrait1_std;
    
    public Text gryffTrait2_mean;
    public Text gryffTrait2_std;
    public Text slythTrait2_mean;
    public Text slythTrait2_std;

    
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateTextFields();
         
    }


    
    //copy serializable into fields into text UI
    private void UpdateTextFields()
    {
        gryffWeight_mean.text = gryffTraits.weight.x.ToString();
        Debug.Log(gryffTraits.weight.x.ToString());
        gryffWeight_std.text = gryffTraits.weight.y.ToString();

    }

    //event for when start game button is clicked
    
    public void OnStartButtonClick()
    {
        
    }

    //writes the contents of each text field into the serializable obj used during runtime
    private void WriteTextFieldsToSerializable()
    {
        
    }
}
