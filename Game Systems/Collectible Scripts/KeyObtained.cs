using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObtained : MonoBehaviour
{
    public GameObject levelDoor;                //Stores the Exit object so the script can be referenced from it.
    private DoorExits levelExit;                //Script for accessing methods relating to using the exit.

    private readonly string playerString = "Player"; //Holds the string matching the tag of the object.

    public SoundController soundController;     //The script controlling all sounds.
    public KeyCollectedUI keyCollectedUIClass;  //The script displaying the key on the UI.

    // Start is called before the first frame update
    void Start()
    {
        levelExit = levelDoor.GetComponent<DoorExits>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelExit.IsPlayerOnExit() && InputManager.GetConfirm())
        {
            levelExit.AdvanceLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            KeyCollected();
        }
    }

    private void KeyCollected()
    {
        soundController.PlayAudioClip(4);
        transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
        levelExit.KeyObtained();
        keyCollectedUIClass.DisplayKey(true);
    }
}
