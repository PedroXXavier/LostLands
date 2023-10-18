using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Movement : MonoBehaviour
{
    Animator anim;
    PhotonView phView;

    [Header("ToolsAnimators")]
    public Animator pickaxe; public Animator shovel;
    public Animator compass; public Animator luneta;

    [SerializeField] bool onSloop;

    GameController gc;

    MovementState state;
    public enum MovementState {
        walking, crouching, air }

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField] bool readyToJump;

    bool bored;
    float boredTimer;

    public AudioSource WalkSFX;
    public AudioSource JumpSFX;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public GameObject hud;

    void Start()
    {
        gc = FindObjectOfType(typeof(GameController)) as GameController;

        anim = GetComponent<Animator>();
        phView = GetComponent<PhotonView>();    

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        if(!phView.IsMine)
            hud.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!phView.IsMine)
            return;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        switch (gc.states)
        {
            case States.Play:
                SpeedControl(); MyInput(); Breathing(); StateHandler(); Dedos();

                if (horizontalInput != 0 || verticalInput != 0)
                {
                    anim.SetBool("DedoMeio", false);
                    anim.SetBool("Joinha", false);

                    anim.SetBool("Walking", true);
                    pickaxe.SetBool("Walking", true);
                    luneta.SetBool("Walking", true);
                    compass.SetBool("Walking", true);
                    shovel.SetBool("Walking", true);

                    bored = false;
                    boredTimer = 0;
                }
                else
                {
                    anim.SetBool("Walking", false);
                    pickaxe.SetBool("Walking", false);
                    luneta.SetBool("Walking", false);
                    compass.SetBool("Walking", false);
                    shovel.SetBool("Walking", false);

                    if (!anim.GetBool("Joinha"))
                    {
                        boredTimer += Time.deltaTime;

                        if (boredTimer >= 6)
                        {
                            bored = true;
                        }
                    }
                }

                // handle drag
                if (grounded)
                    rb.drag = groundDrag;
                else
                    rb.drag = 0;
                break;
            case States.Pause:
                break;
        }
    }


    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = 7;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void FixedUpdate()
    {
        switch (gc.states)
        {
            case States.Play:
                MovePlayer();
                break;
            case States.Pause:
                break;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Breathing()
    {
        if (bored)
        {
            anim.SetBool("Bored", true);
        }
        else
            anim.SetBool("Bored", false);
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        bored = false;
        boredTimer = 0;

        anim.SetBool("Joinha", false);
        anim.SetBool("DedoMeio", false);

        anim.SetTrigger("Jump");
        phView.RPC("JumpSound", RpcTarget.AllBuffered);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    [PunRPC]
    private void JumpSound()
    {
        JumpSFX.Play();
    }

    private void Dedos()
    {
        if (Input.GetButtonDown("K"))
        {
            if(!anim.GetBool("DedoMeio"))
            {
                anim.SetBool("DedoMeio", true);
                anim.SetBool("Joinha", false);
            }
            else
                anim.SetBool("DedoMeio", false);
        }

        else if (Input.GetButtonDown("J"))
        {
            if (!anim.GetBool("Joinha"))
            {
                anim.SetBool("Joinha", true);
                anim.SetBool("DedoMeio", false);
            }
            else
                anim.SetBool("Joinha", false);
        }
    }
}
