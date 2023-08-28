using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chair : MonoBehaviour, IInteractable
{
    //exists to internally tag items for interaction

    [SerializeField] private ShipMovement drillMovement;
    [SerializeField] private GameObject playerController;
    [SerializeField] private ShipManager shipManager;
    [SerializeField] private GameObject playerSittingLocation;

    [SerializeField] private InteractableType interactableType;
    public InteractableType InteractableType
    {
        get { return interactableType; }
        set { interactableType = value; }
    }


    bool playerIsSitting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                playerController.GetComponent<CharacterController>().enabled = true;
                drillMovement.PlayerIsSeated = false;
            }

        }
    }

    void IInteractable.Interact(ShipManager manager)
    {

        //set the players tranform to equal the chair
        playerController.transform.position = playerSittingLocation.transform.position;
        playerController.GetComponent<CharacterController>().enabled = false;

        //turn on the player is seated bool
        playerIsSitting = true;

        //potentially limit the head turning ability

        //toggle on the movement for the ship
        drillMovement.PlayerIsSeated = true;

    }
    void IInteractable.Interact(ShipManager manager, ManagerToObjectivePacket dataPacket)
    {
        return;
    }


}
