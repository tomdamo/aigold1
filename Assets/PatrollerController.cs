using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolIndex = 0;

    public void SetPatrolPoints(Vector3 point1, Vector3 point2)
    {
        patrolPoints.Clear();
        patrolPoints.Add(point1);
        patrolPoints.Add(point2);

        // Start patrolling between the two points
        currentPatrolIndex = 0;
        agent.SetDestination(patrolPoints[currentPatrolIndex]);
    }

    public void ClearPatrolPoints()
    {
        patrolPoints.Clear();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found!");
        }
    }

    void Update()
    {
        if (patrolPoints.Count > 0 && agent != null && agent.remainingDistance < 0.5f)
        {
            // Reached the patrol point, move to the next one in the list
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPatrolIndex]);
        }
    }
}
