using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class TurnLightOn : MonoBehaviour {

    public GameObject pointLight1;
    public GameObject pointLight2;
    public GameObject pointLight3;
    private bool isOn; 
    private bool collisionHappend = false;

	// Use this for initialization
	void Start () {
        isOn = pointLight1.GetComponent<Light>().enabled;
	}

    private void OnCollisionEnter(Collision collision)
    {
        collisionHappend = true;
        Debug.Log("collisionHappend: " +collisionHappend);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionHappend = false;
        Debug.Log("collisionHappend: " + collisionHappend);
    }

    // Update is called once per frame
    void Update () {

        if ( (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend ) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend ) )
        {
            if (isOn)
            {
                pointLight1.GetComponent<Light>().enabled = false;
                pointLight2.GetComponent<Light>().enabled = false;
                pointLight3.GetComponent<Light>().enabled = false;
                isOn = false;
            }
            else
            {
                pointLight1.GetComponent<Light>().enabled = true;
                pointLight2.GetComponent<Light>().enabled = true;
                pointLight3.GetComponent<Light>().enabled = true;
                isOn = true;
            }
        }


    }
}
