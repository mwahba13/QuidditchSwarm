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

        [Tooltip("If agent's currentSpeed/MaxSpeed ratio is above this threshold, it will exhaust")]
        public float exhaustThreshold;
        
        
        
        [Header("Boid Values")]
        
        [Tooltip("Radius that players try to stay away from neighbours")]
        public int neighbourAvoidanceRadius;
    }
}