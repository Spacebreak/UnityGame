using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionsSelector : MonoBehaviour
{
    public static MenuOptionsSelector menuOptionsSelector;  //Ensure only one instance of Menu Options exists.

    public Button menuOptionsGeneralButton; //The first menu tab, to be selected first.

    public GameObject menuMain;             //Main menu for navigating the game.
    public GameObject menuOptions;          //Options menu for turning on/off menu when selected.
    public GameObject menuHowToPlay;        //How to play menu for checking which menu is activated by the user.
    public Button menuHowToPlayBackButton;  //Back button on HowToPlay menu so onclick method can be invoked via code.

    public List<Dropdown> allDropdownItems; //Holds all the dropdowns in the menu. Exiting checks no dropdowns are selected.
    private bool keyBindUpdating = false;   //For checking when a keybind is being updated in MenuOptionsControls class.

    public Image menuMainPanelImage;
    public Sprite panelButtonRed;
    public Sprite panelButtonGreen;
    public Sprite panelButtonBlue;
    public Sprite panelButtonYellow;
    
    private void Awake()
    {
        //If no previous instances of LocalizationManager have been created, set this instance as the one.
        if (menuOptionsSelector == null)
        {
            Debug.Log("Menu Options Not Instantiated");
            menuOptionsSelector = this;
        }
        //Ensures only one instance of LocalizationManager exists by destroying prior instances.
        else if (menuOptionsSelector != this)
        {
            Destroy(gameObject);
        }

        //Will persist when new scenes are loaded.
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }

    private void FixedUpdate()
    {
        //It was fine to call this in FixedUpdate until the InputManager class was used instead of directly going through
        //the Unity Input class. 
        //CheckUserInput();
    }

    void SelectGeneralButton()
    {
        menuOptionsGeneralButton.Select();
    }

    void CheckUserInput()
    {
        //BButton is the button for escaping/returning.
        if (InputManager.GetCancel())//BackButton()) 
        {
            //Ensures no dropdowns are selected or keybindings are being updated before exiting.
            if (CheckAllDropdowns() || keyBindUpdating)
            {
                return;
            }
            else
            {
                ExitToMainMenu();
            }
        }
    }

    //Disables the current menu and re-enables the main menu.
    public void ExitToMainMenu()
    {
        //Nested menus means if I press escape on the lowest level instead of returning to the middle level menu
        //I would return to the highest level, checking to see which menu is active avoids this scenario.
        if (menuHowToPlay.activeSelf)
        {
            menuHowToPlayBackButton.onClick.Invoke();
        }
        else
        {
            menuOptions.SetActive(false);
            menuMain.SetActive(true);
        }
    }

    public void AddDropdownToList(Dropdown dropdownToAdd)
    {
        allDropdownItems.Add(dropdownToAdd);
    }

    //Checks every dropdown to ensure none of them are currently opened.
    public bool CheckAllDropdowns()
    {
        bool DropdownOpen = false;

        foreach (Dropdown dropdown in allDropdownItems)
        {
            if(dropdown.transform.childCount > int.Parse(dropdown.tag))
            {
                DropdownOpen = true;
            }
        }

        return DropdownOpen;
    }

    //Called in MenuOptionsControls, when a keybind is being changed.
    public void KeyBindUpdateStarted()
    {
        keyBindUpdating = true;
    }

    public void KeyBindUpdateFinished()
    {
        keyBindUpdating = false;
    }

    public void UpdatePanelColour(int panelIndex)
    {
        if(panelIndex == 0)
        {
            menuMainPanelImage.sprite = panelButtonYellow;
        }
        else if(panelIndex == 1)
        {
            menuMainPanelImage.sprite = panelButtonRed;
        }
        else if (panelIndex == 2)
        {
            menuMainPanelImage.sprite = panelButtonGreen;
        }
        else if (panelIndex == 3)
        {
            menuMainPanelImage.sprite = panelButtonBlue;
        }
        else
        {
            Debug.Log("MenuOptionsSelector: No Panel Index Given.");
        }
    }



    /* The majority of the following script became redundant when discovering how to move through Unity UI
     * elements. A similar code is used for the Main Menu script, but all other sub menus will be using the
     * default Unity Eventhandler for moving.
     * 
    public GameObject menuMain;         //Main menu for navigating the game.
    public GameObject menuOptions;      //Options menu for turning on/off menu when selected.
    public GameObject menuHowToPlay;    //How to play menu for checking which menu is activated by the user.
    public Button menuHowToPlayBackButton; //Back button on HowToPlay menu so onclick method can be invoked via code.
    public Button[] menuOptionButtons;  //Stores the buttons of all the options menu tabs.
    private float durationKeyHeld;      //Tracks how long the player has held an axis key down.
    private int currentPosition = 0;    //Current menu Position from 0 to 3.
    private int nextPosition;           //Menu position to move towards.

    //Every frame checks for user input on the movement axis, and if the user is holding down a button
    //this will also register.
    public void CheckUserInput()
    {
        if (Input.anyKeyDown || durationKeyHeld > 0.20)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                MenuOptionSelected();
                durationKeyHeld = 0;
            }
        }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            durationKeyHeld += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToMainMenu();
        }        
    }

    //Checks the current menu position of the user, and if they input horizontal will readjust what next menu
    //position they should be at.
    public void MenuOptionSelected()
    {
        if (currentPosition == 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                nextPosition = 1;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                nextPosition = 3;
            }
        }
        else if (currentPosition == 1)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
            nextPosition = 2;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                nextPosition = 0;
            }
        }
        else if (currentPosition == 2)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                nextPosition = 3;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                nextPosition = 1;
            }
        }
        else if (currentPosition == 3)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                nextPosition = 0;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                nextPosition = 2;
            }
        }

        SetCurrentPosition(nextPosition);
    }

    //Sets the menu options tab that is selected by the user depending on which button they click.
    public void SetCurrentPositionOnClick(int clickedPosition)
    {
        currentPosition = clickedPosition;
        SetCurrentPosition(currentPosition);
    }

    //Sets the menu options tab that is selected by the user (General/Controls/Video/Audio)
    private void SetCurrentPosition(int selectedPosition)
    {
        menuOptionButtons[selectedPosition].Select();
        //menuOptionButtons[selectedPosition].onClick.Invoke();
        currentPosition = selectedPosition;
    }
    */
}
