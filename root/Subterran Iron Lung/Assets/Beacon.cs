using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beacon : BaseInteractactable
{
    [SerializeField] private float m_countdownTime = 5.0f; // Adjust this as needed
    Coroutine m_drillAction;
    bool hasBeenUsed = false;


    public ShipManager FindShipManagerInPlayerScene()
    {
        // Loop through all active scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            // Check if the scene is named "Player"
            if (scene.name == "Player")
            {
                // Find the ShipManager component in the "Player" scene
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    ShipManager shipManager = obj.GetComponent<ShipManager>();
                    if (shipManager != null)
                    {
                        // ShipManager found, return it
                        return shipManager;
                    }
                }
            }
        }

        // If no "Player" scene or ShipManager is found, return null
        return null;
    }

    private void Start()
    {
        m_shipManager = FindShipManagerInPlayerScene();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drill") && !hasBeenUsed) // Replace "YourObjectTag" with the tag of your object
        {
            Debug.Log("Beacon collided with drill, calling coroutine");
            m_drillAction = StartCoroutine(m_shipManager.DrillActionCountdown(m_countdownTime, InteractableType));
            hasBeenUsed = true;
        }
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
