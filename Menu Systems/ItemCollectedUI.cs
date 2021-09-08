using UnityEngine;
using TMPro;

//Using the Singleton subclass ensures that only one instance of this class is created.
public class ItemCollectedUI : Singleton<ItemCollectedUI>
{
    public int itemID;  //0 = Coin, 1 = GemStone.

    public Animator itemUIPanelAnimator;        //The animation that moves the panel.
    public Animator itemUIAnimator;             //The animation that moves the item.

    private bool itemUIDisplayed = false;       //Tracks whether the item collected UI is on display.
    public float itemUIMaxDisplayTime;          //The time the UI of the item will be displayed for.
    private float itemUICurrentDisplayTime;     //How long the item has been displayed in the UI.

    public ColourReference colourReference;         //The class containing a reference to the 6 main colours of the game.
    public TextMeshProUGUI collectibleCounterText;  //The text component displaying the number of the item collected.
    public GameObject[] itemParticles;              //The gameobject holding all the particle components.

    private void Start()
    {
        itemUIAnimator.SetInteger("ItemColour", -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (itemUIDisplayed)
        {
            itemUICurrentDisplayTime += Time.deltaTime;
            //Debug.Log($"Text: {coinUICurrentDisplayTime}");

            if(itemUICurrentDisplayTime > itemUIMaxDisplayTime)
            {
                EndItemUIDisplay();
            }
        }
    }

    //Makes the UI panel move so the coin counter gets displayed on screen. Gets called from CoinGroupColourizer class.
    public void DisplayItemUI(Colour ColourOfItem)
    {
        collectibleCounterText.color = colourReference.tokenColours[(int)ColourOfItem];
        UpdateItemCounterText((int)ColourOfItem);
        ChangeItemAnimation(ColourOfItem);
        UpdateParticleColours((int)ColourOfItem);
        StartItemUIDisplayDuration();
    }

    //Updates the animation the ItemUI should play. Coins and Gemstones are handled different based on their itemID.
    public void ChangeItemAnimation(Colour ColourOfItem)
    {
        if (ColourOfItem != Colour.None)
        {
            if(itemID == 0)
            {
                itemUIAnimator.SetInteger("ItemColour", (int)ColourOfItem);
            }
            else if (itemID == 1)
            {
                itemUIAnimator.Play("Base Layer.Gemstone Spin UI " + ConvertItemIDToString(ColourOfItem), -1);
            }
        }
        else
        {
            Debug.Log("ItemCollectedUI: Colour returned result of null.");
        }
    }

    private string ConvertItemIDToString(Colour ColourOfItem)
    {
        string animationLabel;

        switch (ColourOfItem)
        {
            case Colour.None: animationLabel = "White";
                Debug.LogError("ItemCollectedUI: No assigned Colour to Gemstone.");
                break;

            case Colour.Red: animationLabel = "Red";
                break;
            case Colour.Green: animationLabel = "Green";
                break;
            case Colour.Blue: animationLabel = "Blue";
                break;
            case Colour.Yellow: animationLabel = "Yellow";
                break;
            case Colour.Black: animationLabel = "Black";
                break;
            case Colour.White: animationLabel = "White";
                break;

            default: animationLabel = "White";
                Debug.LogError("ItemCollectedUI: Default cast reached.");
                break;
        }

        return animationLabel;
    }

    public void UpdateItemCounterText(int itemColourIndex)
    {
        if(itemID == 0)
        {
            collectibleCounterText.text = GameManager.GetCoinsCollected(itemColourIndex).ToString();
        }
        else if(itemID == 1)
        {
            collectibleCounterText.text = GameManager.CollectedGemstoneAmount(itemColourIndex).ToString();
        }
    }

    //Disables the particle system in effect and activates the newly collected colours' particle effect.
    private void UpdateParticleColours(int coinColourIndex)
    {
        for (int i = 0; i < itemParticles.Length; i++)
        {
            if (itemParticles[i].activeSelf && i != coinColourIndex)
            {
                itemParticles[i].SetActive(false);
            }
        }

        itemParticles[coinColourIndex].SetActive(true);
    }

    private void StartItemUIDisplayDuration()
    {
        itemUICurrentDisplayTime = 0;
        itemUIDisplayed = true;
        itemUIPanelAnimator.SetBool("ItemRecentlyCollected", true);
    }

    private void EndItemUIDisplay()
    {
        itemUIDisplayed = false;
        itemUIPanelAnimator.SetBool("ItemRecentlyCollected", false);
    }

}
