using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    private SpawnManager _spawnManager;
    
    public GameObject mainMenuObj;
    public GameObject debugMenuObj;
    public GameObject scoreboardObj;
    public GameObject inGameUIObj;
    


    private static CameraController _cameraController;
    
    //todo ingame debug menu
    //todo main menu, initialize team numbers and stats
    
    private void Start()
    {
        _cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
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

    public void OnDebugMenuButtonClicked()
    {
        debugMenuObj.SetActive(!debugMenuObj.activeSelf);
    }
    
    //event for end of game
    public void OnGameOver()
    {
        
    }




}
