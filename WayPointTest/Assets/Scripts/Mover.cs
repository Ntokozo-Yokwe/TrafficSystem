using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	#region Public tools
	[Tooltip("Populate with waypoints for this character to move towards")]
	public Waypoint[] wayPoints;
	[Tooltip("The speed at which the character moves towards a waypoint")]
	public float speed = 3f;
	[Tooltip("Character moves from last waypoint back to the first, which allows the character to circuit the waypoint layout")]
	public bool isCircular;
	[Tooltip("Character reaches the last waypoint and follows the exact same trail back to reach the first waypoint again")]
	public bool inReverse = true;
	[Tooltip("The closest this character can get to the object before stopping")]
	public float inRange = 4.0f;
	[Tooltip("The distance it needs in order for it to proceed in its circuit")]
	public float outOfRange = 5.0f;
	[Tooltip("Populate with objects that this character will keep its distance from")]
	public GameObject[] players;
	#endregion
	private GameObject player;
	private Waypoint currentWaypoint;
	private int currentIndex = 0;
	private bool isWaiting = false;
	private float speedStorage = 0;



	/**
	 * Initialisation
	 * 
	 */
	void Start()
	{
		if (wayPoints.Length > 0) //If we have a waypoint in our array, we set currentWaypoint to the first waypoint in the array
		{
			currentWaypoint = wayPoints[0];
		}
	}



	/**
	 * Update is called once per frame
	 * 
	 */
	void Update()
	{
		CheckOtherObjects(); 
	}


	/**
	 * Check distance of character
	 * 
	 */
	void CheckOtherObjects()
	{
		//for every gameobj in the array we run this code
		foreach (GameObject player in players)
			// If the distance between the character and an obj "player" in our array is closer than the inRange, we don't continue with the script
			if (Vector3.Distance(transform.position, player.transform.position) < inRange) return;

			//If the distance between the character and an obj is greater than the outOfRange, then we continue with the code
			else if (Vector3.Distance(transform.position, player.transform.position) > outOfRange)
			{
				// if the currentWaypoint is not null, and there is no wait time on it, then we run the movement script
				if (currentWaypoint != null && !isWaiting)
				{
					MoveTowardsWaypoint();
				}
			}
	}

	/**
	 * Pause the character
	 * 
	 */
	void Pause()
	{
		//We set it to True when called apon
		isWaiting = !isWaiting;
	}

	/**
	 * Move the character towards the selected waypoint
	 * 
	 */
	private void MoveTowardsWaypoint()
	{
		// Get the characters current position
		Vector3 currentPosition = this.transform.position;

		// Get the target waypoints position
		Vector3 targetPosition = currentWaypoint.transform.position;

		// If the character isn't that close to the waypoint
		if (Vector3.Distance(currentPosition, targetPosition) > .1f)
		{

			// Get the direction and normalize
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();

			//scale the movement on each axis by the directionOfTravel vector components
			this.transform.Translate(
				directionOfTravel.x * speed * Time.deltaTime,
				directionOfTravel.y * speed * Time.deltaTime,
				directionOfTravel.z * speed * Time.deltaTime,
				Space.World
			);
		}
		else
		{

			// If the waypoint has a pause amount then wait a bit
			if (currentWaypoint.waitSeconds > 0)
			{
				// We wait for the seconds determined by the Waypoint code on the current waypoint
				Pause();
				Invoke("Pause", currentWaypoint.waitSeconds);

			}

			// If the current waypoint has a speed change then change to it.. could be used to increase spead on a ramp off to the freeway
			if (currentWaypoint.speedOut > 0)
			{
				speedStorage = speed;
				speed = currentWaypoint.speedOut;
			}
			else if (speedStorage != 0)
			{
				speed = speedStorage;
				speedStorage = 0;
			}
			// Call to move to the following waypoint
			NextWaypoint();
		}
	}

	/**
	 * Work out what the next waypoint is going to be
	 * 
	 */
	private void NextWaypoint()
	{
		// If we have the isCircular toggle selected we run this code
		if (isCircular)
		{

			if (!inReverse)
			{
				currentIndex = (currentIndex + 1 >= wayPoints.Length) ? 0 : currentIndex + 1;
			}
			else
			{
				currentIndex = (currentIndex == 0) ? wayPoints.Length - 1 : currentIndex - 1;
			}

		}
		// inReverse is the default so if not isCircular, we proceed with this code
		else
		{

			// If at the start or the end then reverse, for continuous motion
			if ((!inReverse && currentIndex + 1 >= wayPoints.Length) || (inReverse && currentIndex == 0))
			{
				inReverse = !inReverse;
			}
			currentIndex = (!inReverse) ? currentIndex + 1 : currentIndex - 1;

		}

		currentWaypoint = wayPoints[currentIndex];
	}
}