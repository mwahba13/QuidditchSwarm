using UnityEngine;

namespace ScriptableObjs
{
    [CreateAssetMenu(fileName = "SlytherinTraitsScriptable", menuName = "SlytherinTraitsScriptable", order = 0)]
    public class SlytherinTraitsScriptable : ScriptableObject
    {
        [Header("Common Traits (Mean, STD)")] 
        
        public Vector2 weight;

        public Vector2 maxVelocity;

        public Vector2 aggression;

        public Vector2 maxExhaustion;
    
        [Header("Slytherin Traits (Mean, STD)")]
        
        [Tooltip("Slytherins are a**holes, they will try to knock out players on the other team.")]
        public Vector2 bruiserLevel;

        [Tooltip("Weighting of a Slytherin's bruiser level. 0 - they ignore gryffindors, 1- they chase gryffindors like junkyard dawgs")]
        public float bruiserWeighting;
    }
}