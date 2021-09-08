using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
    public GameObject menuMain;         //Main menu for navigating the game.
    public GameObject menuLevelSelect;  //Level Select menu for turning on/off menu when going back.

    public Button levelOneButton;       //The first level button, to be highlighted when level select is chosen.
    public Toggle redTokenToggle;       //The first toggle, for highlighting when the user clicks on a level.


         
    private void Start()
    {
        SelectButtonLevelOne();
    }

    //Onclick function for touch and mouse input.
    public void SelectLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    //Disables the current menu and re-enables the main menu.
    public void ExitToMainMenu()
    {
        menuLevelSelect.SetActive(false);
        menuMain.SetActive(true);
    }
    
    //Method is called from the HexagonSelector class when clicking on the Level Select menu option.
    public void SelectButtonLevelOne()
    {
        levelOneButton.Select();
    }

    public void SelectToggleRedToken()
    {
        redTokenToggle.Select();
    }




    /*
         *       

        public Image shipsWheel;
        private bool wheelTurning;
        private float wheelDegreesRotated = 0;
        private float wheelDegreesToRotate = 0;
        public CanvasGroup menuOptionsCanvas;           //Canvas group for disabling touch input/UI navigation.
        GameObject objecttocapture;

         * 
         * 
         * 
         * 
        if(InputManager.HorizontalInput() > 0 && !wheelTurning)
        {
            //nokeypressed = false;


            objecttocapture = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight.gameObject;
            //objecttocapture = objecttocapture.GetComponent<Button>().navigation.selectOnLeft.gameObject;
            //menuOptionsCanvas.blocksRaycasts = false;
            //EventSystem.current.SetSelectedGameObject(null);

            wheelDegreesToRotate = wheelDegreesToRotate + 60;
            wheelTurning = true;
        }
        else if (InputManager.HorizontalInput() < 0 && !wheelTurning)
        {
            //nokeypressed = false;


            objecttocapture = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft.gameObject;
            //objecttocapture = objecttocapture.GetComponent<Button>().navigation.selectOnRight.gameObject;
            //menuOptionsCanvas.blocksRaycasts = false;
            //EventSystem.current.SetSelectedGameObject(null);

            wheelDegreesToRotate = wheelDegreesToRotate - 60;
            wheelTurning = true;
        }

        //EventSystem.current.SetSelectedGameObject(objecttocapture);

        if (wheelDegreesToRotate > wheelDegreesRotated)
        {
            shipsWheel.transform.Rotate(0, 0, 1);
            wheelDegreesRotated += 1;
            //Debug.Log("destination is: " + destination + " current is: " + currentspot);

        }
        else if (wheelDegreesToRotate < wheelDegreesRotated)
        {
            shipsWheel.transform.Rotate(0, 0, -1);
            wheelDegreesRotated -= 1;
        }
        else
        {
            //menuOptionsCanvas.blocksRaycasts = true;
            //EventSystem.current.SetSelectedGameObject(objecttocapture);

            wheelTurning = false;
            wheelDegreesToRotate = 0;
            wheelDegreesRotated = 0;

        }
        */







    /* The following code is how controllers were manually coded, but this is unnecessary after properly
     * using Unity UI.
     *     
    public GameObject[] levelButtons;   //Holds the button for each level in the game.
    private int levelSelected = 0;      //Tracks what level is highlighted from the level select screen.
    private float durationKeyHeld;      //Tracks how long the player has held an axis key down.

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
        LevelHighlight(levelSelected);
    }

    //Every frame checks for user input on the movement axis, and if the user is holding down a button
    //this will also register.
    public void CheckUserInput()
    {
        if (Input.anyKeyDown || durationKeyHeld > 0.12)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                LevelSelection();
                //CorrectScrollbarOnUserInput();
                durationKeyHeld = 0;
            }
        }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            durationKeyHeld += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            levelButtons[levelSelected].GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToMainMenu();
        }
    }



    //Depending on the input of the user, will move the levelselection in that direction if there is a 
    //level to move to.
    public void LevelSelection()
    {
        if (Input.GetAxisRaw("Horizontal") > 0 && levelSelected +1 !=  levelButtons.Length)
        {
            LevelDehighlight(levelSelected);
            levelSelected = levelSelected + 1;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && levelSelected != 0)
        {
            LevelDehighlight(levelSelected);
            levelSelected = levelSelected - 1;
        }
        else if (Input.GetAxisRaw("Vertical") < 0 && !(levelSelected > levelButtons.Length - 7))
        {
            LevelDehighlight(levelSelected);
            levelSelected = levelSelected + 6;
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && !(levelSelected < 6))
        {
            LevelDehighlight(levelSelected);
            levelSelected = levelSelected - 6;
        }
    }

    //Highlights the button of the level the user has selected with their keyboard.
    public void LevelHighlight(int levelToHighlight)
    {
        levelButtons[levelToHighlight].GetComponent<Image>().color = ColourOfLevel(levelSelected);
    }

    //Removes the highlight when transitioning to a new menu selection.
    public void LevelDehighlight(int levelToDehighlight)
    {
        levelButtons[levelToDehighlight].GetComponent<Image>().color = Color.white;
    }

    //Determines the menu colour of the level based on menu position.
    public Color ColourOfLevel(int levelSelected)
    {
        Color levelColour = Color.white;

        if (levelSelected < 6)
        {
            levelColour = Color.green;
        }
        else if (levelSelected >= 6 && levelSelected < 12)
        {
            levelColour = Color.grey;
        }
        else if (levelSelected >= 12 && levelSelected < 18)
        {
            levelColour = Color.red;
        }
        else if (levelSelected >= 18 && levelSelected < 24)
        {
            levelColour = Color.yellow;
        }
        else if (levelSelected >= 24 && levelSelected < 30)
        {
            levelColour = Color.blue;
        }
        else 
        {
            levelColour = Color.black;
        }

        return levelColour;
    }

    */



}
