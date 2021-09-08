using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TokenManager : MonoBehaviour
{
    public bool redTokenActive;         //Determines if the Red token is active in a level.
    public bool greenTokenActive;       //Determines if the Green token is active in a level.
    public bool blueTokenActive;        //Determines if the Blue token is active in a level.
    public bool yellowTokenActive;      //Determines if the Yellow token is active in a level.
    public bool blackTokenActive;       //Determines if the Black token is active in a level.
    public bool whiteTokenActive;       //Determines if the White token is active in a level.

    public static List<Token> activeTokens;    //Stores all active tokens.

    private void Awake()
    {
        activeTokens = new List<Token>();
        CreateTokenClasses();
        UpdateTokenSelection();
    }

    //Based on the bools selected, will create a Token class for each active token. Should only ever be 2 max.
    private void CreateTokenClasses()
    {
        if (redTokenActive)
        {
            activeTokens.Add(new Token(Colour.Red));
            AddTokenColourScene(Colour.Red);
        }

        if (greenTokenActive)
        {
            activeTokens.Add(new Token(Colour.Green));
            AddTokenColourScene(Colour.Green);
        }

        if (blueTokenActive)
        {
            activeTokens.Add(new Token(Colour.Blue));
            AddTokenColourScene(Colour.Blue);
        }

        if (yellowTokenActive)
        {
            activeTokens.Add(new Token(Colour.Yellow));
            AddTokenColourScene(Colour.Yellow);
        }

        if (blackTokenActive)
        {
            activeTokens.Add(new Token(Colour.Black));
            AddTokenColourScene(Colour.Black);
        }

        if (whiteTokenActive)
        {
            activeTokens.Add(new Token(Colour.White));
            AddTokenColourScene(Colour.White);
        }

        while (activeTokens.Count > 2)
        {
            activeTokens.RemoveAt(0);
        }
    }

    //GameManager handles what tokens are active but is a static class. This method should only be called once in the game.
    private void UpdateTokenSelection()
    {
        GameManager.UpdateTokenSelection(redTokenActive, greenTokenActive, blueTokenActive, 
            yellowTokenActive, whiteTokenActive, blackTokenActive);
    }

    public class Token
    {
        public Token(Colour colourOfToken)
        {
            Colour = colourOfToken;
        }

        public Colour Colour { get; set; }
    }

    //Returns a random colour from what colours are active. Used to determine coin colour.
    //If no tokens are active, a random colour is assigned.
    public static Colour ReturnRandomColour()
    {
        if(activeTokens.Count != 0)
        {
            return activeTokens[Random.Range(0, activeTokens.Count)].Colour;
        }
        else
        {
            switch (Random.Range(0, 6))
            {
                case 0:
                    return Colour.Red;

                case 1:
                    return Colour.Green;

                case 2:
                    return Colour.Blue;

                case 3:
                    return Colour.Yellow;

                case 4:
                    return Colour.Black;

                case 5:
                    return Colour.White;

                default:
                    Debug.Log("TokenManager Class: Default Case Reached, Assigning Colour None");
                    return Colour.None;
            }
        }
    }

    // Gets the name of the level and the active tokens and loads the appropriate scenes additively.
    private void AddTokenColourScene(Colour tokenColour)
    {
        string sceneToLoad = SceneManager.GetActiveScene().name;
        string colourActiveToken = tokenColour.ToString();
        sceneToLoad += colourActiveToken;

        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log("Cannot load: " + sceneToLoad);
        }
    }
}

// Enums are great for storing numbered labels
public enum Colour
{
    None = -1,
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
    Black = 4,
    White = 5
}
