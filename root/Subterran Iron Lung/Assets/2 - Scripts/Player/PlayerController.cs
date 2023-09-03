using System.Collections;
using System.Collections.Generic;
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
    /// 
    /// </summary>
    private KeyCode m_interactKey;

    /// <summary>
    /// reference to the ships manager script
    /// </summary>
    
    private ShipManager m_shipManager;


    public bool useControls;

    #endregion
    
    private void Start()
    {
        StartAssembly();
    }

    private void Update()
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
        if(useControls)
        {
            HandleKeyboardInput();

            HandleMouseInput();

            HandleInteract();
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

        if(m_characterController.enabled)
        {
            m_characterController.Move(move * m_moveSpeed * Time.deltaTime);
            // Jumping
            if (m_isGrounded && Input.GetButtonDown("Jump"))
            {
                m_playerVelocity.y = Mathf.Sqrt(m_jumpHeight * -2.0f * m_gravity);
            }
        }
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
        m_cameraTransform.Rotate(Vector3.left * mouseY);
    }


    void HandleInteract()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
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
