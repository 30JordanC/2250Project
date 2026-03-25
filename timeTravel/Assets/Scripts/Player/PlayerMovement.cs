using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float sprintSpeed;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;
    private float currentSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float jumpForce;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    public Transform groundCheckPosition;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    private Stamina stamina;

    private bool grounded;
    private bool isCrouching;

    public Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        stamina = GetComponent<Stamina>();
        currentSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheckPosition.position, groundCheckRadius, whatIsGround);
        
        TakeDirectionalInput();
        
        bool isMoving = horizontalInput !=0 || verticalInput != 0;

        if (Input.GetKeyDown(crouchKey) && grounded)
        {
            isCrouching = !isCrouching;
        }
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            if (isCrouching)
            {
                isCrouching = false;
            }
            else if (stamina.currentStamina>=stamina.jumpStaminaDrain)
            {
                Jump();
            }
        }
        
        if (Input.GetKey(sprintKey) && grounded && isMoving && stamina.HasStamina())
        {
            if (isCrouching)
            {
                isCrouching = false;
            }
            currentSpeed = sprintSpeed;
            stamina.UseStamina(stamina.sprintStaminaDrain * Time.deltaTime);
        }
        else if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else 
        {
            currentSpeed = walkSpeed;
        }
        
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void TakeDirectionalInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward*verticalInput + orientation.right * horizontalInput;
        moveDirection = moveDirection.normalized;
        
        Vector3 moveVelocity = moveDirection * currentSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        stamina.UseStamina(stamina.jumpStaminaDrain);

        animator.SetTrigger("Jump");
    }

    void UpdateAnimator()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = flatVelocity.magnitude;

        bool isMoving = moveDirection.magnitude > 0.1f;
        bool isSprinting = Input.GetKey(sprintKey) && isMoving && !isCrouching && stamina.HasStamina();
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsGrounded", grounded);
    }
}
