using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PauseManager : MonoBehaviour
{

    public static bool gamePaused;          //Tracks current Pause state of the game.
    public static bool gameUnpausedDelay;   //A grace period where pressing unpause doesn't detect user input.
    public GameObject resumeButton;         //Resume button to be selected when pause menu activated.

    public GameObject pauseMenu;            //The UI of the Pause Menu.
    public GameObject mobileCanvas;         //The right-handed UI for Mobile users.
    public GameObject mobileCanvasInvert;   //The left-handed UI for Mobile users.
    public GameObject healthCanvas;         //The right-handed health UI for Mobile users.
    public GameObject healthCanvasInvert;   //The left-handed health UI for Mobile users.
    public CoinCounters pauseMenuCoinDisplay;   //The display on pause menu showing total collected coins.

    public GameObject optionsMenu;          //The menu containing the options.
    public GameObject deathMenu;          //The menu containing the options.

    public KeyCollectedUI keyCollectedUIClass;
       
    private void Start()
    {
        gamePaused = false;

        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        //Will only pause if the user is not in the options menu.
        if (InputManager.GetPause())
        {
            if (!optionsMenu.activeSelf && !deathMenu.activeSelf)
            {
                SetGamePaused();
            }
            else
            {
                DisableOptionsMenu();
            }
        }
    }

    //Pauses and unpauses the game by altering the TimeScale and disabling/enabling Mobile UI.
    public void SetGamePaused()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            Time.timeScale = 0f;

            if (Application.isMobilePlatform)
            {
                DisableMobileUI();
            }

            pauseMenu.SetActive(true);
            pauseMenuCoinDisplay.UpdateCoinValues();
            keyCollectedUIClass.ShowKeyOnPauseMenu();

            //Need to set the selected object to null first, otherwise it will deselect the resume button after selecting it.
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);

        }
        else
        {
            gameUnpausedDelay = true;
            StartCoroutine(UnpauseDelay());


            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            keyCollectedUIClass.HideKeyOnPauseMenu();

            if (Application.isMobilePlatform)
            {
                EnableMobileUI();
            }

        }
    }

    //Stops the game detecting input right as it is unpaused.
    private IEnumerator UnpauseDelay()
    {
        yield return new WaitForEndOfFrame();
        gameUnpausedDelay = false;
    }

    //Will determine what Mobile UI is in use and set it to inactive.
    private void DisableMobileUI()
    {
        if (mobileCanvas.activeSelf)
        {
            mobileCanvas.SetActive(false);
            healthCanvas.SetActive(false);
        }
        else if (mobileCanvasInvert.activeSelf)
        {
            mobileCanvasInvert.SetActive(false);
            healthCanvasInvert.SetActive(false);
        }
    }

    //Will determine what Mobile UI the player prefers (Left vs. Right handed) and re-enable it.
    private void EnableMobileUI()
    {
        if (PlayerPrefs.GetInt("MobileInvert", 0) == 0) //Right handed orientation
        {
            mobileCanvas.SetActive(true);
            healthCanvas.SetActive(true);
        }
        else
        {
            mobileCanvasInvert.SetActive(true);
            healthCanvasInvert.SetActive(true);
        }
    }

    //When the player presses the options button will enable the pause menu.
    public void EnableOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    //When the player selects back on the options menu it will take them back to the ingame menu.
    public void DisableOptionsMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Application");
        Application.Quit();
    }
}
