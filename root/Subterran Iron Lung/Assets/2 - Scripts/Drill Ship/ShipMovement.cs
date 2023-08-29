using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class ShipMovement : MonoBehaviour
{
    private float currentElevation = 0;
    [SerializeField] float drillMovementSpeed = 1;
    [SerializeField] float drillRotationSpeed = 1;
    [SerializeField] float drillTurnSpeed = 1;
    [SerializeField] private CharacterController characterController;
    [SerializeField]
    public GameObject groundPad;
    [SerializeField] private bool leveledOut = false;
    [SerializeField] private bool Moving = false;
    public bool PlayerIsSeated = false;


    private void Update()
    {
        if(PlayerIsSeated)
        {
            MoveDrill();
        }

        if (!leveledOut && !Moving)
        {
            ConfirmLevelOut();
        }
    }



    //negatives to turn left, positives to turn right
    void MoveDrill()
    {
        bool turning = false;
        //handle the movement going forward
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 forwardDirection = transform.forward;
            characterController.Move(forwardDirection * drillMovementSpeed);
            Moving = true;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            Vector3 backwardDirection = -transform.forward;
            characterController.Move(backwardDirection * drillMovementSpeed);
            Moving = true;
        }
        else
        {
            Moving = false;
            turning = false;
        }

        //lerp the rotation toward the angle and the rotation speed
        if (Moving)
        {
            turning = true;
            float rotationInput = Input.GetAxis("Horizontal"); // Input for rotation left/right

            // Calculate yaw rotation based on rotation input
            float yawRotation = rotationInput * drillTurnSpeed * Time.deltaTime;

            // Apply yaw rotation
            transform.Rotate(Vector3.up * yawRotation);
        }
        if (turning)
        {
            RotateUpAndDown();
        }
        else ConfirmLevelOut();

        
    }

    private void RotateUpAndDown()
    {
        float rotationAmount = 0.0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationAmount = -drillRotationSpeed;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationAmount = drillRotationSpeed;
        }

        Vector3 rotation = new Vector3(rotationAmount * Time.deltaTime, 0, 0);
        transform.Rotate(rotation);
    }

    private void ConfirmLevelOut()
    {
        //Debug.LogWarning(CheckIfObjectIsLevel());
        if (!CheckIfObjectIsLevel())
        {
            leveledOut = false;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundPad.transform.up) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, drillRotationSpeed * Time.deltaTime);
        }
        else
        {
            leveledOut = true;
        }
    }

    private bool CheckIfObjectIsLevel()
    {
        if (groundPad == null)
        {
            Debug.LogWarning("References not set!");
            return false;
        }

        Vector3 groundNormal = groundPad.transform.up;
        Vector3 objectUp = transform.up;

        float angle = Vector3.Angle(objectUp, groundNormal);

        return angle <= 2;
    }

    
}
