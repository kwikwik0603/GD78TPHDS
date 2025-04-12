using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]

    //movement speed
    private float moveSpeed;
    
    //walkign speed 
    public float walkSpeed;
    
    //sprinting speed
    public float sprintSpeed;

    //orientation transform
    public Transform orientation;

    //friction drag when on ground
    public float groundDrag;

    [Header("Ground Check")]

    //player height variable
    public float playerheight;

    //layer mask to identify ground layer
    public LayerMask whatIsGround;

    //grounded boolean
    bool grounded;

    [Header("Jumping")]

    //force for jumping
    public float jumpForce;

    //cooldown for jump
    public float jumpCooldown;

    //air control multiplier
    public float airMultiplier;

    //ready to jump boolean
    bool readyToJump;

    [Header("Keybinds")]

    //key bind for jumping
    public KeyCode jumpKey = KeyCode.Space;
    
    //key bind for sprinting
    public KeyCode sprintKey = KeyCode.LeftShift;

    //KB input
    float horInput;
    float verInput;

    //movement direction
    Vector3 moveDirection;

    //rigibody
    Rigidbody rb;
    
    //current state of player
    public MovementState state;

    //Enum to check for movement state
    public enum MovementState{
        walking, 
        sprinting,
        air
    }

    void Start()
    {
        //getting rigidbody component and freezing rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

    }

    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerheight * 0.5f + 0.2f, whatIsGround);

        //calling MyInput function
        MyInput();

        //calling SpeedControl function
        SpeedControl();

        //calling Statehandler function
        StateHandler();

        //friction drag
        if(grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
    
    private void FixedUpdate()
    {
        //calling MovePlayer function
        MovePlayer();
    }

    private void MyInput() 
    {
        //getting user input for horizontal and vertical axis
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");

        //checking for jump input from user
        if(Input.GetKey(jumpKey) && readyToJump && grounded) {
            
            //set ready to jump to false
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer() 
    {
        //calculating and setting move direction
        moveDirection = orientation.forward *verInput + orientation.right * horInput;

        //adding air control if in air
        if(grounded)
            //adding force to the rigidbody of the character
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl() {
        //getting the gorund velocity of the character
        Vector3 flatvel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limiting velocity
        if(flatvel.magnitude > moveSpeed) {
            //making new velocity & assigning it to the rigidbody
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        //reset the y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //adding force to the character
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {

        //resets value for jump
        readyToJump = true;
    }

    private void StateHandler() {
        //checking state
        if(grounded && Input.GetKey(sprintKey)){
            //setting state to sprinting and changing to sprint speed
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(grounded) {
            //setting state to walking and changing to walk speed
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else{
            //setting state to air
            state = MovementState.air;
        }
    }


}

