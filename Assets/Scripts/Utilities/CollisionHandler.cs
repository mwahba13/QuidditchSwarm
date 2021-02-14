using UnityEngine;
using System.Collections;
namespace System
{
    public class CollisionHandler : MonoBehaviour
    {
        //ref to last handled collision
        private static Collision _lastCollision = null;
        
        
        private static System.Random _sysRand = new System.Random();
        
        
        //resolves collisions between members of different teams
        public static void ResolveDiffTeamCollision(PlayerBase play1, PlayerBase play2)
        {
            
            Debug.Log("Enter collision resolution b/w " + play1.GetInstanceID() + " & " + play2.GetInstanceID());
            
            //first check if this collision already happened by seeing if both players are conscious
            if (!play1.GETState().Equals(PlayerBase.PlayerState.Unconscious) &&
                !play2.GETState().Equals(PlayerBase.PlayerState.Unconscious))
            {
                Debug.Log("Inner collision resolution b/w " + play1.GetInstanceID() + " & " + play2.GetInstanceID());
                double play1Val = play1.GetAggro() * (_sysRand.NextDouble() * (1.2 - 0.8) + 0.8)
                                                   * (1 - (play1.GetCurrentExhaust() / play1.GetMaxExhaust()));

                double play2Val = play2.GetAggro() * (_sysRand.NextDouble() * (1.2 - 0.8) + 0.8)
                                                   * (1 - (play2.GetCurrentExhaust() / play2.GetMaxExhaust()));

                if (play1Val > play2Val)
                    play2.SetState(PlayerBase.PlayerState.Unconscious);
                else        
                    play1.SetState(PlayerBase.PlayerState.Unconscious);
            }

        
        }
    
        //resolves collision between members of same team
        public static void ResolveSameTeamCollision(GameObject obj1, GameObject obj2 )
        {
            //check if this is the five percent
            float randFloat = UnityEngine.Random.Range(0.0f, 1.0f);
            if (randFloat < 0.05f)
            {
                ResolveDiffTeamCollision(obj1.GetComponent<PlayerBase>(),obj2.GetComponent<PlayerBase>());    
            }
            
        }



    }
}