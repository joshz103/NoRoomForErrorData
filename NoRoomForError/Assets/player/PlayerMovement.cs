using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float airAccelMult;
    public float speedCap;
    public float jumpHeight;
    public float wallJumpHeight;
    public float wallJumpDirectional;
    public int direction = 1;
    public float iceSpeed;
    public bool slipping = false;

    [Header("Flags")]
    public bool isDead = false;
    public bool actionable = true;
    [SerializeField]private bool isGrounded;
    private bool canJump = true;
    public bool isCrouching = false;
    public bool isOnWall = false;
    public bool deathCam = true;
    public bool isOnIce = false;
    public bool isGrabbingLedge = false;
    public bool canBeOnWall = true;

    [Header("Death Setup")]
    public int playerHealth;
    public GameObject deathEffect;

    [Header("Setup")]
    private Rigidbody rb;
    private Animator animator;

    private float moveX;
    //private float moveY;

    public CapsuleCollider collisionStanding;
    public CapsuleCollider collisionStanding2;
    public CapsuleCollider collisionCrouching;
    public Cloth headband;

    private Vector3 isOnWallOffset = new Vector3(0f, 1.65f, 0f); //Vector3(0f, 1.65f, 0f);
    private Vector3 isOnWallOffset2 = new Vector3(0f, 0.05f, 0f); //Vector3(0f, 1.65f, 0f);

    private Quaternion forwardRot;
    private Quaternion backwardRot;

    public LayerMask groundLayer;

    //private Vector3 leftGroundOffset = new Vector3(-0.525f, 0.075f, 0);
    //private Vector3 rightGroundOffset = new Vector3(0.525f, 0.075f, 0);
    private Vector3 leftGroundOffset = new Vector3(-0.35f, 0.075f, 0);
    private Vector3 rightGroundOffset = new Vector3(0.35f, 0.075f, 0);
    private Vector3 verticalVelocityReset = new Vector3(1f, 0f, 1f);

    private Vector3 ledgeForwardCheckOffset = new Vector3(0.1f, 0f, 0);
    private Vector3 ledgeBackwardCheckOffset = new Vector3(-0.1f, 0f, 0);

    public CinemachineVirtualCamera camera;

    public Transform ledgeTarget; // The position the character should climb to
    public float climbUpSpeed = 2f; // Speed for climbing upwards
    public float climbForwardSpeed = 2f; // Speed for moving forward
    public AnimationCurve climbCurve; // Optional curve for smooth animation
    private bool isClimbing = false;
    private bool currentlyGrabbingLedge = false;
    private bool isGrabIdle = false;

    public BoxCollider grabPoint;

    [Header("Debug")]
    [SerializeField] private bool beatLevel = false;
    [SerializeField] private Transform levelFinishPos;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        forwardRot = Quaternion.Euler(0, 90, 0);
        backwardRot = Quaternion.Euler(0, 270, 0);

        levelFinishPos = GameObject.FindGameObjectWithTag("Finish").transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        MovementFramerateDependent();
        StartClimb();
        ReleaseLedge();
    }

    private void FixedUpdate()
    {
        Movement();
        if (actionable)
        {
            Rotation();
        }
        UpdateAnimations();

        //Debug
        DebugBeatLevel();
    }

    public void UpdateInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        if (moveX > 0 && actionable)
        {
            direction = 1;
        }
        if (moveX < 0 && actionable)
        {
            direction = -1;
        }

    }

    public void UpdateAnimations()
    {
        if(isGrounded)
        {
            animator.SetBool("isAirborne", false);
        }
        else
        {
            animator.SetBool("isAirborne", true);
        }

        animator.SetFloat("runSpeed", rb.velocity.x / 3f);

        if (Mathf.Round(rb.velocity.x * 1000f) / 1000f > 0f) //fixes floating point errors
        {
            animator.SetBool("isRunning", true);
        }
        else if (Mathf.Round(rb.velocity.x * 1000f) / 1000f < 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        //OnLedgeForwards();
        //OnLedgeBackwards();
    }


    public void Movement()
    {
        if (isGrounded)
        {
            //Left/Right Movement
            if (moveX > 0f && actionable)
            {
                if (!isCrouching)
                {
                    if (!isOnIce)
                    {
                        rb.AddForce(speed * Vector3.right, ForceMode.Acceleration);
                    }
                    else
                    {
                        rb.AddForce(iceSpeed * Vector3.right, ForceMode.Acceleration);
                    }
                }
            }
            else if (moveX < 0f && actionable)
            {
                if (!isCrouching)
                {
                    if(!isOnIce)
                    {
                        rb.AddForce(speed * Vector3.left, ForceMode.Acceleration);
                    }
                    else
                    {
                        rb.AddForce(iceSpeed * Vector3.left, ForceMode.Acceleration);
                    }
                }
            }
        }
        else
        {
            //Left/Right Movement airborne
            if (moveX > 0f && actionable)
            {
                if (!isCrouching)
                {
                    rb.AddForce((airAccelMult * speed) * Vector3.right, ForceMode.Acceleration);
                }
            }
            else if (moveX < 0f && actionable)
            {
                if (!isCrouching)
                {
                    rb.AddForce((airAccelMult * speed) * Vector3.left, ForceMode.Acceleration);
                }
            }
        }

        
    }

    public void MovementFramerateDependent()
    {
        //Cap Movement Speed
        if (rb.velocity.x > speedCap && direction == 1)
        {
            rb.velocity = new Vector3(speedCap, rb.velocity.y, 0f);
        }
        if (rb.velocity.x < -speedCap && direction == -1)
        {
            rb.velocity = new Vector3(-speedCap, rb.velocity.y, 0f);
        }

        if (Physics.Raycast(transform.position, Vector3.down, 0.025f, groundLayer) || Physics.Raycast(transform.position + leftGroundOffset, Vector3.down, 0.15f, groundLayer) || Physics.Raycast(transform.position + rightGroundOffset, Vector3.down, 0.15f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded && actionable && canJump)
        {
            canJump = false;
            animator.SetTrigger("isJumping");
        }

        //Walljump
        if (Input.GetButtonDown("Jump") && isOnWall && direction == 1 && !currentlyGrabbingLedge)
        {
            actionable = false;
            StartCoroutine(WallJumpAction());
            rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * wallJumpHeight, ForceMode.VelocityChange);
            rb.AddForce(Vector3.left * wallJumpDirectional, ForceMode.VelocityChange);
            animator.SetTrigger("isWallJumping");
            

        }
        if (Input.GetButtonDown("Jump") && isOnWall && direction == -1 && !currentlyGrabbingLedge)
        {
            actionable = false;
            StartCoroutine(WallJumpAction());
            rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * wallJumpHeight, ForceMode.VelocityChange);
            rb.AddForce(Vector3.right * wallJumpDirectional, ForceMode.VelocityChange);
            animator.SetTrigger("isWallJumping");
        }

        IEnumerator WallJumpAction()
        {
            yield return new WaitForSeconds(0.05f);

            if (!currentlyGrabbingLedge)
            {
                if (direction == 1)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, backwardRot, 150);
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, forwardRot, 150);
                }

                yield return new WaitForSeconds(0.15f);

                if (direction == 1)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }

                actionable = true;
            } 
        }

        if (direction == 1 && !slipping)
        {
            if (Physics.Raycast(transform.position + isOnWallOffset, Vector3.right, 0.4f, groundLayer) && !isGrounded && canBeOnWall)
            {
                isOnWall = true;
                animator.SetBool("isOnWall", true);
            }
            else
            {
                isOnWall = false;
                animator.SetBool("isOnWall", false);
            }
        }
        if (direction == -1 && !slipping)
        {
            if ((Physics.Raycast(transform.position + isOnWallOffset, Vector3.left, 0.4f, groundLayer) || Physics.Raycast(transform.position + isOnWallOffset2, Vector3.left, 0.4f, groundLayer)) && !isGrounded && canBeOnWall)
            {
                isOnWall = true;
                animator.SetBool("isOnWall", true);
            }
            else
            {
                isOnWall = false;
                animator.SetBool("isOnWall", false);
            }
        }

        //Crouching

        if (Input.GetButton("Crouch") && actionable && isGrounded)
        {
            isCrouching = true;
            animator.SetBool("isCrouching", true);
            collisionStanding.enabled = false;
            collisionStanding2.enabled = false;
            collisionCrouching.enabled = true;
        }
        else
        {
            isCrouching = false;
            animator.SetBool("isCrouching", false);
            collisionStanding.enabled = true;
            collisionStanding2.enabled = true;
            collisionCrouching.enabled = false;
        }
    }

    public void Rotation()
    {
        //Direction rotation
        if (direction == 1 && actionable)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, forwardRot, 25);
        }
        if (direction == -1 && actionable)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, backwardRot, 25);
        }
    }

    public void applyJumpForce()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        canJump = true;
    }

    //Slipping
    public void Slip(float duration, float speed, float height)
    {
        actionable = false;
        isOnWall = false;
        isGrabbingLedge = false;
        canBeOnWall = false;
        animator.SetBool("isSlipping", true);
        animator.SetBool("isGrabbingLedge", false);
        slipping = true;

        if(direction == 1)
        {
            rb.AddForce((Vector3.right * speed) + (Vector3.up * height), ForceMode.Impulse);
            Debug.Log("lol u slipped");
        }
        else
        {
            rb.AddForce((Vector3.left * speed) + (Vector3.up * height), ForceMode.Impulse);
            Debug.Log("lol u slipped");
        }
        
        StartCoroutine(ResetSlip(duration));
    }

    IEnumerator ResetSlip(float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.SetBool("isSlipping", false);
        actionable = true;
        slipping = false;
        canBeOnWall = true;
    }

    public void beActionable()
    {
        actionable = true;
    }

    /*
    public void OnLedgeForwards()
    {
        if (direction == 1)
        {
            if (!Physics.Raycast(transform.position + ledgeForwardCheckOffset, Vector3.down, 0.25f) && isGrounded && actionable && !isCrouching && animator.GetBool("isRunning") == false)
            {
                {
                    animator.SetBool("onLedgeForward", true);
                    //Debug.Log("OnLedgeForward RIGHT");
                }
            }
            else
            {
                animator.SetBool("onLedgeForward", false);
                //animator.SetBool("onLedgeBackward", false)        }
            }
        }
        else if (direction == -1)
        {
            if (!Physics.Raycast(transform.position + ledgeBackwardCheckOffset, Vector3.down, 0.25f) && isGrounded && actionable && !isCrouching && animator.GetBool("isRunning") == false)
            {
                {
                    animator.SetBool("onLedgeForward", true);
                    //Debug.Log("OnLedgeForward RIGHT");
                }
            }
            else
            {
                animator.SetBool("onLedgeForward", false);
                //animator.SetBool("onLedgeBackward", false)        }
            }
        }
        else
        {
            animator.SetBool("onLedgeForward", false);
        }

    }

    public void OnLedgeBackwards()
    {
        if (!Physics.Raycast(transform.position + ledgeBackwardCheckOffset, Vector3.down, 0.25f) && isGrounded && actionable && direction == 1 && !isCrouching)
        {
            {
                animator.SetBool("onLedgeBackward", true);
                Debug.Log("OnLedgeBackward RIGHT");
            }
        }
        else if (!Physics.Raycast(transform.position + ledgeBackwardCheckOffset, Vector3.down, 0.25f) && isGrounded && actionable && direction == -1 && !isCrouching)
        {
            {
                animator.SetBool("onLedgeBackward", true);
                Debug.Log("OnLedgeBackward LEFT");
            }
        }
        else
        {
            animator.SetBool("onLedgeBackward", false);
        }
    }
    */

    public void GrabLedge(int dir, Transform ledgePos)
    {
        if (!currentlyGrabbingLedge && !slipping)
        {
            currentlyGrabbingLedge = true;
            canBeOnWall = false;
            isGrabbingLedge = true;
            direction = dir;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            actionable = false;
            ledgeTarget = ledgePos;
            ConstrainPlayer();

            animator.SetBool("isGrabbingLedge", true);

            Debug.Log("Player Grabbed Ledge");
            StartCoroutine(ResetGrab());

            if (dir == 1)
            {
                transform.position = ledgePos.position + new Vector3(-0.05f, -1.75f, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, forwardRot, 250);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, backwardRot, 250);
                transform.position = ledgePos.position + new Vector3(0.05f, -1.75f, 0);
            }
        }
    }

    IEnumerator ResetGrab()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isGrabbingLedge", false);
        isGrabIdle = true;
    }

    public void StartClimb()
    {
        if (Input.GetButtonDown("Jump") && isGrabbingLedge && isGrabIdle)
        {
            isGrabIdle = false;
            StartCoroutine(ClimbLedge());
        }
    }

    public void ReleaseLedge()
    {
        if (Input.GetButtonDown("Crouch") && isGrabbingLedge && isGrabIdle)
        {
            isGrabIdle = false;
            UnconstrainPlayer();
            isGrabbingLedge = false;
            isClimbing = false;
            actionable = true;
            rb.useGravity = true;
            canBeOnWall = true;
            currentlyGrabbingLedge = false;
            StartCoroutine(HideGrabHitbox());
        }
    }

    IEnumerator HideGrabHitbox()
    {
        grabPoint.enabled = false;
        yield return new WaitForSeconds(0.5f);
        grabPoint.enabled = true;
    }

    private IEnumerator ClimbLedge()
    {
        isClimbing = true;
        animator.SetBool("isGrabbingLedge", false);
        animator.SetTrigger("startClimbing");

        // Capture starting position
        Vector3 startPosition = transform.position;
        Vector3 endPosition = ledgeTarget.position;

        // Break the movement into two phases: upward and forward
        float upHeight = ledgeTarget.position.y - transform.position.y; // Vertical movement
        float forwardDistance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(ledgeTarget.position.x, 0, ledgeTarget.position.z)
        );

        // Phase 1: Move up
        float elapsedTime = 0;
        while (elapsedTime < upHeight / climbUpSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (upHeight / climbUpSpeed);
            float smoothT = climbCurve != null ? climbCurve.Evaluate(t) : t;
            transform.position = Vector3.Lerp(
                startPosition,
                new Vector3(startPosition.x, ledgeTarget.position.y, startPosition.z),
                smoothT
            );
            yield return null;
        }

        // Phase 2: Move forward
        elapsedTime = 0;
        while (elapsedTime < forwardDistance / climbForwardSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (forwardDistance / climbForwardSpeed);
            float smoothT = climbCurve != null ? climbCurve.Evaluate(t) : t;
            transform.position = Vector3.Lerp(
                new Vector3(startPosition.x, ledgeTarget.position.y, startPosition.z),
                endPosition,
                smoothT
            );
            yield return null;
        }

        // Final position correction (ensure snapping to target)
        transform.position = endPosition;
        UnconstrainPlayer();
        isGrabbingLedge = false;
        isClimbing = false;
        actionable = true;
        rb.useGravity = true;
        animator.SetTrigger("finishedClimbing");
        canBeOnWall = true;
        currentlyGrabbingLedge = false;
    }

    public void killPlayer()
    {
        if (deathCam)
        {
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 2f;
            StartCoroutine(deathCamWait());
        }
        else
        {
            isDead = true;
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void GetRandomAnimationSpeed()
    {
        //Random Animation
        animator.SetFloat("randomSpeed", UnityEngine.Random.Range(0.1f, 0.5f));
    }

    IEnumerator deathCamWait()
    {
        yield return new WaitForSeconds(0.1f);
        isDead = true;
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void ConstrainPlayer()
    {
        rb.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
    }

    private void UnconstrainPlayer()
    {
        rb.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY);
    }

    private void OnDrawGizmosSelected()
    {
        //Wall Jumping Detection
        Debug.DrawRay(transform.position + isOnWallOffset, Vector3.right * 0.4f, Color.yellow);
        Debug.DrawRay(transform.position + isOnWallOffset2, Vector3.right * 0.4f, Color.yellow);

        Debug.DrawRay(transform.position + rightGroundOffset, Vector3.down * 0.025f, Color.red);
        Debug.DrawRay(transform.position + leftGroundOffset, Vector3.down * 0.025f, Color.red);
        Debug.DrawRay(transform.position, Vector3.down * 0.025f, Color.red);

        Debug.DrawRay(transform.position + ledgeForwardCheckOffset, Vector3.down * 0.25f, Color.cyan);
        Debug.DrawRay(transform.position + ledgeBackwardCheckOffset, Vector3.down * 0.25f, Color.cyan);
    }

    private void DebugBeatLevel()
    {
        if (beatLevel)
        {
            transform.position = levelFinishPos.transform.position;
            beatLevel = false;
        }
    }

}
