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

    private float movementSpeed = 5.0f;
    private float rotationSpeed = 3.0f;


    #region Object variables
    public enum ObjectState
    {
        InMotion,
        StopMotion,
    };

    ObjectState state = ObjectState.InMotion;

    public Transform player;
    public Transform currentPosition;
    private Transform lastKnownWaypoint;
    private float inRange = 5.0f;
    private float escapeDistance = 6.0f;
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
        ControlObjectState();
    }

    void ControlObjectState()
    {
        CheckDistanceToPlayer();

        switch (state)
        {
            case ObjectState.InMotion:
                float distance = Vector3.Distance(transform.position, targetWaypoint.position);
                CheckDistanceToWaypoint(distance);
                break;

            case ObjectState.StopMotion:
                targetWaypoint = lastKnownWaypoint;
                break;


        }
    }

    void CheckDistanceToPlayer()
    {
        switch (state)
        {
            case ObjectState.InMotion:
                if (Vector3.Distance(transform.position, player.position) < inRange)
                {
                    targetWaypoint = lastKnownWaypoint;
                    lastKnownWaypoint = currentPosition.transform;
                    state = ObjectState.StopMotion;
                }
                break;

            case ObjectState.StopMotion:
                if (Vector3.Distance(transform.position, player.position) > escapeDistance) //Player out of inRange
                {
                    //targetWaypoint = lastKnownWaypoint;
                    ReturnToStartingPoint();
                }
                break;
        }
    }

    /// <summary>
    /// Called when the Object is moving again
    /// </summary>
    void ReturnToStartingPoint()
    {
        targetWaypoint = lastKnownWaypoint;
        state = ObjectState.InMotion;
        

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
    /// Checks to see if the object is within distance of the waypoint. If it is, it called the UpdateTargetWaypoint function 
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