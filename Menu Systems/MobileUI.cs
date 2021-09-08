using UnityEngine;
using UnityEngine.UI;

public class MobileUI : MonoBehaviour
{
    public GameObject rightMobileUI;        //The mobile UI with right-handed orientation.
    public GameObject leftMobileUI;         //The mobile UI with left-handed orientation.
    public GameObject rightHealthMobileUI;  //The mobile UI showing the health.
    public GameObject leftHealthMobileUI;   //The mobile UI showing the health inverted.

    public Image[] mobileUIImages;          //Some mobile UI graphics, stored so they can be made opaque.
    //public Image[] mobileUIImagesCustomRGB; //Some mobile UI graphics that need to be handled differently to the rest.

    public GameObject gravityUI;            //The UI showing the gravity controls that Luper has available.
    public GameObject gravityInvertedUI;    //The Inverted UI showing the gravity controls that Luper has available.
    public GameObject tokenWhite;           //The GameObject holding the White Token Gravity script.


    // Start is called before the first frame update
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            CorrectInversion();
            CorrectUIOpacity();

            if (tokenWhite.activeSelf)
            {
                DisplayGravityControls();
            }
        }
    }

    //Depending on the playerpref setting from the settings menu, will select the correct UI to use.
    private void CorrectInversion()
    {
        if (PlayerPrefs.GetInt("MobileInvert", 0) == 0)
        {
            leftMobileUI.SetActive(false);
            leftHealthMobileUI.SetActive(false);

            rightMobileUI.SetActive(true);
            rightHealthMobileUI.SetActive(true);
        }
        else
        {
            rightMobileUI.SetActive(false);
            rightHealthMobileUI.SetActive(false);

            leftMobileUI.SetActive(true);
            leftHealthMobileUI.SetActive(true);
        }
    }

    //Depending on the playerpref setting from the settings menu, will select the correct menu opacity.
    public void CorrectUIOpacity()
    {
        foreach (Image imageToAlter in mobileUIImages)
        {
            imageToAlter.color = new Color(1, 1, 1, PlayerPrefs.GetFloat("MenuOpacity", 1));
        }
    }

    private void DisplayGravityControls()
    {
        if (PlayerPrefs.GetInt("MobileInvert", 0) == 0)
        {
            gravityInvertedUI.SetActive(false);
            gravityUI.SetActive(true);
        }
        else
        {
            gravityUI.SetActive(false);
            gravityInvertedUI.SetActive(true);
        }
    }

}
