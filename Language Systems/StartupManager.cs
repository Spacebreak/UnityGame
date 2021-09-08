using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{    private void Awake()
    {
        if (PlayerPrefs.GetString("LanguageChoice") != null)
        {
            LocalizationManager.localizationManagerInstance.LoadLocalizedText(PlayerPrefs.GetString("LanguageChoice"));

            //Load main menu if the user has already chosen a default language.
            SceneManager.LoadScene("Menu Main");
        }

        CheckAllPlayerPrefs();
    }

    //This coroutine runs in conjunction with LocalizationManager class.       
    private IEnumerator Start()
    {
        //This is for testing purposes.
        //PlayerPrefs.DeleteKey("LanguageChoice"); 

        SetGameParameters();

        //If this is the first time the player has played the game, wait for their language selection.
        if (PlayerPrefs.GetString("LanguageChoice") == null || PlayerPrefs.GetString("LanguageChoice") == "")
        {
            //Waiting for language selection class which is initiated by selecting the language on the first launch.                        
            while (!LocalizationManager.localizationManagerInstance.GetIsReady())
            {
                //Waits one frame, effectively stops this script progressing until a language choice is selected.
                yield return null;
            }
                       
            SceneManager.LoadScene("Menu Main");
        }
        else
        {
            //LocalizationManager.localizationManagerInstance.LoadLocalizedText(PlayerPrefs.GetString("LanguageChoice"));

            //Load main menu if the user has already chosen a default language.
            //SceneManager.LoadScene("Menu Main");
        }
    }

    //Method for loading the main menu, called by all buttons.
    public void LoadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    //Ensures the game has correct playerprefs, and if not will instatiate them and set them to a default value.
    public void CheckAllPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("ConfirmBindingCont"))
        {
            PlayerPrefs.SetInt("ConfirmBindingCont", 330);
        }

        if (!PlayerPrefs.HasKey("CancelBindingCont"))
        {
            PlayerPrefs.SetInt("CancelBindingCont", 331);
        }

        if (!PlayerPrefs.HasKey("JumpBindingCont"))
        {
            PlayerPrefs.SetInt("JumpBindingCont", 330);
        }

        if (!PlayerPrefs.HasKey("SpinBindingCont"))
        {
            PlayerPrefs.SetInt("SpinBindingCont", 332);
        }

        if (!PlayerPrefs.HasKey("SlideBindingCont"))
        {
            PlayerPrefs.SetInt("SlideBindingCont", 331);
        }

        if (!PlayerPrefs.HasKey("PowerBindingCont"))
        {
            PlayerPrefs.SetInt("PowerBindingCont", 333);
        }





        if (!PlayerPrefs.HasKey("ConfirmBindingLocal"))
        {
            PlayerPrefs.SetInt("ConfirmBindingLocal", 1);
        }

        if (!PlayerPrefs.HasKey("CancelBindingLocal"))
        {
            PlayerPrefs.SetInt("CancelBindingLocal", 1);
        }

        if (!PlayerPrefs.HasKey("JumpBindingLocal"))
        {
            PlayerPrefs.SetInt("JumpBindingLocal", 1);
        }

        if (!PlayerPrefs.HasKey("SpinBindingLocal"))
        {
            PlayerPrefs.SetInt("SpinBindingLocal", 0);
        }

        if (!PlayerPrefs.HasKey("SlideBindingLocal"))
        {
            PlayerPrefs.SetInt("SlideBindingLocal", 0);
        }

        if (!PlayerPrefs.HasKey("PowerBindingLocal"))
        {
            PlayerPrefs.SetInt("PowerBindingLocal", 0);
        }

        if (!PlayerPrefs.HasKey("UpBindingLocal"))
        {
            PlayerPrefs.SetInt("UpBindingLocal", 0);
        }

        if (!PlayerPrefs.HasKey("DownBindingLocal"))
        {
            PlayerPrefs.SetInt("DownBindingLocal", 0);
        }

        if (!PlayerPrefs.HasKey("LeftBindingLocal"))
        {
            PlayerPrefs.SetInt("LeftBindingLocal", 0);
        }

        if (!PlayerPrefs.HasKey("RightBindingLocal"))
        {
            PlayerPrefs.SetInt("RightBindingLocal", 0);
        }
    }


    //For initializing properties of the game on startup such as screen rotation settings. IS THIS SET IN INSPECTOR ALREADY?
    private void SetGameParameters()
    {
        //Will only modify the screen rotation parameters if the device is an Android or Iphone mobile device.
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }

}
