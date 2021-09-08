using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TokenSelection : MonoBehaviour
{       
    private string levelName;               //Stores the name of the level to be loaded.
    public Toggle[] tokenToggles;           //Stores all the toggles for each token. 

    private Toggle recentToggledToken;      //The most recently toggled token.
    private Toggle earlierToggledToken;     //The second most recent toggled token. 

    //Ensuring that previous saved options don't influence current choice by setting the prefs to 0.
    public void ResetTokenPlayerPrefs()
    {
        PlayerPrefs.SetInt("ToggleRed", 0);
        PlayerPrefs.SetInt("ToggleGreen", 0);
        PlayerPrefs.SetInt("ToggleBlue", 0);
        PlayerPrefs.SetInt("ToggleYellow", 0);
        PlayerPrefs.SetInt("ToggleBlack", 0);
        PlayerPrefs.SetInt("ToggleWhite", 0);

        foreach (Toggle toggleToCheck in tokenToggles)
        {
            toggleToCheck.isOn = false;
        }
    }

    public void CheckToggles(Toggle togglePressed)
    {
        PlayerPrefs.SetInt(togglePressed.tag, togglePressed.isOn ? 1 : 0);

        //Will only record the token if it is turned on. Toggle behaviour activates even if turning off.
        if (togglePressed.isOn)
        {
            CaptureToggledToken(togglePressed);
        }
        else
        {
            return;
        }

        TokenCount();

    }

    //Captures the tokens selected by the player. Due to the order of execution I need to capture three
    //tokens as opposed to two.
    private void CaptureToggledToken(Toggle toggleBeingCaptured)
    {
        earlierToggledToken = recentToggledToken;
        recentToggledToken = toggleBeingCaptured;        
    }

    //Counts how many tokens are currently selected. Two tokens is max, if this is exceeded the first token
    //that was selected will be switched off.
    private void TokenCount()
    {
        int toggledCount = 0;

        foreach (Toggle toggleToCheck in tokenToggles)
        {
            if (toggleToCheck.isOn)
            {
                toggledCount = toggledCount + 1;
            }

            if (toggledCount > 2)
            {
                ResetTokens();           
                toggledCount = toggledCount - 1;
            }
        }
    }

    //If the amount of tokens select exceeds two this method will reset all tokens that aren't 
    //the most recent two.
    private void ResetTokens()
    {
        foreach (Toggle toggleToCheck in tokenToggles)
        {
            if (toggleToCheck != recentToggledToken && toggleToCheck != earlierToggledToken)
            {
                toggleToCheck.isOn = false;
            }
        }
    }

    public void LevelToBeLoaded(string levelCode)
    {
        levelName = levelCode;
    }

    public void LoadLevel()
    {
        //FinalizeLevelName();
        SceneManager.LoadScene(levelName);
    }

    //Based on what tokens are selected, will select the right level name by appending the 
    //modifiers to the level name.
    private void FinalizeLevelName()
    {
        if (PlayerPrefs.GetInt("ToggleRed") == 1)
        {
            levelName = levelName + "Red";
        }

        if (PlayerPrefs.GetInt("ToggleGreen") == 1)
        {
            levelName = levelName + "Green";
        }

        if (PlayerPrefs.GetInt("ToggleBlue") == 1)
        {
            levelName = levelName + "Blue";
        }

        if (PlayerPrefs.GetInt("ToggleYellow") == 1)
        {
            levelName = levelName + "Yellow";
        }

        if (PlayerPrefs.GetInt("ToggleBlack") == 1)
        {
            levelName = levelName + "Black";
        }

        if (PlayerPrefs.GetInt("ToggleWhite") == 1)
        {
            levelName = levelName + "White";
        }

    }

}
