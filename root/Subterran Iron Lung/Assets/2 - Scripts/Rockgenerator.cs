using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject rockPrefab02;   // Prefab for Rock_02
    public GameObject rockPrefab03;   // Prefab for Rock_03
    public GameObject rockPrefab04;   // Prefab for Rock_04
    public GameObject rockPrefab05;   // Prefab for Rock_05
    public GameObject rockPrefab06;   // Prefab for Rock_06
    public GameObject radioPrefab;    // Prefab for the Radio
    public Vector3 rockSpacing = new Vector3(2, 0, 2); // Spacing between rocks
    public Vector3 mapSize = new Vector3(5, 1, 5);     // Size of the map in terms of rock count
    public float minYPosition = 1.0f;  // Minimum Y position for rocks
    public float maxYPosition = 3.0f;  // Maximum Y position for rocks

    public int maxRadios = 3;          // Maximum number of radios to place

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Generate rocks
        GenerateRocks();

        // Generate radios
        GenerateRadios();
    }

    void GenerateRocks()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.z; z++)
            {
                Vector3 spawnPosition = new Vector3(x * rockSpacing.x, Random.Range(minYPosition, maxYPosition), z * rockSpacing.z);

                // Determine which rock prefab to instantiate based on position
                GameObject rockPrefabToInstantiate;

                // Replace this placeholder logic with your actual logic for choosing rock prefabs
                if (IsPurpleRockPosition(x, z))
                {
                    rockPrefabToInstantiate = rockPrefab03;
                }
                else
                {
                    int randomRockIndex = Random.Range(0, 4); // Choose from Rock_02, Rock_04, Rock_05, and Rock_06
                    if (randomRockIndex == 0)
                        rockPrefabToInstantiate = rockPrefab02;
                    else if (randomRockIndex == 1)
                        rockPrefabToInstantiate = rockPrefab04;
                    else if (randomRockIndex == 2)
                        rockPrefabToInstantiate = rockPrefab05;
                    else
                        rockPrefabToInstantiate = rockPrefab06;
                }

                // Instantiate the rock prefab
                GameObject rockInstance = Instantiate(rockPrefabToInstantiate, spawnPosition, Quaternion.identity);

                // Add rotation
                rockInstance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                // Add any necessary customization to the instantiated rock instance here
            }
        }
    }

    void GenerateRadios()
    {
        int radiosPlaced = 0;

        while (radiosPlaced < maxRadios)
        {
            int x = Random.Range(0, (int)mapSize.x);
            int z = Random.Range(0, (int)mapSize.z);

            Vector3 spawnPosition = new Vector3(x * rockSpacing.x, Random.Range(minYPosition, maxYPosition), z * rockSpacing.z);

            // Instantiate the radio prefab
            GameObject radioInstance = Instantiate(radioPrefab, spawnPosition, Quaternion.identity);

            radiosPlaced++;
        }
    }

    bool IsPurpleRockPosition(int x, int z)
    {
        // Your logic for determining purple rock positions goes here
        return (x % 2 == 0) && (z % 3 == 0);
    }
}





