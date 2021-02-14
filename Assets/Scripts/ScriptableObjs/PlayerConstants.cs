using UnityEngine;

namespace ScriptableObjs
{
    [CreateAssetMenu(fileName = "PlayerConstantsObj", menuName = "PlayerConstantsObj", order = 0)]
    public class PlayerConstants : ScriptableObject
    {
        [Header("Exhaustion Values")]
        
        [Tooltip("How many seconds before players exhaustion ticks")]
        public int exhastionTickFreq;

        [Tooltip("The maximum amount of exhaustion that can be subtracted from a player on a tick")]
        public int maxExhaustionDepletion;

        [Tooltip("Amount of exhaustion that can be regained on a tick")]
        public float exhaustRegainValue;
        
        [Tooltip("If agent's currentSpeed/MaxSpeed ratio is above this threshold, it will exhaust")]
        public float exhaustThreshold;
        
        
        
        [Header("Boid Values")]
        
        [Tooltip("Radius that agents will detect neighbours within")]
        public float neighbourDetctionRadius;

        [Tooltip("Weighting for how hard an agent should try to avoid neighbour")]
        public float neighbourAvoidanceWeight;

        [Tooltip("Weighting for how hard the force towards the snitch is")]
        public float snitchFollowWeight;

        [Tooltip("Speed modifier to speed up or slow down all agents")]
        public float speedTuningValue;

        public float maxSteeringForce;
        
        
        [Header("Debug")]
        
        [Tooltip("Shows lines between nearest detected neighbours")]
        public bool showNeighbourLineTraces;

        [Tooltip("Shows agent direction vector")]
        public bool showAgentVector;

    }
}