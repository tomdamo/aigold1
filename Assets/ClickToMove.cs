using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private NavMeshAgent selectedPatroller;
    
    private bool isPatrollerSelected;
    private PatrollerController currentPatrollerController;

    private bool AgentMode;
    private Vector3 defaultScale = Vector3.one;
    public TMP_Text currentMode;

    private void Start()
    {
        mainCamera = Camera.main;
        currentMode.text = "agents";
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
                    if (hit.collider.GetComponent<NavMeshAgent>() == selectedAgent ||
                        hit.collider.GetComponent<NavMeshAgent>() == selectedPatroller)
                    {
                        // If the clicked agent or patroller is already selected, deselect
                        DeselectAgentOrPatroller(hit.collider.GetComponent<NavMeshAgent>());
                    }
                    else
                    {
                        // If a different agent or patroller is clicked, select it
                        SelectAgentOrPatroller(hit.collider.GetComponent<NavMeshAgent>());
                    }
                

                if (hit.collider.CompareTag("Agent"))
                    {
                        selectedAgent = hit.collider.GetComponent<NavMeshAgent>();
                        selectedAgent.transform.localScale = defaultScale * highlightScaleFactor;
                        isPatrollerSelected = false;
                        Debug.Log("Agent selected!");
                    }
                    else if (hit.collider.CompareTag("Patroller"))
                    {
                        selectedPatroller = hit.collider.GetComponent<NavMeshAgent>();
                        isPatrollerSelected = true;
                        selectedPatroller.transform.localScale = defaultScale * highlightScaleFactor;
                        currentPatrollerController = hit.collider.GetComponent<PatrollerController>(); 
                        if (currentPatrollerController == null)
                        {
                            Debug.LogError("PatrollerController component not found!");
                        }
                        Debug.Log("Patroller selected!");
                    }
                }
                else if (hit.collider.CompareTag("Floor"))
                {
                    if (isPatrollerSelected && currentPatrollerController.patrolPoints.Count < 2)
                    {
                        // Add patrol points for the selected patroller
                        currentPatrollerController.patrolPoints.Add(hit.point);

                        if (currentPatrollerController.patrolPoints.Count == 2)
                        {
                            // Set patrolling points for the patroller
                            PatrollerController patrollerController = selectedPatroller.GetComponent<PatrollerController>();
                            if (patrollerController != null)
                            {
                                patrollerController.SetPatrolPoints(currentPatrollerController.patrolPoints[0], currentPatrollerController.patrolPoints[1]);
                            }
                        }
                    }
                    else if (selectedAgent != null && !isPatrollerSelected) 
                    {
                        selectedAgent.SetDestination(hit.point);
                        isPatrollerSelected = false;
                        Debug.Log("agent walks to" + hit.point);

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
            Console.WriteLine("You can now spawn agents");
            currentMode.text = "agents";
            AgentMode = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Console.WriteLine("You can now spawn patrollers");
            currentMode.text = "patrollers";
            AgentMode = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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

    void DeselectAgentOrPatroller(NavMeshAgent agent)
    {
        if (agent == selectedAgent)
        {
            selectedAgent.transform.localScale = defaultScale; // Scale down the previously selected agent
            selectedAgent = null;
            isPatrollerSelected = false;
            Debug.Log("Agent deselected!");
        }
        else if (agent == selectedPatroller)
        {
            selectedPatroller.transform.localScale = defaultScale; // Scale down the previously selected patroller
            selectedPatroller = null;
            isPatrollerSelected = false;
            currentPatrollerController = null; // Reset the patroller controller
            Debug.Log("Patroller deselected!");
        }
    }

    void SelectAgentOrPatroller(NavMeshAgent agent)
    {
        if (agent.CompareTag("Agent"))
        {
            if (selectedAgent != null)
            {
                // Scale down the previously selected agent
                selectedAgent.transform.localScale = defaultScale;
            }
            selectedAgent = agent;
            selectedAgent.transform.localScale = defaultScale * highlightScaleFactor; // Scale up the newly selected agent
            isPatrollerSelected = false;
            Debug.Log("Agent selected!");
        }
        else if (agent.CompareTag("Patroller"))
        {
            if (selectedPatroller != null)
            {
                // Scale down the previously selected patroller
                selectedPatroller.transform.localScale = defaultScale;
            }
            selectedPatroller = agent;
            selectedPatroller.transform.localScale = defaultScale * highlightScaleFactor; // Scale up the newly selected patroller
            isPatrollerSelected = true;
            currentPatrollerController = agent.GetComponent<PatrollerController>();
            if (currentPatrollerController == null)
            {
                Debug.LogError("PatrollerController component not found!");
            }
            Debug.Log("Patroller selected!");
        }
    }

}
