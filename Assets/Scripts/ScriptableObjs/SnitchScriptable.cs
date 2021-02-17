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

        public float maxSteeringForce;

        public float maxVelocity;

        [Tooltip("Radius that detects collisions objects to avoid")]
        public float collisionRadiusDetection;

        [Tooltip("How hard the snitch will turn to avoid environment")]
        public float environmentAvoidanceWeighting;

        
        
        [Header("Debug")]
        
        [Tooltip("Shows environment avoidance vector (Red)")]
        public bool showEnvironmentAvoidVector;
        
        [Tooltip("Show snitches new calculated direction (Blue)")]
        public bool showDirectionVector;

    }
}