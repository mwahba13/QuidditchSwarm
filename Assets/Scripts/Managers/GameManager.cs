using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState : uint { Playing, Paused, MainMenu}




    private static SpawnManager _spawnManager;

    private GameState _gameState;

    private void Start()
    {
        _gameState = GameState.MainMenu;
        _spawnManager = new SpawnManager();

    }





    public void setupGame()
    {

    }

    public void startGame()
    {

    }


}
