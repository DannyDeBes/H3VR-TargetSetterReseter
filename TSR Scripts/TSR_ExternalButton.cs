using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSR_ExternalButton : MonoBehaviour 
{
	public TSR_Course mainButton;
    public bool isResetButton;
    public AudioSource resetAudio;
    [Tooltip("Game Object thats enabled whenever the targets are turned on")]
    public GameObject enabledVis;
    [Tooltip("Game Object thats enabled whenever the targets are turned off")]
    public GameObject disabledVis;

    [HideInInspector]
    public bool buttonPressed;

    void FixedUpdate()
    {
        if (enabledVis == null || disabledVis == null)
        {
            return;
        }

        if (mainButton.isTargetsActive)
        {
            enabledVis.SetActive(true);
            disabledVis.SetActive(false);
        }
        else
        {
            enabledVis.SetActive(false);
            disabledVis.SetActive(true);
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
            if (isResetButton) 
            {
                mainButton.ResetTargets();
                resetAudio.Play();
            }
            else
            {
                mainButton.SpawnTargets();
            }

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
}
