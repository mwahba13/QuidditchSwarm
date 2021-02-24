using ScriptableObjs;
using UnityEngine;
using UnityEngine.UI;

namespace System
{
    public class DebugMenu : MonoBehaviour
    {
        public GryffindorTraitsScriptable gryffTraits;
        public SlytherinTraitsScriptable slythTraits;
        public PlayerConstants agentTraits;
        public SnitchScriptable snitchTraits;
        
        //gryffindor
        public InputField gryffCrunkField;
        public Toggle gryffCrunkVectorToggle;
        public InputField gryffBuddyField;
        public Toggle gryffBuddyToggle;

        //slytherin
        public InputField slythBruiseField;
        public Toggle slythBruiseVectorToggle;
        
        //general agent
        public Toggle snitchVector;
        public Toggle seperationVector;
        public Toggle agentVelocity;

        public InputField snitchFollowField;
        public InputField neighbourAvoidField;
        
        //snitch settings
        public InputField snitchAccel;
        public InputField snitchMaxVel;

        public Toggle environmentAvoidVector;
        public Toggle randomVector;
        public Toggle showDirectionVector;


        public void UpdateUIFields()
        {
            //gryff
            gryffBuddyToggle.isOn = gryffTraits.showBuddyVector;
            gryffCrunkVectorToggle.isOn = gryffTraits.showCrunknessVector;
            
            gryffCrunkField.text = gryffTraits.crunknessWeighting.ToString();
            gryffBuddyField.text = gryffTraits.buddySystemWeighting.ToString();
            
            //slyth
            slythBruiseField.text = slythTraits.bruiserWeighting.ToString();

            slythBruiseVectorToggle.isOn = slythTraits.showBruiserVector;
            
            //agent traits
            
            snitchVector.isOn = agentTraits.showVectorTowardSnitch;
            seperationVector.isOn = agentTraits.showSeperationVector;
            agentVelocity.isOn = agentTraits.showAgentVelocity;

            neighbourAvoidField.text = agentTraits.neighbourAvoidanceRadius.ToString();
            snitchFollowField.text = agentTraits.snitchFollowWeight.ToString();
            
            //snitch traits
            
            environmentAvoidVector.isOn = snitchTraits.showEnvironmentAvoidVector;
            showDirectionVector.isOn = snitchTraits.showDirectionVector;
            randomVector.isOn = snitchTraits.showRandomVector;
    
            snitchAccel.text = snitchTraits.snitchSpeed.ToString();
            snitchMaxVel.text = snitchTraits.maxVelocity.ToString();

        }


        public void ResetEverything()
        {
            environmentAvoidVector.isOn = false;
            showDirectionVector.isOn = false;
            randomVector.isOn = false;
            UpdateAgentTraits();

            snitchVector.isOn = false;
            seperationVector.isOn = false;
            agentVelocity.isOn = false;
            UpdateSnitchTraits();

            gryffBuddyToggle.isOn = false;
            gryffCrunkVectorToggle.isOn = false;
            UpdateAgentTraits();

            slythBruiseVectorToggle.isOn = false;
            UpdateSlythTraits();


        }
        
        public void UpdateGryffTraits()
        {
            gryffTraits.crunknessWeighting = float.Parse(gryffCrunkField.text);
            gryffTraits.buddySystemWeighting = float.Parse(gryffBuddyField.text);
            gryffTraits.showBuddyVector = gryffBuddyToggle.isOn;
            gryffTraits.showCrunknessVector = gryffCrunkVectorToggle.isOn;


        }

        public void UpdateSlythTraits()
        {
            slythTraits.bruiserWeighting = float.Parse(slythBruiseField.text);

            slythTraits.showBruiserVector = slythBruiseVectorToggle.isOn;
        }
        
        
        public void UpdateAgentTraits()
        {
            agentTraits.showVectorTowardSnitch = snitchVector.isOn;
            agentTraits.showSeperationVector = seperationVector.isOn;
            agentTraits.showAgentVelocity = agentVelocity.isOn;

            agentTraits.neighbourAvoidanceRadius = float.Parse(neighbourAvoidField.text);
            agentTraits.snitchFollowWeight = float.Parse(snitchFollowField.text);
        }

        public void UpdateSnitchTraits()
        {
            snitchTraits.showEnvironmentAvoidVector = environmentAvoidVector.isOn;
            snitchTraits.showDirectionVector = showDirectionVector.isOn;
            snitchTraits.showRandomVector = randomVector.isOn;

            snitchTraits.snitchSpeed = float.Parse(snitchAccel.text);
            snitchTraits.maxVelocity = float.Parse(snitchMaxVel.text);
        }
        
        

        

    }
}