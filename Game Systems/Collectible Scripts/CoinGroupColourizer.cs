using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGroupColourizer : MonoBehaviour
{
    private Colour CoinGroupColour;         //The colour of the coins in this group.
    public ColourReference colourReference; //The class containing a reference to the 6 main colours of the game.

    private List<CollectibleCoin> coinClassesList = new List<CollectibleCoin>();    //The list containing all Coins.
    public AudioSource coinCollectedSound;  //The sound file to be played when collecting a coin.
    public GameObject particleEffect;       //The prefab to instantiate particles at the coin collection location.

    public ItemCollectedUI itemUIClass;     //The class that handles displaying the item collected UI.

    // Start is called before the first frame update
    void Start()
    {
        CoinGroupColour = TokenManager.ReturnRandomColour();
        UpdateCoinSprites();
    }

    //Checks coin isn't already on the list before adding.
    public void AddCoinToList(CollectibleCoin coinToAdd)
    {
        if (!coinClassesList.Contains(coinToAdd))
        {
            coinClassesList.Add(coinToAdd);
        }
    }

    //Removes a coin from the list. Checks it is in the list before removal.
    public void RemoveCoinFromList(CollectibleCoin coinToRemove, Vector3 coinPosition)
    {
        if (coinClassesList.Contains(coinToRemove))
        {
            coinCollectedSound.Play();
            UpdateParticleLocation(coinPosition);
            coinClassesList.Remove(coinToRemove);

            GameManager.AddCoinCollected((int)CoinGroupColour);
            itemUIClass.DisplayItemUI(CoinGroupColour);
        }
        else
        {
            Debug.Log("CoinGroupColourizer: Tried to remove a coin from a list that it wasn't added to.");
        }

        //Destroys object if nothing remaining in the list. 
        if (coinClassesList.Count == 0)
        {
            StartCoroutine(DestroyClass());
        }
    }
    
    //Sets all of the coins to have the same colour as that of the tokens in effect.
    private void UpdateCoinSprites()
    {
        if (coinClassesList.Count > 0)
        {
            foreach (CollectibleCoin coinClass in coinClassesList)
            {
                coinClass.SetColourOfCoin(CoinGroupColour, colourReference.tokenColours[(int)CoinGroupColour]);
            }
        }
    }

    //Creates particle effects, changes the colour to the correct colour of the class and proceeds to destroy the object.
    private void UpdateParticleLocation(Vector3 coinPosition)
    {
        GameObject coinParticles = Instantiate(particleEffect, coinPosition, Quaternion.identity);
        ParticleSystem.MainModule coinParticlesMain = coinParticles.GetComponent<ParticleSystem>().main;
        coinParticlesMain.startColor = colourReference.tokenColours[(int)CoinGroupColour];
        StartCoroutine(DestroyGameObject(coinParticles));
    }

    //Wait to destroy the object so it can finish playing any animations/code.
    private IEnumerator DestroyGameObject(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(objectToDestroy);
    }

    //Class needs to wait before destroying itself or else the sound won't play.
    private IEnumerator DestroyClass()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
