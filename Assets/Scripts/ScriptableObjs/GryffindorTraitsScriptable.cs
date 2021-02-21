﻿using UnityEngine;

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

        [Header("Gryffindor Traits (Mean, STD)")] 
        
        [Tooltip("Uh Oh, Harry's been hitting the skooma pipe hard these days...")]
        public Vector2 crunkness;

        [Tooltip("How potent that kryptachronicrackalck is.")]
        public float crunkPower;

        [Tooltip("How hard that dankness supreme is hittin' each player")]
        public float crunknessWeighting;


        [Header("Debug")] 
        [Tooltip("Shows crunkness vector in yellow")]
        public bool showCrunknessVector;
    }
}