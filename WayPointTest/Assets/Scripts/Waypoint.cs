using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
	[Tooltip("Set a time for objects to stop at this waypoint")]
	public float waitSeconds = 0;
	[Tooltip("Set a time for objects to stop at this waypoint")]
	public float speedOut = 0;
}
