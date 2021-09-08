using UnityEngine;

public class ElevatorActivation : MonoBehaviour
{
    private readonly string playerString = "Player";    //The string holding the GameObject tag reference to the player.

    private Elevator elevatorClass;                     //The class controlling the individual elevator.
    private LuperController luperControllerClass;       //For disabling Luper in the LuperController class.

    public TokenWhite whiteTokenClass;

    // Start is called before the first frame update
    void Awake()
    {
        elevatorClass = GetComponentInParent<Elevator>();

        if (elevatorClass == null)
        {
            Debug.LogError("ElevatorActivation: Couldn't find Elevator component in parent.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString) && whiteTokenClass.GravityIndex() == 0)
        {
            if(luperControllerClass == null)
            {
                ObtainLuperControllerClass(collision);
            }

            LuperDisabledStatus(true);

            StartCoroutine(elevatorClass.ActivateLift());
        }
    }

    private void ObtainLuperControllerClass(Collider2D collision)
    {
        luperControllerClass = collision.GetComponentInParent<LuperController>();

        if (luperControllerClass == null)
        {
            Debug.LogError("ElevatorActivation: Could not retrieve LuperController from the player.");
        }
    }

    public void LuperDisabledStatus(bool luperDisable)
    {
        luperControllerClass.LuperDisabledPermanent(luperDisable);
    }
}
