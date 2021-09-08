using System.Collections;
using UnityEngine;

public class TokenWhite : MonoBehaviour
{
    private Vector2[] gravityValues = {new Vector2(0, -9.81f), new Vector2(-9.81f, 0), new Vector2(0, 9.81f),
        new Vector2(9.81f, 0) };        //An array storing the various gravity values. Used by GravityIndex.
    private int gravityIndex = 0;       //Stores what current value Gravity is set to.
    private int oldGravityIndex = 0;    //Knowing what direction was the previous gravity helps Luper's sprite rotate properly.

    public Transform luperTransform;    //The transform of Luper.
    private int rotationDegrees = 0;    //How many degrees Luper is rotated.

    public BuoyancyEffector2D[] waterObjects;   //The effectors attached to the water bodies. Used for altering GravShift flow direction.
    private Gemstones luperGems;        //The class that controls the gemstones. These need to be rotated when GravShift happens.

    //Stores the triggers for what is ground that Luper can walk on. When gravity changes the surface of every object in a
    //scene needs to be updated. Moving platforms need to be handled separately as these cannot be set to inactive if Luper
    //is a child object to one of them.
    public GameObject DownwardsGravity;
    public GameObject LeftwardsGravity;
    public GameObject UpwardsGravity;
    public GameObject RightwardsGravity;
    public GameObject[] MovingPlatformFloors;
    public GameObject[] MovingPlatformSideLefts;
    public GameObject[] MovingPlatformRoofs;
    public GameObject[] MovingPlatformSideRights;

    public GravityPuzzle[] gravityPuzzleScripts;

    private void Start()
    {
        luperGems = FindObjectOfType<Gemstones>();
        InitializeLuperFromCheckpoint();
    }

    //On Game Start will set the correct gravity that is stored in the Game Manager class.
    private void InitializeLuperFromCheckpoint()
    {
        //Gets the correct gravity based on the checkpoint orientation and updates the current scene.
        gravityIndex = GameManager.GetGravityIndex();
        //Physics2D.gravity = gravityValues[gravityIndex];
        ChangeGravity(gravityIndex);

        //OldGravityIndex needs to be 0 to get Luper's rotation correct. This is because on a freshly
        //loaded scene his orientation will always be 0.
        oldGravityIndex = 0;
        RotateLuper();
        ShiftWaterFlowAngle();
    }

    // Update is called once per frame
    private void Update()
    {
        //Just for testing purposes.
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            ChangeGravity(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            ChangeGravity(3);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            ChangeGravity(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ChangeGravity(0);
        }
    }

    //Will convert a direction based off Luper's local orientation into the correct number
    //so the gravity converts properly.
    public void ChangeGravityMobile(int gravityIndexToConvert)
    {
        /* This code prevents Gravity changing when Luper is moving, but it's not very fun to play with.
        if (luperHero.LuperVelocity() != Vector2.zero)
        {
            Debug.Log("Cannot GravShift during movement");
            return;
        }
        */

        int newGravityIndex = (GravityIndex() + gravityIndexToConvert) % 4;
        ChangeGravity(newGravityIndex);
    }

    //Changes gravity if the new index is different to the old index. Calls the function to change the 
    //ground hitboxes so Luper will behave appropriately with ground that is too steep for him.
    public void ChangeGravity(int gravityIndexToChange)
    {
        StartCoroutine(GravityShift());
        oldGravityIndex = gravityIndex;

        if (gravityIndex != gravityIndexToChange)
        {
            gravityIndex = gravityIndexToChange;
            ShiftWaterFlowAngle();
            RotateLuper();
            AlterPuzzleGravity();

            if(luperGems != null)
            {
                luperGems.UpdateGemstoneOrientation();
            }
        }

        ChangeGroundHitboxes();
    }

    //Will tell the LuperController that Gravity is being altered.
    private IEnumerator GravityShift()
    {
        yield return new WaitForSeconds(0.1f);
        LuperController.SetGravityChanging();
    }

    //Turns on/off surfaces so Luper will always know what ground is steep and what isn't relative
    //to the direction of gravity.
    private void ChangeGroundHitboxes()
    {
        switch (gravityIndex)
        {
            case 0: //Down
                ChangeHitboxes(DownwardsGravity, MovingPlatformFloors);
                return;

            case 1: //Left
                ChangeHitboxes(LeftwardsGravity, MovingPlatformSideLefts);
                return;

            case 2: //Up
                ChangeHitboxes(UpwardsGravity, MovingPlatformRoofs);
                return;

            case 3: //Right
                ChangeHitboxes(RightwardsGravity, MovingPlatformSideRights);
                return;
        }
    }

    //Turns off every trigger in the scene for Luper being grounded, then re-enables the one that is current.
    private void ChangeHitboxes(GameObject groundToAlter, GameObject[] platformToAlter)
    {
        DisableAllGround(DownwardsGravity, MovingPlatformFloors);
        DisableAllGround(UpwardsGravity, MovingPlatformRoofs);
        DisableAllGround(LeftwardsGravity, MovingPlatformSideLefts);
        DisableAllGround(RightwardsGravity, MovingPlatformSideRights);

        groundToAlter.SetActive(true);

        foreach (GameObject movingPlatformsGravity in platformToAlter)
        {
            movingPlatformsGravity.SetActive(true);
        }
    }

    //Turns off every trigger in the scene for Luper being grounded.
    private void DisableAllGround(GameObject groundGravityToAlter, GameObject[] platformGravityToAlter)
    {
        groundGravityToAlter.SetActive(false);

        foreach (GameObject movingPlatformsGravity in platformGravityToAlter)
        {
            movingPlatformsGravity.SetActive(false);
        }
    }

    //Changes the flow angle of the water when gravity is altered so Luper will sink in the correct direction.
    private void ShiftWaterFlowAngle()
    {
        float buoyancyAngle = 0;

        switch (GravityIndex())
        {
            case 0:
                buoyancyAngle = 90;
                break;

            case 1:
                buoyancyAngle = 0;
                break;

            case 2:
                buoyancyAngle = -90;
                break;

            case 3:
                buoyancyAngle = 180;
                break;
        }

        foreach (BuoyancyEffector2D waterBuoyancy in waterObjects)
        {
            if(waterBuoyancy != null)
            {
                waterBuoyancy.flowAngle = buoyancyAngle;
            }
        }        
    }

    public int GravityIndex()
    {
        return gravityIndex;
    }

    private void RotateLuper()
    {
        rotationDegrees = Mathf.Abs((4 - (gravityIndex - oldGravityIndex)) * 90);
        luperTransform.Rotate(new Vector3(0, 0, rotationDegrees));     
    }

    private void AlterPuzzleGravity()
    {
        foreach (GravityPuzzle gravityPuzzleScript in gravityPuzzleScripts)
        {
            gravityPuzzleScript.AlterPuzzleGravity(gravityIndex);
        }
    }

    //Returns the correct direction the player wants based on an offset.
    public Vector2 GetDirection(int gravityIndexOffset)
    {
        switch ((GravityIndex() + gravityIndexOffset) % 4)
        {
            case 0:
                return Vector2.down;

            case 1:
                return Vector2.left;

            case 2:
                return Vector2.up;

            case 3:
                return Vector2.right;

            default:
                return Vector2.down;
        }
    }

}
