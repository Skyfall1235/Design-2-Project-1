using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beacon : BaseInteractactable
{
    [SerializeField] private float m_countdownTime = 5.0f; // Adjust this as needed
    Coroutine m_drillAction;
    bool hasBeenUsed = false;

    private void Start()
    {
        m_shipManager = FindShipManagerInPlayerScene();
    }

    public override void Interact()
    {
        //if i can interact with it, im close enough. also, only a drill can interact with this anyway.
        Debug.Log("Beacon collided with drill, calling coroutine");
        m_drillAction = StartCoroutine(m_shipManager.DrillActionCountdown(m_countdownTime, InteractableType));
        hasBeenUsed = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Drill") && m_drillAction != null) // Replace "YourObjectTag" with the tag of your object
        {
            StopCoroutine(m_shipManager.DrillActionCountdown(m_countdownTime, InteractableType));
            hasBeenUsed = false;
        }
    }


}
