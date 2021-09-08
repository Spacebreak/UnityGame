using UnityEngine;
using UnityEngine.EventSystems;

public class InputModuleSelector : MonoBehaviour
{
    public StandaloneInputModule inputModule;       //The Unity module for navigating UI.

    private void Awake()
    {
        InputManager.ConnectedControllers();
    }

    //If a controller input is received, updates the input module to check for controller inputs, otherwise will only check
    //for keyboard inputs. To prevent updating the input module every frame, uses a boolean to check.
    void Update()
    {
        if (InputManager.connectedControllers.Length > 0 && (InputManager.IsControllerButtonPressed() || InputManager.IsControllerAxisPressed()))
        {
            if (InputManager.controllerPlaystation)
            {
                inputModule.horizontalAxis = "PHorizontal";
                inputModule.verticalAxis = "PVertical";
                inputModule.submitButton = "PConfirm";
                inputModule.cancelButton = "PCancel";
            }
            else if (InputManager.controllerXbox)
            {
                inputModule.horizontalAxis = "XHorizontal";
                inputModule.verticalAxis = "XVertical";
                inputModule.submitButton = "XConfirm";
                inputModule.cancelButton = "XCancel";
            }
            else
            {
                inputModule.horizontalAxis = "ControllerHorizontal";
                inputModule.verticalAxis = "ControllerVertical";
                inputModule.submitButton = "ControllerSubmit";
                inputModule.cancelButton = "ControllerCancel";
            }
        }
        else if( Input.anyKeyDown)
        {
            inputModule.horizontalAxis = "KeyboardHorizontal";
            inputModule.verticalAxis = "KeyboardVertical";
            inputModule.submitButton = "KeyboardSubmit";
            inputModule.cancelButton = "KeyboardCancel";
        }
    }
}
