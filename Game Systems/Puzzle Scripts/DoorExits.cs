using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class DoorExits : MonoBehaviour
{
    bool playerAtExit = false;          //Keeps track of whether Luper is at the door.
    bool keyObtained = false;           //Tracks if Luper has found the key to open the door.
    bool levelRequirementsMet = false;  //Tracks if Luper has satisfied the requirements to advance to the next level.

    private List<TokenYellow> yellowTokenSets = new List<TokenYellow>();

    private readonly string playerString = "Player"; //Holds the string matching the tag of the object.

    private void Update()
    {
        if (playerAtExit && InputManager.GetConfirm())
        {
            AdvanceLevel();
        }
    }

    // Checks the object that has collided with the door.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            playerAtExit = true;
        }
    }

    //Checks the object that has exited the collider of the door.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            playerAtExit = false;
        }
    }

    //Returns whether the player is currently standing on the door.
    public bool IsPlayerOnExit()
    {
        return playerAtExit;
    }

    public void AdvanceLevel()
    {
        if (ConditionsMet())
        {
            SceneManager.LoadScene("WorldOneLevelOne", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("DoorExits: Conditions not met for advancing level.");
        }
    }

    // Checks that all the conditions are met before Luper can open the door to advance.
    private bool ConditionsMet()
    {
        levelRequirementsMet = CheckAllTokens()/*yellowTokens.CheckYellowCoinsCollected()*/ && keyObtained;
        return levelRequirementsMet;
    }

    public void KeyObtained()
    {
        keyObtained = true;
    }

    public bool CheckAllTokens()
    {
        foreach (TokenYellow yellowTokenSet in yellowTokenSets)
        {
            if (!yellowTokenSet.CheckYellowCoinsCollected())
            {
                Debug.Log("DoorExits: Not all yellow coins collected.");
                return false;
            }
        }

        return true;

    }

    public void AddYellowTokenSet(TokenYellow yellowTokenSetToAdd)
    {
        yellowTokenSets.Add(yellowTokenSetToAdd);
    }

}
