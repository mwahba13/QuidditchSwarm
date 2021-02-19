using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState : uint { Playing, Paused, MainMenu}

    private static SpawnManager _spawnManager;

    private static GameState _gameState;

    
    
    
    //todo main menu, initialize team numbers and stats
    
    private void Start()
    {
        _gameState = GameState.MainMenu;
        
        StartGame();

    }





    public void SetupGame()
    {

    }

    public void StartGame()
    {
        
    }


}
