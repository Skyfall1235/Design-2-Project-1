using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : BaseInteractactable
{
    [SerializeField] public bool m_hasEngineProblem = false;

    [SerializeField] AudioSource m_audioSource;
    // steam animation

    public float interactDistance = 5f; // Adjust this to your desired distance.
    private bool isInRange = false;

    private bool m_isHoldingKey = false;
    [SerializeField] private float m_holdDuration = 0.5f; // Adjust this to your desired hold time in seconds

    public override void Interact()
    {
        base.Interact();
        m_isHoldingKey = false; // Reset the key hold flag when interacting
    }

    void Update()
    {
        // Check if the Render Texture's position is within the camera's view.
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);

        // Calculate the direction from the camera to this object.
        Vector3 directionToPlayer = Camera.main.transform.position - transform.position;

        // Cast a ray to check if the object is in line of sight and within the interact distance.
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, interactDistance))
        {
            // Check if the hit object is the player.
            if (hit.collider.CompareTag("Player"))
            {
                isInRange = true;

                if (Input.GetKey(m_playerController.GetComponent<PlayerController>().InteractKey) && m_hasEngineProblem)
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
            else
            {
                isInRange = false;
            }
        }
        else
        {
            isInRange = false;
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

        Debug.Log("calls the stop event");
        // Key has been held for the specified duration, complete the action
        m_hasEngineProblem = false;
        StopEvent();
    }


    //timer to end game can be on the manager
    protected override void StopEvent()
    {
        GlobalMethods.PlaySoundAtLocation(SoundType.SoundEffect, "Event Specific", 0, m_audioSource, 0.5f);
        m_shipManager.CancelCountdown();
    }
}
