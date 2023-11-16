using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform targetPoint; // The point where the platform will move
    public float moveSpeed = 2f; // Speed at which the platform moves
    public float waitTime = 1f; // Time to wait at each point

    private Vector3 initialPosition; // Initial position of the platform
    private bool moving; // Flag to check if the platform is currently moving

    void Start()
    {
        initialPosition = transform.position;
        moving = false;
        MoveToNextPoint();
    }

    void Update()
    {
        if (!moving)
        {
            // Wait at the current point for the specified time
            Invoke("MoveToNextPoint", waitTime);
        }
    }

    void MoveToNextPoint()
    {
        moving = true;
        Vector3 targetPos = targetPoint.position;
        StartCoroutine(MovePlatform(targetPos));
    }

    IEnumerator MovePlatform(Vector3 targetPos)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = transform.position;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        moving = false;
    }

    // For visualization purposes only, to see the platform's movement in the Scene view
    void OnDrawGizmosSelected()
    {
        if (targetPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPoint.position);
        }
    }
}
