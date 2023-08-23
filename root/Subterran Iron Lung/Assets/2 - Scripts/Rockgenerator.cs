using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockgenerator : MonoBehaviour
{
    public GameObject whiteBoxPrefab;   // Prefab for regular white box
    public GameObject purpleBoxPrefab;  // Prefab for purple unbreakable box
    public Vector3 boxSpacing = new Vector3(2, 0, 2); // Spacing between boxes
    public Vector3 mapSize = new Vector3(5, 1, 5);    // Size of the map in terms of box count
    public float minYPosition = 1.0f;  // Minimum Y position for boxes
    public float maxYPosition = 3.0f;  // Maximum Y position for boxes

    void Start()
    {
        GenerateBoxes();
    }

    void GenerateBoxes()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.z; z++)
            {
                Vector3 spawnPosition = new Vector3(x * boxSpacing.x, Random.Range(minYPosition, maxYPosition), z * boxSpacing.z);

                // Determine if this box should be purple (unbreakable)
                bool isPurple = IsPurpleBoxPosition(x, z);

                // Instantiate the appropriate box prefab
                GameObject boxInstance = Instantiate(isPurple ? purpleBoxPrefab : whiteBoxPrefab, spawnPosition, Quaternion.identity);

                // Set box color
                Renderer boxRenderer = boxInstance.GetComponent<Renderer>();
                boxRenderer.material.color = isPurple ? Color.magenta : Color.white;
            }
        }
    }

    bool IsPurpleBoxPosition(int x, int z)
    {
        // You can define your own logic here to determine purple box positions
        // For example, every 2nd column and every 3rd row could be purple boxes
        return (x % 2 == 0) && (z % 3 == 0);
    }
}
