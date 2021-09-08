using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounters : MonoBehaviour
{
    //public PlayerData playerData;

    public TextMeshProUGUI coinsTextRed;
    public TextMeshProUGUI coinsTextGreen;
    public TextMeshProUGUI coinsTextBlue;
    public TextMeshProUGUI coinsTextYellow;
    public TextMeshProUGUI coinsTextBlack;
    public TextMeshProUGUI coinsTextWhite;

    //Adds the current coins collected to the total.
    public void UpdateCoinValues()
    {
        /*
        coinsTextRed.text = (GameManager.GetCoinsRed() + GameManager.redCoinsCollected).ToString();
        coinsTextGreen.text = (GameManager.GetCoinsGreen() + GameManager.greenCoinsCollected).ToString();
        coinsTextBlue.text = (GameManager.GetCoinsBlue() + GameManager.blueCoinsCollected).ToString();
        coinsTextYellow.text = (GameManager.GetCoinsYellow() + GameManager.yellowCoinsCollected).ToString();
        coinsTextBlack.text = (GameManager.GetCoinsBlack() + GameManager.blackCoinsCollected).ToString();
        coinsTextWhite.text = (GameManager.GetCoinsWhite() + GameManager.whiteCoinsCollected).ToString();
        */

        coinsTextRed.text = GameManager.GetCoinsRed().ToString() + ", " + GameManager.redCoinsCollected.ToString();
        coinsTextGreen.text = GameManager.GetCoinsGreen().ToString() + ", " + GameManager.greenCoinsCollected.ToString();
        coinsTextBlue.text = GameManager.GetCoinsBlue().ToString() + ", " + GameManager.blueCoinsCollected.ToString();
        coinsTextYellow.text = GameManager.GetCoinsYellow().ToString() + ", " + GameManager.yellowCoinsCollected.ToString();
        coinsTextBlack.text = GameManager.GetCoinsBlack().ToString() + ", " + GameManager.blackCoinsCollected.ToString();
        coinsTextWhite.text = GameManager.GetCoinsWhite().ToString() + ", " + GameManager.whiteCoinsCollected.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        //playerData = FindObjectOfType<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
