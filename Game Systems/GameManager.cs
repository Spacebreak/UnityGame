using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager{

    //private static GameManager gameManagerInstance; //Ensures only one instance of GameMaster exists on scene reload.
    private static PlayerData playerdata;           //The saved progress of the player.

    public static Vector3 lastCheckpointPosition;   //The last checkpoint Luper moved through.
    //If checkpoints stop working its because game manager was an object in Unity and not a static class

    private static int gravityIndex = 0;            //The gravity index for when Luper hits the Checkpoint.
    private static int oldGravityIndex = 0;         //The previous gravity index for when Luper hits the Checkpoint.

    public static List<bool> tokensActive = new List<bool>();

    private static bool tokenActiveRed;         //Determines if the red token is active.
    private static bool tokenActiveGreen;       //Determines if the green token is active.
    private static bool tokenActiveBlue;        //Determines if the blue token is active.
    private static bool tokenActiveYellow;      //Determines if the yellow token is active.
    private static bool tokenActiveBlack;       //Determines if the black token is active.
    private static bool tokenActiveWhite;       //Determines if the white token is active.

    public static int redCoinsCollected = 0;       //Temporary value for red coins collected (permanent is in PlayerData).
    public static int greenCoinsCollected = 0;     //Temporary value for green coins collected (permanent is in PlayerData).
    public static int blueCoinsCollected = 0;      //Temporary value for blue coins collected (permanent is in PlayerData).
    public static int yellowCoinsCollected = 0;    //Temporary value for yellow coins collected (permanent is in PlayerData).
    public static int blackCoinsCollected = 0;     //Temporary value for black coins collected (permanent is in PlayerData).
    public static int whiteCoinsCollected = 0;     //Temporary value for white coins collected (permanent is in PlayerData).

    public static void Save()
    {
        UpdateCoinsCollected();
        SaveManager.Save(playerdata);
    }    

    public static void Initialize()
    {
        /*
        if (gameManagerInstance == null)
        {
            //gameManagerInstance = this;
            //DontDestroyOnLoad(gameManagerInstance);
        }
        else
        {
            //Destroy(gameObject);
        }
        */

        //If playerdata is not loaded, will load the players data.
        if(playerdata == null)
        {
            try
            {
                playerdata = SaveManager.Load();
                Debug.Log("GameManager: Loaded Save Data");
            }
            catch
            {
                SaveManager.Save(new PlayerData());
                playerdata = SaveManager.Load();
                Debug.LogError("GameManager: Could not find Save data, creating new file.");
            }
        }

    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerdata.SaveEssence(3,3);
            //playerdata.SaveEssence()
            //playerdata.DisplayTest();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            playerdata.ResetAllData();
            playerdata.DisplayTest();
        }
    }
    */

    //Tracks what tokens are currently active in the level.
    public static void UpdateTokenSelection(bool redToken, bool greenToken, bool blueToken, bool yellowToken,
        bool whiteToken, bool blackToken)
    {
        tokenActiveRed = redToken;
        tokenActiveGreen = greenToken;
        tokenActiveBlue = blueToken;
        tokenActiveYellow = yellowToken;
        tokenActiveBlack = blackToken;
        tokenActiveWhite = whiteToken;

        tokensActive = new List<bool>{redToken, greenToken, blueToken, yellowToken, blackToken, whiteToken };
    }

    //Returns the list containing all the active tokens in the game.
    public static List<bool> ActiveTokens()
    {
        return tokensActive;
    }

    //Tracks the number of Tokens in effect. This should only ever result in 0,
    public static int NumberOfTokensSelected()
    {
        int tokenCount = 0;

        foreach (bool tokenColour in tokensActive)
        {
            if(tokenColour == true)
            {
                tokenCount = tokenCount + 1;
            }
        }

        return tokenCount;
    }
    
    public static int GetCoinsRed()
    {
        return playerdata.redCoins;
        //return PlayerData.redCoins;
    }
    public static int GetCoinsGreen()
    {
        return playerdata.greenCoins;
        //return PlayerData.greenCoins;
    }
    public static int GetCoinsBlue()
    {
        return playerdata.blueCoins;
        //return PlayerData.blueCoins;
    }
    public static int GetCoinsYellow()
    {
        return playerdata.yellowCoins;
        //return PlayerData.yellowCoins;
    }
    public static int GetCoinsBlack()
    {
        return playerdata.blackCoins;
        //return PlayerData.blackCoins;
    }
    public static int GetCoinsWhite()
    {
        return playerdata.whiteCoins;
        //return PlayerData.whiteCoins;
    }

    public static void UpdateCoinsCollected()
    {
        playerdata.SaveCoinsCollected(redCoinsCollected, greenCoinsCollected, blueCoinsCollected, yellowCoinsCollected, blackCoinsCollected, whiteCoinsCollected);
    }

    //Updates a newly collected gemstone.
    public static void UpdateGemstone(int gemstoneIdentifier, int gemstoneNumber)
    {
        playerdata.SaveGemstone(gemstoneIdentifier, gemstoneNumber);
    }

    //Returns if a specific gem has been collected. Identifier is for the colour of the gem, number is the unique value.
    public static bool RetrieveGemstone(int gemstoneIdentifier, int gemstoneNumber)
    {
        try
        {
            return playerdata.RetrieveGemstone(gemstoneIdentifier, gemstoneNumber);
        }
        catch
        {
            //Debug.LogError("GameManager: No Save Data could be found");
            return false;
        }
    }

    //Returns the amount of gems collected of a specific gem type. Used for gemstone gates.
    public static int CollectedGemstoneAmount(int gemstoneIdentifier)
    {
        return playerdata.RetrieveTotalGemstones(gemstoneIdentifier);
    }

    //Displays all saved data for a player.
    public static void DisplayPlayerData()
    {
        playerdata.DisplayTest();
    }

    public static void SetGravityIndex(int newGravityIndex)
    {
        gravityIndex = newGravityIndex;
    }

    public static int GetGravityIndex()
    {
        return gravityIndex;
    }

    public static void SetOldGravityIndex(int previousGravityIndex)
    {
        oldGravityIndex = previousGravityIndex;
    }

    public static int GetOldGravityIndex()
    {
        return oldGravityIndex;
    }

    public static void AddCoinCollected(int coinIdentifier)
    {
        switch (coinIdentifier)     
        {
            case 0:
                redCoinsCollected++;
                //Debug.Log(redCoinsCollected);
                break;

            case 1:
                greenCoinsCollected++;
                //Debug.Log(greenCoinsCollected);
                break;

            case 2:
                blueCoinsCollected++;
                //Debug.Log(blueCoinsCollected);
                break;

            case 3:
                yellowCoinsCollected++;
                //Debug.Log(yellowCoinsCollected);
                break;

            case 4:
                blackCoinsCollected++;
                //Debug.Log(blackCoinsCollected);
                break;

            case 5:
                whiteCoinsCollected++;
                //Debug.Log(whiteCoinsCollected);
                break;

            default:
                Debug.Log("GameManager: Default class reached. AddCoinsCollected.");
                break;
        }
    }

    public static int GetCoinsCollected(int coinIdentifier)
    {
        switch (coinIdentifier)
        {
            case 0:
                return redCoinsCollected;

            case 1:
                return greenCoinsCollected;

            case 2:
                return blueCoinsCollected;

            case 3:
                return yellowCoinsCollected;

            case 4:
                return blackCoinsCollected;

            case 5:
                return whiteCoinsCollected;

            default:
                Debug.Log("GameManager: Default class reached. GetCoinsCollected.");
                return -1;
        }
    }
}
