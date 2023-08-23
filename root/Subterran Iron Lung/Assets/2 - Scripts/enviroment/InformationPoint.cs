using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InformationPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractableType interactableType;
    public InteractableType InteractableType
    {
        get { return interactableType; }
        set { interactableType = value; }
    }


    Collider m_detectionCollider;
    bool switchMesh = false;




    void NotifyManagerOfLocation()
    {

    }

    void PerformInteraction()
    {

    }

    void IInteractable.Interact(ShipManager manager)
    {

    }
    /// <summary>
    /// Interacts with this object from the manager's perspective.
    /// </summary>
    /// <param name="dataPacket">The data packet from the manager to the objective.</param>
    void IInteractable.Interact(ManagerToObjectivePacket dataPacket, ShipManager manager)
    {
        //datapacket tells the object to update certain things like its mesh
        
    }





}
