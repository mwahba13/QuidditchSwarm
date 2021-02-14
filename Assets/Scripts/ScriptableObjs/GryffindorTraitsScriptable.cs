using UnityEngine;

namespace ScriptableObjs
{
    [CreateAssetMenu(fileName = "GryffindorTraitsScriptable", menuName = "GryffindorTraitsScriptable", order = 0)]
    public class GryffindorTraits : ScriptableObject
    {
        [Header("Traits (Mean,STD)")] 
        
        public Vector2 weight;

        public Vector2 maxVelocity;

        public Vector2 aggression;

        public Vector2 maxExhaustion;

        //[Header("Gryffindor Traits (Mean, STD)")]

    }
}