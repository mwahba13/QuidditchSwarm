using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



[System.Serializable]
public class ScoreEvent : UnityEvent<PlayerBase.Team>
{}
public class ScoreManager : MonoBehaviour
{

    //reference to UI score
    private static Text gryffScore;
    private static Text slythScore;
    //internal counter of score
    private static int _gryffScoreInt;
    private static int _slythScoreInt;

    private static bool _boardIsHidden;

    //keeps track of the last team to score
    private static PlayerBase.Team _lastScoringTeam;

    private void Start()
    {
        //get the text objects that show the score
        //this is so stupid but the only way to keep this all static
        Text[] textComps = new Text[2];
        textComps = GetComponentsInChildren<Text>();
        if (textComps[0].name.Equals("GScore"))
        {
            gryffScore = textComps[0];
            slythScore = textComps[1];
        }
        else
        {
            slythScore = textComps[0];
            gryffScore = textComps[1];
        }
       
        
        _gryffScoreInt = 0;
        _slythScoreInt = 0;
        UpdateInGameUI();
        
        _lastScoringTeam = PlayerBase.Team.Null;
        _boardIsHidden = false;
    }

    public static void IncrementScore(PlayerBase.Team team)
    {
        int scoreInc = 1;
        if (team == _lastScoringTeam)
            scoreInc++;

        switch (team)
        {
            case PlayerBase.Team.Gryffindor:
                _gryffScoreInt += scoreInc;
                break;
            
            case PlayerBase.Team.Slytherin:
                _slythScoreInt += scoreInc;
                break;
            
            default:
                break;
                
        }
        
        if(!_boardIsHidden)
            UpdateInGameUI();

    }
    
    
    //INGAME UI ///

    public static void HideScoreboard()
    {
        _boardIsHidden = true;
    }

    public static void ShowScoreboard()
    {
        UpdateInGameUI();

        _boardIsHidden = false;

    }
    
    
        
    //updates the in game UI    
    private static void UpdateInGameUI()
    {
        gryffScore.text = _gryffScoreInt.ToString();
        slythScore.text = _slythScoreInt.ToString();
    }
    
    
    //INIT//
    private static void InitTextObjects()
    {
        
    }

}
