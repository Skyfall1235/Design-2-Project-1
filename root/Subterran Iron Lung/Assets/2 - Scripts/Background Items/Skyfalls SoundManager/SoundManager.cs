using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement; // Temp Functionality

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the ScriptableObject containing audio files and settings data.
    /// </summary>
    [SerializeField]
    SO_AudioFiles audioFiles;

    /// <summary>
    /// Reference to the ScriptableObject containing game settings data.
    /// </summary>
    [SerializeField]
    SO_SettingData settingsData;

    /// <summary>
    /// Initializes the audio file dictionary during awake.
    /// </summary>
    private void Awake()
    {
        audioFiles.SetUpDictionary();
    }

    /// <summary>
    /// Plays a sound at a specified location.
    /// </summary>
    /// <param name="type">The type of sound (e.g., Music, SoundEffect).</param>
    /// <param name="soundName">The name of the sound to play.</param>
    /// <param name="indexLocation">The index of the sound at the location.</param>
    /// <param name="source">The AudioSource to play the sound.</param>
    /// <param name="rawVolume">The raw volume of the sound.</param>
    public void PlaySoundAtLocation(SoundType type, string soundCollectionName, int indexLocation, AudioSource source, float rawVolume)
    {
        float volume = TrueVolume(rawVolume, type);
        AudioClip chosenSound;

        if (audioFiles.sound.TryGetValue(soundCollectionName, out Sounds chosenStruct))
        {
            chosenSound = chosenStruct.soundFile[indexLocation];
            source.PlayOneShot(chosenSound, volume);
            Debug.Log($"Sound {soundCollectionName} played successfully");
        }
        else
        {
            Debug.LogWarning($"Sound {soundCollectionName} was not found in the 'sounds' dictionary.");
        }
    }

    /// <summary>
    /// Calculates the true volume based on the raw volume and sound type.
    /// </summary>
    /// <param name="raw">The raw volume value.</param>
    /// <param name="type">The type of sound (e.g., Music, SoundEffect).</param>
    /// <returns>The calculated true volume value.</returns>
    private float TrueVolume(float raw, SoundType type)
    {
        //blank volume value
        float volumeValue = 0;

        switch (type)
        {
            case SoundType.Music:
                Console.WriteLine("Playing music");
                volumeValue = raw * settingsData.musicVolume;
                break;
            case SoundType.SoundEffect:
                Console.WriteLine("Playing sound effect");
                volumeValue = raw * settingsData.soundEffectVolume;
                break;
        }

        return volumeValue;
    }

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
/// Types of sound files.
/// </summary>

public enum SoundType
{
    SoundEffect,
    Music
}