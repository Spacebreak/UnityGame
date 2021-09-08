using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HealthManager : MonoBehaviour
{
    public static int currentHealth;
    public static int currentShield;
    
    public GameObject healthUI1;
    public GameObject healthUI2;
    public GameObject healthUI3;
    public GameObject shieldUI1;
    public GameObject shieldUI2;
    public GameObject shieldUI3;

    public GameObject healthInvertedUI1;
    public GameObject healthInvertedUI2;
    public GameObject healthInvertedUI3;
    public GameObject shieldInvertedUI1;
    public GameObject shieldInvertedUI2;
    public GameObject shieldInvertedUI3;

    public SoundController soundController;

    public GameObject deathCanvas;      //The canvas that displays that the player has died.
    public GameObject otherCanvases;    //The other canvases that need to be disabled when the death canvas is active.

    public GameObject retryButton;      //Retry button to be activated and selected when death menu activated.
    public GameObject menuButton;       //Menu button to be activated when death menu activated.

    private bool hurtCooldown;          //Ensures Luper cannot be hurt consecutively.

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 3;
        currentShield = 0;

        UIHealthUpdate();
    }

    public void UIHealthUpdate()
    {
        switch (currentHealth)
        {
            case 0:
                healthUI1.SetActive(false);
                healthUI2.SetActive(false);
                healthUI3.SetActive(false);
                healthInvertedUI1.SetActive(false);
                healthInvertedUI2.SetActive(false);
                healthInvertedUI3.SetActive(false);
                break;

            case 1:
                healthUI1.SetActive(true);
                healthUI2.SetActive(false);
                healthUI3.SetActive(false);
                healthInvertedUI1.SetActive(true);
                healthInvertedUI2.SetActive(false);
                healthInvertedUI3.SetActive(false);
                break;

            case 2:
                healthUI1.SetActive(true);
                healthUI2.SetActive(true);
                healthUI3.SetActive(false);
                healthInvertedUI1.SetActive(true);
                healthInvertedUI2.SetActive(true);
                healthInvertedUI3.SetActive(false);
                break;

            case 3:
                healthUI1.SetActive(true);
                healthUI2.SetActive(true);
                healthUI3.SetActive(true);
                healthInvertedUI1.SetActive(true);
                healthInvertedUI2.SetActive(true);
                healthInvertedUI3.SetActive(true);
                break;
        }

        switch (currentShield)
        {
            case 0:
                shieldUI1.SetActive(false);
                shieldUI2.SetActive(false);
                shieldUI3.SetActive(false);
                shieldInvertedUI1.SetActive(false);
                shieldInvertedUI2.SetActive(false);
                shieldInvertedUI3.SetActive(false);
                break;

            case 1:
                shieldUI1.SetActive(true);
                shieldUI2.SetActive(false);
                shieldUI3.SetActive(false);
                shieldInvertedUI1.SetActive(true);
                shieldInvertedUI2.SetActive(false);
                shieldInvertedUI3.SetActive(false);
                break;

            case 2:
                shieldUI1.SetActive(true);
                shieldUI2.SetActive(true);
                shieldUI3.SetActive(false);
                shieldInvertedUI1.SetActive(true);
                shieldInvertedUI2.SetActive(true);
                shieldInvertedUI3.SetActive(false);
                break;

            case 3:
                shieldUI1.SetActive(true);
                shieldUI2.SetActive(true);
                shieldUI3.SetActive(true);
                shieldInvertedUI1.SetActive(true);
                shieldInvertedUI2.SetActive(true);
                shieldInvertedUI3.SetActive(true);
                break;
        }
    }

    public void LuperDamaged()
    {
        if (hurtCooldown)
        {
            return;
        }

        if (currentShield > 0)
        {
            currentShield = currentShield - 1;
        }
        else if (currentHealth > 0)
        {
            currentHealth = currentHealth - 1;
        }

        if(currentHealth <= 0)
        {
            //Debug.Log("Luper has Died");
            Death();
        }

        hurtCooldown = true;
        StartCoroutine(DelayHurtCooldown());
        PlayHurtSound();
        UIHealthUpdate();
    }

    private IEnumerator DelayHurtCooldown()
    {
        yield return new WaitForSeconds(1.2f);

        hurtCooldown = false;
    }

    public void GainHealth()
    {
        if(currentHealth < 3)
        {
            currentHealth = currentHealth + 1;
            UIHealthUpdate();
        }
    }

    public void GainShield()
    {
        if(currentShield < 3)
        {
            currentShield = 3;
            UIHealthUpdate();
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetShield()
    {
        return currentShield;
    }

    private void PlayHurtSound()
    {
        int randomNumber = Random.Range(0, soundController.hurtSounds.Length);
        soundController.PlayAudioClip(SoundController.AudioCollection.Hurt, randomNumber, 0.02f);
    }

    private void Death()
    {
        Time.timeScale = 0f;
        otherCanvases.SetActive(false);
        deathCanvas.SetActive(true);

        StartCoroutine(DelayMenuSelection());
    }

    private IEnumerator DelayMenuSelection()
    {
        yield return new WaitForSecondsRealtime(1.1f);

        retryButton.SetActive(true);
        menuButton.SetActive(true);

        //Need to set the selected object to null first, otherwise it will deselect the resume button after selecting it.
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
