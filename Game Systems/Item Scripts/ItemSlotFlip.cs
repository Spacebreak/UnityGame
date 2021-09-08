using UnityEngine;

public class ItemSlotFlip : MonoBehaviour
{
    public SpriteRenderer luperSprite;      //The sprite of Luper.
    public GameObject luperItemSlot;        //The space where Luper stores his carried item.
    public GameObject luperItemDetection;   //The gameobject that stored the item detection collider.

    public Vector3 itemPositionLeft;        //Where Luper should drop the item when facing left.
    public Vector3 itemPositionRight;       //Where Luper should drop the item when facing right.

    // Update is called once per frame
    void Update()
    {
        if (luperSprite.flipX)
        {
            luperItemSlot.transform.localPosition = itemPositionLeft;
            luperItemDetection.transform.localPosition = itemPositionLeft;
        }
        else
        {
            luperItemSlot.transform.localPosition = itemPositionRight;
            luperItemDetection.transform.localPosition = itemPositionRight;
        }
    }

    public bool SpriteFlipped()
    {
        return luperSprite.flipX;
    }

    public void DisableItemDetection()
    {
        luperItemDetection.SetActive(false);
    }

    public void EnableItemDetection()
    {
        luperItemDetection.SetActive(true);
    }


}
