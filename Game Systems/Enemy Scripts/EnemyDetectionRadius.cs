using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionRadius : MonoBehaviour
{
    public static bool enemyInRange;        //Checks if there is an enemy Luper can dive too.
    public static Vector3 enemyPosition;    //The enemies position.

    private static readonly string enemyString = "Enemy";    //The string holding the GameObject tag reference to the enemy.

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag(enemyString))
        {
            enemyInRange = true;
            enemyPosition = new Vector3(collision.attachedRigidbody.position.x, collision.attachedRigidbody.position.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(enemyString))
        {
            enemyInRange = false;
            enemyPosition = Vector3.zero;
        }
    }

    public static bool EnemyDetected()
    {
        return enemyInRange;
    }

    //Determines if there is an object between Luper and the enemy based on a raycast result.
    public static bool ObstacleBetweenEnemy(Transform luperTransform, Vector2 enemyDirection)
    {

        bool obstacleBetweenEnemy = false;

        RaycastHit2D raycastHit = Physics2D.Raycast(luperTransform.position, enemyDirection, 10);

        //If the raycast collided with an object, will check to see if the tag is the enemies tag. If it is
        //that means there is no obstacle in between Luper and the enemy.
        if (raycastHit.collider != null)
        {
            if(raycastHit.collider.CompareTag(enemyString))
            {
                obstacleBetweenEnemy = false;
            }
            else
            {
                obstacleBetweenEnemy = true;
            }
        }        

        return obstacleBetweenEnemy;        
    }
}
