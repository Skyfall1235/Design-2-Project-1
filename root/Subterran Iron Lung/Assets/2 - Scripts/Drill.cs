using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        // Cast a ray from the drill's tip forward
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object is the underwater terrain
            if (hit.collider.CompareTag("IInteractable"))
            {
                // Perform drilling effects on the terrain
                // You can adjust terrain height, textures, and more here
            }
        }
    }
}

