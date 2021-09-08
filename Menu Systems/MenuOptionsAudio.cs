using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MenuOptionsAudio : MonoBehaviour
{    
    public AudioMixer audioMixer;               //The Audiomixer that all audio sources in the project are routed to.

    public Slider musicSlider;                  //The slider in the options menu for controlling the music volume.
    public Slider soundEffectsSlider;           //The slider in the options menu for controlling the sound effects volume.
    public Slider menuSoundEffectsSlider;       //The slider in the options menu for controlling the menu volume.

    public Toggle muteMusicToggle;              //The music toggle that will be manually set when script is started.
    public Toggle muteSoundEffectsToggle;       //The sound effects toggle that will be manually set when script is started.
    public Toggle muteMenuSoundEffectsToggle;   //The menu sound toggle that will be manually set when script is started.

    [Header("Colourblock Audio")]               //Sets the heading in the inspector.
    public ColorBlock uiColourBlock;            //Stores 4 colours to be used for UI elements.

    //Remembers the volume choices of the user even after the app has been closed. Uses PlayerPrefs.
    private void Start()
    {
        /*Currently not in use. May be needed in future for mobile colouring.
        if (!(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
        {
            SetHighlightColourForMobiles();
        }
        */

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 0.5f);
        menuSoundEffectsSlider.value = PlayerPrefs.GetFloat("MenuSoundEffectsVolume", 0.5f);

        muteMusicToggle.isOn = (PlayerPrefs.GetInt("MusicVolumeToggle", 0) != 0);
        muteSoundEffectsToggle.isOn = (PlayerPrefs.GetInt("SoundEffectsVolumeToggle", 0) != 0);
        muteMenuSoundEffectsToggle.isOn = (PlayerPrefs.GetInt("MenuSoundEffectsVolumeToggle", 0) != 0);

        InitializeUserVolumeSettings();
    }

    private void Update()
    {
        //If a keyboard key is pressed, ensures that all the sliders are selectable. This is to prevent a bug when using both
        //mouse input and keyboard input which could prevent the sliders from being selectable.
        //Eventually want to replace this code with a keyboard/mouse manager class.
        if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            SelectObject(musicSlider.gameObject);
            SelectObject(soundEffectsSlider.gameObject);
            SelectObject(menuSoundEffectsSlider.gameObject);
        }
    }

    //Obtains a slider from the object, gets the text object from the child of the slider, converts the slider
    //value into decibels, updates the audiomixer based on the tag of the slider and then updates the text
    //of the slider to be more user friendly.
    public void SetVolumeOfSlider(Slider sliderChanged)
    {
        TextMeshProUGUI sliderVolumeText = sliderChanged.GetComponentInChildren<TextMeshProUGUI>();
        Toggle sliderCheckbox = GameObject.FindGameObjectWithTag(sliderChanged.tag).GetComponentInParent<Toggle>();

        float volumeOfSlider = ConvertToDecibel(sliderChanged.value);

        //Will only update the volume if the mute box isn't checked, but will still change the value of the slider.
        if (!sliderCheckbox.isOn)
        {
            audioMixer.SetFloat(sliderChanged.tag, volumeOfSlider);
        }

        UpdateSliderText(sliderChanged, sliderVolumeText);

        //Remembers the volume for each slider, the playerpref is the tag name of the object, which is also 
        //the audio mixer exposed property.
        PlayerPrefs.SetFloat(sliderChanged.tag, sliderChanged.value);
    }

    //Slider uses values 0 to 1 but decibels works on a logarithmic scale from -80 to 20, this will convert.
    private float ConvertToDecibel(float volumeToConvert)
    {
        return Mathf.Log10(Mathf.Max(volumeToConvert, 0.0001f)) * 20f;
    }

    //Takes the Slider and Text and converts the float value from the slider to a recognizable int (0-100), 
    //then converts this int to a string and updates the text field.
    private void UpdateSliderText(Slider sliderToUpdate, TextMeshProUGUI sliderTextToUpdate)
    {
        int volumeSliderNumber = Mathf.RoundToInt(sliderToUpdate.value * 100);

        sliderTextToUpdate.text = volumeSliderNumber.ToString();

        //The following line of code will convert any language to an int in the dictionary. May be necessary but
        //I imagine it takes up more resources.
        //sliderToUpdate.GetComponentInChildren<LocalizedText>().UpdateDictionaryKey(volumeSliderNumber.ToString());
    }

    //Obtains the checkbox from the slider, and if it is toggled on it will mute the sound, if not it will call
    //the regular volume slider function and set the audio mixer volume to what it should be.
    public void MuteVolume(Slider sliderToMute)
    {
        Toggle sliderMuteCheckbox = GameObject.FindGameObjectWithTag(sliderToMute.tag).GetComponentInParent<Toggle>();

        if (sliderMuteCheckbox.isOn)
        {
            audioMixer.SetFloat(sliderToMute.tag, -80.0f);
        }
        else
        {
            SetVolumeOfSlider(sliderToMute);
        }

        //Adds 'toggle' to the playerpref string to differentiate it from existing playerpref
        PlayerPrefs.SetInt(sliderToMute.tag + "Toggle", (sliderMuteCheckbox.isOn ? 1 : 0));
    }

    //When the player selects the defaults button, returns each slider to their defaults.
    public void SetDefaultsAudio()
    {
        ResetAudioSettings(musicSlider);
        ResetAudioSettings(soundEffectsSlider);
        ResetAudioSettings(menuSoundEffectsSlider);
    }

    //Resets sliders and toggles to their default values.
    private void ResetAudioSettings(Slider sliderToReset)
    {
        sliderToReset.value = 0.5f;
        SetVolumeOfSlider(sliderToReset);

        Toggle sliderMuteCheckbox = GameObject.FindGameObjectWithTag(sliderToReset.tag).GetComponentInParent<Toggle>();

        if (sliderMuteCheckbox.isOn)
        {
            MuteVolume(sliderToReset);
            sliderMuteCheckbox.isOn = false;
        }
    }

    //Sets the volume of the sliders using playerprefs on startup.
    private void InitializeUserVolumeSettings()
    {
        SetVolumeOfSlider(musicSlider);
        SetVolumeOfSlider(soundEffectsSlider);
        SetVolumeOfSlider(menuSoundEffectsSlider);
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
       
    //This is used for the slider components so they can be used again after they have been disabled.
    public void SelectObject(GameObject uiToSelect)
    {
        if(uiToSelect.GetComponent<Selectable>().enabled == false)
        {
            uiToSelect.GetComponent<Selectable>().enabled = true;
        }

    }

    //This is used for the slider components so they cannot still be used when the user has dragged their 
    //finger away from them.
    public void DeselectObject(GameObject uiToDeselect)
    {
        if (uiToDeselect.GetComponent<Selectable>().enabled == true)
        {
            uiToDeselect.GetComponent<Selectable>().enabled = false;
        }
    }

    /*             
        //The following code is useful for finding an object within a child based on a tag.
        Image sliderBack = sliderToRevert.GetComponentInChildren<Image>(GameObject.FindGameObjectWithTag("SliderBackground"));
    */

    //If playing on mobile devices, highlighted UI elements behave slightly differently for touch input
    //than they do for mouse input, so it's easier to revert the colours to stop them being highlighted.
    /*
    public void SetHighlightColourForMobiles()
    {
        muteMusicToggle.colors = uiColourBlock;
        muteSoundEffectsToggle.colors = uiColourBlock;
        muteMenuSoundEffectsToggle.colors = uiColourBlock;
    }
    */
}
