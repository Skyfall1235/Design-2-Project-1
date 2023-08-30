using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    public InteractableType InteractableType { get; }

    /// <summary>
    /// Interacts with this object.
    /// </summary>
    void Interact(ShipManager manager) { }

    /// <summary>
    /// Interacts with this object from the manager's perspective.
    /// </summary>
    /// <param name="dataPacket">The data packet from the manager to the objective.</param>
    void Interact(ShipManager manager, ManagerToObjectivePacket dataPacket) { }

}

public static class GlobalMethods
{
    public static void PlaySoundAtLocation(SoundType type, string soundName, int indexLocation, AudioSource source, float rawVolume)
    {
        SoundManager soundManager = FindSoundManager();
        if (soundManager != null)
        {
            // Call the PlaySoundAtLocation method on the SoundManager component
            soundManager.PlaySoundAtLocation(type, soundName, indexLocation, source, rawVolume);
        }
        else
        {
            Debug.LogWarning("Could not play sound. SoundManager not found.");
        }

    }
    #region Finding Relevent Objects
    private static SoundManager FindSoundManager()
    {
        // Find the GameManager object in the scene
        GameObject gameManager = GameObject.Find("GameManager");

        if (gameManager != null)
        {
            // Get the SoundManager component attached to the GameManager object
            try
            {
                return gameManager.GetComponent<SoundManager>();
            }
            catch (Exception ex)
            {
                //i think i nedd to use a try/catch here?
                Debug.LogError("Could not find SoundManager component on GameManager." + ex.Message + " StackTrace: " + ex.StackTrace);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Could not find GameManager object in scene.");
            return null;
        }
    }

    #endregion
}

#region Structs
/// <summary>
/// A packet struct for sending data from objectives to the manager.
/// </summary>
[System.Serializable]
public struct ObjectiveToManagerPacket
{
    /// <summary>
    /// Indicates if the packet should trigger a drill event.
    /// </summary>
    public bool triggerDrillEvent;

    /// <summary>
    /// The type of event trigger.
    /// </summary>
    public EventTriggerType triggerType;

    /// <summary>
    /// Text to display on the console.
    /// </summary>
    public string consoleText;

    /// <summary>
    /// AudioClip associated with the packet.
    /// </summary>
    public AudioClip audioClip;

    /// <summary>
    /// Indicates if the packet should cause rumble.
    /// </summary>
    public bool rumble;
}

/// <summary>
/// A packet struct for sending data from the manager to objectives.
/// </summary>
[System.Serializable]
public struct ManagerToObjectivePacket
{
    /// <summary>
    /// Indicates if the packet should trigger an objective event.
    /// </summary>
    public bool triggerObjectiveEvent;
}

/// <summary>
/// A struct to hold console message data.
/// </summary>
[System.Serializable]
public struct ConsoleMessage
{
    /// <summary>
    /// Panel to display warning.
    /// </summary>
    public GameObject warningPanel;

    /// <summary>
    /// Warning message to display.
    /// </summary>
    public string warningMessage;

    /// <summary>
    /// Indicates if the message is active.
    /// </summary>
    public bool isActive;
}

/// <summary>
/// A struct to hold collections of sound files.
/// </summary>
[System.Serializable]
public struct Sounds
{
    /// <summary>
    /// Name of the collection for reference.
    /// </summary>
    [SerializeField] public string collectionName;

    /// <summary>
    /// List of sound clips in the collection.
    /// </summary>
    [SerializeField] public List<AudioClip> soundFile;
}

/// <summary>
/// Represents a task that needs to be completed for an objective.
/// </summary>
[System.Serializable]
public struct ObjectiveTask
{
    /// <summary>
    /// The name of the object that the objective is linked to.
    /// </summary>
    public string objectiveObjectName;
    /// <summary>
    /// Types of tasks that can be assigned to an objective.
    /// </summary>
    public enum TaskType
    {
        GoToLocation,   // Task to reach a specific location.
        Interact,       // Task to interact with an object.
        Drill           // Task to perform drilling action.
                        // Add more task types as needed
    }

    /// <summary>
    /// The type of task.
    /// </summary>
    public TaskType type;

    /// <summary>
    /// The interactable object for the task (used for Interact tasks).
    /// </summary>
    public GameObject interactableObject;

    /// <summary>
    /// The target location for the task (used for GoToLocation tasks).
    /// </summary>
    public Vector3 targetLocation;

    /// <summary>
    /// Indicates whether the task has been completed.
    /// </summary>
    public bool taskCompleted;
}

/// <summary>
/// Represents an objective in the game.
/// </summary>
[System.Serializable]
public struct Objective
{
    /// <summary>
    /// The description of the objective.
    /// </summary>
    public string objectiveDescription;

    /// <summary>
    /// Indicates whether the objective has been completed.
    /// </summary>
    public bool objectiveCompleted;

    /// <summary>
    /// List of tasks associated with the objective.
    /// </summary>
    public List<ObjectiveTask> objectiveTasks;
}


#endregion


#region Enums
/// <summary>
/// Types of events that can be triggered.
/// </summary>
[System.Serializable]
public enum EventTriggerType
{
    EngineMalfunction,
    ReactorMalfunction,
    Biological,
    ConsoleWarning,
    Rumble,
    SoundTrigger,
    ConsoleAndLights,
    All,
    None
}

/// <summary>
/// Types of interactable objects.
/// </summary>
[System.Serializable]
public enum InteractableType
{
    Console,
    InfoPoint,
    Ship,
    Player,
    Button,
    Rock,
    Ore,
    DataBeacon,
    Harmful
}
/// <summary>
/// Types of sound files.
/// </summary>
public enum SoundType
{
    SoundEffect,
    Music
}
#endregion




