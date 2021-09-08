using UnityEngine;

public class GroundHitboxRelocation : MonoBehaviour
{
    public GameObject objectToParentTo; //Ground Hitbox needs to be parented to this in order to be switched off/on correctly.

    //The purpose of this class is to make game-dev easier. If I move an object that has GroundHitBoxes attached to it, having
    //them attached to the Ground Hitbox Collection in Inspector Mode means that if I move the object the GroundHitBoxes will 
    //not follow the GameObject I am moving. This script means I can move them and at the start of the game they will assign 
    //themselves to the correct parent object.
    void Start()
    {
        gameObject.transform.SetParent(objectToParentTo.transform);
        Destroy(this);
    }

}
