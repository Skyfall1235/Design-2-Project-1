using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region member variables

    /// <summary>
    /// The speed at which the player moves.
    /// </summary>
    [SerializeField] private float m_moveSpeed = 5.0f;

    /// <summary>
    /// The gravitational acceleration applied to the player.
    /// </summary>
    [SerializeField] private float m_gravity = -9.81f;

    /// <summary>
    /// The height the player can jump.
    /// </summary>
    [SerializeField] private float m_jumpHeight = 2.0f;

    /// <summary>
    /// The character controller component attached to the player.
    /// </summary>
    private CharacterController m_characterController;

    /// <summary>
    /// The current velocity of the player in world space.
    /// </summary>
    private Vector3 m_playerVelocity;

    /// <summary>
    /// Indicates whether the player is currently grounded.
    /// </summary>
    private bool m_isGrounded;

    /// <summary>
    /// The transform of the main camera used for player orientation.
    /// </summary>
    private Transform m_cameraTransform;

    /// <summary>
    /// the selected key for interaction
    /// </summary>
    [SerializeField]
    private KeyCode m_interactKey;

    public KeyCode InteractKey
    {
        get { return m_interactKey; }
    }


    [SerializeField] private AudioSource m_audioSource;

    [SerializeField] private TextMeshProUGUI playerInteractionPrompt;
    [SerializeField] float m_maxRaycastRange;

    /// <summary>
    /// reference to the ships manager script
    /// </summary>
    [SerializeField] private ShipManager m_shipManager;



    public bool useControls;

    [SerializeField] GameObject pauseMenuPanel;

    #endregion

    private void Start()
    {
        StartAssembly();

    }

    private void Update()
    {

        if (m_characterController.enabled)
        {
            // Check if the player is on the ground
            m_isGrounded = m_characterController.isGrounded;
            // Applying gravity
            if (m_isGrounded && m_playerVelocity.y < 0)
            {
                m_playerVelocity.y = -2.0f; // A small value to ensure the player sticks to the ground
            }

            m_playerVelocity.y += m_gravity * Time.deltaTime;
            m_characterController.Move(m_playerVelocity * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        AssembleControls();

    }


    /// <summary>
    /// Initializes the assembly by setting up the character controller and camera transform.
    /// </summary>
    void StartAssembly()
    {
        m_characterController = GetComponent<CharacterController>();
        m_cameraTransform = Camera.main.transform;
    }

    /// <summary>
    /// Manages player controls, including movement, jumping, and camera rotation.
    /// </summary>
    void AssembleControls()
    {
        if (useControls)
        {
            Cursor.lockState = CursorLockMode.Locked;
            HandleKeyboardInput();

            HandleMouseInput();

            PromptUIForInteraction();
        }

    }

    /// <summary>
    /// Handles player movement and jumping based on keyboard input.
    /// </summary>
    void HandleKeyboardInput()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        //Debug.Log(m_characterController.velocity);
        //Debug.Log(m_characterController.velocity.magnitude);

        if (m_characterController.enabled)
        {
            m_characterController.Move(move * m_moveSpeed * Time.deltaTime);
            // Jumping
            if (m_isGrounded && Input.GetButtonDown("Jump"))
            {
                m_playerVelocity.y = Mathf.Sqrt(m_jumpHeight * -2.0f * m_gravity);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //set the controller to off and toggle on the pause menu
            pauseMenuPanel.SetActive(true);
            useControls = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    public void ClosePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        useControls = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMenu()
    {
        Debug.Log(m_shipManager.m_loader);
        m_shipManager.m_loader.LoadSceneWithFade(m_shipManager.m_loader.sceneNames[1].sceneName, false);
    }



    /// <summary>
    /// Handles camera rotation based on mouse input.
    /// </summary>
    void HandleMouseInput()
    {
        // Rotate the player based on camera rotation
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera based on mouse input (looking up and down)
        float mouseY = Input.GetAxis("Mouse Y");

        // Define the clamping angles to prevent flipping
        float minVerticalRotation = -80f;
        float maxVerticalRotation = 80f;

        // Clamp mouseY to the specified range
        mouseY = Mathf.Clamp(mouseY, minVerticalRotation, maxVerticalRotation);

        // Rotate the camera using the clamped mouseY value
        m_cameraTransform.localRotation *= Quaternion.Euler(Vector3.left * mouseY);
    }









    void HandleInteract()
    {
        if(Input.GetKeyDown(m_interactKey))
        {
            Interact();
        }
    }

    void PromptUIForInteraction()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo))
        {
            // Check if the hit object implements the IInteractable interface
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // Prompt the UI to interact with the object
                playerInteractionPrompt.text = $"Press {m_interactKey} to interact with {hitInfo.collider.gameObject.name}";

                // Check if the distance to the hit object is within a maximum distance
                float maxDistance = 10f; // Adjust this value as needed
                if (hitInfo.distance <= maxDistance)
                {
                    // Perform interactions here, e.g., call a method on the interactable object
                    HandleInteract();
                }
            }
            else
            {
                // No interactable object found
                playerInteractionPrompt.text = "";
            }
        }
        else
        {
            // No object hit by the raycast
            playerInteractionPrompt.text = "";
        }
    }


    void Interact()
    {
        //shoot raycoast out, and retrieve thier interaction data
        //stuff like levers, going through doors, and buttons

        // Perform raycast from the camera's position along its forward direction
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo))
        {
            // Check if the hit object implements the IInteractable interface

            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null) 
            {
                Debug.Log(hitInfo.collider.gameObject);
                // If the interface is implemented, call the Interact method
                if (interactable.InteractableType == InteractableType.Console)
                {
                    interactable.Interact(m_shipManager);
                }
                else if (interactable.InteractableType == InteractableType.Button)
                {

                }
                else
                {
                    Debug.Log("attempts to run interaction Code");
                    interactable.Interact(m_shipManager);
                }
            }
        }
    }


}
