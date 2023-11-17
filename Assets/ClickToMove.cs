using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public LayerMask agentLayer;
    public GameObject agentPrefab;
    public GameObject patrollerPrefab;
    public float highlightScaleFactor = 1.3f; // Scale factor for highlighting
    private Camera mainCamera;

    private NavMeshAgent selectedAgent;
    
    private bool isPatrollerSelected;
    private NavMeshAgent selectedPatroller;
    private List<Vector3> patrolPoints = new List<Vector3>();
    private PatrollerController currentPatrollerController;

    private bool AgentMode;
    private Vector3 defaultScale = Vector3.one;
    public TMP_Text currentMode;

    private void Start()
    {
        mainCamera = Camera.main;
        AgentMode = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Agent") || hit.collider.CompareTag("Patroller"))
                {

                    if (selectedAgent != null)
                    {
                        selectedAgent.transform.localScale = defaultScale;
                    }
                    if (selectedPatroller != null)
                    {
                        selectedPatroller.transform.localScale = defaultScale;

                    }

                    if (hit.collider.CompareTag("Agent"))
                    {
                        selectedAgent = hit.collider.GetComponent<NavMeshAgent>();
                        selectedAgent.transform.localScale = defaultScale * highlightScaleFactor;
                        Debug.Log("Agent selected!");
                    }
                    else if (hit.collider.CompareTag("Patroller"))
                    {
                        selectedPatroller = hit.collider.GetComponent<NavMeshAgent>();
                        isPatrollerSelected = true;
                        selectedPatroller.transform.localScale = defaultScale * highlightScaleFactor;
                        Debug.Log("Patroller selected!");
                    }
                }
                else if (hit.collider.CompareTag("Floor"))
                {
                    if (isPatrollerSelected && patrolPoints.Count < 2)
                    {
                        // Add patrol points for the selected patroller
                        patrolPoints.Add(hit.point);

                        if (patrolPoints.Count == 2)
                        {
                            // Set patrolling points for the patroller
                            PatrollerController patrollerController = selectedPatroller.GetComponent<PatrollerController>();
                            if (patrollerController != null)
                            {
                                patrollerController.SetPatrolPoints(patrolPoints[0], patrolPoints[1]);
                            }
                        }
                    }
                    else if (selectedAgent != null && !isPatrollerSelected) 
                    {
                        selectedAgent.SetDestination(hit.point);
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (AgentMode)
            {
                SpawnAgent();
            }
            if (!AgentMode)
            {
                SpawnPatroller();
            }
        }

        // Handle right-click to spawn a new agent or patroller
        if (Input.GetKeyDown(KeyCode.A))
        {
            AgentMode = true;
            Console.WriteLine("You can now spawn agents");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            // Spawn a new patroller at the clicked position
            AgentMode = false;
            Console.WriteLine("You can now spawn patrollers");
        }
    }

    // Spawns a new agent at the clicked position
    void SpawnAgent()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Floor")) // Check if clicked on the ground
            {
                GameObject newAgent = Instantiate(agentPrefab, hit.point, Quaternion.identity);
                NavMeshAgent newNavMeshAgent = newAgent.GetComponent<NavMeshAgent>();
                newNavMeshAgent.transform.localScale = defaultScale;
                Debug.Log("New agent spawned!");
            }
        }
    }

    // Spawns a new patroller at the clicked position
    void SpawnPatroller()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Floor")) // Check if clicked on the ground
            {
                GameObject newPatroller = Instantiate(patrollerPrefab, hit.point, Quaternion.identity);
                NavMeshAgent newNavMeshAgent = newPatroller.GetComponent<NavMeshAgent>();
                newPatroller.transform.localScale = defaultScale;
                Debug.Log("New patroller spawned!");
            }
        }
    }
}
