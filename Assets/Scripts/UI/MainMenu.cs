using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneToLoad = "GameScene";
    [SerializeField] private string mainMenuSceneToLoad = "MainMenu";
    [SerializeField] GameObject mainMenuButtons = null;
    [SerializeField] GameObject inGameButtons = null;
    [SerializeField] SettingsPanel settingsPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject WinPanel;

    private void Start() 
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable() 
    {
        GameManager.OnGameOver += OnGameOver; 
        GameManager.OnWin += SetWinPanelActive;
    }

    public void SetWinPanelActive()
    {
        WinPanel.SetActive(true);
    }   

    public void PlayButton()
    {
        SceneManager.LoadSceneAsync(gameSceneToLoad);
        SetButtonsActive(false);
    }

    public void ExitToMainMenuButton()
    {
        SceneManager.LoadSceneAsync(mainMenuSceneToLoad);
        SetButtonsActive(true);
        GameOverPanel.SetActive(false);
    }

    public void SettingsButton()
    {
        settingsPanel.EnablePanel();
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void RetryButton()
    {
        SceneManager.LoadSceneAsync(gameSceneToLoad);
        GameOverPanel.SetActive(false);
        WinPanel.SetActive(false);
    }

    private void SetButtonsActive(bool mainMenuActive) // Не знаю, как назвать функцию, 
    {                                  // которая включает кнопки меню и выключает игровые
        mainMenuButtons.SetActive(mainMenuActive);
        inGameButtons.SetActive(!mainMenuActive);
    }
}
