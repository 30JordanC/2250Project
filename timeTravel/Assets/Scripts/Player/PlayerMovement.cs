using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float sprintSpeed;
    public KeyCode sprintKey = KeyCode.LeftShift;
    private float currentSpeed;
    public float walkSpeed;
    public float jumpForce;

    public Transform orientation;

    private float horizontalInput;

    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    public float groundCheckRadius;
    public LayerMask whatIsGround;

    private Stamina stamina;

    private bool grounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        stamina = GetComponent<Stamina>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(transform.position, groundCheckRadius, whatIsGround);
        
        TakeInput();
        
        bool isMoving = horizontalInput !=0 || verticalInput != 0;
        if (Input.GetKey(sprintKey) && grounded && isMoving && stamina.HasStamina())
        {
            currentSpeed = sprintSpeed;
            stamina.UseStamina(stamina.sprintStaminaDrain * Time.deltaTime);
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded && stamina.currentStamina>=stamina.jumpStaminaDrain)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void TakeInput()
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
    }
}
