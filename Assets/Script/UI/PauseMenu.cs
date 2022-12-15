using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    private PlayerStates playerStates;

    [SerializeField] private GameObject allPauseMenuUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject diedMenuUI;
    [SerializeField] private GameObject saveUI;
    [SerializeField] private GameObject loadUI;
    [SerializeField] private GameObject skillUI;

    [SerializeField] private Button saveButton;
    private DialogueManager dialogueManager;
    private LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = UIManager.instance.UI.GetComponent<LevelLoader>();
        dialogueManager = UIManager.instance.dialogueManager;
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        allPauseMenuUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0 && !dialogueManager.dialoguePlaying)
        {
            if (GameIsPaused && !settingsMenuUI.activeInHierarchy)
            {
                Resume();
                ResumCloseUI();
            }
            else if(!GameIsPaused)
            {
                Pause();
                PauseOpenUI();
            }
        }

        if (playerStates.died)
        {
            Pause();
            diedMenuUI.SetActive(true);
        }

        if (pauseMenuUI.activeInHierarchy)
        {
            if (playerStates.saveAble || SceneManager.GetActiveScene().buildIndex == 3)
            {
                saveButton.interactable = true;
            }
            else if (!playerStates.saveAble)
            {
                saveButton.interactable = false;
            }
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void ResumCloseUI()
    {
        allPauseMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        saveUI.SetActive(false);
        loadUI.SetActive(false);
        skillUI.SetActive(false);
    }
    public void PauseOpenUI()
    {
        allPauseMenuUI.SetActive(true);
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
    public void Option()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }
    public void OptionBack()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        levelLoader.LoadLevel(0);
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        
    }
}
