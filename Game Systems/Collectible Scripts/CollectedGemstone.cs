using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedGemstone : MonoBehaviour
{
    public int gemstoneIdentifier;          //The identifier for the type of gemstone.
    public int gemstoneNumber;              //The identifier for the individual gemstone.
    public bool gemstoneCollected;          //Whether the gemstone has been collected already or not.     

    private Gemstones masterGemClass;        //The class that tracks the individual gems and holds the audio file.
    public SpriteRenderer gemSprite;        //The sprite displaying the gem.
    public ParticleSystem gemParticles;     //The particles attached to the gemsprite.
    public ParticleSystem gemParticlesTwo;  //Secondary particles attached to the gemsprite. Only yellow gems have two.

    public Color32 colorChange;             //The colour the gem will change to if it has already been collected. Less alpha.

    private ItemCollectedUI itemUIClass;     //The class that handles displaying the item collected UI.


    private readonly string playerString = "Player";    //Holds the string matching the tag of the object.

    private void Start()
    {

        itemUIClass = FindObjectOfType<ItemCollectedUI>();

        if(itemUIClass == null)
        {
            Debug.Log("CollectedGemstone: Could not find ItemCollectedUI class.");
        }

        masterGemClass = FindObjectOfType<Gemstones>();

        if (masterGemClass == null)
        {
            Debug.Log("CollectedGemstone: Could not find Gemstones class.");
        }

        gemstoneCollected = GameManager.RetrieveGemstone(gemstoneIdentifier, gemstoneNumber);     

        if (gemstoneCollected)
        {
            gemSprite.color = colorChange;
            StartColorOne = colorChange;

            if(gemParticlesTwo != null)
            {
                StartColorTwo = colorChange;
            }

            //masterGemClass.RemoveGem(gameObject);
            //Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(playerString))
        {
            //Only calls the save script if the gem has not been collected before.
            if (!gemstoneCollected)
            {
                GameManager.UpdateGemstone(gemstoneIdentifier, gemstoneNumber);
                //itemUIClass.DisplayItemUI(TranslateIDToColour(gemstoneIdentifier));
            }

            if(itemUIClass != null)
            {
                itemUIClass.DisplayItemUI(TranslateIDToColour(gemstoneIdentifier));
            }

            if(masterGemClass != null)
            {
                masterGemClass.PlayGemCollectSound();
                masterGemClass.RemoveGem(gameObject);
            }

            Destroy(gameObject);
        }        
    }

    //Translates the ID of the Gemstone to a colour.
    private Colour TranslateIDToColour(int gemstoneIdentifier)
    {
        Colour gemstoneColour;

        switch (gemstoneIdentifier)
        {
            case 0: gemstoneColour = Colour.Red;
                break;
            case 1: gemstoneColour = Colour.Green;
                break;
            case 2: gemstoneColour = Colour.Blue;
                break;
            case 3: gemstoneColour = Colour.Yellow;
                break;
            case 4: gemstoneColour = Colour.Black;
                break;
            case 5: gemstoneColour = Colour.White;
                break;

            default: gemstoneColour = Colour.None;
                Debug.LogError("CollectedGemstone: Gemstone returned no colour.");
                break;
        }

        return gemstoneColour;
    }

    //Main interface script for setting the colour on a particle system.
    public Color StartColorOne
    {
        set
        {
            var particleSystemMain = gemParticles.main;
            particleSystemMain.startColor = value;
        }
    }

    //Main interface script for setting the colour on a particle system. Only called if a second particle system exists.
    public Color StartColorTwo
    {
        set
        {
            var particleSystemMain = gemParticlesTwo.main;
            particleSystemMain.startColor = value;
        }
    }

}
