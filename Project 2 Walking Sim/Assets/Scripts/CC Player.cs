using System;
using Unity.VisualScripting;
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
    private float gravity = -20; //constant downward acceleration
    private float pitch; //up and down

    //interaction variables
    private GameObject currentTarget;
    public Image reticleImage;

    private bool interactPressed;
    private Interactable currentInteractable;
    private DialogueInteractable currentDialogueInteractable;

    //variables for menu and manager object
    public bool menuOpened = false;
    private MenuManager manager;

    private bool isRunning;
    private bool isJumping;
    //this is our event that the other scripts will be listening for
    public static event Action<DialogueData> OnDialogueRequested;
    public static event Action<string> OnTrigger;
    void Awake()
    {
        cc = GetComponent<CharacterController>();

        //optional cursor locking
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //find menu manager object
        manager = GameObject.FindFirstObjectByType<MenuManager>();
        currentDialogueInteractable = FindFirstObjectByType<DialogueInteractable>();

        //find the reticle
        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(0, 0, 0, 0.7f); //slightly transparent black
    }

    
    void Update()
    {
        //we want our player to be frozen if the menu is open
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            HandleLook();
            HandleMovement();
            CheckInteract();
            HandleInteract();
        }
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
        //Debug.Log("Is Grounded: " + grounded);

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
        //if(isJumping && grounded)
        //{
        //    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}
        //else
        //{
        //    isJumping = false;
        //}

        //apply gravity to every frame
        verticalVelocity += gravity * Time.deltaTime;
        //convert vertical velocity into movement vector
        Vector3 velocity = Vector3.up * verticalVelocity;
        //NOW we are finally moving player
        cc.Move((move + velocity) * Time.deltaTime);
    }

    void CheckInteract()
    {
        //reset reticle image to normal color first
        if (reticleImage != null) reticleImage.color = new Color(0, 0, 0, .7f);
        currentInteractable = null;
        //make a ray that goes straight out of the camera(center of screen)
        //players eyesight
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        //RaycastHit hit;
        //asking unity if it hit something within 3 units
        //hit stores what we hit like the collider
        //bool didHit = Physics.Raycast(ray, out hit, 3);
        //if (!didHit) return;//if we didn't hit anything start here
        //if we hit something tagged interactable
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            currentInteractable = hit.collider.GetComponentInParent<Interactable>();
            if (currentInteractable != null && reticleImage != null && currentInteractable.BroadcastActive())
            {
                reticleImage.color = Color.red;
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
            else
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
        }

        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
    }

    void HandleInteract()
    {
        //if the player did not press interact this frame do nothing
        if (!interactPressed) return;
        //consume the input so one click only triggers one interactions
        //this changes next frame
        interactPressed = false;
        if (currentInteractable == null) return;
        currentInteractable.Interact(this);
        Debug.Log("Handle interact should be running");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    //public void OnJump(InputAction.CallbackContext context)
    //{
    //    //when the key is hit, isJumping = true
    //    //can only jump if the menu isn't open (prevents jumping after space is pressed in the menu and then exiting)
    //    if (context.performed && Cursor.lockState == CursorLockMode.Locked) isJumping = true;
    //}

    public void OnSprint(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //prevents interaction if menu is open
        if(context.performed && Cursor.lockState == CursorLockMode.Locked) interactPressed = true;
        Debug.Log("OnInteracted fired performed: " + context.performed);
    }

    public void OnMenuOpen(InputAction.CallbackContext context)
    {
        //toggle menu either on or off when 'q' is pressed
        //tooltip to show that the menu has new text
        if (context.performed)
        {
            menuOpened = !menuOpened;
            //changed this to a referencing a var instead of class
            //DialogueInteractable.ShowTooltip(false);
            
            currentDialogueInteractable.ShowTooltip(false);
            //Debug.Log("Menu opened");
        }
        //if the menu is open, we want to see and unlock our cursor
        Cursor.visible = menuOpened;
        if(menuOpened)
        {
            Cursor.lockState = CursorLockMode.None;
        } 
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        //sending message to actually show the menu
        manager.ToggleMenu(menuOpened);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("CC collided with: " + hit.gameObject.name);
    }

    public void RequestDialogue(DialogueData dialogueData)
    {
        OnDialogueRequested?.Invoke(dialogueData);
    }

    public void BroadcastTrigger(string trigger)
    {
        OnTrigger?.Invoke(trigger);
    }
}
