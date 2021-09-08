using UnityEngine;

public static class InputManager 
{
    public static KeyCode confirmKeyboard;      //The keycode for holding the confirmation binding for keyboard input.
    public static KeyCode cancelKeyboard;       //The keycode for holding the cancel binding for keyboard input.
    public static KeyCode jumpKeyboard;         //The keycode for holding the jump binding for keyboard input.
    public static KeyCode spinKeyboard;         //The keycode for holding the spin binding for keyboard input.
    public static KeyCode slideKeyboard;        //The keycode for holding the slide and dive binding for keyboard input.
    public static KeyCode powerKeyboard;        //The keycode for holding the power binding for keyboard input.

    public static KeyCode confirmController;    //The keycode for holding the confirmation binding for controller input.
    public static KeyCode cancelController;     //The keycode for holding the cancel binding for controller input.
    public static KeyCode jumpController;       //The keycode for holding the jump binding for controller input.
    public static KeyCode spinController;       //The keycode for holding the spin binding for controller input.
    public static KeyCode slideController;      //The keycode for holding the slide binding for controller input.
    public static KeyCode powerController;      //The keycode for holding the power binding for controller input.

    public static string verticalController = "ControllerVertical";       //The string for pointing to the correct vertical axis.
    public static string horizontalController = "ControllerHorizontal";   //The string for pointing to the correct horizontal axis.
    
    //THESE ARE BEING TESTED AND NOT IN USE YET
    public static KeyCode upKeyboard;
    public static KeyCode downKeyboard;
    public static KeyCode leftKeyboard;
    public static KeyCode rightKeyboard;
    public static KeyCode upController;
    public static KeyCode downController;
    public static KeyCode leftController;
    public static KeyCode rightController;

    public static KeyCode controllerPressed;
    public static string[] connectedControllers;
    public static bool controllerPlaystation;
    public static bool controllerXbox;
    //public static bool controllerWireless;  //PS4 controller detecting as this.
    public static bool controllerSteam;     //Unsure if needed.

    public static void InitiateKeys()
    {
        //Currently uncertain if the numbers correlating to these values are correct.
        confirmKeyboard = (KeyCode)PlayerPrefs.GetInt("ConfirmBinding", 13);
        cancelKeyboard = (KeyCode)PlayerPrefs.GetInt("CancelBinding", 27);
        jumpKeyboard = (KeyCode)PlayerPrefs.GetInt("JumpBinding", 32);
        spinKeyboard = (KeyCode)PlayerPrefs.GetInt("SpinBinding", 113);
        slideKeyboard = (KeyCode)PlayerPrefs.GetInt("SlideBinding", 101);
        powerKeyboard = (KeyCode)PlayerPrefs.GetInt("PowerBinding", 114);
        upKeyboard = (KeyCode)PlayerPrefs.GetInt("UpBinding", 119);
        downKeyboard = (KeyCode)PlayerPrefs.GetInt("DownBinding", 115);
        leftKeyboard = (KeyCode)PlayerPrefs.GetInt("LeftBinding", 97);
        rightKeyboard = (KeyCode)PlayerPrefs.GetInt("RightBinding", 100);

        confirmController = (KeyCode)PlayerPrefs.GetInt("ConfirmBindingCont", 330);
        cancelController = (KeyCode)PlayerPrefs.GetInt("CancelBindingCont", 331);
        jumpController = (KeyCode)PlayerPrefs.GetInt("JumpBindingCont", 330); //Jump and Confirm are the same for controllers.
        spinController = (KeyCode)PlayerPrefs.GetInt("SpinBindingCont", 332);
        slideController = (KeyCode)PlayerPrefs.GetInt("SlideBindingCont", 331); //Slide and Cancel are the same for controllers.
        powerController = (KeyCode)PlayerPrefs.GetInt("PowerBindingCont", 333);     
    }

    //Updates all keys for all commands based on playerprefs. Even if one key is changed all
    //will be re-updated. Could attempt to make this more efficient one day.
    public static void UpdateAllKeys()
    {
        confirmKeyboard = (KeyCode)PlayerPrefs.GetInt("ConfirmBinding", 13);
        cancelKeyboard = (KeyCode)PlayerPrefs.GetInt("CancelBinding", 27);
        jumpKeyboard = (KeyCode)PlayerPrefs.GetInt("JumpBinding", 32);
        spinKeyboard = (KeyCode)PlayerPrefs.GetInt("SpinBinding", 113);
        slideKeyboard = (KeyCode)PlayerPrefs.GetInt("SlideBinding", 101);
        powerKeyboard = (KeyCode)PlayerPrefs.GetInt("PowerBinding", 114);
        upKeyboard = (KeyCode)PlayerPrefs.GetInt("UpBinding", 119);
        downKeyboard = (KeyCode)PlayerPrefs.GetInt("DownBinding", 115);
        leftKeyboard = (KeyCode)PlayerPrefs.GetInt("LeftBinding", 97);
        rightKeyboard = (KeyCode)PlayerPrefs.GetInt("RightBinding", 100);
    }

    //Updates all controls for all commands based on playerprefs. Even if one key is changed all
    //will be re-updated. Could attempt to make this more efficient one day.
    public static void UpdateAllControls()
    {
        ConnectedControllers();

        confirmController = (KeyCode)PlayerPrefs.GetInt("ConfirmBindingCont", 330);
        cancelController = (KeyCode)PlayerPrefs.GetInt("CancelBindingCont", 331);
        jumpController = (KeyCode)PlayerPrefs.GetInt("JumpBindingCont", 330); //Jump and Confirm are the same for controllers.
        spinController = (KeyCode)PlayerPrefs.GetInt("SpinBindingCont", 332);
        slideController = (KeyCode)PlayerPrefs.GetInt("SlideBindingCont", 331); //Slide and Cancel are the same for controllers.
        powerController = (KeyCode)PlayerPrefs.GetInt("PowerBindingCont", 333);
    }

    //AXES
    //Returns both X and Y input, note in Unity the Z axis correlates to Y axis controller input.
    //Vector3 used instead of Vector2, because a lot of Unity movement systems use Vector3 even in 2D space.
    public static Vector3 CombinedInput()
    {
        return new Vector3(HorizontalInput(), 0, VerticalInput());
    }

    //Returns horizontal input from both keyboards and controller. Values must be clamped to 1 incase both controller
    //and keyboard input is done simultaneously which will cause behavioural issues.
    public static float HorizontalInput()
    {
        float horizontalAxisInput = 0.0f;

        horizontalAxisInput += Input.GetAxis("KeyboardHorizontal");

        if (controllerPlaystation)
        {
            horizontalAxisInput += Input.GetAxis("PHorizontal");
        }
        else if (controllerXbox)
        {
            horizontalAxisInput += Input.GetAxis("XHorizontal");
        }
        else
        {
            horizontalAxisInput += Input.GetAxis("ControllerHorizontal");
        }

        return Mathf.Clamp(horizontalAxisInput, -1, 1);
    }

    //Returns vertical input from both keyboards and controller.
    public static float VerticalInput()
    {
        float verticalAxisInput = 0.0f;

        verticalAxisInput += Input.GetAxis("KeyboardVertical");

        if (controllerPlaystation)
        {
            verticalAxisInput += Input.GetAxis("PVertical");
        }
        else if (controllerXbox)
        {
            verticalAxisInput += Input.GetAxis("XVertical");
        }
        else
        {
            verticalAxisInput += Input.GetAxis("ControllerVertical");
        }

        return Mathf.Clamp(verticalAxisInput, -1, 1);
    }

    public static bool GetConfirm()
    {
        //Ensures that Confirm will always be possible, and if users bind Enter both Enter and Return will work.
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            return true;
        }

        if (controllerPlaystation)
        {
            if (Input.GetButtonDown("PConfirm"))
            {
                return true;
            }
        }
        else if (controllerXbox)
        {
            if (Input.GetButtonDown("XConfirm"))
            {
                return true;
            }
        }

        bool pressedConfirm = (Input.GetKeyDown(confirmKeyboard) || Input.GetKeyDown(confirmController)) ? true : false;
        return pressedConfirm;
    }

    public static bool GetCancel()
    {
        //Ensures that Escape will always work, even if Cancel is rebound.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }

        if (controllerPlaystation)
        {
            if (Input.GetButtonDown("PCancel"))
            {
                return true;
            }
        }
        else if (controllerXbox)
        {
            if (Input.GetButtonDown("XCancel"))
            {
                return true;
            }
        }

        bool pressedCancel = (Input.GetKeyDown(cancelKeyboard) || Input.GetKeyDown(cancelController)) ? true : false;
        return pressedCancel;
    }

    public static bool GetJump()
    {
        bool pressedJump = (Input.GetKeyDown(jumpKeyboard) || Input.GetKeyDown(jumpController)) ? true : false;
        return pressedJump;
    }

    //Pause needs to be handled separately to GetCancel otherwise Controller Slide input will activate Pausing.
    public static bool GetPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }
        else if (controllerPlaystation)
        {
            return Input.GetButtonDown("PCancel");
        }
        else if (controllerXbox)
        {
            return Input.GetButtonDown("XCancel");
        }
        else
        {
            return Input.GetButtonDown("ControllerPause");
        }
    }

    public static bool GetSpin()
    {
        bool pressedSpin = (Input.GetKeyDown(spinKeyboard) || Input.GetKeyDown(spinController)) ? true : false;
        return pressedSpin;
    }

    public static bool GetSlide()
    {
        bool pressedSlide = (Input.GetKeyDown(slideKeyboard) || Input.GetKeyDown(slideController)) ? true : false;
        return pressedSlide;
    }

    public static bool GetPower()
    {
        bool pressedPower = (Input.GetKeyDown(powerKeyboard) || Input.GetKeyDown(powerController)) ? true : false;
        return pressedPower;
    }

    public static KeyCode ControllerButtonPressed()
    {
        return controllerPressed;
    }

    //Determines what controllers are connected, and stores the values.
    public static void ConnectedControllers()
    {
        connectedControllers = Input.GetJoystickNames();

        for (int i = 0; i < connectedControllers.Length; i++)
        {
            if (connectedControllers[i].Contains("Xbox"))
            {
                controllerXbox = true;
            }
            else if (connectedControllers[i].Contains("Wireless"))
            {
                controllerPlaystation = true;
            }
            else if (connectedControllers[i].Contains("Steam"))
            {
                controllerSteam = true;
            }
        }
    }

    public static bool IsControllerAxisPressed()
    {
        if (Input.GetAxis("ControllerHorizontal") != 0 || Input.GetAxis("ControllerVertical") != 0)
        {
            return true;
        }

        return false;
    }

    //Checks if a controller button is pressed, and if so will update the keycode with the one of the button pressed.
    //This code is more inefficient if the later buttons are pressed (10-19).
    public static bool IsControllerButtonPressed()
    {        
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton1) 
            || Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.JoystickButton3)
            || Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.JoystickButton5)
            || Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.JoystickButton7)
            || Input.GetKeyDown(KeyCode.JoystickButton8) || Input.GetKeyDown(KeyCode.JoystickButton9)
            || Input.GetKeyDown(KeyCode.JoystickButton10) || Input.GetKeyDown(KeyCode.JoystickButton11)
            || Input.GetKeyDown(KeyCode.JoystickButton12) || Input.GetKeyDown(KeyCode.JoystickButton13)
            || Input.GetKeyDown(KeyCode.JoystickButton14) || Input.GetKeyDown(KeyCode.JoystickButton15)
            || Input.GetKeyDown(KeyCode.JoystickButton16) || Input.GetKeyDown(KeyCode.JoystickButton17)
            || Input.GetKeyDown(KeyCode.JoystickButton18) || Input.GetKeyDown(KeyCode.JoystickButton19))
        {

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                controllerPressed = KeyCode.JoystickButton0;
                //Debug.Log("0 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                controllerPressed = KeyCode.JoystickButton1;
                //Debug.Log("1 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                controllerPressed = KeyCode.JoystickButton2;
                //Debug.Log("2 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                controllerPressed = KeyCode.JoystickButton3;
                //Debug.Log("3 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                controllerPressed = KeyCode.JoystickButton4;
                //Debug.Log("4 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                controllerPressed = KeyCode.JoystickButton5;
                //Debug.Log("5 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                controllerPressed = KeyCode.JoystickButton6;
                //Debug.Log("6 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                controllerPressed = KeyCode.JoystickButton7;
                //Debug.Log("7 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton8))
            {
                controllerPressed = KeyCode.JoystickButton8;
                //Debug.Log("8 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton9))
            {
                controllerPressed = KeyCode.JoystickButton9;
                //Debug.Log("9 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton10))
            {
                controllerPressed = KeyCode.JoystickButton10;
                //Debug.Log("10 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton11))
            {
                controllerPressed = KeyCode.JoystickButton11;
                //Debug.Log("11 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton12))
            {
                controllerPressed = KeyCode.JoystickButton12;
                //Debug.Log("12 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton13))
            {
                controllerPressed = KeyCode.JoystickButton13;
                //Debug.Log("13 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton14))
            {
                controllerPressed = KeyCode.JoystickButton14;
                //Debug.Log("14 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton15))
            {
                controllerPressed = KeyCode.JoystickButton15;
                //Debug.Log("15 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton16))
            {
                controllerPressed = KeyCode.JoystickButton16;
                //Debug.Log("16 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton17))
            {
                controllerPressed = KeyCode.JoystickButton17;
                //Debug.Log("17 pressed");
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton18))
            {
                controllerPressed = KeyCode.JoystickButton18;
                //Debug.Log("18 pressed");
                return true;
            }

            controllerPressed = KeyCode.JoystickButton19;
            //Debug.Log("19 pressed");
            return true;         

        }
        else
        {
            return false;
        }          
        
    }

    public static bool ControllerConnectedPlaystation()
    {
        return controllerPlaystation;
    }

    public static bool ControllerConnectedXbox()
    {
        return controllerXbox;
    }

    public static bool ControllerConnectedSteam()
    {
        return controllerSteam;
    }
}
