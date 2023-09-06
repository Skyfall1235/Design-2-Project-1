using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : BaseInteractactable
{
    [SerializeField] bool m_hasEngineProblem = false;

    [SerializeField] AudioSource m_audioSource;
    // steam animation

    private bool m_isHoldingKey = false;
    [SerializeField] private float m_holdDuration = 2.0f; // Adjust this to your desired hold time in seconds

    public override void Interact()
    {
        base.Interact();
        m_isHoldingKey = false; // Reset the key hold flag when interacting
    }

     void Update()
     { 

        if (Input.GetKey(m_playerController.GetComponent<PlayerController>().InteractKey) && m_hasEngineProblem) // Change to the key you want to hold down
        {
            if (!m_isHoldingKey)
            {
                StartCoroutine(HoldKeyCoroutine());
            }
        }
        else
        {
            m_isHoldingKey = false; // Key is released
        }
     }

    IEnumerator HoldKeyCoroutine()
    {
        m_isHoldingKey = true;

        float elapsedTime = 0f;

        while (elapsedTime < m_holdDuration)
        {
            // While the key is held down, do something (e.g., a method call)
            // Replace the following line with your method call or action
            Debug.Log("Holding the key...");

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Key has been held for the specified duration, complete the action
        StopEvent();
    }


    //timer to end game can be on the manager
    protected override void StopEvent()
    {
        GlobalMethods.PlaySoundAtLocation(SoundType.SoundEffect, "Event Specific", 0, m_audioSource, 0.5f);
        m_shipManager.CancelCountdown();
    }
}
