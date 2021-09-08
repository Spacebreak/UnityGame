using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForestFlying : MonoBehaviour
{
    private readonly string playerString = "Player"; //The string holding reference to GameObject tag.

    private bool playerInRange;             //Tracks if player is in range of the enemy.
    public Rigidbody2D hawkRigidBody;       //Rigidbody of the hawk.    
    private Transform playerTransform;      //Tracks position of the player.

    public float hawkSpeed;                 //The speed the hawk moves.
    private Vector3 startingPosition;       //The starting position of the hawk. For returning to.
    private bool returnToStartingPosition;  //Tracks whether the Hawk is returning to its origin.

    private bool facingLeft;                //Tracks orientation of the hawk.
    private Vector3 previousPosition;       //Tracks previous position of the hawk.
    private Vector3 currentPosition;        //Tracks current position of the hawk.
    private Animator enemyAnimator;         //Holds the Animator component of the hawk.


    private void Start()
    {
        startingPosition = transform.position;

        enemyAnimator = GetComponent<Animator>();

        if(enemyAnimator == null)
        {
            Debug.LogError("EnemyHawk: Couldn't obtain Animator component");
        }
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, startingPosition) > 7)
        {
            returnToStartingPosition = true;
            //hawkSpeed = 2f;
        }

        if (!facingLeft && enemyAnimator != null)
        {
            enemyAnimator.SetBool("Reverse", true);
        }
        else
        {
            enemyAnimator.SetBool("Reverse", false);
        }
    }

    private void FixedUpdate()
    {
        previousPosition = transform.position;

        if (returnToStartingPosition)
        {
            if (Vector3.Distance(transform.position, startingPosition) > 0.1)
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, hawkSpeed * 0.65f * Time.deltaTime);
                currentPosition = transform.position;
            }
            else
            {
                returnToStartingPosition = false;
                //hawkSpeed = 0.08f;
            }

            UpdateOrientation();
            return;
        }

        if (playerInRange && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, hawkSpeed * Time.deltaTime);
            currentPosition = transform.position;
            UpdateOrientation();
        }

    }

    private void UpdateOrientation()
    {
        if (currentPosition.x > previousPosition.x)
        {
            facingLeft = false;
        }
        else if (currentPosition.x < previousPosition.x)
        {
            facingLeft = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            playerInRange = true;
            playerTransform = collision.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            playerInRange = false;
            //playerObject = collision.GetComponent<GameObject>();
        }
    }
}
