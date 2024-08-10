using FistVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSR_Course : MonoBehaviour
{
    [Header("Target Setter/Resetter Setup")]
    [Tooltip("Array of targets")]
    public TSR_Target[] targets;
    [Tooltip("Whether this button and its targets is enabled or disabled whenever the map starts")]
    public bool isTargetsEnabledAtStart;
    [Tooltip("Game Object thats enabled whenever the targets are turned on")]
    public GameObject enabledVis;
    [Tooltip("Game Object thats enabled whenever the targets are turned off")]
    public GameObject disabledVis;
    [HideInInspector]
    public bool isTargetsActive;
    FVRObject mainObject;
    [HideInInspector]
    public List<GameObject> allTargets = new List<GameObject>();
    [HideInInspector]
    public bool buttonPressed;

    void Start()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            for (var a = targets[i].transform.childCount - 1; a >= 0; a--)
            {
                Object.Destroy(targets[i].transform.GetChild(a).gameObject);
            }
        }

        if (isTargetsEnabledAtStart)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].itemID != null && targets[i].targetPrefab == null)
                {
                    if (IM.OD.TryGetValue(targets[i].itemID, out mainObject))
                    {
                        GameObject newTarget = Instantiate(mainObject.GetGameObject(), targets[i].transform.position, targets[i].transform.rotation);
                        allTargets.Add(newTarget);
                        if (targets[i].isPhysicsLocked)
                        {
                            newTarget.GetComponent<Rigidbody>().isKinematic = true;
                        }
                    }
                }
                else if (targets[i].targetPrefab != null && targets[i].itemID == null)
                {
                    GameObject newTarget = Instantiate(targets[i].targetPrefab, targets[i].transform.position, targets[i].transform.rotation);
                    allTargets.Add(newTarget);
                    if (targets[i].isPhysicsLocked)
                    {
                        newTarget.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
                else
                {
                    return;
                }
            }

            isTargetsActive = true;

            enabledVis.SetActive(true);
            disabledVis.SetActive(false);
        }
        else if (!isTargetsEnabledAtStart)
        {
            isTargetsActive = false;

            enabledVis.SetActive(false);
            disabledVis.SetActive(true);
        }
    }

    public void ResetTargets()
    {
        if (!isTargetsActive)
        {
            return;
        }
        else if (isTargetsActive)
        {
            for (int i = 0; i < allTargets.Count; i++)
            {
                if (allTargets[i] != null)
                {
                    Destroy(allTargets[i]);
                }
            }

            allTargets.Clear();

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].itemID != null && targets[i].targetPrefab == null)
                {
                    if (IM.OD.TryGetValue(targets[i].itemID, out mainObject))
                    {
                        GameObject newTarget = Instantiate(mainObject.GetGameObject(), targets[i].transform.position, targets[i].transform.rotation);
                        allTargets.Add(newTarget);
                        if (targets[i].isPhysicsLocked)
                        {
                            newTarget.GetComponent<Rigidbody>().isKinematic = true;
                        }
                    }
                }
                else if (targets[i].targetPrefab != null && targets[i].itemID == null)
                {
                    GameObject newTarget = Instantiate(targets[i].targetPrefab, targets[i].transform.position, targets[i].transform.rotation);
                    allTargets.Add(newTarget);
                    if (targets[i].isPhysicsLocked)
                    {
                        newTarget.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void SpawnTargets()
    {
        if (!isTargetsActive)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].itemID != null && targets[i].targetPrefab == null)
                {
                    if (IM.OD.TryGetValue(targets[i].itemID, out mainObject))
                    {
                        GameObject newTarget = Instantiate(mainObject.GetGameObject(), targets[i].transform.position, targets[i].transform.rotation);
                        allTargets.Add(newTarget);
                        if (targets[i].isPhysicsLocked && newTarget.GetComponent<Rigidbody>() != null)
                        {
                            newTarget.GetComponent<Rigidbody>().isKinematic = true;
                        }
                    }
                }
                else if (targets[i].targetPrefab != null && targets[i].itemID == null)
                {
                    GameObject newTarget = Instantiate(targets[i].targetPrefab, targets[i].transform.position, targets[i].transform.rotation);
                    allTargets.Add(newTarget);
                    if (targets[i].isPhysicsLocked && newTarget.GetComponent<Rigidbody>() != null)
                    {
                        newTarget.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
                else
                {
                    return;
                }
            }

            enabledVis.SetActive(true);
            disabledVis.SetActive(false);

            isTargetsActive = true;
        }
        else if (isTargetsActive)
        {
            for (int i = 0; i < allTargets.Count; i++)
            {
                if (allTargets[i] != null)
                {
                    Destroy(allTargets[i]);
                }
            }

            allTargets.Clear();

            enabledVis.SetActive(false);
            disabledVis.SetActive(true);

            isTargetsActive = false;
        }
    }

	void OnTriggerEnter(Collider collider)
	{
		if (buttonPressed)
		{
			return;
		}

        if (collider.gameObject.tag == "GameController")
		{
            SpawnTargets();

			buttonPressed = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "GameController")
        {
            buttonPressed = false;
        }
    }

    //WHY THE FUCK DONT YOU WORK!
    void OnDrawGizmos()
    {
        if (targets == null)
        {
            return;
        }

        for ( int i = 0; i < targets.Length; i++) 
        {
            if (targets[i].targetPrefab != null && !string.IsNullOrEmpty(targets[i].itemID))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(targets[i].transform.position, .1f);
                Debug.LogWarning("Target " + i + " has both filled at the same time!");
            }
            else if (!string.IsNullOrEmpty(targets[i].itemID))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(targets[i].transform.position, .1f);
            }
            else if (targets[i].targetPrefab != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(targets[i].transform.position, .1f);
            }
            else
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(targets[i].transform.position, .1f);
                Debug.LogWarning("Target " + i + " is empty!");
            }

            if (targets[i].isPhysicsLocked == true)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(targets[i].transform.position, .11f);
            }
        }
    }
}