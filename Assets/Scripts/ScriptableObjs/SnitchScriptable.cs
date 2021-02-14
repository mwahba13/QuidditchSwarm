using UnityEngine;

namespace ScriptableObjs
{
    [CreateAssetMenu(fileName = "SnitchScriptable", menuName = "SnitchScriptable", order = 0)]
    public class SnitchScriptable : ScriptableObject
    {
        [Header("Snitch Properties")]
        
        [Tooltip("Time after which snitch will choose a new random direction")]
        public float directionTimer;

        public float snitchSpeed;

        [Tooltip("Radius that detects collisions objects to avoid")]
        public float collisionRadiusDetection;
        
        [Header("Debug")] 
        
        [Tooltip("Show snitches new calculated direction")]
        public bool showNewDirection;

    }
}