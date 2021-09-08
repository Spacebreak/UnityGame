using UnityEngine;
using UnityEngine.UI;

public class KeyCollectedUI : MonoBehaviour
{

    public Animator keyUIPanelAnimator;        //The animation that moves the panel.
    public Animator keyUIAnimator;             //The animation that moves the item.

    private bool keyUIDisplayed;
    private float keyUICurrentDisplayTime;
    public float keyUIMaxDisplayTime;

    public Image keyImage;
    public GameObject keyParticles;

    // Update is called once per frame
    void Update()
    {
        if (keyUIDisplayed)
        {
            keyUICurrentDisplayTime += Time.deltaTime;
            //Debug.Log($"Text: {itemUICurrentDisplayTime}");

            if (keyUICurrentDisplayTime > keyUIMaxDisplayTime)
            {
                EndItemUIDisplay();
            }
        }
    }

    public void DisplayKey(bool keyObtained)
    {
        if (keyObtained)
        {
            KeyUISolidColour();
        }

        StartKeyUIDisplayDuration();
    }

    private void StartKeyUIDisplayDuration()
    {
        keyUICurrentDisplayTime = 0;
        keyUIDisplayed = true;
        keyUIPanelAnimator.SetBool("ItemRecentlyCollected", true);
    }

    private void EndItemUIDisplay()
    {
        keyUIDisplayed = false;
        keyUIPanelAnimator.SetBool("ItemRecentlyCollected", false);
    }

    private void KeyUISolidColour()
    {
        keyImage.color = new Color(keyImage.color.r, keyImage.color.g, keyImage.color.b, 1);
        keyParticles.SetActive(true);
    }

    public void ShowKeyOnPauseMenu()
    {
        keyUIPanelAnimator.SetBool("ItemRecentlyCollected", true);
    }

    public void HideKeyOnPauseMenu()
    {
        keyUIPanelAnimator.SetBool("ItemRecentlyCollected", false);
    }

}
