using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public int currentFloor;                            //The floor that this individual elevator is on.
    private bool elevatorAccessible;                    //Tracks when Luper is near the panel selection of the elevator.
    private readonly string playerString = "Player";    //The string holding the GameObject tag reference to the player.

    private ElevatorController elevatorControllerClass; //The overriding class keeping track of all Elevator scripts.
    private ElevatorActivation elevatorActivationClass; //The child class checking when Luper steps inside the activation area.

    [Header("Image Components")]
    public Animator leftDoorAnimator;                   //The animator component for the left door.
    public Animator rightDoorAnimator;                  //The animator component for the right door.
    public Renderer leftDoorRenderer;                   //The renderer component for the left door.
    public Renderer rightDoorRenderer;                  //The renderer component for the left door.

    private bool liftMoving;                            //Tracks whether the lift is currently in motion.
    private bool liftReady;                             //Tracks if the lift is waiting for the player to activate it.
    private float liftCurrentWaitTime;                  //How long the lift has been waiting for the player to activate it.
    private float liftMaxWaitTime = 5;                  //The maximum time the lift will wait for the player to activate it.

    [Header("Lift Components")]
    public CircleCollider2D liftActivationZoneCollider; //The collider on the LiftActivationZone object.
    public Transform liftPlatform;                      //The lift platform that moves the player when activate.
    private Vector3 liftPlatformOriginalPosition;       //Reference to original position for the lift platform to return to.

    [Header("Player Components")]
    public GameObject luperGameObject;                  //Reference to player for parenting object to the lift platform.
    public Collider2D luperCollider;                    //Reference to collider for disabling so can move through surfaces.
    public Rigidbody2D luperRigidbody2D;                //Reference to rigidbody to stop the effects of forces when moving.
    public SpriteRenderer luperSprite;                  //Reference to sprite renderer to hide the player when moving floors.

    [Header("Button Components")]
    public GameObject[] buttonLocations;                //The location of the buttons on the lift.
    public Color[] buttonColours;                       //The colour of the buttons on the lift.
    public GameObject buttonParticleGlow;               //The gameobject holding the particle system. For setting position.
    public ParticleSystem buttonParticles;              //The particle system for the elevator button.

    private void Awake()
    {
        elevatorControllerClass = GetComponentInParent<ElevatorController>();

        if(elevatorControllerClass == null)
        {
            Debug.LogError("Elevator: Couldn't find ElevatorController component in parent.");
        }

        elevatorActivationClass = GetComponentInChildren<ElevatorActivation>();

        if (elevatorActivationClass == null)
        {
            Debug.LogError("Elevator: Couldn't find ElevatorActivation component in child.");
        }

        liftPlatformOriginalPosition = liftPlatform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (elevatorAccessible && InputManager.GetPower() && !elevatorControllerClass.GetElevatorCooldown())
        {
            elevatorControllerClass.ChangeFloorSelection(currentFloor);
            SetButtonParticles(true);
            LiftWaitingForPlayer(true);
            OpenDoors();
            elevatorActivationClass.enabled = true;
        }

        if (liftReady)
        {
            liftCurrentWaitTime += Time.deltaTime;

            if (liftCurrentWaitTime > liftMaxWaitTime)
            {
                liftActivationZoneCollider.enabled = false;

                SetButtonParticles(false);
                LiftWaitingForPlayer(false);
                StartCoroutine(DoorsBehind());
                CloseDoors();
            }
        }
    }

    private void FixedUpdate()
    {
        if (liftMoving)
        {
            liftPlatform.position = Vector2.MoveTowards(liftPlatform.position, 
                elevatorControllerClass.ReturnFloorPlatformLocation().position, 4 * Time.deltaTime);

            if(Vector2.Distance(liftPlatform.position, elevatorControllerClass.ReturnFloorPlatformLocation().position) < 0.05)
            {
                StartCoroutine(EnableLuper());
                StartCoroutine(DeactivateLift());
                StartCoroutine(elevatorControllerClass.SetElevatorCooldown());
            }
        }
    }

    private void SetButtonParticles(bool particlesActivated)
    {
        if (particlesActivated)
        {
            int elevatorFloorSelected = elevatorControllerClass.GetFloorSelected();

            SetButtonParticleColour(elevatorFloorSelected);
            SetButtonParticleLocation(elevatorFloorSelected);
            buttonParticles.Play();
        }
        else
        {
            buttonParticles.Stop();
        }
    }

    private void SetButtonParticleColour(int elevatorFloorSelected)
    {
        var main = buttonParticles.main;
        main.startColor = buttonColours[elevatorFloorSelected];
    }

    private void SetButtonParticleLocation(int elevatorFloorSelected)
    {
        buttonParticleGlow.transform.position = buttonLocations[elevatorFloorSelected]
            .transform.position;
    }

    private void LiftWaitingForPlayer(bool liftWaiting)
    {
        if (liftWaiting)
        {
            StartCoroutine(EnableActivationZone());

            liftReady = true;
            liftCurrentWaitTime = 0;
        }
        else
        {
            liftReady = false;
        }
    }

    //Delay enabling the activation zone so the lift doesn't instantly start when activating from the center.
    private IEnumerator EnableActivationZone()
    {
        yield return new WaitForSeconds(1.0f);
        liftActivationZoneCollider.enabled = true;
    }

    public IEnumerator ActivateLift()
    {
        //Now that the desired floor is confirmed, need to keep this in memory in ElevatorController class.
        elevatorControllerClass.SetFloorDestination();

        //Doors need to appear in front of the player now to appear like the player is inside the lift.
        StartCoroutine(DoorsInFront());
        CloseDoors();        
        
        StartCoroutine(DisableLuper());

        //Doors need time to close before moving the lift.
        yield return new WaitForSeconds(1.8f);
        liftMoving = true;

        //The doors on the destination lift need to be in front of the player so it is consistent.
        StartCoroutine(elevatorControllerClass.ReturnDestinationElevatorClass().DoorsInFront());
    }

    private IEnumerator DeactivateLift()
    {
        //Doors need to go back to rendering behind the player so it appears the player is outside the lift.
        StartCoroutine(DoorsBehind());        
        elevatorControllerClass.ReturnDestinationElevatorClass().OpenDoors();

        //Doors need time to open before the player can move outside of them.
        yield return new WaitForSeconds(1.8f);

        liftReady = true; //TEST; Uncertain why this was written here.

        liftPlatform.position = liftPlatformOriginalPosition;
        StartCoroutine(elevatorControllerClass.ReturnDestinationElevatorClass().DoorsBehind());

        yield return new WaitForSeconds(1.8f);
        elevatorControllerClass.ReturnDestinationElevatorClass().CloseDoors();

        liftMoving = false;
    }

    private IEnumerator DisableLuper()
    {
        luperGameObject.transform.parent = liftPlatform.transform;
        luperCollider.enabled = false;
        luperRigidbody2D.simulated = false;

        yield return new WaitForSeconds(1.8f);
        luperSprite.enabled = false;
    }

    private IEnumerator EnableLuper()
    {
        luperSprite.enabled = true;

        //Need to wait for Elevator doors to open before enabling Luper again.
        yield return new WaitForSeconds(1.8f);

        luperGameObject.transform.parent = null;
        luperCollider.enabled = true;
        luperRigidbody2D.simulated = true;

        elevatorActivationClass.LuperDisabledStatus(false);
    }

    private void OpenDoors()
    {
        leftDoorAnimator.SetBool("DoorsOpening", true);
        rightDoorAnimator.SetBool("DoorsOpening", true);
        leftDoorAnimator.SetBool("DoorsClosing", false);
        rightDoorAnimator.SetBool("DoorsClosing", false);
    }

    private void CloseDoors()
    {
        leftDoorAnimator.SetBool("DoorsClosing", true);
        rightDoorAnimator.SetBool("DoorsClosing", true);
        leftDoorAnimator.SetBool("DoorsOpening", false);
        rightDoorAnimator.SetBool("DoorsOpening", false);

        elevatorActivationClass.enabled = false;
    }

    private IEnumerator DoorsInFront()
    {
        yield return new WaitForSeconds(0.2f);

        leftDoorRenderer.sortingLayerName = "Environment";
        rightDoorRenderer.sortingLayerName = "Environment";

        leftDoorRenderer.sortingOrder = -1;
        rightDoorRenderer.sortingOrder = -1;
    }

    private IEnumerator DoorsBehind()
    {
        yield return new WaitForSeconds(0.2f);

        leftDoorRenderer.sortingLayerName = "EnvironmentFar";
        rightDoorRenderer.sortingLayerName = "EnvironmentFar";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            elevatorAccessible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerString))
        {
            elevatorAccessible = false;
        }
    }
}
