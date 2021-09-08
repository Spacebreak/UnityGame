using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointGravityIndex;  //The Gravity Index orientation of the checkpoint. For correcting Luper's
                                        //gravity and rotation.

    private readonly string playerString = "Player";    //Holds the string matching the tag of the object.

    public ParticleSystem checkpointParticles;      //The particle system that emits when hitting a checkpoint.
    public AudioSource checkpointSound;             //The sound that plays when hitting a checkpoint.

    public static Checkpoint activeCheckpoint;      //Keeps track of the active Checkpoint script.
    public static Checkpoint previousCheckpoint;    //Keeps track of the last active Checkpoint script.
    private static List<Checkpoint> managedCheckpoints = new List<Checkpoint>(); //Tracks each Checkpoint class.

    public SpriteRenderer checkpointSpriteRenderer; //The sprite renderer for each Checkpoint object.
    public Sprite defaultCheckpointSprite;          //The sprite to revert back to upon Checkpoint becoming inactive.
    public Animator bookAnimator;                   //The animator attached to this Checkpoint.

    private void Awake()
    {
        if (!managedCheckpoints.Contains(this))
        {
            managedCheckpoints.Add(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activeCheckpoint != this && collision.CompareTag(playerString))
        {
            GameManager.lastCheckpointPosition = transform.position;
            GameManager.SetGravityIndex(checkpointGravityIndex);

            ActivateParticles();
            ActivateCheckpointSound();
            ActivateBookAnimator();

            ResetSprites();

            activeCheckpoint = this;
        }
    }

    private void ActivateParticles()
    {
        checkpointParticles.transform.position = transform.position;
        checkpointParticles.Play();
    }

    private void ActivateCheckpointSound()
    {
        checkpointSound.Play();
    }

    private void ActivateBookAnimator()
    {
        bookAnimator.enabled = true;
        StartCoroutine(DisableBookAnimator());
    }    

    private IEnumerator DisableBookAnimator()
    {
        yield return new WaitForSeconds(0.5f);
        bookAnimator.enabled = false;
    }

    //Iterates through each Checkpoint in the level and resets the sprite to default unless it is the active Checkpoint.
    public void ResetSprites()
    {
        foreach (Checkpoint checkpoint in managedCheckpoints)
        {
            if(checkpoint != this && checkpoint != null)
            {
                checkpoint.checkpointSpriteRenderer.sprite = defaultCheckpointSprite;
            }
        }
    }

    /* VALUABLE CODE: Returns if the current animation is being played.
    if (bookAnimator.GetCurrentAnimatorStateInfo(0).IsName("BookOpen"))
    {
        // Avoid any reload.
    }
    */
}
