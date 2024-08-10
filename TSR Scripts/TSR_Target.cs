using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSR_Target : MonoBehaviour
{
	[Header("Target Setup")]
	[Tooltip("Spawn this object ID at this transform (dont use both of these at the same time)")]
	public string itemID;
	[Tooltip("Spawn a custom prefab at this transform (dont use both of these at the same time)")]
	public GameObject targetPrefab;
	[Tooltip("Makes sure the object that is spawned is unable to be affected by physics (by checking for rigidbodies and making them kinematic)")]
	public bool isPhysicsLocked;
}
