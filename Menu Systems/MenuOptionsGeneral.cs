using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuOptionsGeneral : MonoBehaviour
{
    public GameObject mainOptionsPanel;         //Holds the main options menu.
    public GameObject menuHowToPlay;            //The panel that contains information for how to play.

    public Button howToPlayBackButton;          //The button for returning from the how to play menu.
    public Button menuOptionsGeneralButton;     //The button that highlights the general tab in settings.

    [Header("Colourblock General")]             //Sets the heading in the inspector.
    public ColorBlock uiColourBlock;            //Stores 4 colours to be used for UI elements.

    //The static class that manages all language components.
    LocalizationManager localizationManagerInstance = LocalizationManager.localizationManagerInstance; 
    public Dropdown languageDropdown;           //The dropdown menu that contains the language options of the game.

    private void Awake()
    {
        //If localization manager isn't active, returns to language select menu in order to reactivate it.
        if (localizationManagerInstance == null)
        {
            Debug.Log("Loading Scene");
            SceneManager.LoadScene(0);            
        }

    }

    public void Start()
    {
        LanguageTextValue();
    }

    //When 'how to play' button is clicked, sets main options to inactive and activates how to play screen.
    public void HowToPlay()
    {
        mainOptionsPanel.SetActive(false);
        menuHowToPlay.SetActive(true);
        howToPlayBackButton.Select();
    }

    //When how to play screen back button is selected, reactivates main options and deactivates how to play menu.
    public void ReturnToOptionsMenu()
    {
        mainOptionsPanel.SetActive(true);
        menuHowToPlay.SetActive(false);
        menuOptionsGeneralButton.Select();
    }
    
    //Highlights the background of a UI element. Event triggers to call this method are set in inspector.
    public void HighlightUIBackground(GameObject objectToHighlight)
    {
        objectToHighlight.GetComponent<Image>().color = uiColourBlock.highlightedColor;
    }

    //Reverts the background of a UI element. Event triggers to call this method are set in inspector.
    public void RevertUIBackground(GameObject objectToRevert)
    {       
        objectToRevert.GetComponent<Image>().color = uiColourBlock.normalColor;
    }
         
    //Changes the filename depending on the language dropdown value, then initiates the load dictionary method.
    public void SetLanguage()
    {
        string fileName;

        switch (languageDropdown.value)
        {
            case 0:
                fileName = "localizedText_en.json";
                break;

            case 1:
                fileName = "localizedText_fr.json";
                break;

            case 2:
                fileName = "localizedText_es.json";
                break;

            case 3:
                fileName = "localizedText_de.json";
                break;

            case 4:
                fileName = "localizedText_ko.json";
                break;

            case 5:
                fileName = "localizedText_zhcn.json";
                break;

            case 6:
                fileName = "localizedText_zhtw.json";
                break;

            case 7:
                fileName = "localizedText_ja.json";
                break;

            case 8:
                fileName = "localizedText_nl.json";
                break;

            case 9:
                fileName = "localizedText_it.json";
                break;

            case 10:
                fileName = "localizedText_ru.json";
                break;

            case 11:
                fileName = "localizedText_pt.json";
                break;

            default:
                fileName = "localizedText_en.json";
                break;
        }

        localizationManagerInstance.LoadLocalizedText(fileName);
        PlayerPrefs.SetString("LanguageChoice", fileName);
    }

    //Sets the dropdown to the correct value depending on the filename of the playerpref.
    private void LanguageTextValue()
    {
        switch (PlayerPrefs.GetString("LanguageChoice"))
        {
            case "localizedText_en.json":
                languageDropdown.value = 0;
            break;

            case "localizedText_fr.json":
                languageDropdown.value = 1;
                break;

            case "localizedText_es.json":
                languageDropdown.value = 2;
                break;

            case "localizedText_de.json":
                languageDropdown.value = 3;
                break;

            case "localizedText_ko.json":
                languageDropdown.value = 4;
                break;

            case "localizedText_zhcn.json":
                languageDropdown.value = 5;
                break;

            case "localizedText_zhtw.json":
                languageDropdown.value = 6;
                break;

            case "localizedText_ja.json":
                languageDropdown.value = 7;
                break;

            case "localizedText_nl.json":
                languageDropdown.value = 8;
                break;

            case "localizedText_it.json":
                languageDropdown.value = 9;
                break;

            case "localizedText_ru.json":
                languageDropdown.value = 10;
                break;

            case "localizedText_pt.json":
                languageDropdown.value = 11;
                break;

            default:
                languageDropdown.value = 0;
                break;
        }
    }
}
