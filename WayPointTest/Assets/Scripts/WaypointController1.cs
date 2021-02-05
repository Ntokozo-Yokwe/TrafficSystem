using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController1 : MonoBehaviour
{

    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f; //If the distance between the object and the waypoint is less than this, then it has reacehd the waypoint
    private int lastWaypointIndex;
    public Transform currentPosition;
    private Transform lastKnownWaypoint;

    private float movementSpeed = 5.0f;
    private float rotationSpeed = 3.0f;


    #region Object variables

    public Transform player; //make a list of these transforms so we can add multiple obstruction objects to the array
    private float inRange = 7.0f;
    private float escapeDistance = 10.0f;
    #endregion

    // Use this for initialization
    void Start()
    {
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex]; //Set the first target waypoint at the start so the object starts moving towards a waypoint
    }

    // Update is called once per frame
    void Update()
    {

        UpdateTransform();
        if (Vector3.Distance(transform.position, player.position) < inRange)
        {
            lastKnownWaypoint = targetWaypoint;
            targetWaypoint = currentPosition.transform;
        }
        else if (Vector3.Distance(transform.position, player.position) > escapeDistance) //Player out of inRange
        {
            float distance = Vector3.Distance(transform.position, targetWaypoint.position);
            CheckDistanceToWaypoint(distance);
        }
    }
    
    /// <summary>
    /// Updating rotation and position values
    /// </summary>
    void UpdateTransform()
    {
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 directionToTarget = targetWaypoint.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

        Debug.DrawRay(transform.position, transform.forward * 50f, Color.green, 0f); //Draws a ray forward in the direction the object is facing
        Debug.DrawRay(transform.position, directionToTarget, Color.red, 0f); //Draws a ray in the direction of the current target waypoint
    

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
    }

    /// <summary>
    /// Checks to see if the object is within distance of the waypoint. If it is, it calls the UpdateTargetWaypoint function 
    /// </summary>
    /// <param name="currentDistance">The objects current distance from the waypoint</param>
    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    /// <summary>
    /// Increaes the index of the target waypoint. If the object has reached the last waypoint in the waypoints list, it resets the targetWaypointIndex to the first waypoint in the list (causes the object to loop)
    /// </summary>
    void UpdateTargetWaypoint()
    {
        if (targetWaypointIndex > lastWaypointIndex)
        {
            targetWaypointIndex = 0;
        }

        targetWaypoint = waypoints[targetWaypointIndex];
    }
}