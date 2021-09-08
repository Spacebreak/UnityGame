using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenYellow : MonoBehaviour
{    
    public GameObject[] yellowTokens;       //Stores all the coins to be used on a level.
    public bool allYellowCoinsCollected;    //Tracks if all the coins have been collected.
    private int yellowCoinsCollected;       //Tracks the amount of coins that has currently been collected.

    public float durationToCollectCoins;    //Custom time to collect all the coins in a given coin set.

    public DoorExits exitDoor;              //Exitdoor holds the list of all TokenYellow Sets.
    public AudioSource coinCollectedSound;  //The sound file to play when collecting a coin.


    public GameObject[] yellowTokensUI;
    public GameObject[] yellowTokensUIParticles;
    public Color32 yellowTokenFading;
    public Color32 yellowTokenColouring;

    public GameObject yellowParticleEffect;
    public GameObject yellowParticleCompleteEffect;

    public Animator[] yellowTokenUIAnimators;


    //Ensures game starts from scratch. Adds this instance to a list containing all other instances.
    private void Start()
    {
        allYellowCoinsCollected = false;
        yellowCoinsCollected = 0;
        exitDoor.AddYellowTokenSet(this);
    }

    //When a yellow coin is obtained, if it is the first coin, begin the timer, otherwise add one to the coin
    //collected total, and set the next coin gameobject to actice.
    public void YellowCoinObtained()
    {
        coinCollectedSound.Play();

        if (yellowCoinsCollected == 0)
        {
            RemoveCoinsUI();
            StartCoroutine(BeginCoinCountdown());
            DisplayCoinsUI();
        }

        AddColourToCoinUI(yellowCoinsCollected);

        if (yellowCoinsCollected + 1 <= yellowTokensUIParticles.Length)
        {
            yellowTokensUIParticles[yellowCoinsCollected].gameObject.SetActive(true);
        }

        yellowCoinsCollected = yellowCoinsCollected + 1;

        if(yellowCoinsCollected + 1 <= yellowTokens.Length)
        {
            yellowTokens[yellowCoinsCollected].gameObject.SetActive(true);
        }

        //If the number of coins collected equals the total amount of coins this will be updated.
        if (yellowCoinsCollected == yellowTokens.Length)
        {
            allYellowCoinsCollected = true;
            yellowParticleCompleteEffect.SetActive(true);

            CompletedCoinsUI();
        }
    }

    public bool CheckYellowCoinsCollected()
    {
        return allYellowCoinsCollected;
    }

    private IEnumerator BeginCoinCountdown()
    {
        yield return new WaitForSeconds(durationToCollectCoins);

        if (!allYellowCoinsCollected)
        {
            ChallengeReset();
        }

    }

    //Resets the challenge if the player doesn't collect all the tokens in time.
    private void ChallengeReset()
    {
        yellowCoinsCollected = 0;

        foreach (GameObject yellowToken in yellowTokens)
        {
            yellowToken.gameObject.SetActive(false);
        }

        foreach (GameObject yellowTokenParticles in yellowTokensUIParticles)
        {
            yellowTokenParticles.gameObject.SetActive(false);
        }

        yellowTokens[0].gameObject.SetActive(true);

        RemoveCoinsUI();
    }

    private void DisplayCoinsUI()
    {
        for (int i = 0; i < yellowTokens.Length; i++)
        {
            yellowTokensUI[i].SetActive(true);
        }
    }

    private void CompletedCoinsUI()
    {
        for (int i = 0; i < yellowTokens.Length; i++)
        {
            yellowTokenUIAnimators[i].SetBool("Finish", true);
        }

        StartCoroutine(WaitToRemoveCoins());
    }

    private IEnumerator WaitToRemoveCoins()
    {
        yield return new WaitForSeconds(1);

        RemoveCoinsUI();
    }

    private void RemoveCoinsUI()
    {
        ClearCoinColouringUI();
        Vector3 originalScale = new Vector3(0.5f, 0.5f, 0.5f);

        for (int i = 0; i < yellowTokensUI.Length; i++)
        {
            yellowTokensUI[i].SetActive(false);
            yellowTokensUI[i].transform.localScale = originalScale;
            yellowTokensUI[i].transform.localRotation = Quaternion.identity;
            yellowTokensUIParticles[i].SetActive(false);
        }

        yellowParticleCompleteEffect.SetActive(false);
    }

    private void ClearCoinColouringUI()
    {
        for (int i = 0; i < yellowTokensUI.Length; i++)
        {
            yellowTokensUI[i].GetComponent<Image>().color = yellowTokenFading;
        }
    }

    private void AddColourToCoinUI(int tokenToColour)
    {
        yellowTokensUI[tokenToColour].GetComponent<Image>().color = yellowTokenColouring;
    }

    public void EnableParticleSystem(Vector3 particlePosition)
    {
        yellowParticleEffect.transform.position = particlePosition;
        yellowParticleEffect.SetActive(false);
        yellowParticleEffect.SetActive(true);
    }

}
