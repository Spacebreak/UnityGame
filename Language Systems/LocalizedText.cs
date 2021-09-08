using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string dictionaryKey;                //The key set in the inspector for the matching value in json language file.

    private bool textMeshPro;                   //Tracks if the object has a TextMeshPro component, if not regular Text will be obtained.
    private TextMeshProUGUI textMeshToLocalize; //The objects TextMeshPro component.
    private Text textToLocalize;                //The objects Text component.

    public bool textNeedsLocalization;          //Ensures not every object with a LocalizedText component gets updated. Set in inspector.
    private bool bindingTextNeedsLocalization;  //If the keybinding is a word (escape, backspace, etc.), will require to be localized.
    private string textToRevert;                //Holds the previous text in case the user cancels rebinding the key in MenuOptionsControls.    
    
    //LocalizationManager localizationManagerInstance = LocalizationManager.localizationManagerInstance;

    private void OnDestroy()
    {
        if(LocalizationManager.localizationManagerInstance != null)
        {
            LocalizationManager.localizationManagerInstance.RemoveTextObjectFromList(this);
        }
    }

    private void Awake()
    {
        //If localization manager isn't active, returns to language select menu in order to reactivate it.
        if (LocalizationManager.localizationManagerInstance == null)
        {
            //SceneManager.LoadScene(0);      
        }

        //This needs to be in awake otherwise MenuOptionsAudio will try calling the method before this class has started.
        GetTextObject();
    }

    //On start, searches the dictionary for the appropriate translation and updates the text field to correspond.
    private void Start()
    {               
        if (tag != "Untagged")
        {
            UpdateTextValue(PlayerPrefs.GetString(tag + "Text"));
            bindingTextNeedsLocalization = PlayerPrefs.GetInt(tag + "Local", 0) != 0 ? true : false;
        }

        if (bindingTextNeedsLocalization || textNeedsLocalization)
        {
            if(bindingTextNeedsLocalization)
            {
                dictionaryKey = PlayerPrefs.GetString(tag + "Key");
            }

            LocalizationManager.localizationManagerInstance.AddTextObjectToList(this);
            UpdateText();
        }
    }

    //Retrieves text component from gameobject. If it doesn't have a TextMeshPro, a Text object will be retrieved instead.
    private void GetTextObject()
    {
        textMeshToLocalize = GetComponent<TextMeshProUGUI>();

        if (textMeshToLocalize != null)
        {
            textMeshPro = true;
        }
        else
        {
            textToLocalize = GetComponent<Text>();
            textMeshPro = false;
        }
    }
    
    //Updates text component of an object, references the dictionary to get the correct value for the right language.
    public void UpdateText()
    {
        if (textNeedsLocalization || DictionaryKeyMatch())
        {
            string localizedText = LocalizationManager.localizationManagerInstance.GetLocalizedValue(dictionaryKey);

            if (textMeshPro)
            {
                textMeshToLocalize.text = localizedText;
            }
            else
            {
                textToLocalize.text = localizedText;
            }
        }
    }

    //Will update the dictionary key to a new one. This should only be used with text that the user is going to change,
    //which is the keybindings. In all other cases the key should never be altered in script.    
    public void UpdateDictionaryKey(string newKeyValue, string objectTag)
    {
        PlayerPrefs.SetString(objectTag + "Key", newKeyValue);
        dictionaryKey = newKeyValue;
        UpdateText();
    }

    public string GetDictionaryKey()
    {
        return dictionaryKey;
    }          

    //Sets the binding from input the user made (MenuOptionsControls). InputManager uses the playerpref set here.
    public void SetBinding(string inputToBind)
    {
        PlayerPrefs.SetString(tag + "Text", inputToBind);
        UpdateTextValue(inputToBind);
    }

    private void UpdateTextValue(string textToUpdate)
    {
        textMeshToLocalize.text = textToUpdate.ToUpper();
    }

    //Temporary text update asking user to select a new key.
    public void UpdateTextNewBinding()
    {
        textToRevert = textMeshToLocalize.text;
        textMeshToLocalize.text = LocalizationManager.localizationManagerInstance.GetLocalizedValue("press_key");
    }

    //If the user cancels changing the keybind, will revert it back here.
    public void RevertText()
    {
        textMeshToLocalize.text = textToRevert;
    }

    //Checks if the dictionary key matches one of the following words.
    private bool DictionaryKeyMatch()
    {
        if(dictionaryKey == "enter" || dictionaryKey == "space" || dictionaryKey == "backspace" || dictionaryKey == "escape" 
            || dictionaryKey == "mouse1" || dictionaryKey == "mouse2" || dictionaryKey == "mouse3")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
          
}
