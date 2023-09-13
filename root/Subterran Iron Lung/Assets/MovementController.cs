using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float patrolRadius = 10f;
    Vector3 nextPosition;
    public GameObject biologicalMesh;
    public float hight;
    public float max_height;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // If the enemy has reached its destination, pick a new one.
        if (agent.remainingDistance < 0.1f)
        {
            nextPosition = agent.transform.position + Random.insideUnitSphere * patrolRadius;
        }

        // Move towards the next position.
        agent.SetDestination(nextPosition);
    }




    int ChooseRandomHeight(int max) => Random.Range(0, max);
}
