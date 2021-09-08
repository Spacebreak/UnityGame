using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownFocus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float dropdownScrollSpeed = 10;              //The speed the dropdown scrolls.
    private bool mouseOver = false;                     //Stores if the mouse is hovering over the dropdown.

    private List<Selectable> dropdownItems = new List<Selectable>();    //Stores each dropdown item.
    private ScrollRect scrollRectComponent;             //Stores the scroll rect attached to the dropdown item.
    private Vector2 nextScrollPosition = Vector2.up;    //Stores the value of the next position to scroll to.
    
    //Static class checking for user input, used to prevent leaving scene if menu dropdown is activated.
    MenuOptionsSelector menuOptionsSelector = MenuOptionsSelector.menuOptionsSelector;
    public Dropdown dropdownToDeselect;                 //The dropdown to check when pressing the escape button.

    void Awake()
    {
        scrollRectComponent = GetComponent<ScrollRect>();
    }

    void Start()
    {
        menuOptionsSelector.AddDropdownToList(dropdownToDeselect);
        
        //On start populates the dropdown items list with the selectable children components under the dropdown.
        if (scrollRectComponent)
        {
            scrollRectComponent.content.GetComponentsInChildren(dropdownItems);
        }

        ScrollToSelected(true);
    }
    
    void Update()
    {      
        InputScroll();

        //If the mouse is not hovering over the Dropdown, scroll to the selected position.
        if (!mouseOver)
        {
            scrollRectComponent.normalizedPosition = Vector2.Lerp(scrollRectComponent.normalizedPosition, nextScrollPosition, dropdownScrollSpeed * Time.deltaTime);
        }
        else
        {
            nextScrollPosition = scrollRectComponent.normalizedPosition;
        }
    }

    //If there are items in the dropdown, and the keyboard is being pressed - prevent the scrollrect scrolling over the mouse input.
    void InputScroll()
    {
        if (dropdownItems.Count > 0)
        {

            if (Input.GetAxis("ControllerHorizontal") != 0 || Input.GetAxis("ControllerVertical") != 0 || Input.GetAxis("KeyboardHorizontal") != 0 || Input.GetAxis("KeyboardVertical") != 0)
            {
                ScrollToSelected(false);
            }

            /*
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                ScrollToSelected(false);
            }
            */
        }
    }

    void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1;

        //If the currently selected gameobject is a selectable, store this object, if not store null.
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;
        
        //If not null, makes the selected index the correct index value of the dropdown, based on the matching selected element. 
        if (selectedElement)
        {
            selectedIndex = dropdownItems.IndexOf(selectedElement);
        }

        //Dropdown indexes start at 0, so if the index is 0 or above, check if we are quick scrolling to it.
        if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                scrollRectComponent.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float) dropdownItems.Count - 1)));
                nextScrollPosition = scrollRectComponent.normalizedPosition;
            }
            else
            {
                nextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float) dropdownItems.Count - 1)));
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    //Won't scroll to the dropdown selection if the mouse is not on the dropdown.
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }

}