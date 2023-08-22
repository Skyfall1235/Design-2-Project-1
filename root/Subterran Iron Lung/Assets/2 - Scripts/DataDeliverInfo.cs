using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{

    /// <summary>
    /// Interacts with this object.
    /// </summary>
    public void Interact(ShipManager manager);

    /// <summary>
    /// Interacts with this object from the manager's perspective.
    /// </summary>
    /// <param name="dataPacket">The data packet from the manager to the objective.</param>
    public void Interact(ManagerToObjectivePacket dataPacket, ShipManager manager)
    {

    }

}


public struct ObjectiveToManagerPacket
{
    //needs to communicate that is is where it it, what is is, and any other math data
    public bool triggerDrillEvent;
    public EventTriggerType triggerType;

    public string consoleText;
    public AudioClip audioClip;
    public bool rumble;

}

public struct ManagerToObjectivePacket
{
    //needs to trigger something on the objective side to either be mines or something else
    public bool triggerObjectiveEvent;

}


public enum EventTriggerType
{ 
    ConsoleWarning,
    Rumble,
    SoundTrigger,
    All
}





