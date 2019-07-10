using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class TurnLightOn : MonoBehaviour {

    public GameObject goLight;
    private bool isOn; 
    private bool collisionHappend = false;

	// Use this for initialization
	void Start () {
        isOn = goLight.GetComponent<Light>().enabled;
	}

    private void OnCollisionEnter(Collision collision)
    {
        collisionHappend = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionHappend = false;
    }

    // Update is called once per frame
    void Update () {

        if ( (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend ) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend ) )
        {
            if (isOn)
            {
                goLight.GetComponent<Light>().enabled = false;
                isOn = false;
            }
            else
            {
                goLight.GetComponent<Light>().enabled = true;
                isOn = true;
            }
        }


    }
}
