using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public LuperController luperControllerScript;   //Luper controller for determining if Power touch input was detected.
    public ItemSlotFlip itemSlotFlipScript; //The script that flips the item slot attached to the player.

    private bool itemPickedUp;              //Tracks if this item has been picked up or not.
    private bool itemInRange;               //Tracks if this item is in range of the player or not.

    private Rigidbody2D itemRigidBody;      //The rigidbody of the item.
    private SpriteRenderer itemSprite;      //The sprite of the item.

    public Transform luperTransform;        //The transform of Luper for deterniming their position.
    public GameObject luperItemSlot;        //For attaching the item object to the item slot on Luper.
    public TokenWhite whiteTokenScript;     //Manages settings affecting gravity and world physics.

    // Start is called before the first frame update
    void Start()
    {
        itemRigidBody = GetComponent<Rigidbody2D>();
        itemSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(itemInRange);
        //Debug.Log(itemPickedUp);
        
        if ((itemInRange || itemPickedUp) && (InputManager.GetPower() || luperControllerScript.ReturnPowerInput()))
        {
            if (itemPickedUp)
            {
                if(CheckItemSpace() == false)
                {
                    DropItem();
                }
            }
            else
            {
                PickupItem();
            }
        }        
    }

    private void DropItem()
    {
        itemSlotFlipScript.EnableItemDetection();
        itemPickedUp = false;
        transform.SetParent(null);
        itemRigidBody.simulated = true;
        itemSprite.enabled = true;
    }

    //Picks up the interactable item. Needs to set the velocity to zero otherwise the object will maintain momentum
    //when being dropped.
    private void PickupItem()
    {
        itemSlotFlipScript.DisableItemDetection();
        itemPickedUp = true;
        transform.SetParent(luperItemSlot.transform);
        transform.SetPositionAndRotation(luperItemSlot.transform.position, Quaternion.identity);
        itemRigidBody.velocity = Vector2.zero;
        itemRigidBody.simulated = false;
        itemSprite.enabled = false;
    }

    //Shoots a raycast from Lupers position. If it hits an object then Luper won't be able to drop the item his holding.
    private bool CheckItemSpace()
    {
        RaycastHit2D raycastHit;

        if (itemSlotFlipScript.SpriteFlipped())
        {
            raycastHit = Physics2D.Raycast(luperTransform.position, whiteTokenScript.GetDirection(1), 2f);// Vector2.left, 2f);
        }
        else
        {
            raycastHit = Physics2D.Raycast(luperTransform.position, whiteTokenScript.GetDirection(3), 2f);//Vector2.right, 2f);
        }

        return raycastHit;
    }

    public void SetItemInteractable(bool itemHoldable)
    {
        if (!itemPickedUp)
        {
            itemInRange = itemHoldable;
        }
    }

    /*
    private void OnDrawGizmos()
    {        
        Gizmos.color = Color.yellow;

        if (itemSlotFlipScript.SpriteFlipped())
        {
            //Vector3 direction = luperItemSlot.transform.TransformDirection(Vector2.left) * 2;
            Gizmos.DrawRay(luperTransform.position, whiteTokenScript.GetDirection(1));
        }
        else
        {
            //Vector3 direction = luperItemSlot.transform.TransformDirection(Vector2.right) * 2;
            //Gizmos.DrawRay(luperItemSlot.transform.position, direction);
            Gizmos.DrawRay(luperTransform.position, whiteTokenScript.GetDirection(3));

        }
    }
    */

}
