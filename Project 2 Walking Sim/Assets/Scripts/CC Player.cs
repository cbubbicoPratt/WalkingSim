using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CCPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5;
    public float runSpeed = 9;
    public float jumpHeight = 5;

    public Transform cameraTransform;
    public float lookSensitivity = 1;

    private CharacterController cc;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private float verticalVelocity; //current upward/downward speed
    private float gravity = -9.81f; //constant downward acceleration
    private float pitch; //up and down

    //interaction variables
    private GameObject currentTarget;
    public Image reticleImage;

    private bool interactPressed;
    private bool isRunning;
    private bool isJumping;
    void Awake()
    {
        cc = GetComponent<CharacterController>();

        //optional cursor locking
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //find the reticle
        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(0, 0, 0, 0.7f); //slightly transparent black
    }

    
    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        //horizontal mouse movement rotates player
        float yaw = lookInput.x * lookSensitivity;
        //vertical mouse movement rotates camera
        float pitchDelta = lookInput.y * lookSensitivity;

        transform.Rotate(Vector3.up * yaw);

        //accumulate vertical rotation
        pitch -= pitchDelta;
        //clamp so we don't flip upside down
        pitch = Mathf.Clamp(pitch, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    private void HandleMovement()
    {
        //updating our bool to be true or false if the player is grounded
        bool grounded = cc.isGrounded;
        Debug.Log("Is Grounded: " + grounded);

        //this keeps character controller snapped to ground
        if (grounded && verticalVelocity <= 0)
        {
            verticalVelocity = -2;
        }

        float currentSpeed = walkSpeed;
        
        //if running is true set the current speed to run speed
        if (isRunning)
        {
            currentSpeed = runSpeed;
        } 
        else //if false, set back to walk speed
        {
            currentSpeed = walkSpeed;
        }

        Vector3 move = transform.right * moveInput.x * currentSpeed + transform.forward * moveInput.y * currentSpeed;
        
        //if jumping is true and we are grounded
        if(isJumping && grounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            isJumping = false;
        }

        //apply gravity to every frame
        verticalVelocity += gravity * Time.deltaTime;
        //convert vertical velocity into movement vector
        Vector3 velocity = Vector3.up * verticalVelocity;
        //NOW we are finally moving player
        cc.Move(move + velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //when the key is hit, isJumping = true
        if (context.performed) isJumping = true;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed) interactPressed = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("CC collided with: " + hit.gameObject.name);
    }
}
