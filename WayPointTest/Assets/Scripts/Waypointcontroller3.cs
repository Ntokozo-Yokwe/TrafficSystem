﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypointcontroller3 : MonoBehaviour
{

    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f; //If the distance between the enemy and the waypoint is less than this, then it has reacehd the waypoint
    private int lastWaypointIndex;
    private float movementSpeed = 3.0f;
    private float rotationSpeed = 2.0f;


    #region breadcrumb variables
    public enum EnemyState
    {
        PATROLLING,
        FOLLOWING_PLAYER,
        WAITFORSECONDS
    };

    EnemyState state = EnemyState.PATROLLING;

    List<Transform> crumbs = new List<Transform>();
    public GameObject crumb;
    public Transform player;
    private Transform lastKnownWaypoint;
    private float inRange = 2.0f;
    private float escapeDistance = 3.0f;
    #endregion

    // Use this for initialization
    void Start()
    {
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex]; //Set the first target waypoint at the start so the enemy starts moving towards a waypoint
    }

    // Update is called once per frame
    void Update()
    {

        UpdateTransform();
        ControlEnemyState();
    }

    void ControlEnemyState()
    {
        CheckDistanceToPlayer();

        switch (state)
        {
            case EnemyState.PATROLLING:
                float distance = Vector3.Distance(transform.position, targetWaypoint.position);
                CheckDistanceToWaypoint(distance);
                break;

            case EnemyState.FOLLOWING_PLAYER:
                if (Vector3.Distance(transform.position, player.position) > escapeDistance) //Player has escaped. Follow breadcrumbs back
                {
                    UpdateTransform();
                }

                break;

            case EnemyState.WAITFORSECONDS:
                WaitAtIntersection();
                break;

        }
    }

    void CheckDistanceToPlayer()
    {
        switch (state)
        {
            case EnemyState.PATROLLING:
                if (Vector3.Distance(transform.position, player.position) < inRange)
                {
                    StartCoroutine(WaitAtIntersection());
                    state = EnemyState.WAITFORSECONDS;
                }
                break;

            case EnemyState.FOLLOWING_PLAYER:
                if (Vector3.Distance(transform.position, player.position) > escapeDistance) //Player has escaped. Follow breadcrumbs back
                {
                    state = EnemyState.FOLLOWING_PLAYER;
                }
                break;
        }
    }

    public IEnumerator WaitAtIntersection()
    {
        yield return new WaitForSeconds(2.0f);
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

        Debug.DrawRay(transform.position, transform.forward * 50f, Color.green, 0f); //Draws a ray forward in the direction the enemy is facing
        Debug.DrawRay(transform.position, directionToTarget, Color.red, 0f); //Draws a ray in the direction of the current target waypoint



        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
    }

    /// <summary>
    /// Checks to see if the enemy is within distance of the waypoint. If it is, it called the UpdateTargetWaypoint function 
    /// </summary>
    /// <param name="currentDistance">The enemys current distance from the waypoint</param>
    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    /// <summary>
    /// Increaes the index of the target waypoint. If the enemy has reached the last waypoint in the waypoints list, it resets the targetWaypointIndex to the first waypoint in the list (causes the enemy to loop)
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
