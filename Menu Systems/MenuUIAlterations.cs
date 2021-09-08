using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUIAlterations : MonoBehaviour
{
    //Used in MenuOptionsControls Class to keep button highlighted when selecting a new key binding.
    public ColorBlock menuControlsBinding;      //The colour block for when a user is rebinding their key.
    private ColorBlock colourBlockToRemember;   //The original colour block to remember so the change can be reverted.
    
    //Used in MenuOptionsControls Class to pulsate text.
    private bool increasingTextSize;            //If a pulsating text object should be increasing in size or decreasing.
    private float originalSize;                 //The original size of the pulsating text, to ensure text stays within bounds.
    private bool IncreasingSpriteSize;          //If a pulsating sprite should be increasing in size or decreasing.  

    //This ScrollRect is for correcting the Scrollbar position depending on what child component is selected.
    [Header("ScrollRect Correction")]
    public ScrollRect bindingsScroller;         //Stores the scroll rect attached to the control bindings.
    public ScrollRect levelScroller;            //Stores the scroll rect attached to the level select buttons.
    
    [Header("Xbox Controller Sprites")]
    public Sprite xboxControllerButton0;        //The sprite for the xbox 0th controller button.
    public Sprite xboxControllerButton1;        //The sprite for the xbox 1st controller button.
    public Sprite xboxControllerButton2;        //The sprite for the xbox 2nd controller button.
    public Sprite xboxControllerButton3;        //The sprite for the xbox 3rd controller button.
    public Sprite xboxControllerButton4;        //The sprite for the xbox 4th controller button.
    public Sprite xboxControllerButton5;        //The sprite for the xbox 5th controller button.
    public Sprite xboxControllerButton6;        //The sprite for the xbox 6th controller button.
    public Sprite xboxControllerButton7;        //The sprite for the xbox 7th controller button.
    public Sprite xboxControllerButton8;        //The sprite for the xbox 8th controller button.
    public Sprite xboxControllerButton9;        //The sprite for the xbox 9th controller button.
    public Sprite xboxControllerButton10;       //The sprite for the xbox 10th controller button.
    public Sprite xboxControllerButton11;       //The sprite for the xbox 11th controller button.
    public Sprite xboxControllerButton12;       //The sprite for the xbox 12th controller button.
    public Sprite xboxControllerButton13;       //The sprite for the xbox 13th controller button.
    public Sprite xboxControllerButton14;       //The sprite for the xbox 14th controller button.
    public Sprite xboxControllerButton15;       //The sprite for the xbox 15th controller button.
    public Sprite xboxControllerButton16;       //The sprite for the xbox 16th controller button.
    public Sprite xboxControllerButton17;       //The sprite for the xbox 17th controller button.
    public Sprite xboxControllerButton18;       //The sprite for the xbox 18th controller button.
    public Sprite xboxControllerButton19;       //The sprite for the xbox 19th controller button.

    [Header("Level Select Navigation Options")]
    //These buttons are for navigating to when pressing right on the scrollbar.
    public Button level1Button;                 //A button for selecting a level.
    public Button level7Button;                 //A button for selecting a level.
    public Button level13Button;                //A button for selecting a level.
    public Button level19Button;                //A button for selecting a level.
    public Button level25Button;                //A button for selecting a level.
    public Button level31Button;                //A button for selecting a level.

    //These buttons are for navigating to when pressing left on the scrollbar.
    public Button level6Button;                 //A button for selecting a level.
    public Button level12Button;                //A button for selecting a level.
    public Button level18Button;                //A button for selecting a level.
    public Button level24Button;                //A button for selecting a level.
    public Button level30Button;                //A button for selecting a level.
    public Button level36Button;                //A button for selecting a level.                                          

    [Header("Binding Navigation Options")]
    //These buttons are for navigating to when pressing right on the scrollbar.
    public Button confirmationKeyboardButton;       //The keyboard button for confirming.
    public Button cancelKeyboardButton;             //The keyboard button for cancelling.
    public Button jumpKeyboardButton;               //The keyboard button for jumping.
    public Button spinKeyboardButton;               //The keyboard button for spinning.
    public Button slideKeyboardButton;              //The keyboard button for sliding and diving.
    public Button powerKeyboardButton;              //The keyboard button for using the power.
    public Button upKeyboardButton;                 //The keyboard button for moving up.
    public Button downKeyboardButton;               //The keyboard button for moving down.
    public Button leftKeyboardButton;               //The keyboard button for moving left. 
    public Button rightKeyboardButton;              //The keyboard button for moving right.
    //These buttons are for navigating to when pressing left on the scrollbar.
    public Button confirmationControllerButton;     //The controller button for confirming.
    public Button cancelControllerButton;           //The controller button for cancelling.
    public Button jumpControllerButton;             //The controller button for jumping. 
    public Button spinControllerButton;             //The controller button for spinning.
    public Button slideControllerButton;            //The controller button for sliding and diving.
    public Button powerControllerButton;            //The controller button for using the power.
    public Button upControllerButton;               //The controller button for moving up. 
    public Button downControllerButton;             //The controller button for moving down. 
    public Button leftControllerButton;             //The controller button for moving left. 
    public Button rightControllerButton;            //The controller button for moving right. 



    public Sprite InitializeButtonSprite(Button controllerButton)
    {
        int controllerButtonPreference = PlayerPrefs.GetInt(controllerButton.tag);

        switch (controllerButtonPreference)
        {
            case 330: 
                {
                    return xboxControllerButton0;                    
                }          
            case 331:       
                {          
                    return xboxControllerButton1;
                }          
            case 332:      
                {          
                    return xboxControllerButton2;
                }          
            case 333:      
                {          
                    return xboxControllerButton3;
                }          
            case 334:      
                {          
                    return xboxControllerButton4;
                }          
            case 335:      
                {          
                    return xboxControllerButton5;
                }
            case 336:
                {
                    return xboxControllerButton6;
                }
            case 337:
                {
                    return xboxControllerButton7;
                }
            case 338:
                {
                    return xboxControllerButton8;
                }
            case 339:
                {
                    return xboxControllerButton9;
                }
            case 340:
                {
                    return xboxControllerButton10;
                }
            case 341:
                {
                    return xboxControllerButton11;
                }
            case 342:
                {
                    return xboxControllerButton12;
                }
            case 343:
                {
                    return xboxControllerButton13;
                }
            case 344:
                {
                    return xboxControllerButton14;
                }
            case 345:
                {
                    return xboxControllerButton15;
                }
            case 346:
                {
                    return xboxControllerButton16;
                }
            case 347:
                {
                    return xboxControllerButton17;
                }
            case 348:
                {
                    return xboxControllerButton18;
                }
            case 349:
                {
                    return xboxControllerButton19;
                }
        }

        //If no matching case, returns button 19.
        Debug.Log("No PlayerPref matching: " + controllerButtonPreference);
        return xboxControllerButton19;

    }

    //Returns the sprite depending on what controller key is pressed.
    public Sprite ControllerButtonSprite()
    {
        if(InputManager.ControllerButtonPressed() == KeyCode.JoystickButton0)
        {
            return xboxControllerButton0;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton1)
        {
            return xboxControllerButton1;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton2)
        {
            return xboxControllerButton2;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton3)
        {
            return xboxControllerButton3;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton4)
        {
            return xboxControllerButton4;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton5)
        {
            return xboxControllerButton5;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton6)
        {
            return xboxControllerButton6;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton7)
        {
            return xboxControllerButton7;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton8)
        {
            return xboxControllerButton8;
        }

        if (InputManager.ControllerButtonPressed() == KeyCode.JoystickButton9)
        {
            return xboxControllerButton9;
        }

        return xboxControllerButton19;
    }


    //////////////Used for pulsating the text when a keybind is being changed. MenuOptionsControls.

    //For when the user clicks on the button, will stop autosizing and start pulsating the text.
    public void TurnOffAutosizing(Button textToPulse)
    {
        originalSize = textToPulse.GetComponentInChildren<TextMeshProUGUI>().fontSize;
        textToPulse.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = false;
    }

    //Turns autosizing back on for the text component.
    public void TurnOnAutosizing(Button textToRevert)
    {
        textToRevert.GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing = true;
    }

    //Pulsates the text by comparing its' size to the original size and increasing/decreasing the text
    //size based on that.
    public void PulsateText(TextMeshProUGUI textToPulse)
    {
        if(textToPulse.fontSize > originalSize + 2.0f)
        {
            increasingTextSize = false;
        }
        else if (textToPulse.fontSize < originalSize - 2.0f)
        {
            increasingTextSize = true;
        }

        if(increasingTextSize)
        {
            textToPulse.fontSize += 0.05f;
        }
        else
        {
            textToPulse.fontSize -= 0.05f;
        }
    }

    public void TurnOffPulsatingSprite(RectTransform spriteToPulse)
    {
        spriteToPulse.localScale = new Vector3(1, 1, 1);
    }

    //Pulsates the sprite by increasing/decreasing the scale
    public void PulsateSprite(RectTransform spriteToPulse)
    {
        if(spriteToPulse.localScale.x > 1 + 0.1f)
        {
            IncreasingSpriteSize = false;
        }
        else if (spriteToPulse.localScale.x < 1 - 0.1f)
        {
            IncreasingSpriteSize = true;
        }

        if (IncreasingSpriteSize)
        {
            spriteToPulse.localScale = new Vector3(spriteToPulse.localScale.x + 0.008f, spriteToPulse.localScale.y + 0.008f,
                spriteToPulse.localScale.z + 0.008f);
        }
        else
        {
            spriteToPulse.localScale = new Vector3(spriteToPulse.localScale.x - 0.008f, spriteToPulse.localScale.y - 0.008f,
                spriteToPulse.localScale.z - 0.008f);
        }
    }


    //////////////Button remaining highlighted when a new key binding is being set. MenuOptionsControls.

    //Remembers the old colour block and updates the current block of colours to the new one.
    public ColorBlock ColourBlockShift(ColorBlock oldColourBlock)
    {
        colourBlockToRemember = oldColourBlock;
        return menuControlsBinding;
    }

    public ColorBlock RememberColourBlock()
    {
        return colourBlockToRemember;
    }

    //////////////Scrollbar changing size when selected/deselected. MenuOptionsControls.

    //Enlargens the scrollbar when it is selected for better accessibility. 
    public void EnlargeUIElement(RectTransform scrollbarToEnlarge)
    {
        Vector3 scaleUp = new Vector3(0.9f, scrollbarToEnlarge.localScale.y,
            scrollbarToEnlarge.localScale.z);

        scrollbarToEnlarge.localScale = scaleUp;
    }

    //Reduces the scrollbar when it is deselected for better accessibility. 
    public void ReduceUIElement(RectTransform scrollbarToShrink)
    {
        Vector3 scaleDown = new Vector3(0.8f, scrollbarToShrink.localScale.y,
            scrollbarToShrink.localScale.z);

        scrollbarToShrink.localScale = scaleDown;
    }

    ///////////////Scrollbar methods. 

    //Ensures that the scrollbar navigates to the correct position based on the selected scroll rect component.
    public void CorrectBindingsScrollbar(int index)
    {
        bindingsScroller.verticalNormalizedPosition = (1 - (index / (((float)bindingsScroller.content.childCount / 3) - 1)));
    }
    
    //Sets the vertical scroll bar to where the user has selected if using keyboard/controller input.
    public void CorrectLevelSelectScrollbar(int index)
    {
        levelScroller.verticalNormalizedPosition = (1 - (index / (((float)levelScroller.content.childCount / 6) - 1)));
    }

    //Updates what objects on the left and right should be focused depending on the scrollbar vertical position.
    public void CorrectBindingsScrollbarNavigation(Scrollbar scrollbarToNavigate)
    {
        Navigation scrollbarNavigation = scrollbarToNavigate.navigation;
        float scrollbarPosition = scrollbarToNavigate.value;
        scrollbarNavigation.mode = Navigation.Mode.Explicit;
        
        if (scrollbarPosition > 0.9)
        {
            scrollbarNavigation.selectOnLeft = confirmationControllerButton;
            scrollbarNavigation.selectOnRight = confirmationKeyboardButton;            
        }
        else if (scrollbarPosition > 0.8)
        {
            scrollbarNavigation.selectOnLeft = cancelControllerButton;
            scrollbarNavigation.selectOnRight = cancelKeyboardButton;
        }
        else if (scrollbarPosition > 0.7)
        {
            scrollbarNavigation.selectOnLeft = jumpControllerButton;
            scrollbarNavigation.selectOnRight = jumpKeyboardButton;
        }
        else if (scrollbarPosition > 0.6)
        {
            scrollbarNavigation.selectOnLeft = spinControllerButton;
            scrollbarNavigation.selectOnRight = spinKeyboardButton;
        }
        else if (scrollbarPosition > 0.5)
        {
            scrollbarNavigation.selectOnLeft = slideControllerButton;
            scrollbarNavigation.selectOnRight = slideKeyboardButton;
        }
        else if (scrollbarPosition > 0.4)
        {
            scrollbarNavigation.selectOnLeft = powerControllerButton;
            scrollbarNavigation.selectOnRight = powerKeyboardButton;
        }
        else if (scrollbarPosition > 0.3)
        {
            scrollbarNavigation.selectOnLeft = upControllerButton;
            scrollbarNavigation.selectOnRight = upKeyboardButton;
        }
        else if (scrollbarPosition > 0.2)
        {
            scrollbarNavigation.selectOnLeft = downControllerButton;
            scrollbarNavigation.selectOnRight = downKeyboardButton;
        }
        else if (scrollbarPosition > 0.1)
        {
            scrollbarNavigation.selectOnLeft = leftControllerButton;
            scrollbarNavigation.selectOnRight = leftKeyboardButton;
        }
        else
        {
            scrollbarNavigation.selectOnLeft = rightControllerButton;
            scrollbarNavigation.selectOnRight = rightKeyboardButton;
        }

        scrollbarToNavigate.navigation = scrollbarNavigation;

    }

    //Updates what objects on the left and right should be focused depending on the scrollbar vertical position.
    public void CorrectLevelSelectScrollbarNavigation(Scrollbar scrollbarToNavigate)
    {
        Navigation scrollbarNavigation = scrollbarToNavigate.navigation;
        float scrollbarPosition = scrollbarToNavigate.value;
        scrollbarNavigation.mode = Navigation.Mode.Explicit;

        if (scrollbarPosition > 0.83)
        {
            scrollbarNavigation.selectOnLeft = level6Button;
            scrollbarNavigation.selectOnRight = level1Button;
        }
        else if (scrollbarPosition > 0.66)
        {
            scrollbarNavigation.selectOnLeft = level12Button;
            scrollbarNavigation.selectOnRight = level7Button;
        }
        else if (scrollbarPosition > 0.5)
        {
            scrollbarNavigation.selectOnLeft = level18Button;
            scrollbarNavigation.selectOnRight = level13Button;
        }
        else if (scrollbarPosition > 0.33)
        {
            scrollbarNavigation.selectOnLeft = level24Button;
            scrollbarNavigation.selectOnRight = level19Button;
        }
        else if (scrollbarPosition > 0.16)
        {
            scrollbarNavigation.selectOnLeft = level30Button;
            scrollbarNavigation.selectOnRight = level25Button;
        }
        else
        {
            scrollbarNavigation.selectOnLeft = level36Button;
            scrollbarNavigation.selectOnRight = level31Button;
        }

        scrollbarToNavigate.navigation = scrollbarNavigation;

    }

}
