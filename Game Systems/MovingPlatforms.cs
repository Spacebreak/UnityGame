using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Transform[] platformDestinations;    //Holds all the locations the platform will move to.
    private int destinationIndex = 0;           //Tracks the next location for the platform to move to.
    public float platformMoveSpeed;             //Speed which the platform moves.
    private bool platformReversing;             //Tracks if platform is going in reverse.
    private readonly string playerString = "Player"; //The string holding reference to GameObject tag.

    public bool platformCycling;                //Tracks if the platform moves in an enclosed loop.

    //Measures distance between current platform position and destination position. If >0.2 will move towards
    //the next platform, if <0.2 will change the destination to the next platform in the cycle.
    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, platformDestinations[destinationIndex].position) < 0.1)
        {
            if (!platformReversing)
            {
                destinationIndex = destinationIndex + 1;
            }
            else
            {
                destinationIndex = destinationIndex - 1;
            }

            if (destinationIndex > platformDestinations.Length - 1)
            {

                if (platformCycling)
                {
                    destinationIndex = 0;
                }
                else
                {
                    platformReversing = true;
                    destinationIndex = platformDestinations.Length - 1;
                }

            }
            else if(destinationIndex < 0)
            {
                platformReversing = false;
                destinationIndex = 0;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
                platformDestinations[destinationIndex].position, platformMoveSpeed * Time.deltaTime);
        }
    }

    //Will set Luper to be a child of the moving platform so he moves in unison with it.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(playerString))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerString))
        {
            collision.collider.transform.SetParent(null);
        }
    }


}
