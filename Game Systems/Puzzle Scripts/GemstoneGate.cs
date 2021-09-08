using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemstoneGate : MonoBehaviour
{

    public int gemstonesRequired;       //The amount of gemstones needed to open the gate.
    public int gemstoneTypeRequired;    //The type of gemstones required to open the gate.

    private bool gateAccessible;        //Checks if Luper is standing near the gate.
    private bool gateUnlockable;        //Checks if Luper can unlock the gate or not depending if he has the gems required.
    private bool gateUnlocked;          //Stores whether the gate has already been unlocked or not.

    public GameObject gateCollider;

    private readonly string playerString = "Player";    //The string holding the GameObject tag reference to the player.

    // Update is called once per frame
    void Update()
    {
        if (gateAccessible && !gateUnlocked && Input.GetKeyDown(KeyCode.U))
        {
            CheckGems();

            if (gateUnlockable)
            {
                gateCollider.SetActive(false);
                gateUnlocked = true;
            }
            else
            {
                Debug.Log("Not enough Gemstones to unlock the gate.");
            }
        }
    }

    private void CheckGems()
    {
        if(GameManager.CollectedGemstoneAmount(gemstoneTypeRequired) >= gemstonesRequired)
        {
            gateUnlockable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            gateAccessible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            gateAccessible = false;
        }
    }
}
