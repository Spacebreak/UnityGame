using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LuperController : MonoBehaviour
{
    private Rigidbody2D luperRigidBody2D;           //Stores the Rigidbody attached to Luper.
    private Animator luperAnimator;                 //Stores the Animator attached to Luper.
    private SpriteRenderer luperSpriteRenderer;     //Stores the Sprite Renderer attached to Luper.

    public float luperMovementSpeed;                //Speed of Luper moving.       
    private float horizontalMovement;               //Storing movement along the x-axis. Ranges from -1 to 1.
    private float verticalMovement;                 //Storing movement along the y-axis. Ranges from -1 to 1.
    private float luperPreviousPositionY;           //Storing position on the y-axis. Used to detect if falling.

    //Mobile-specific variables.
    public Joystick mobileJoystick;                 //The joystick acting as a virtual UI axis for horizontal movement.
    public Joystick mobileJoystickInverted;         //The inverted joystick, needs to be a seperate object.
    private bool touchedJump;                       //Tracking if the user has pressed jump on mobile platforms.
    private bool touchedSpin;                       //Tracking if the user has pressed jump on mobile platforms.
    private bool touchedSlide;                      //Tracking if the user has pressed jump on mobile platforms.
    private bool touchedPower;                      //Tracking if the user has pressed jump on mobile platforms.

    public float luperJumpSpeed;                    //Speed of Luper jumping high.
    private bool jumpRequest;                       //Tracks if Jump has been pressed.
    private bool doubleJumpRequest;                 //Tracks if Jump has been pressed whilst mid-air.
    private bool doubleJumped;                      //Tracks if Luper has already jumped a second time.

    public float luperWaterJumpSpeed;               //Speed of Luper jumping high in the water.
    public bool underWater;                         //Checks if Luper is currently underwater.
    private bool waterJumpRequest;                  //Checks if Luper wants to jump whilst submerged.

    private bool luperSpinning;                     //Checks if Luper is currently spinning.
    private bool luperSliding;                      //Checks if Luper is currently sliding.
    public float luperSlideSpeed;                   //Speed that Luper slides.
    public float luperAirDiveSpeed;                 //Speed that Luper air dives enemies.
    public float luperWaterSpinSpeed;               //Speed that Luper descends when spinning underwater.

    public bool luperDisabled;                      //When Luper stands on sliding terrain.
    private static bool gravityChanging;            //When gravity is altering.

    private bool luperCrouching;                    //When Luper is crouching.
    private bool tailSlamJumped;                    //Whether Luper has tail-slammed or not.
    private bool checkGround;                       //Checks if there is ground beneath Luper before tail-slamming.

    public bool grounded;                           //Tracks if Luper is on the ground.
    public float groundBoxHeight;                   //Stores the height of the box below Luper to detect the ground.
    public LayerMask maskToTriggerOn;               //Detects what types of objects to trigger on (set in Unity inspector).

    private Vector2 groundBoxCenter;                //Stores location of the ground-detection box underneath Luper.
    private Vector2 groundBoxSize;                  //Stores size of the ground-detection box underneath Luper.

    public CapsuleCollider2D luperCollider;

    public PhysicsMaterial2D minorFriction;         //Stops Luper from sliding along the ground after he has stopped moving.
    public PhysicsMaterial2D noFriction;            //Stops Luper from getting caught on the edge of cliffs when still moving.

    public GameObject whiteToken;                   //The token controlling gravity.
    private TokenWhite whiteTokenClass;            //Manages settings affecting gravity and world physics.

    public SoundController luperSoundController;    //The script managing sound components on Luper.

    private void Awake()
    {
        GameManager.Initialize();
    }

    private void Start()
    {
        //GameManager.Initialize();
        //GameManager.DisplayPlayerData();

        //Ensures Luper is loaded at the correct checkpoint on reload.
        //gameManager = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();

        if (GameManager.lastCheckpointPosition == Vector3.zero)
        {
            //Do nothing.
        }
        else
        {
            transform.position = GameManager.lastCheckpointPosition;
        }

        //These two aren't necessary, only in Unity as starting the scene from inside the game means some inputs aren't set.
        InputManager.UpdateAllControls();
        InputManager.UpdateAllKeys();

        whiteTokenClass = whiteToken.GetComponent<TokenWhite>();

        luperRigidBody2D = GetComponent<Rigidbody2D>();
        luperAnimator = GetComponent<Animator>();
        luperSpriteRenderer = GetComponent<SpriteRenderer>();

        //Determines the size of the box below Luper to detect the ground.
        groundBoxSize = new Vector2(1.1f, groundBoxHeight);
    }

    private float HorizontalInput()
    {
        float horizontalValue = Mathf.Clamp(InputManager.HorizontalInput() + mobileJoystick.Horizontal +
            mobileJoystickInverted.Horizontal, -1, 1);

        return horizontalValue;
    }

    private void Update()
    {
        //Debug.Log("Velocity: " + luperRigidBody2D.velocity);

        //If game is paused, will prevent Luper from remembering input upon unpausing.
        if (PauseManager.gamePaused || PauseManager.gameUnpausedDelay)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManager.Save();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.DisplayPlayerData();
        }

        //If Luper is on a slippery slope, will set his movement to 0 so gravity can take full effect.
        if (luperDisabled)
        {
            horizontalMovement = 0;
            return;
        }

        //Only gets user input when Luper isn't slipping.
        if (!luperDisabled && !gravityChanging)
        {
#if UNITY_ANDROID
            {
                horizontalMovement = mobileJoystick.Horizontal + mobileJoystickInverted.Horizontal;
                verticalMovement = mobileJoystick.Vertical + mobileJoystickInverted.Vertical;

                /*
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                    Debug.DrawLine(Vector3.zero, touchpos, Color.red);
                }
                */

            }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            {
                horizontalMovement = InputManager.HorizontalInput();
                verticalMovement = InputManager.VerticalInput();
            }
#endif
        }

        AnimatorSetState();

        if (verticalMovement < 0 && grounded)
        {
            luperCrouching = true;
        }
        else
        {
            luperCrouching = false;
        }

        //If Luper has tail-slam jumped, will begin to check the ground to tell the game he can perform the move again.
        if (checkGround)
        {
            if (grounded)
            {
                checkGround = false;
                tailSlamJumped = false;
            }
        }

        if ((InputManager.GetJump() || touchedJump) && grounded && !tailSlamJumped)
        {
            if (!luperCrouching)
            {
                if (!underWater)
                {
                    jumpRequest = true;
                }
                else
                {
                    waterJumpRequest = true;
                    //waterJumpRdddequest = true;

                }
            }
            else
            {
                luperSoundController.PlayAudioClip(SoundController.AudioCollection.Jump, 2, 0.4f);
                TailSlamJump();
            }
        }

        //If Luper is in the air and hasn't already jumped a second time, will request a second jump.
        if (!doubleJumped && !grounded && !doubleJumpRequest && !tailSlamJumped && (InputManager.GetJump() || touchedJump))
        {
            doubleJumpRequest = true;
        }

        if (grounded)
        {
            luperCollider.sharedMaterial = minorFriction;
            doubleJumped = false;
        }
        else
        {
            luperCollider.sharedMaterial = noFriction;
        }

        if (gravityChanging && (grounded || underWater))
        {
            gravityChanging = false;
        }

        if (underWater && !waterJumpRequest && !grounded && (InputManager.GetJump() || touchedJump))
        {
            waterJumpRequest = true;
        }

        //Checks if Luper should be spinning
        if (!luperSpinning && !luperSliding && (InputManager.GetSpin() || touchedSpin))
        {
            LuperSpin();
        }

        if (!luperSliding && !luperSpinning && (InputManager.GetSlide() || touchedSlide))
        {
            LuperSlide();
        }

    }

    private void FixedUpdate()
    {
        if (horizontalMovement != 0)
        {
            Movement(horizontalMovement);
            SetLupersFacingDirection(horizontalMovement);
            CorrectColliderOffset();
        }

        if (jumpRequest)
        {
            Jump();
            luperSoundController.PlayAudioClip(SoundController.AudioCollection.Jump, 0, 0.4f);
            jumpRequest = false;
            grounded = false;
        }
        else if (doubleJumpRequest && !underWater)
        {
            Jump();
            luperSoundController.PlayAudioClip(SoundController.AudioCollection.Jump, 1, 0.4f);
            doubleJumpRequest = false;
            doubleJumped = true;
        }
        else //If the player doesn't want to jump, will update the box detection area and whether Luper is grounded.
        {
            //boxCenter = (Vector2)transform.position + VectorGroundBox() * (playerSize.y + boxSize.y) * 0.5f;
            groundBoxCenter = (Vector2)transform.position + VectorGroundBox();
            grounded = (Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, maskToTriggerOn) != null);
        }

        if (waterJumpRequest)
        {
            luperSoundController.PlayAudioClip(SoundController.AudioCollection.Jump, 3, 0.7f);
            WaterJump();
        }

        luperPreviousPositionY = luperRigidBody2D.position.y;
    }

    //Returns the direction Lupers hitbox should be generated in so when gravity rotates, the detection box for detecting
    //ground correlates to the right direction.
    private Vector2 VectorGroundBox()
    {
        switch (whiteTokenClass.GravityIndex())
        {
            case 0:
                groundBoxSize = new Vector2(1.1f, groundBoxHeight);
                return Vector2.down;

            case 1:
                groundBoxSize = new Vector2(groundBoxHeight, 1.1f);
                return Vector2.left;

            case 2:
                groundBoxSize = new Vector2(1.1f, groundBoxHeight);
                return Vector2.up;

            case 3:
                groundBoxSize = new Vector2(groundBoxHeight, 1.1f);
                return Vector2.right;

            default:
                groundBoxSize = new Vector2(1.1f, groundBoxHeight);
                return Vector2.down;
        }
    }

    private void Jump()
    {
        luperRigidBody2D.AddRelativeForce(Vector2.up * luperJumpSpeed, ForceMode2D.Impulse);
    }

    private void TailSlamJump()
    {
        tailSlamJumped = true;
        luperRigidBody2D.AddRelativeForce(Vector2.up * (luperJumpSpeed * 2.2f), ForceMode2D.Impulse);
        StartCoroutine(TailSlamJumpDelay());
    }

    private void WaterJump()
    {
        luperRigidBody2D.AddRelativeForce(Vector2.up * luperWaterJumpSpeed, ForceMode2D.Impulse);
        StartCoroutine(WaterJumpDelay());        
    }

    public bool GetLuperSpin()
    {
        return luperSpinning;
    }

    private void LuperSpin()
    {
        luperSpinning = true;

        if (underWater)
        {
            luperRigidBody2D.AddRelativeForce(Vector2.down * luperWaterSpinSpeed, ForceMode2D.Force);
            StartCoroutine(LuperSpinDelay(0.2f));
        }
        else
        {
            StartCoroutine(LuperSpinDelay(0.8f));
        }

    }

    //If there is an enemy in range, will dive towards them at quick speed, otherwise will perform a regular slide.
    private void LuperSlide()
    {
        luperSliding = true;
        luperRigidBody2D.sharedMaterial = noFriction;

        //If an enemy is detected in range, will shoot a raycast to determine if there is an obstacle between them.
        if (EnemyDetectionRadius.EnemyDetected())
        {
            Vector2 enemyDirection = new Vector2(EnemyDetectionRadius.enemyPosition.x - luperRigidBody2D.position.x,
                EnemyDetectionRadius.enemyPosition.y - luperRigidBody2D.position.y);

            if (!EnemyDetectionRadius.ObstacleBetweenEnemy(transform, enemyDirection))
            {
                //Normalization so the magnitude of the dive is not influenced by distance of Luper to the enemy.
                enemyDirection = enemyDirection.normalized;

                luperRigidBody2D.AddForce(enemyDirection * luperAirDiveSpeed, ForceMode2D.Impulse);
            }
        }
        else
        {
            SlidingMovement();
        }

        StartCoroutine(LuperSlideDelay());
    }

    //After a short duration, will start checking for ground beneath Luper. Cannot happen instantly as 
    //it will automatically become true on the first frame.
    private IEnumerator TailSlamJumpDelay()
    {
        yield return new WaitForSeconds(0.3f);
        checkGround = true;
    }

    private IEnumerator WaterJumpDelay()
    {
        yield return new WaitForSeconds(0.2f);
        waterJumpRequest = false;
    }

    private IEnumerator LuperSpinDelay(float timeToDelay)
    {
        yield return new WaitForSeconds(0.8f);
        luperSpinning = false;
    }

    private IEnumerator LuperSlideDelay()
    {
        yield return new WaitForSeconds(0.8f);
        luperSliding = false;
    }

    public void TouchSlideInput()
    {
        touchedSlide = true;
        StartCoroutine(DelayTouchedSlide());
    }

    public void TouchSpinInput()
    {
        touchedSpin = true;
        StartCoroutine(DelayTouchedSpin());
    }

    public void TouchJumpInput()
    {
        touchedJump = true;
        StartCoroutine(DelayTouchedJump());
    }

    public void TouchPowerInput()
    {
        touchedPower = true;
        StartCoroutine(DelayTouchedPower());
    }

    public bool ReturnPowerInput()
    {
        return touchedPower;
    }

    //Will only revert the jump/slide/spin after the end of the current frame.
    private IEnumerator DelayTouchedJump()
    {
        yield return new WaitForEndOfFrame();
        touchedJump = false;
    }

    private IEnumerator DelayTouchedSpin()
    {
        yield return new WaitForEndOfFrame();
        touchedSpin = false;
    }

    private IEnumerator DelayTouchedSlide()
    {
        yield return new WaitForEndOfFrame();
        touchedSlide = false;
    }

    private IEnumerator DelayTouchedPower()
    {
        yield return new WaitForEndOfFrame();
        touchedPower = false;
    }

    //If input is detected, will move the character. Input is a value from -1 to 1.
    private void Movement(float horizontalMovement)
    {
        //Dynamically affects Lupers movement based on the gravity scale.
        switch (whiteTokenClass.GravityIndex())
        {
            case 0:
                luperRigidBody2D.velocity = new Vector2(horizontalMovement * luperMovementSpeed, luperRigidBody2D.velocity.y);
                break;

            case 1:
                luperRigidBody2D.velocity = new Vector2(luperRigidBody2D.velocity.x, -horizontalMovement * luperMovementSpeed);
                break;

            case 2:
                luperRigidBody2D.velocity = new Vector2(-horizontalMovement * luperMovementSpeed, luperRigidBody2D.velocity.y);
                break;

            case 3:
                luperRigidBody2D.velocity = new Vector2(luperRigidBody2D.velocity.x, horizontalMovement * luperMovementSpeed);
                break;
        }

    }

    //Will make Luper slide in the direction he is facing depending on whether the sprite is flipped or not.
    private void SlidingMovement()
    {
        if (!luperSpriteRenderer.flipX)
        {
            luperRigidBody2D.AddRelativeForce((Vector2.right + Vector2.down) * luperSlideSpeed, ForceMode2D.Impulse);
        }
        else
        {
            luperRigidBody2D.AddRelativeForce((Vector2.left + Vector2.down) * luperSlideSpeed, ForceMode2D.Impulse);
        }
    }

    //Will apply the correct animation to Luper depending on his current action.
    private void AnimatorSetState()
    {
        if (luperSpinning)
        {
            luperAnimator.SetInteger("Luper State", 4);
        }
        else if (luperSliding)
        {
            luperAnimator.SetInteger("Luper State", 5);
        }
        else if (HorizontalInput() != 0 && grounded)
        {
            luperAnimator.SetInteger("Luper State", 1);
        }
        else if (HorizontalInput() != 0 && (luperRigidBody2D.position.y > luperPreviousPositionY))
        {
            luperAnimator.SetInteger("Luper State", 2); //Luper is jumping.
        }
        else if (HorizontalInput() != 0 && (luperRigidBody2D.position.y <= luperPreviousPositionY))
        {
            luperAnimator.SetInteger("Luper State", 3); //Luper is falling.
        }
        else if (HorizontalInput() == 0 && grounded)
        {
            luperAnimator.SetInteger("Luper State", 0); //Luper is idle.
        }
        else if (HorizontalInput() == 0 && !grounded && (luperRigidBody2D.position.y > luperPreviousPositionY))
        {
            luperAnimator.SetInteger("Luper State", 2);
        }
        else if (HorizontalInput() == 0 && !grounded && (luperRigidBody2D.position.y <= luperPreviousPositionY))
        {
            luperAnimator.SetInteger("Luper State", 3);
        }
    }

    private void SetLupersFacingDirection(float horizontalMovement)
    {
        if (horizontalMovement < 0)
        {
            luperSpriteRenderer.flipX = true;
        }
        else if (horizontalMovement > 0)
        {
            luperSpriteRenderer.flipX = false;
        }
    }

    //Because Lupers sprite is not centered when the sprite is flipped the collider can become
    //detached from the true position. So needs to be corrected when changing directions.
    private void CorrectColliderOffset()
    {
        if (luperSpriteRenderer.flipX)
        {
            luperCollider.offset = new Vector2(-0.2f, 0.06f);
        }
        else
        {
            luperCollider.offset = new Vector2(0.2f, 0.06f);
        }
    }

    //Force to be applied to Luper when being damaged or hurt.
    public void LuperKnockback(Vector3 knockbackSource, float xKnockback, float yKnockback)
    {
        luperDisabled = true;
        StartCoroutine(LuperDisabledDelay(true));

        Vector2 enemyContactLeftOrRight = transform.position - knockbackSource;

        switch (whiteTokenClass.GravityIndex())
        {
            case 0:

                if (enemyContactLeftOrRight.x < 0)
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(xKnockback, yKnockback), ForceMode2D.Impulse);
                }
                else 
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(-xKnockback, yKnockback), ForceMode2D.Impulse);
                }
                return;

            case 1:
                if (enemyContactLeftOrRight.y < 0)
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(-xKnockback, -yKnockback), ForceMode2D.Impulse);
                }
                else
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(-xKnockback, yKnockback), ForceMode2D.Impulse);
                }
                return;

            case 2:
                if (enemyContactLeftOrRight.x > 0)
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(-xKnockback, -yKnockback), ForceMode2D.Impulse);
                }
                else
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(xKnockback, -yKnockback), ForceMode2D.Impulse);
                }
                return;

            case 3:
                if (enemyContactLeftOrRight.y > 0)
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(xKnockback, yKnockback), ForceMode2D.Impulse);
                }
                else
                {
                    luperRigidBody2D.AddRelativeForce(new Vector2(xKnockback, -yKnockback), ForceMode2D.Impulse);
                }
                return;
        }
    }

    //WaitForSeconds to ensure there is some disabled time, and then when Luper is grounded or underwater
    //give control back to the player. Strong disables will prevent player input for longer.
    private IEnumerator LuperDisabledDelay(bool strongDisable)
    {
        if (strongDisable)
        {
            yield return new WaitForSeconds(0.6f);
        }

        yield return new WaitUntil(() => grounded || underWater);

        luperDisabled = false;
    }

    //Permanent disables to ensure Luper is disabled for a fixed duration, e.g when catching the elevator.
    public void LuperDisabledPermanent(bool luperDisableStatus)
    {
        if (luperDisableStatus)
        {
            luperRigidBody2D.velocity = Vector2.zero;
            luperAnimator.SetInteger("Luper State", 0); //Want player to idle when catching the lift.
        }

        luperDisabled = luperDisableStatus;
    }

    //Checks the object that Luper colliders with and acts depending on what is encountered.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "YellowToken")
        {
            collision.gameObject.SetActive(false);
            collision.GetComponentInParent<TokenYellow>().YellowCoinObtained();
        }
               
        /*
        if (collision.gameObject.layer == 4) //Layer 4 is water.
        {
            //luperRigidBody2D.gravityScale = 2;
        }
        */
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4) //Layer 4 is water.
        {
            underWater = true;
            jumpRequest = false;
            doubleJumped = true; //Prevents jumping behaviour being strange whilst in the water.
        }

        if (collision.gameObject.layer == 11) //Layer 11 is slippery slopes.
        {
            luperDisabled = true;
            StartCoroutine(LuperDisabledDelay(false));
        }
    }

    //Checks the object that Luper colliders with and acts depending on what is encountered.
    private void OnTriggerExit2D(Collider2D collision)
    {
        //When exiting the water applies a stronger gravity scale to Luper to offset the jump force transition out of water.
        if (collision.gameObject.layer == 4) //Layer 4 is water.
        {
            underWater = false;
        }

        if (collision.gameObject.layer == 11) //Layer 11 is slippery slopes.
        {
            luperDisabled = false;
        }

    }

    /*
    //Returns the correct direction the player wants based on an offset.
    public Vector2 GetDirection(int gravityIndexOffset)
    {
        switch ((whiteTokenScript.GravityIndex() + gravityIndexOffset) % 4)
        {
            case 0:
                return Vector2.down;

            case 1:
                return Vector2.left;

            case 2:
                return Vector2.up;

            case 3:
                return Vector2.right;

            default:
                return Vector2.down;
        }
    }
    */

    public static void SetGravityChanging()
    {
        gravityChanging = true;     
    }

    /* Was in use with White Token class to stop Gravity changing when Luper was mid-air.
    public Vector2 LuperVelocity()
    {
        return luperRigidBody2D.velocity;
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //Draws the ground-detection box.
        Gizmos.DrawWireCube(groundBoxCenter, groundBoxSize);
    }
}
