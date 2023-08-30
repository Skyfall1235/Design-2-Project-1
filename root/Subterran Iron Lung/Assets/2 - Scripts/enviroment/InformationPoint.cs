using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
//fun fact, INHERIT FROM THIS CLASS.
public class InformationPoint : MonoBehaviour, IInteractable
{
    [SerializeField] protected InteractableType interactableType;
    public InteractableType InteractableType
    {
        get { return interactableType; }
        set { interactableType = value; }
    }


    protected Collider m_detectionCollider;
    protected bool switchMesh = false;

    void IInteractable.Interact(ShipManager manager)
    {
        //basic interaction that doesnt require a confirmation
        PerformInteraction(manager);
    }
    /// <summary>
    /// Interacts with this object from the manager's perspective.
    /// </summary>
    /// <param name="dataPacket">The data packet from the manager to the objective.</param>
    void IInteractable.Interact(ShipManager manager, ManagerToObjectivePacket dataPacket)
    {
        //only varaible is dataPacket.triggerObjectiveEvent
        if (dataPacket.triggerObjectiveEvent)
        {
            PerformInteraction(manager);
        }
    }



    #region interactions available to object
    protected virtual void NotifyManagerOfLocation(ShipManager manager)
    {
        //if needed to notify the mamnage, like pass some text, that can be done here
    }



    protected virtual void PerformInteraction(ShipManager manager)
    {
        //since this beacon is just giving infomation, we just need to update the task
    }
    #endregion 






}
