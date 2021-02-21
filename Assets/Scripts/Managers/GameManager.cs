using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState : uint { Playing, Paused, MainMenu}

    private static SpawnManager _spawnManager;
    
    private static GameObject _mainMenuObj;
    private static GameObject _debugMenuObj;
    private static GameObject _scoreboardObj;
    

    private static GameState _gameState;

    private static CameraController _cameraController;
    
    //todo ingame debug menu
    //todo main menu, initialize team numbers and stats
    
    private void Start()
    {
        _gameState = GameState.MainMenu;
        _cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        

    }

    private void MainMenu()
    {
        //show main menu, hide debug menu and scoreboard
        _mainMenuObj.SetActive(true);
        _debugMenuObj.SetActive(false);
        _scoreboardObj.SetActive(false);
    }




}
