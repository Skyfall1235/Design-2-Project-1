using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractableType interactableType; // The type of interactable object.

    /// <summary>
    /// Gets or sets the type of this interactable object.
    /// </summary>
    public InteractableType InteractableType
    {
        get { return interactableType; }
        set { interactableType = value; }
    }

    /// <summary>
    /// The current event trigger type for the interactable object.
    /// </summary>
    public EventTriggerType currentEvent;

    //[SerializeField] private ConsoleMessage[] consoleMessages = new ConsoleMessage[0]; // Array of console messages associated with this interactable object.

    [SerializeField] CenterConsole console;

    [SerializeField] Light[] consoleLight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectChange();
    }

    //DEBUG
    private void DetectChange()
    {
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentEvent = EventTriggerType.EngineMalfunction;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentEvent = EventTriggerType.ReactorMalfunction;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentEvent = EventTriggerType.Biological;
            }

            // Call a method to handle the changed event
            console.UpdateMonitorThree();

        }
    }

    void IInteractable.Interact(ShipManager manager)
    {

    }

    void IInteractable.Interact(ShipManager manager, ManagerToObjectivePacket dataPacket)
    {
        //datapacket tells the object to update certain things like its mesh

    }
}
