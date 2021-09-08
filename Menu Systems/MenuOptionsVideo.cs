using UnityEngine;
using UnityEngine.UI;

public class MenuOptionsVideo : MonoBehaviour
{
    public Dropdown resolutionDropdown;             //The dropdown selection for choosing the resolution.
    public Dropdown displayModeDropdown;            //The dropdown selection for choosing fullscreen or windowed mode.

    public GameObject resolutionDisplaySettings;    //Contains the video settings for computer version only.
    public GameObject qualityVsyncSettings;         //Contains the video settings for all Luper versions.
    public GameObject antialiasTextureSettings;     //Contains the video settings for all Luper versions.

    public Dropdown antialiasingDropdown;           //The dropdown selection for choosing the level of antialiasing.
    public Dropdown qualityLevelDropdown;           //The dropdown selection for choosing the quality level.

    public Dropdown textureQualityDropdown;         //The dropdown selection for choosing the quality of textures.
    public Toggle verticalSyncToggle;               //The checkbox for enabling/disabling vertical sync.

    private FullScreenMode displayMode = FullScreenMode.Windowed;   //Stores the current display mode setting.
    private int resolutionX = 1280;                 //Stores the X resolution value. Default 1280 for testing.
    private int resolutionY = 1024;                 //Stores the Y resolution value. Default 1024 for testing.

    [Header("Colourblock Video")]                   //Sets the heading in the inspector.
    public ColorBlock uiColourBlock;                //Stores 4 colours to be used for UI elements.

    public RectTransform qualityPosition;           //Holds the general position of Quality and Vsync settings.
    public RectTransform qualityMobilePosition;     //Holds the mobile position of Quality and Vsync settings.
    public RectTransform antialiasPosition;         //Holds the general position of Antialias and Texture quality settings.
    public RectTransform antialiasMobilePosition;   //Holds the mobile position of Antialias and Texture quality settings.

    private void Awake()
    {
        InitializeUserVideoSettings();
    }

    //Start is called before the first frame update.
    void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            EnableDesktopSettings();               
        }
        else 
        {
            qualityPosition.localPosition = qualityMobilePosition.localPosition;
            antialiasPosition.localPosition = antialiasMobilePosition.localPosition;
        }

        //InitializeUserVideoSettings();
    }

    //Depending on the resolution choice of the user will invoke the method with the correct resolution parameters.
    public void ResolutionChoiceChanged()
    {
        switch (resolutionDropdown.value)
        {
            case 0:
                SetResolutions(4096, 2160);
                break;

            case 1:
                SetResolutions(2560, 1440);
                break;

            case 2:
                SetResolutions(2048, 1080);
                break;

            case 3:
                SetResolutions(1920, 1080);
                break;

            case 4:
                SetResolutions(1680, 1050);
                break;

            case 5:
                SetResolutions(1600, 900);
                break;

            case 6:
                SetResolutions(1440, 900);
                break;

            case 7:
                SetResolutions(1400, 1050);
                break;

            case 8:
                SetResolutions(1366, 768);
                break;

            case 9:
                SetResolutions(1280, 1024);
                break;

            case 10:
                SetResolutions(1280, 768);
                break;

            case 11:
                SetResolutions(1280, 720);
                break;

            case 12:
                SetResolutions(1152, 648);
                break;

            case 13:
                SetResolutions(1024, 768);
                break;

            case 14:
                SetResolutions(1024, 600);
                break;

            case 15:
                SetResolutions(800, 600);
                break;

            case 16:
                SetResolutions(800, 480);
                break;

            case 17:
                SetResolutions(720, 480);
                break;

            case 18:
                SetResolutions(640, 480);
                break;

            default:
                SetResolutions(1024, 768);
                break;
        }

        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
    }

    //Depending on the choice of the user will render the correct display mode.
    public void ScreenRenderChoice()
    {
        switch (displayModeDropdown.value)
        {
            case 0:
                displayMode = FullScreenMode.FullScreenWindow;
                break;

            case 1:
                displayMode = FullScreenMode.Windowed;
                break;

            default:
                displayMode = FullScreenMode.Windowed;
                break;
        }

        PlayerPrefs.SetInt("DisplayMode", displayModeDropdown.value);
    }

    //Sets the resolution to the new choice and stores it in memory.
    private void SetResolutions(int resolutionXValue, int resolutionYValue)
    {
        resolutionX = resolutionXValue;
        resolutionY = resolutionYValue;
    }

    //Changes the display mode, uses previously stored resolution parameters to maintain the resolution.
    private void SetDisplayMode(FullScreenMode displayMode)
    {
        Screen.SetResolution(resolutionX, resolutionY, displayMode);
    }

    //Applies changes from both the resolution and window mode at the same time.
    public void ApplyChanges()
    {
        Screen.SetResolution(resolutionX, resolutionY, displayMode);
    }

    private FullScreenMode GetDisplayMode()
    {
        return displayMode;
    }

    private int GetXResolution()
    {
        return resolutionX;
    }

    private int GetYResolution()
    {
        return resolutionY;
    }

    //Sets antialiasing level in unity from the dropdown. AA is a setting under Quality as well, but can be
    //set independantly.
    public void SetAntiAliasing()
    {
        switch (antialiasingDropdown.value)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;

            case 1:
                QualitySettings.antiAliasing = 2;                
                break;

            case 2:
                QualitySettings.antiAliasing = 4;
                break;

            case 3:
                QualitySettings.antiAliasing = 8;
                break;

            default:
                QualitySettings.antiAliasing = 2;
                break;
        }

        PlayerPrefs.SetInt("Antialiasing", antialiasingDropdown.value);
    }

    //Sets quality level from the dropdown. 0 is lowest and 6 is ultra settings. Quality level in Unity encompasses
    //many different settings, profiles can be made in the project settings editor.
    public void SetQualityLevel()
    {
        switch (qualityLevelDropdown.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                break;

            case 1:
                QualitySettings.SetQualityLevel(1);
                break;

            case 2:
                QualitySettings.SetQualityLevel(2);
                break;

            case 3:
                QualitySettings.SetQualityLevel(3);
                break;

            case 4:
                QualitySettings.SetQualityLevel(4);
                break;

            case 5:
                QualitySettings.SetQualityLevel(5);
                break;
        }

        PlayerPrefs.SetInt("QualityLevel", qualityLevelDropdown.value);

        //Because VSync and AA is changed in Quality settings, need to reset them to what the user has specified.
        SetVerticalSync();
        SetAntiAliasing();
        SetTextureQuality();
    }

    public void SetVerticalSync()
    {
        if (verticalSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        PlayerPrefs.SetInt("VerticalSync", (verticalSyncToggle.isOn ? 1 : 0));
    }

    public void SetTextureQuality()
    {
        switch (textureQualityDropdown.value)
        {
            case 0:
                QualitySettings.masterTextureLimit = 0;
                break;

            case 1:
                QualitySettings.masterTextureLimit = 1;
                break;

            case 2:
                QualitySettings.masterTextureLimit = 2;
                break;

            case 3:
                QualitySettings.masterTextureLimit = 3;
                break;
        }

        PlayerPrefs.SetInt("TextureQuality", textureQualityDropdown.value);
    }

    //When the player selects the defaults button, returns each value to their defaults.
    public void SetDefaultsVideo()
    {
        DefaultQuality();
        DefaultAntialiasing();
        DefaultVerticalSync();
        DefaultTextureQuality();

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {            
            DefaultResolution();
            DefaultDisplayMode();
            ApplyChanges();            
        }
    }

    //Sets the quality level to default, quality default dependant on target platform.
    private void DefaultQuality()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            qualityLevelDropdown.value = 5;
        }
        else
        {
            qualityLevelDropdown.value = 2;
        }

        SetQualityLevel();
    }

    //Sets the antialiasing level to default, quality default dependant on target platform.
    private void DefaultAntialiasing()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            antialiasingDropdown.value = 2;
        }
        else
        {
            antialiasingDropdown.value = 0;
        }

        SetAntiAliasing();
    }

    //Sets the resolution to default on PC.
    private void DefaultResolution()
    {
        resolutionDropdown.value = 9;
        ResolutionChoiceChanged();
    }

    //Sets the window display setting on PC.
    private void DefaultDisplayMode()
    {
        displayModeDropdown.value = 1;
        ScreenRenderChoice();
    }

    private void DefaultVerticalSync()
    {
        verticalSyncToggle.isOn = false;
        SetVerticalSync();        
    }

    private void DefaultTextureQuality()
    {
        textureQualityDropdown.value = 0;
        SetTextureQuality();
    }

    //Will only enable the settings related to the PC when played on PC.
    private void EnableDesktopSettings()
    {
        resolutionDisplaySettings.SetActive(true);
    }

    //On start, will initialize the game with the correct settings based on Playerprefs. Will not initialize
    //controls that are only relevant to PC platform. Vertical Sync and Antialias Playerprefs have to be 
    //called before Quality, otherwise Quality method calls VSync and AA methods which reset them first.
    private void InitializeUserVideoSettings()
    {
        verticalSyncToggle.isOn = (PlayerPrefs.GetInt("VerticalSync", 0) != 0);
        textureQualityDropdown.value = (PlayerPrefs.GetInt("TextureQuality", 0));

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            antialiasingDropdown.value = PlayerPrefs.GetInt("Antialiasing", 2);
            qualityLevelDropdown.value = PlayerPrefs.GetInt("QualityLevel", 5);
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 3);
            displayModeDropdown.value = PlayerPrefs.GetInt("DisplayMode", 0);
         
            ApplyChanges();
        }
        else
        {
            antialiasingDropdown.value = PlayerPrefs.GetInt("Antialiasing", 0);
            qualityLevelDropdown.value = PlayerPrefs.GetInt("QualityLevel", 3);
        }
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

    /*Issues relating to colour were solved when using the proper Unity interface.
    //If playing on mobile devices, highlighted UI elements behave slightly differently for touch input
    //than they do for mouse input, so it's easier to revert the colours to stop them being highlighted.
    public void SetHighlightColourForMobiles()
    {
        qualityLevelDropdown.colors = uiColourBlock;
        antialiasingDropdown.colors = uiColourBlock;
        verticalSyncToggle.colors = uiColourBlock;
        textureQualityDropdown.colors = uiColourBlock;
    }
    */
}
