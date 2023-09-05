using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseInteractactable : MonoBehaviour, IInteractable
{
    //exists to internally tag items for interaction
    /// <summary>
    /// The reference to the drill ship's movement script
    /// </summary>
    [SerializeField] protected ShipMovement m_drillMovement;

    /// <summary>
    /// Reference to the players movement and interaction script
    /// </summary>
    [SerializeField] protected GameObject m_playerController;

    /// <summary>
    /// Reference to the drills ship manager
    /// </summary>
    [SerializeField] protected ShipManager m_shipManager;

    /// <summary>
    /// the type of interactable type that this object is associated with
    /// </summary>
    [SerializeField] private InteractableType m_interactableType;
    public InteractableType InteractableType
    {
        get { return m_interactableType; }
        set { m_interactableType = value; }
    }




    void IInteractable.Interact(ShipManager manager)
    {
        Interact();
    }

    /// <summary>
    /// Interaction code written to handle the interaction between the ship and its objectives outside of the players visible space.
    /// </summary>
    /// <param name="manager">The reference to the ships manager</param>
    /// <param name="dataPacket">The packet of data to pass to an objective to determine its specific behavior</param>
    void IInteractable.Interact(ShipManager manager, ManagerToObjectivePacket dataPacket)
    {
        return;
    }

    public virtual void Interact()
    {

    }
    protected virtual void StopEvent()
    {

    }

}
