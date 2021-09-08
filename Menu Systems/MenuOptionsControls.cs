using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuOptionsControls : MonoBehaviour
{
    public GameObject desktopBindings;
    public GameObject mobileBindings;

    public CanvasGroup menuOptionsCanvas;           //Canvas group for disabling touch input/UI navigation.
    public MenuUIAlterations menuUIClass;           //The class that contains UI alteration methods.
    private MenuOptionsSelector menuOptionsSelector;//Prevents the menu from going back to main menu on Escape.

    [Header("Colourblock Controls")]                
    public ColorBlock uiColourBlock;                //Stores 4 colours to be used for UI elements.

    private string userKeyPressed = "";             //The string for storing the user key/mouse press.
    private int mouseKeyCode;                       //For assigning the keycode of mouse inputs.

    private LocalizedText keyBindLocalizedText;     //Gets the Localized Text object from the button selected.
    private TextMeshProUGUI buttonTextMesh;         //The textmeshpro component of a button.

    private bool keyboardInputRequired;             //Waiting for the user to press a key.
    private Button buttonToChange;                  //Stores the button having its' hotkey changed, so it can be reselected.o

    [Header("Keyboard Bindings")]
    public Button confirmationKeyboardButton;       //The keyboard button for confirming. Needed for resetting defaults.
    public Button cancelKeyboardButton;             //The keyboard button for cancelling. Needed for resetting defaults.
    public Button jumpKeyboardButton;               //The keyboard button for jumping. Needed for resetting defaults.
    public Button spinKeyboardButton;               //The keyboard button for spinning. Needed for resetting defaults.
    public Button slideKeyboardButton;              //The keyboard button for sliding and diving. Needed for resetting defaults.
    public Button powerKeyboardButton;              //The keyboard button for using the power. Needed for resetting defaults.
    public Button upKeyboardButton;                 //The keyboard button for moving up. Needed for resetting defaults.
    public Button downKeyboardButton;               //The keyboard button for moving down. Needed for resetting defaults.
    public Button leftKeyboardButton;               //The keyboard button for moving left. Needed for resetting defaults.
    public Button rightKeyboardButton;              //The keyboard button for moving right. Needed for resetting defaults.
       
    private bool controllerButtonPressed;           //Checks if the controller button has been pressed this update.
    private Image[] controllerButtonImage;          //The Image from both the controller button, and the buttons child.
    private RectTransform[] controllerButtonTransform;//The RectTransform for pulsating the image when controllerinputrequired is true.

    private bool controllerInputRequired;           //Waiting for the user to press a button.
    private Button buttonToChangeJoystick;          //Stores the button having its' hotkey changed, so it can be reselected.

    [Header("Controller Bindings")]
    public Button confirmationControllerButton;     //The controller button for confirming. Needed for resetting defaults.
    public Button cancelControllerButton;           //The controller button for cancelling. Needed for resetting defaults.
    public Button jumpControllerButton;             //The controller button for jumping. Needed for resetting defaults.
    public Button spinControllerButton;             //The controller button for spinning. Needed for resetting defaults.
    public Button slideControllerButton;            //The controller button for sliding and diving. Needed for resetting defaults.
    public Button powerControllerButton;            //The controller button for using the power. Needed for resetting defaults.

    [Header("Mobile Functions")]
    public Toggle vibrationToggle;                  //The checkbox for enabling/disabling mobile vibration.
    public Slider opacitySlider;                    //The slider for controlling mobile UI opacity.

    public GameObject normalDemoUI;                 //The regular right-handed demo UI for mobile users.
    public GameObject invertedDemoUI;               //The inverted left-handed demo UI for mobile users.
    public Toggle invertedControlsToggle;           //The toggle tracking if inversion is selected.

    public Image[] demoUI;                          //All the images on the demo sprite, for setting opacity.
    public MobileUI mobileUI;                       //The class will call the methods to make some sprites opaque.



    //If user input is required, the GUI component will wait for user input, and use this to change the value of
    //the keybinding.
    private void OnGUI()
    {
        if(controllerInputRequired && InputManager.GetCancel() && buttonToChangeJoystick.tag != "CancelBindingCont")
        {
            controllerInputRequired = false;
            StartCoroutine(WaitToSelectControllerObject());

            return;
        }

        //Doesn't allow user to bind the escape key, when escaping cancels the key rebinding.
        if (keyboardInputRequired && Input.GetKeyDown(KeyCode.Escape) && buttonToChange.tag != "CancelBinding")
        {
            keyBindLocalizedText.RevertText();

            keyboardInputRequired = false;        
            StartCoroutine(WaitToSelectKeyboardObject());

            return;
        }
        
        if(controllerInputRequired && controllerButtonPressed)
        {
            controllerInputRequired = false;
            PlayerPrefs.SetInt(buttonToChangeJoystick.tag, (int)(InputManager.ControllerButtonPressed()));

            UpdateControllerBindingDetails();
            FinalizeControllerBinding();
            return;
        }

        if (keyboardInputRequired && (Input.anyKeyDown))
        {
            mouseKeyCode = 0;

            //Checks for mouse buttons being selected as the bindings. Mouse keycodes start from 323.
            if (Input.GetMouseButtonDown(0))
            {
                UpdateMouseBindingDetails("mouse1", 323);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                UpdateMouseBindingDetails("mouse2", 324);
            }
            else if (Input.GetMouseButtonDown(2))
            {
                UpdateMouseBindingDetails("mouse3", 325);
            }
            //If input string is "", means the code is non-ASCII and need to use the event keycode. This
            //is for keys like F4 and Escape.
            else if (Input.inputString == "")
            {
                if (Event.current.keyCode.ToString() != "Escape")
                {
                    Debug.Log("HERE");

                    UpdateKeyboardBindingDetails("", false);
                }
                else
                {
                    UpdateKeyboardBindingDetails("escape", true);
                }
            }
            //A unique scenario for the spacebar which appears as nothing " " but needs text.
            else if (Input.inputString == " ")
            {
                UpdateKeyboardBindingDetails("space", true);
            }
            //Special handling for checking if a user has pressed backspace, return and enter.
            else if ((Input.inputString == "\n") || (Input.inputString == "\r"))
            {
                UpdateKeyboardBindingDetails("enter", true);
            }
            else if(Input.inputString == "\b")
            {
                UpdateKeyboardBindingDetails("backspace", true);
            }
            //If the user pressed a key which is resembled by an ASCII, uses the ASCII format. The ASCII
            //format looks like regular keys (e.g 4 instead of alpha4)
            else
            {
                userKeyPressed = Input.inputString;
                keyBindLocalizedText.textNeedsLocalization = false;
                PlayerPrefs.SetInt(buttonToChange.tag + "Local", 0);
                LocalizationManager.localizationManagerInstance.RemoveTextObjectFromList(keyBindLocalizedText);
            }

            PlayerPrefs.SetString(buttonToChange.tag + "Key", keyBindLocalizedText.GetDictionaryKey());

            keyboardInputRequired = false;
            FinalizeKeyBinding(userKeyPressed);
        }
    }

    //Ensures that when the script is activated that the menu options is functional.
    private void Awake()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            desktopBindings.SetActive(false);
            mobileBindings.SetActive(true);
            InitializeMobileControlSettings();
        }

        menuOptionsSelector = GetComponentInParent<MenuOptionsSelector>();
        menuOptionsSelector.KeyBindUpdateFinished();
        keyboardInputRequired = false;
        controllerInputRequired = false;

        InputManager.ConnectedControllers();
    }

    //Ensures the users saved bindings are loaded. Note these don't effect the Unity UI navigation.
    void Start()
    {
        InputManager.InitiateKeys();
        SetControllerButtonSprites();
    }

    private void Update()
    {
        if (keyboardInputRequired)
        {
            menuUIClass.PulsateText(buttonTextMesh);
        }

        if (controllerInputRequired)
        {
            menuUIClass.PulsateSprite(controllerButtonTransform[1]);
        }

        if (InputManager.IsControllerButtonPressed())
        {
            controllerButtonPressed = true;
        }
        else
        {
            controllerButtonPressed = false;
        }

    }

    //Changes the sprites for the controller bindings.
    private void SetControllerButtonSprites()
    {
        RetrieveSprite(confirmationControllerButton);
        RetrieveSprite(cancelControllerButton);
        RetrieveSprite(jumpControllerButton);
        RetrieveSprite(spinControllerButton);
        RetrieveSprite(slideControllerButton);
        RetrieveSprite(powerControllerButton);
    }

    private void RetrieveSprite(Button buttonToGetSprite)
    {
        controllerButtonImage = buttonToGetSprite.GetComponentsInChildren<Image>();
        controllerButtonImage[1].sprite = menuUIClass.InitializeButtonSprite(buttonToGetSprite);
    }

    //When the user selects a new control binding, will update all the controls.
    private void FinalizeControllerBinding()
    {
        InputManager.UpdateAllControls();
        StartCoroutine(WaitToSelectControllerObject());
    }

    //When the user selects a new key binding, will update the text, the playerpref and the keybinding.
    private void FinalizeKeyBinding(string newBindingKey)
    {
        keyBindLocalizedText.SetBinding(newBindingKey);

        if (mouseKeyCode != 0)
        {
            PlayerPrefs.SetInt(buttonToChange.tag, mouseKeyCode);
        }
        else
        {
            PlayerPrefs.SetInt(buttonToChange.tag, (int)Event.current.keyCode);
        }

        InputManager.UpdateAllKeys();
        StartCoroutine(WaitToSelectKeyboardObject());
    }
    
    //When the button is selected/clicked, remembers the button and disables UI navigation.
    //Triggers the coroutine and sets controllerinputrequired to true.
    public void ChangeControllerBinding(Button buttonToRemember)
    {
        //Remembers the button in case user decides to revert.
        buttonToChangeJoystick = buttonToRemember;

        menuOptionsCanvas.blocksRaycasts = false;

        buttonToRemember.GetComponent<Button>().colors = menuUIClass.ColourBlockShift(buttonToRemember.GetComponent<Button>().colors);

        EventSystem.current.SetSelectedGameObject(null);
        menuOptionsSelector.KeyBindUpdateStarted();

        StartCoroutine(SelectNewControllerBinding());
    }

    //Need to wait for the end of the frame before selecting new user input, otherwise it happens instantly.
    private IEnumerator SelectNewControllerBinding()
    {
        controllerButtonImage = buttonToChangeJoystick.GetComponentsInChildren<Image>();
        controllerButtonTransform = buttonToChangeJoystick.GetComponentsInChildren<RectTransform>();

        yield return new WaitForEndOfFrame();
        controllerInputRequired = true;
    }

    //Need to wait for the end of the frame to re-enable the UI otherwise it happens instantly and gets reselected.
    private IEnumerator WaitToSelectControllerObject()
    {
        yield return new WaitForEndOfFrame();

        menuOptionsCanvas.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(buttonToChangeJoystick.gameObject);
        buttonToChangeJoystick.GetComponent<Button>().colors = menuUIClass.RememberColourBlock();
        menuOptionsSelector.KeyBindUpdateFinished();

        menuUIClass.TurnOffPulsatingSprite(controllerButtonTransform[1]);
    }

    //When the button is selected/clicked, remembers the button and disables UI navigation.
    //Triggers the coroutine and sets keyboardinputrequired to true.
    public void ChangeKeyBinding(Button buttonToRemember)
    {
        //Remembers the button in case user decides to revert.
        buttonToChange = buttonToRemember;

        keyBindLocalizedText = buttonToChange.GetComponentInChildren<LocalizedText>();
        keyBindLocalizedText.UpdateTextNewBinding();
        menuOptionsCanvas.blocksRaycasts = false;
        
        buttonToRemember.GetComponent<Button>().colors = menuUIClass.ColourBlockShift(buttonToRemember.GetComponent<Button>().colors);
        
        EventSystem.current.SetSelectedGameObject(null);
        menuOptionsSelector.KeyBindUpdateStarted();
        StartCoroutine(SelectNewKeyboardBinding());
    }

    private void UpdateControllerBindingDetails()
    {        
        InputManager.UpdateAllControls();        
        controllerButtonImage = buttonToChangeJoystick.GetComponentsInChildren<Image>();  
        controllerButtonImage[1].sprite = menuUIClass.ControllerButtonSprite();
    }

    //Need to wait for the end of the frame before selecting new user input, otherwise it happens instantly.
    private IEnumerator SelectNewKeyboardBinding()
    {
        //Holds text mesh object for pulsating the text in Menu UI.
        buttonTextMesh = buttonToChange.GetComponentInChildren<TextMeshProUGUI>();

        yield return new WaitForEndOfFrame();
        keyboardInputRequired = true;
        menuUIClass.TurnOffAutosizing(buttonToChange);
    }

    //Need to wait for the end of the frame to re-enable the UI otherwise it happens instantly and gets reselected.
    private IEnumerator WaitToSelectKeyboardObject()
    {
        yield return new WaitForEndOfFrame();

        menuOptionsCanvas.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(buttonToChange.gameObject);
        buttonToChange.GetComponent<Button>().colors = menuUIClass.RememberColourBlock();
        menuOptionsSelector.KeyBindUpdateFinished();

        menuUIClass.TurnOnAutosizing(buttonToChange);
    }

    //If the user selects a mouse binding, updates the keypressed, and tells the LocalizedText script that this text
    //requires localizing. Updates the dictionary key to be localized. Also saves the details of this in Playerprefs.
    private void UpdateMouseBindingDetails(string mouseInputName, int newMouseKeyCode)
    {
        userKeyPressed = LocalizationManager.localizationManagerInstance.GetLocalizedValue(mouseInputName);
        keyBindLocalizedText.textNeedsLocalization = true;
        PlayerPrefs.SetInt(buttonToChange.tag + "Local", 1);
        LocalizationManager.localizationManagerInstance.AddTextObjectToList(keyBindLocalizedText);

        keyBindLocalizedText.UpdateDictionaryKey(mouseInputName, buttonToChange.tag);
        mouseKeyCode = newMouseKeyCode;
    }

    //If the user selects a keyboard binding, updates the keypressed depending on if the key needs to be localized or 
    //not (e.g mouse needs to be localized, key G does not). If the object needs to be localized, adds it to the 
    //Localization manager dictionary, if not it will remove it. Sets the playerpref for if the text needs localization
    //based on the boolean, using ternary operator ?.
    private void UpdateKeyboardBindingDetails(string keyboardInputName, bool needsLocalization)
    {
        if (needsLocalization)
        {
            userKeyPressed = LocalizationManager.localizationManagerInstance.GetLocalizedValue(keyboardInputName);
            LocalizationManager.localizationManagerInstance.AddTextObjectToList(keyBindLocalizedText);
            keyBindLocalizedText.UpdateDictionaryKey(keyboardInputName, buttonToChange.tag);
        }
        else
        {
            userKeyPressed = Event.current.keyCode.ToString();
            LocalizationManager.localizationManagerInstance.RemoveTextObjectFromList(keyBindLocalizedText);
        }

        keyBindLocalizedText.textNeedsLocalization = needsLocalization;
        PlayerPrefs.SetInt(buttonToChange.tag + "Local", needsLocalization == true ? 1 : 0);
    }

    //Returns all keybindings to their default state.
    public void SetControlsDefaults()
    {
        ResetKeyboardBinding(confirmationKeyboardButton, "enter", 1, 13);
        ResetKeyboardBinding(cancelKeyboardButton, "escape", 1, 27);
        ResetKeyboardBinding(jumpKeyboardButton, "space", 1, 32);
             
        ResetKeyboardBinding(spinKeyboardButton, KeyCode.Q.ToString(), 0, 113);
        ResetKeyboardBinding(slideKeyboardButton, KeyCode.E.ToString(), 0, 101);
        ResetKeyboardBinding(powerKeyboardButton, KeyCode.R.ToString(), 0, 114);
        ResetKeyboardBinding(upKeyboardButton, KeyCode.W.ToString(), 0, 119);
        ResetKeyboardBinding(downKeyboardButton, KeyCode.S.ToString(), 0, 115);
        ResetKeyboardBinding(leftKeyboardButton, KeyCode.A.ToString(), 0, 97);
        ResetKeyboardBinding(rightKeyboardButton, KeyCode.D.ToString(), 0, 100);

        InputManager.UpdateAllKeys();

        if (InputManager.controllerXbox)
        {
            ResetControllerBinding(confirmationControllerButton, 330);
            ResetControllerBinding(cancelControllerButton, 331);
            ResetControllerBinding(jumpControllerButton, 330);
            ResetControllerBinding(spinControllerButton, 332);
            ResetControllerBinding(slideControllerButton, 331);
            ResetControllerBinding(powerControllerButton, 333);
        }
        else if (InputManager.controllerPlaystation)
        {
            ResetControllerBinding(confirmationControllerButton, 331);
            ResetControllerBinding(cancelControllerButton, 332);
            ResetControllerBinding(jumpControllerButton, 331);
            ResetControllerBinding(spinControllerButton, 330);
            ResetControllerBinding(slideControllerButton, 332);
            ResetControllerBinding(powerControllerButton, 333);
        }
        else
        {
            ResetControllerBinding(confirmationControllerButton, 330);
            ResetControllerBinding(cancelControllerButton, 331);
            ResetControllerBinding(jumpControllerButton, 330);
            ResetControllerBinding(spinControllerButton, 332);
            ResetControllerBinding(slideControllerButton, 331);
            ResetControllerBinding(powerControllerButton, 333);
        }

        InputManager.UpdateAllControls();

    }

    //Resets a controller binding to default, by setting the original playerpref, and getting the sprite of the 
    //binding and updating it to the correct one.
    private void ResetControllerBinding(Button buttonToDefault, int controllerBinding)
    {
        PlayerPrefs.SetInt(buttonToDefault.tag, controllerBinding);
        Image[] controllerImage = buttonToDefault.GetComponentsInChildren<Image>();                     
        controllerImage[1].sprite = menuUIClass.InitializeButtonSprite(buttonToDefault);
    }

    //Returns a keybinding to default state. Sets whether the keybind needs localization, the string and the keycode
    //number corresponding to the key. Interacts with LocalizedText and LocalizationManager. 
    private void ResetKeyboardBinding(Button buttonToDefault, string newDictionaryKey, int localizationRequired, int keyBindingMapCode)
    {
        LocalizedText textLocalizer = buttonToDefault.GetComponentInChildren<LocalizedText>();

        PlayerPrefs.SetInt(buttonToDefault.tag + "Local", localizationRequired);
        PlayerPrefs.SetInt(buttonToDefault.tag, keyBindingMapCode);

        if (localizationRequired != 0)
        {
            textLocalizer.textNeedsLocalization = true;
            textLocalizer.UpdateDictionaryKey(newDictionaryKey, buttonToDefault.tag);
            LocalizationManager.localizationManagerInstance.AddTextObjectToList(textLocalizer);
            textLocalizer.SetBinding(LocalizationManager.localizationManagerInstance.GetLocalizedValue(newDictionaryKey));
        }
        else
        {
            textLocalizer.textNeedsLocalization = false;
            LocalizationManager.localizationManagerInstance.RemoveTextObjectFromList(textLocalizer);
            textLocalizer.SetBinding(newDictionaryKey);
        }

        PlayerPrefs.SetString(buttonToDefault.tag + "Key", textLocalizer.GetDictionaryKey());        
    }
         
    //Toggles the Demo UI inversion depending on what is selected.
    public void InvertedUI()
    {
        if (invertedControlsToggle.isOn)
        {
            normalDemoUI.SetActive(false);
            invertedDemoUI.SetActive(true);
        }
        else
        {
            invertedDemoUI.SetActive(false);
            normalDemoUI.SetActive(true);
        }

        PlayerPrefs.SetInt("MobileInvert", invertedControlsToggle.isOn ? 1 : 0);
    }

    //Updates the number of the Opacity and calls the method that makes the example UI opaque.
    public void SetMenuOpacity(Slider sliderChanged)
    {
        TextMeshProUGUI sliderOpacityText = sliderChanged.GetComponentInChildren<TextMeshProUGUI>();
        UpdateSliderText(sliderChanged, sliderOpacityText);
        OpaqueUI(sliderChanged.value);
        PlayerPrefs.SetFloat(sliderChanged.tag, sliderChanged.value);
        mobileUI.CorrectUIOpacity();
    }

    //Updates the text next to a slider to properly represent the value. Uses a value of 0-100 instead of 0-1.
    private void UpdateSliderText(Slider sliderToUpdate, TextMeshProUGUI sliderTextToUpdate)
    {
        int opacitySliderNumber = Mathf.RoundToInt(sliderToUpdate.value * 100);
        sliderTextToUpdate.text = opacitySliderNumber.ToString();
    }

    //Makes each object in the UI demonstration opaque, sets outer joystick individually so it doesn't ruin the image.
    private void OpaqueUI(float opacityValue)
    {
        foreach (Image imageToEdit in demoUI)
        {
            imageToEdit.color = new Color(1, 1, 1, opacityValue);
        }
    }

    //Sets all the defaults for mobile controls, opacity is 1, vibration is on and inversion is off.
    public void SetMobileControlsDefaults()
    {
        opacitySlider.value = 1;
        SetMenuOpacity(opacitySlider);

        vibrationToggle.isOn = true;
        ToggleVibration(false);

        invertedControlsToggle.isOn = false;
        InvertedUI();
    }

    //When the user is on mobile, initializes the defaults for mobile control settings.
    private void InitializeMobileControlSettings()
    {
        vibrationToggle.isOn = PlayerPrefs.GetInt("MobileVibration", 1) != 0;
        opacitySlider.value = PlayerPrefs.GetFloat("MenuOpacity", 1);
        SetMenuOpacity(opacitySlider);

        invertedControlsToggle.isOn = PlayerPrefs.GetInt("MobileInvert", 0) != 0;
        InvertedUI();
    }

    //Turns the settings for vibration on or off.
    public void ToggleVibration(bool sampleVibration)
    {
        if (sampleVibration)
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }

        PlayerPrefs.SetInt("MobileVibration", (vibrationToggle.isOn ? 1 : 0));
    }

    //This changes the background panel of a slider. Needs own method so can change multiple colours at once.
    public void HighlightSliderBackground(Slider sliderToChange)
    {
        sliderToChange.GetComponentInChildren<Image>().color = uiColourBlock.highlightedColor;
    }

    //This changes the background panel of a slider back to normal. Needs to change multiple colours at once.
    public void RevertSliderBackground(Slider sliderToChange)
    {
        sliderToChange.GetComponentInChildren<Image>().color = uiColourBlock.normalColor;
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

}
