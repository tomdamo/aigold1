using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollerController : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolIndex = 0;
    public float speed = 6f;

    public void SetPatrolPoints(Vector3 point1, Vector3 point2)
    {
        patrolPoints.Clear();
        patrolPoints.Add(point1);
        patrolPoints.Add(point2);

        currentPatrolIndex = 0;
        agent.SetDestination(patrolPoints[currentPatrolIndex]);
        Debug.Log("Points has been set" + point1 + " " + point2);
    }

    public void ClearPatrolPoints()
    {
        Debug.Log("Patrol points cleared");
        patrolPoints.Clear();

    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found!");
        }
        else
        {
            agent.stoppingDistance = 0.1f; 
        }
    }

    void Update()
    {
        if (patrolPoints.Count > 0 && agent != null)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Debug.Log("Reached patrol point " + currentPatrolIndex);
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                agent.SetDestination(patrolPoints[currentPatrolIndex]);
            }
        }
    }
}
