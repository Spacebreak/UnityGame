using System.Collections;
using UnityEngine;

public class EnemyForestWalking : MonoBehaviour
{
    public Transform leftBoundary;              //The furthest left point the enemy will travel before turning back.
    public Transform rightBoundary;             //The furthest right point the enemy will travel before turning back.

    public Rigidbody2D enemyRigidBody;          //The rigidbody of the enemy this script is attached to.
    private bool movingLeft;                    //Tracks whether the enemy is moving left (true) or right (false).

    private readonly string playerString = "Player"; //The string holding reference to GameObject tag.

    private Animator enemyAnimator;             //The animator animating the enemy sprite.
    private EnemyState enemyState;

    public int whiteTokenGravityIndex;              //

    private void Start()
    {
        enemyState = EnemyState.Dormant;
        enemyAnimator = GetComponent<Animator>();

        if(enemyAnimator == null)
        {
            Debug.LogError("EnemyBehaviourSimpleLeftRight: No animator component found on the enemy.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState == EnemyState.Patrolling)
        {
            GravityDependantMovement(whiteTokenGravityIndex);
        }
    }

    private void GravityDependantMovement(int gravityIndex)
    {
        switch (gravityIndex)
        {
            case 0:
                if (transform.position.x < leftBoundary.position.x)
                {
                    movingLeft = false;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", false);
                    }
                }

                if (transform.position.x > rightBoundary.position.x)
                {
                    movingLeft = true;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", true);
                    }
                }
                break;

            case 1:
                if (transform.position.y < leftBoundary.position.y)
                {
                    movingLeft = false;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", true);
                    }
                }

                if (transform.position.y > rightBoundary.position.y)
                {
                    movingLeft = true;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", false);
                    }
                }
                break;

            case 2:
                if (transform.position.x < leftBoundary.position.x)
                {
                    movingLeft = false;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", true);
                    }
                }

                if (transform.position.x > rightBoundary.position.x)
                {
                    movingLeft = true;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", false);
                    }
                }
                break;

            case 3:
                if (transform.position.y > leftBoundary.position.y)
                {
                    movingLeft = true;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", true);
                    }
                }

                if (transform.position.y < rightBoundary.position.y)
                {
                    movingLeft = false;

                    if (enemyAnimator != null)
                    {
                        enemyAnimator.SetBool("Flip", false);
                    }
                }
                break;

            default:
                break;
        }


    }

    private void FixedUpdate()
    {
        if (enemyState == EnemyState.Patrolling)
        {
            if(whiteTokenGravityIndex == 0 || whiteTokenGravityIndex == 2)
            {
                if (movingLeft)
                {
                    enemyRigidBody.MovePosition(enemyRigidBody.position - new Vector2(0.1f, 0));
                    //enemyRigidBody.AddForce(new Vector2(-enemyMoveSpeed, 0));
                }
                else
                {
                    enemyRigidBody.MovePosition(enemyRigidBody.position + new Vector2(0.1f, 0));
                    //enemyRigidBody.AddForce(new Vector2(enemyMoveSpeed, 0));
                }
            }

            if (whiteTokenGravityIndex == 1 || whiteTokenGravityIndex == 3)
            {
                if (movingLeft)
                {
                    enemyRigidBody.MovePosition(enemyRigidBody.position - new Vector2(0, 0.1f));
                    //enemyRigidBody.AddForce(new Vector2(-enemyMoveSpeed, 0));
                }
                else
                {
                    enemyRigidBody.MovePosition(enemyRigidBody.position + new Vector2(0, 0.1f));
                    //enemyRigidBody.AddForce(new Vector2(enemyMoveSpeed, 0));
                }
            }


        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerString))
        {
            enemyAnimator.SetBool("Awake", true);
            StartCoroutine(DelayPatrol());
        }
    }

    private IEnumerator DelayPatrol()
    {
        yield return new WaitForSeconds(1.0f);

        enemyState = EnemyState.Patrolling;
    }

    private enum EnemyState
    {
        Dormant,
        Patrolling
    }
}
