using System;
using UnityEngine;

namespace ScriptableObjs
{
    [CreateAssetMenu(fileName = "PlayerConstantsObj", menuName = "PlayerConstantsObj", order = 0)]
    public class PlayerConstants : ScriptableObject
    {
        [Header("Exhaustion Values")]
        
        [Tooltip("How many seconds before players exhaustion ticks")]
        public int exhastionTickFreq;

        [Tooltip("Amount of time that player remains inactive when unconscious.")]
        public float inactiveTime;

        [Tooltip("Maximum amount an agent will slowdown when close to exhaustion.")]
        public float exhastSlowdownWeight;
        
        [Header("Boid Values")]
        
        [Tooltip("Radius that agents will detect neighbours within")]
        public float neighbourDetectionRadius;

        [Tooltip("Radius that agents will actually avoid neighbours")]
        public float neighbourAvoidanceRadius;
        
        [Tooltip("Weighting for how hard an agent should try to avoid neighbour")]
        public float neighbourAvoidanceWeight;

        [Tooltip("Weighting for how hard the force towards the snitch is")]
        public float snitchFollowWeight;

        [Tooltip("Weighting for how hard the force to avoid the environment is.")]
        public float environmentAvoidanceWeight;
        
       
        public float environmentAvoidanceRadius;
        
        [Tooltip("Speed modifier to speed up or slow down all agents")]
        public float speedTuningValue;

        public float maxSteeringForce;
        
        
        [Header("Debug")]
        
        [Tooltip("Shows lines between nearest detected neighbours/obstacles - Magenta")]
        public bool showNeighbourLineTraces;

        [Tooltip("Shows vector towards snitch - Red")]
        public bool showVectorTowardSnitch;

        [Tooltip("Shows seperation vector - Green")]
        public bool showSeperationVector;

        [Tooltip("Shows final calculated velocity - White")]
        public bool showAgentVelocity;

    }
}