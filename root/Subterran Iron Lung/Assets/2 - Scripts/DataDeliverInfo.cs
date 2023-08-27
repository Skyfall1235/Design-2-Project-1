using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    public InteractableType InteractableType { get; set; }

    /// <summary>
    /// Interacts with this object.
    /// </summary>
    public void Interact(ShipManager manager);

    /// <summary>
    /// Interacts with this object from the manager's perspective.
    /// </summary>
    /// <param name="dataPacket">The data packet from the manager to the objective.</param>
    public void Interact(ManagerToObjectivePacket dataPacket, ShipManager manager);

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


[System.Serializable]
public struct ObjectiveToManagerPacket
{
    //needs to communicate that is is where it it, what is is, and any other math data
    public bool triggerDrillEvent;
    public EventTriggerType triggerType;

    public string consoleText;
    public AudioClip audioClip;
    public bool rumble;

}

[System.Serializable]
public struct ManagerToObjectivePacket
{
    //needs to trigger something on the objective side to either be mines or something else
    public bool triggerObjectiveEvent;

}

[System.Serializable]
public enum EventTriggerType
{ 
    ConsoleWarning,
    Rumble,
    SoundTrigger,
    EngineMalfunction,
    ConsoleAndLights,
    All
}

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

[System.Serializable]
public struct Sounds
{
    //collection name is for referring in the inspector. please use the soundName string when required
    [SerializeField] public string collectionName;
    //soudfile should be the onl;y accessible part
    [SerializeField] public List<AudioClip> soundFile;
}
public enum SoundType
{
    SoundEffect,
    Music
}




