using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chair : BaseInteractactable
{
    //exists to internally tag items for interaction
    [SerializeField] private GameObject playerSittingLocation;



    bool playerIsSitting = false;


    // Update is called once per frame
    void Update()
    {
        CheckIfExitChair();
    }

    void CheckIfExitChair()
    {
        if (playerIsSitting)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerIsSitting = false;
                m_playerController.GetComponent<CharacterController>().enabled = true;
                m_drillMovement.PlayerIsSeated = false;
            }

        }
    }
    public override void Interact()
    {
        base.Interact();
        //set the players tranform to equal the chair
        m_playerController.transform.position = playerSittingLocation.transform.position;
        m_playerController.GetComponent<CharacterController>().enabled = false;

        //turn on the player is seated bool
        playerIsSitting = true;

        //potentially limit the head turning ability

        //toggle on the movement for the ship
        m_drillMovement.PlayerIsSeated = true;
    }


}
