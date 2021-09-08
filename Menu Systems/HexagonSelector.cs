using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class HexagonSelector : MonoBehaviour
{
    private Rigidbody selectorRigidBody;        //Rigidbody of the spherical selector, used for moving object to different menus.
    public Transform[] selectorPositions;       //Array of empty objects at the 6 different positions on the menu.
    public GameObject[] menuTextMeshObjects;    //Array of UI Text objects at the 6 positions so their class can be accessed.
    public GameObject menuMain;                 //Main menu for navigating the game.
    public GameObject menuOptions;              //Options menu for turning on/off menu when selected.
    public GameObject menuLevelSelect;          // Level select menu for choosing a game level.

    private float colourShiftSpeed;             //Time left for interpolating colours to change.
    public float colourChangeSpeed;             //Time left for interpolating colours to change - able to set in inspector.
    public float selectorSpeed;                 //How fast selector moves.
    private bool selectorMovement;              //Tracks if the selector is moving or not before accepting anymore input.

    private int currentPosition;                //Current menu Position from 0 to 5.
    private int nextPosition;                   //Menu position to move towards.    

    private Color targetColour = Color.white;   //Selector colour of next menu option.
    private RawImage selectorImage;             //RawImage control that stores the image of the selector.
    
    private MenuOptionsSelector menuOptionsScript; //The class that holds the method for activating the general button.
    private LevelSelector menuLevelSelectScript;//The class that holds the method for selecting the first level in Level Select.

    void Start()
    {
        InputManager.UpdateAllKeys();
        InputManager.UpdateAllControls();

        selectorRigidBody = GetComponent<Rigidbody>();
        selectorImage = GetComponent<RawImage>();
        menuOptionsScript = menuOptions.GetComponent<MenuOptionsSelector>();

        menuLevelSelectScript = menuLevelSelect.GetComponent<LevelSelector>();
    }

    //Update is called once per frame
    void Update()
    {
        if (!selectorMovement && (InputManager.HorizontalInput() != 0 || InputManager.VerticalInput() != 0))
        {
            SelectorDirection();
        }

        if (InputManager.GetConfirm())
        {
            SelectMenuOption(currentPosition);
        }

    }

    private void FixedUpdate()
    {
        MoveSelector(nextPosition);
        ColourShift(currentPosition, nextPosition);
    }

    //Looks at the current position of the selector, and depending on which position will change
    //the inputs required to move to the adjacted positions.
    void SelectorDirection()
    {
        if (currentPosition == 0)
        {
            if (InputManager.HorizontalInput() > 0)
            {
                nextPosition = 1;
            }
            else if (InputManager.HorizontalInput() < 0)
            {
                nextPosition = 5;
            }

        }

        if (currentPosition == 1)
        {
            if (InputManager.VerticalInput() < 0)
            {
                nextPosition = 2;
            }
            else if (InputManager.HorizontalInput() < 0 || InputManager.VerticalInput() > 0)
            {
                nextPosition = 0;
            }

        }

        if (currentPosition == 2)
        {
            if (InputManager.HorizontalInput() < 0 || InputManager.VerticalInput() < 0)
            {
                nextPosition = 3;
            }
            else if (InputManager.VerticalInput() > 0)
            {
                nextPosition = 1;
            }

        }

        if (currentPosition == 3)
        {
            if (InputManager.HorizontalInput() > 0)
            {
                nextPosition = 2;
            }
            else if (InputManager.HorizontalInput() < 0)
            {
                nextPosition = 4;
            }

        }

        if (currentPosition == 4)
        {
            if (InputManager.HorizontalInput() > 0 || InputManager.VerticalInput() < 0)
            {
                nextPosition = 3;
            }
            else if (InputManager.VerticalInput() > 0)
            {
                nextPosition = 5;
            }

        }

        if (currentPosition == 5)
        {
            if (InputManager.HorizontalInput() > 0 || InputManager.VerticalInput() > 0)
            {
                nextPosition = 0;
            }
            else if (InputManager.VerticalInput() < 0)
            {
                nextPosition = 4;
            }

        }
    }

    void MoveSelector(int positionToMoveTo)
    {
        Vector3 selectorPosition; //Physical coordinates of the selectors' rigidbody.

        if (GetComponent<Rigidbody>().position != selectorPositions[positionToMoveTo].position)
        {
            selectorMovement = true;
            DecreaseTextSize(currentPosition);

            selectorPosition = Vector3.MoveTowards(transform.position, selectorPositions[positionToMoveTo].position, selectorSpeed * Time.deltaTime);
            selectorRigidBody.MovePosition(selectorPosition);
            currentPosition = positionToMoveTo;
        }
        else
        {
            selectorMovement = false;
            IncreaseTextSize(currentPosition);
        }

    }

    //Changes the selector colour linearly as it moves to the next menu option.
    void ColourShift(int selectorPosition, int positionToMoveTo)
    {
        if (colourShiftSpeed <= Time.deltaTime)
        {
            //Change the colour of the material to the target colour, the final change.
            selectorImage.color = targetColour;

            //Acquire a new colour to transform to based on the position the selector is moving to.
            targetColour = ColourPosition(positionToMoveTo);
            colourShiftSpeed = colourChangeSpeed;
        }
        else
        {
            //Calculate and change colour of the material via interpolation.
            selectorImage.color = Color.Lerp(selectorImage.color, targetColour, Time.deltaTime / colourShiftSpeed); //

            //Update time remaining.
            colourShiftSpeed = colourShiftSpeed - (Time.deltaTime * 4);
        }        
    } 

    //Returns a colour based on menu UI position.
    Color ColourPosition(int selectorPosition)
    {
        Color selectorColour;

        if (selectorPosition == 0)
        {
            selectorColour = Color.white;
        }
        else if (selectorPosition == 1)
        {
            selectorColour = Color.blue;
        }
        else if (selectorPosition == 2)
        {
            selectorColour = Color.red;
        }
        else if (selectorPosition == 3)
        {
            selectorColour = Color.black;
        }
        else if (selectorPosition == 4)
        {
            selectorColour = Color.yellow;
        }
        else if (selectorPosition == 5)
        {
            selectorColour = Color.green;
        }
        else
        {
            selectorColour = Color.clear;
        }

        return selectorColour;
    }
    
    //Will perform the action of the chosen menu option.
    void SelectMenuOption(int currentPosition)
    {
        switch (currentPosition)
        {
            case 0:
                SceneManager.LoadScene("WorldOneLevelOne", LoadSceneMode.Single);
                break;

            case 1:
                Debug.Log("Vault");
                break;

            case 2:
                Debug.Log("Colections");
                break;

            case 3:
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;

            case 4:
                menuMain.SetActive(false);
                menuOptions.SetActive(true);
                menuOptionsScript.menuOptionsGeneralButton.Select();
                break;

            case 5:
                menuMain.SetActive(false);
                menuLevelSelect.SetActive(true);
                menuLevelSelectScript.SelectButtonLevelOne();
                break;
        }
    }

    public void ButtonStart()
    {
        SceneManager.LoadScene("WorldOneLevelOne", LoadSceneMode.Single);
    }

    //Deactivates main menu, activates level select menu.
    public void ButtonLevelSelect()
    {
        menuMain.SetActive(false);
        menuLevelSelect.SetActive(true);
    }

    //Deactivates main menu, activates options menu.
    public void ButtonOptions()
    {
        menuMain.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void ButtonExit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //Increases the text size of the menu option hovered over.
    public void IncreaseTextSize(int selectorPosition)
    {
        menuTextMeshObjects[selectorPosition].GetComponent<EditTextOnHover>().SetFontSizeActive();
        //menuTextMeshObjects[selectorPosition].GetComponent<EditTextOnHover>().SetFontSizeActive();

    }

    //Decreases the text size of the menu option when moving to another menu option.
    public void DecreaseTextSize(int selectorPosition)
    {
        menuTextMeshObjects[selectorPosition].GetComponent<EditTextOnHover>().SetFontSizeNotActive();
    }

}
