using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class PlayerData
{    
    public static bool[] cityLevels = new bool[6];         //The array storing which city levels are unlocked.
    public static bool[] forestLevels = new bool[6];       //The array storing which forest levels are unlocked.
    public static bool[] desertLevels = new bool[6];       //The array storing which desert levels are unlocked.
    public static bool[] skyLevels = new bool[6];          //The array storing which sky levels are unlocked.
    public static bool[] seaLevels = new bool[6];          //The array storing which sea levels are unlocked.
    public static bool[] castleLevels = new bool[6];       //The array storing which castle levels are unlocked.

    public static bool[] redEssence = new bool[30];        //The array storing which red essences are unlocked.                  
    public static bool[] greenEssence = new bool[30];      //The array storing which green essences are unlocked.
    public static bool[] blueEssence = new bool[30];       //The array storing which blue essences are unlocked.
    public static bool[] yellowEssence = new bool[30];     //The array storing which yellow essences are unlocked.
    public static bool[] blackEssence = new bool[30];      //The array storing which black essences are unlocked.
    public static bool[] whiteEssence = new bool[30];      //The array storing which white essences are unlocked.
    
    public int redCoins = 0;       //The int storing how many red coins have been collected.
    public int greenCoins = 0;     //The int storing how many green coins have been collected.
    public int blueCoins = 0;      //The int storing how many blue coins have been collected.
    public int yellowCoins = 0;    //The int storing how many yellow coins have been collected.
    public int blackCoins = 0;     //The int storing how many black coins have been collected.
    public int whiteCoins = 0;     //The int storing how many white coins have been collected.    

    public PlayerData()
    {
        cityLevels = new bool[6];         //The array storing which city levels are unlocked.
        forestLevels = new bool[6];       //The array storing which forest levels are unlocked.
        desertLevels = new bool[6];       //The array storing which desert levels are unlocked.
        skyLevels = new bool[6];          //The array storing which sky levels are unlocked.
        seaLevels = new bool[6];          //The array storing which sea levels are unlocked.
        castleLevels = new bool[6];       //The array storing which castle levels are unlocked.
        
        redEssence = new bool[30];        //The array storing which red essences are unlocked.                  
        greenEssence = new bool[30];      //The array storing which green essences are unlocked.
        blueEssence = new bool[30];       //The array storing which blue essences are unlocked.
        yellowEssence = new bool[30];     //The array storing which yellow essences are unlocked.
        blackEssence = new bool[30];      //The array storing which black essences are unlocked.
        whiteEssence = new bool[30];      //The array storing which white essences are unlocked.
        
        redCoins = 0;       //The int storing how many red coins have been collected.
        greenCoins = 0;     //The int storing how many green coins have been collected.
        blueCoins = 0;      //The int storing how many blue coins have been collected.
        yellowCoins = 0;    //The int storing how many yellow coins have been collected.
        blackCoins = 0;     //The int storing how many black coins have been collected.
        whiteCoins = 0;     //The int storing how many white coins have been collected.    
    }    

    //The coins that the character collected get added to the total and saved. This should only be called when the 
    //character completes a level, otherwise on death/reset you can collect infinite coins.
    public void SaveCoinsCollected(int redCoinsCollected, int greenCoinsCollected, int blueCoinsCollected, 
        int yellowCoinsCollected, int blackCoinsCollected, int whiteCoinsCollected)
    {
        redCoins += redCoinsCollected;
        greenCoins += greenCoinsCollected;
        blueCoins += blueCoinsCollected;
        yellowCoins += yellowCoinsCollected;
        blackCoins += blackCoinsCollected;
        whiteCoins += whiteCoinsCollected;

        SaveManager.Save(this);
    }

    //Will identify the correct array and correct level unlocked based on numerical identifiers.
    public void SaveLevelUnlocked(int levelIdentifier, int levelNumber)
    {
        switch (levelIdentifier)
        {
            case 0:
                cityLevels[levelNumber] = true;
                break;

            case 1:
                forestLevels[levelNumber] = true;
                break;

            case 2:
                desertLevels[levelNumber] = true;
                break;

            case 3:
                skyLevels[levelNumber] = true;
                break;

            case 4:
                seaLevels[levelNumber] = true;
                break;

            case 5:
                castleLevels[levelNumber] = true;
                break;
        }

        SaveManager.Save(this);
    }

    //Will identify the correct array and correct gemstone based on numerical identifiers.
    public void SaveGemstone(int gemstoneIdentifier, int gemstoneNumber)
    {
        switch (gemstoneIdentifier)
        {
            case 0:
                redEssence[gemstoneNumber] = true;
                break;

            case 1:
                greenEssence[gemstoneNumber] = true;
                break;

            case 2:
                blueEssence[gemstoneNumber] = true;
                break;

            case 3:
                yellowEssence[gemstoneNumber] = true;
                break;

            case 4:
                blackEssence[gemstoneNumber] = true;
                break;

            case 5:
                whiteEssence[gemstoneNumber] = true;
                break;
        }

        SaveManager.Save(this);
    }

    //Will identify the correct array and correct essence and return if it has been collected.
    public bool RetrieveGemstone(int gemstoneIdentifier, int gemstoneNumber)
    {
        bool gemstoneCollected = false;

        switch (gemstoneIdentifier)
        {
            case 0:
                gemstoneCollected = redEssence[gemstoneNumber];
                break;

            case 1:
                gemstoneCollected = greenEssence[gemstoneNumber];
                break;

            case 2:
                gemstoneCollected = blueEssence[gemstoneNumber];
                break;

            case 3:
                gemstoneCollected = yellowEssence[gemstoneNumber];
                break;

            case 4:
                gemstoneCollected = blackEssence[gemstoneNumber];
                break;

            case 5:
                gemstoneCollected = whiteEssence[gemstoneNumber];
                break;

            default:
                Debug.LogError("Couldn't identify correct Array for Essences.");
                break;
        }

        return gemstoneCollected;
    }

    //Will return the total number of gemstones collected in the specified gemstone colouring. Used for gemstone gates.
    public int RetrieveTotalGemstones(int gemstoneIdentifier)
    {

        int gemstonesCollected = 0;
        bool[] gemstoneArray;

        switch (gemstoneIdentifier)
        {
            case 0:
                gemstoneArray = redEssence;
                break;

            case 1:
                gemstoneArray = greenEssence;
                break;

            case 2:
                gemstoneArray = blueEssence;
                break;

            case 3:
                gemstoneArray = yellowEssence;
                break;

            case 4:
                gemstoneArray = blackEssence;
                break;

            case 5:
                gemstoneArray = whiteEssence;
                break;

            default:
                gemstoneArray = new bool[30];
                Debug.LogError("PlayerData: Couldn't identify correct Array for Gemstones.");
                break;
        }

        foreach(bool gemStone in gemstoneArray)
        {
            if (gemStone == true)
            {
                gemstonesCollected = gemstonesCollected + 1;
            }
        }

        return gemstonesCollected;
    }

    //Displays the value of all stored game data.
    public void DisplayTest()
    {
        Debug.Log("Red Coins: " + redCoins);
        Debug.Log("Green Coins: " + greenCoins);
        Debug.Log("Blue Coins: " + blueCoins);
        Debug.Log("Yellow Coins: " + yellowCoins);
        Debug.Log("Black Coins: " + blackCoins);
        Debug.Log("White Coins: " + whiteCoins);

        foreach (bool status in cityLevels)
        {
            Debug.Log("City Result: " + status);
        }

        foreach(bool status in forestLevels)
        {
            Debug.Log("Forest Result: " + status);
        }

        foreach(bool status in desertLevels)
        {
            Debug.Log("Desert Result: " + status);
        }

        foreach(bool status in skyLevels)
        {
            Debug.Log("Sky Result: " + status);
        }

        foreach(bool status in seaLevels)
        {
            Debug.Log("Sea Result: " + status);
        }

        foreach(bool status in castleLevels)
        {
            Debug.Log("Castle Result: " + status);
        }

        foreach (bool status in redEssence)
        {            
            Debug.Log("Red Essence: " + status);
        }

        foreach (bool status in greenEssence)
        {
            Debug.Log("Green Essence: " + status);
        }

        foreach (bool status in blueEssence)
        {
            Debug.Log("Blue Essence: " + status);
        }

        foreach (bool status in yellowEssence)
        {
            Debug.Log("Yellow Essence: " + status);
        }

        foreach (bool status in blackEssence)
        {
            Debug.Log("Black Essence: " + status);
        }

        foreach (bool status in whiteEssence)
        {
            Debug.Log("White Essence: " + status);
        }

    }

    //For deleting all game data.
    public void ResetAllData()
    {
        redCoins = 0;
        greenCoins = 0;
        blueCoins = 0;
        yellowCoins = 0;
        blackCoins = 0;
        whiteCoins = 0;

        Array.Clear(cityLevels, 0, cityLevels.Length);
        Array.Clear(forestLevels, 0, forestLevels.Length);
        Array.Clear(desertLevels, 0, desertLevels.Length);
        Array.Clear(skyLevels, 0, skyLevels.Length);
        Array.Clear(seaLevels, 0, seaLevels.Length);
        Array.Clear(castleLevels, 0, castleLevels.Length);

        Array.Clear(redEssence, 0, redEssence.Length);
        Array.Clear(greenEssence, 0, greenEssence.Length);
        Array.Clear(blueEssence, 0, blueEssence.Length);
        Array.Clear(yellowEssence, 0, yellowEssence.Length);
        Array.Clear(blackEssence, 0, blackEssence.Length);
        Array.Clear(whiteEssence, 0, whiteEssence.Length);
    }
}
