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
        public InputField snitchMinVel;
        public InputField snitchMaxVel;

        public Toggle environmentAvoidVector;
        public Toggle randomVector;
        public Toggle showDirectionVector;

        
        //todo update UI to show current values
        
        public void UpdateGryffTraits()
        {
            gryffTraits.crunknessWeighting = float.Parse(gryffCrunkField.text);
        }

        public void UpdateSlythTraits()
        {
            slythTraits.bruiserWeighting = float.Parse(slythBruiseField.text);
        }
        
        
    //TODO: change debug lines to linerenderer
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

            snitchTraits.minVelocity = float.Parse(snitchMinVel.text);
            snitchTraits.maxVelocity = float.Parse(snitchMaxVel.text);
        }

    }
}