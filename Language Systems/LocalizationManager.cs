using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager localizationManagerInstance; //For ensuring only one instance is created.

    private Dictionary<string, string> localizedText;   //Stores a JSON file depending on language chosen.
    public List<LocalizedText> allTextObjects;          //Holds each text object with the Localized Text script in the game.   
    private bool isReady = false;                       //StartupManager class only proceeds once a language is chosen and loaded.
    
    // Awake is called before anything else.
    void Awake()
    {
        //If no previous instances of LocalizationManager have been created, set this instance as the one.
        if (localizationManagerInstance == null)
        {
            Debug.Log("LocalizationManager: No previous LocalizationManagers were found.");
            localizationManagerInstance = this;
        }
        //Ensures only one instance of LocalizationManager exists by destroying prior instances.
        else if (localizationManagerInstance != this)
        {
            Destroy(gameObject);
            Debug.Log("LocalizationManager: Destroy.");

        }

        //Will persist when new scenes are loaded.
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(localizationManagerInstance);
    }

    //Takes a filename from the button and looks for a file matching the chosen name. Stores choice in Playerprefs.
    public void LoadLocalizedText(string fileName)
    {
        //Creates an empty dictionary using the structure key:value (string:string)
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        //Depending on the platform running Luper, will load the language file from the correct source.
        if (Application.platform == RuntimePlatform.Android)
        {
            LoadAndroidFile(filePath);
        }
        else
        {
            LoadWindowsFile(filePath);
        }

        PlayerPrefs.SetString("LanguageChoice", fileName);
    }

    //Retrieves the correct localized text based off the corresponding key. Searches the localizedText 
    //dictionary for the key value.
    public string GetLocalizedValue(string dictionaryKey)
    {
        string dictionarySearchResult;

        int dictionaryKeyNumberTest; //Tests if the dictionary key is a number, only used for int.TryParse function.

        //Trys to parse a string, if it is a number the string is split into a char array then each individual value
        //is searched for in the dictionary.
        if (int.TryParse(dictionaryKey, out dictionaryKeyNumberTest))
        {
            char[] dictionaryKeySplit = dictionaryKey.ToCharArray();

            if (dictionaryKeySplit.Length > 1)
            {
                dictionarySearchResult = SplitNumber(dictionaryKeySplit);
            }
            else if(localizedText.ContainsKey(dictionaryKey))
            {
                dictionarySearchResult = localizedText[dictionaryKey];
            }
            else
            {
                dictionarySearchResult = "Localized text not found for the key: " + dictionaryKeyNumberTest + ".";
            }
        }
        else if (localizedText.ContainsKey(dictionaryKey))
        {
            dictionarySearchResult = localizedText[dictionaryKey];
        }
        else
        {
            dictionarySearchResult = "Localized text not found for the key: " + dictionaryKey + ".";
        }

        return dictionarySearchResult;
    }

    //The StartupManager script will progress only when LocalizationManager is ready.
    public bool GetIsReady()
    {
        return isReady;
    }

    //If running Luper on Android, needs to handle text parsing differently to Windows.
    private void LoadAndroidFile(string fileName)
    {
        StartCoroutine(GetTextFileOnAndroid(fileName));
    }

    //Loads the JSON file if it exists.
    private void LoadWindowsFile(string fileName)
    {
        //Will attempt to store the text from the file specified by filepath.
        if (File.Exists(fileName))
        {
            string dataAsJson = File.ReadAllText(fileName);
            LoadDictionary(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot find localization file by the name of " + fileName + ".");
        }
    }

    //Using a JSON file, creates a Localization Data class to hold each item in the file. For each object parsed 
    //into the LocalizationData class, passes each object into the localizedText dictionary.
    private void LoadDictionary(string jsonData)
    {
        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(jsonData);

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }

        Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries.");
        isReady = true;

        UpdateAllTextObjects();
    }

    //Once all dictionary values have been loaded, updates each object's text component.
    private void UpdateAllTextObjects()
    {
        foreach (LocalizedText textToUpdate in allTextObjects)
        {
            textToUpdate.UpdateText();            
        }
    }

    //Called by the LocalizedText class, each text object adds itself to the dictionary to be referenced here
    //in the LocalizationManager class.
    public void AddTextObjectToList(LocalizedText textToAdd)
    {
        if (!allTextObjects.Contains(textToAdd))
        {
            allTextObjects.Add(textToAdd);
        }
    }

    public void RemoveTextObjectFromList(LocalizedText textToAdd)
    {
        if (allTextObjects.Contains(textToAdd))
        {
            allTextObjects.Remove(textToAdd);
        }
    }

    //Coroutine runs on Android platforms, sends a web request and downloads JSON file contents. While 
    //incomplete it sends a yield statement, when complete the downloaded data is stored in a string.
    IEnumerator GetTextFileOnAndroid(string fileName)
    {
        UnityWebRequest wwwLocalizationManager = new UnityWebRequest(fileName);
        wwwLocalizationManager.downloadHandler = new DownloadHandlerBuffer();

        yield return wwwLocalizationManager.SendWebRequest();

        if (wwwLocalizationManager.isNetworkError || wwwLocalizationManager.isHttpError)
        {
            Debug.Log("Network or Http error: " + wwwLocalizationManager.error);
        }
        else
        {
            string dataAsJson = wwwLocalizationManager.downloadHandler.text;
            LoadDictionary(dataAsJson);
        }

        wwwLocalizationManager.downloadHandler.Dispose();
    }

    //Will break a valid number (in char format) and get the dictionary value for each individual digit 
    //and combine it into recombinedkeyvalue. Need to reset key value each time.
    private string SplitNumber(char[] numberToSplit)
    {
        string recombinedKeyValue = "";

        for (int i = 0; i < numberToSplit.Length; i++)
        {
            recombinedKeyValue += localizationManagerInstance.GetLocalizedValue(numberToSplit[i].ToString());
        }

        return recombinedKeyValue;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
