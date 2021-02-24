using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private SpawnManager _spawnManager;
    
    public GameObject mainMenuObj;
    public GameObject debugMenuObj;
    public GameObject scoreboardObj;
    public GameObject inGameUIObj;

    private static GameObject _winScreen;


    private static CameraController _cameraController;
    

    
    private void Start()
    {
        _cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        _winScreen = GameObject.FindWithTag("WinScreen");
        _spawnManager = GetComponent<SpawnManager>();
        
        
        ShowMainMenu();
        

    }
    
    //MENU HANDLING//

    private void ShowMainMenu()
    {
        //show main menu, hide debug menu and scoreboard
        mainMenuObj.SetActive(true);
        debugMenuObj.SetActive(false);
        scoreboardObj.SetActive(false);
        inGameUIObj.SetActive(false);

        mainMenuObj.GetComponent<MainMenu>().UpdateTextFields();
        
        _cameraController.ParalyzePlayer();
    }

    private void ShowDebugMenu()
    {
        debugMenuObj.SetActive(true);
    }

    
    //EVENTS//
    //event for when start button is pressed
    public void OnStartButtonClicked()
    {
        MainMenu mainMen = mainMenuObj.GetComponent<MainMenu>();
        
        mainMen.WriteTextFieldsToSerializable();
        mainMenuObj.SetActive(false);
        
        debugMenuObj.GetComponent<DebugMenu>().ResetEverything();
        
        scoreboardObj.SetActive(true);
        inGameUIObj.SetActive(true);
        
        _spawnManager.SpawnTeam(int.Parse(mainMen.gryffNumPlayers.text),true);
        _spawnManager.SpawnTeam(int.Parse(mainMen.slythNumPlayers.text),false);
        
        _cameraController.UnparalyzePlayer();
    }


    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0;
    }

    public void OnPlayButtonClicked()
    {
        Time.timeScale = 1;
    }

    public void OnResetButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void OnDebugMenuButtonClicked()
    {
        debugMenuObj.SetActive(!debugMenuObj.activeSelf);
        debugMenuObj.GetComponent<DebugMenu>().UpdateUIFields();
    }
    
    //event for end of game
    public static void GameOver(PlayerBase.Team winningTeam)
    {
        _winScreen.SetActive(true);
        _winScreen.GetComponentInChildren<Text>().text = winningTeam.Equals(PlayerBase.Team.Gryffindor) ? 
            "Gryffindor Wins!" : "Slytherin Wins!";
        
    }




}
