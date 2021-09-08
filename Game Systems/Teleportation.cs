using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform teleportationDestination;  //A Transform containing the destination location.
    public Teleportation destinationTeleporter; //The Teleportation Script that is on the destination teleporter.
    private bool teleporterActive = true;       //Whether the Teleporter can be used currently or not.

    public AudioSource teleportSound;         //The sound that plays when hitting a checkpoint.

    private readonly string playerString = "Player"; //The string holding reference to GameObject tag.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(teleporterActive && collision.CompareTag(playerString))
        {
            collision.gameObject.transform.position = teleportationDestination.position;
            destinationTeleporter.SetTeleporterInactive();

            ActivateTeleportSound();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(playerString))
        {
            teleporterActive = true;
        }
    }

    //Sets destination teleporter to inactive so an infinite teleportation loop won't occur.
    private void SetTeleporterInactive()
    {
        teleporterActive = false;
    }

    private void ActivateTeleportSound()
    {
        teleportSound.Play();
    }
}
