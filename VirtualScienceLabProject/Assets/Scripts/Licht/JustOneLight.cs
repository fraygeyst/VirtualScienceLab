using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class JustOneLight : MonoBehaviour {

    public GameObject pointLight;
    private bool isOn;
    private bool collisionHappend = false;

    // Use this for initialization
    void Start()
    {
        isOn = pointLight.GetComponent<Light>().enabled;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionHappend = true;
        Debug.Log("collisionHappend: " + collisionHappend);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionHappend = false;
        Debug.Log("collisionHappend: " + collisionHappend);
    }

    // Update is called once per frame
    void Update()
    {

        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {
            if (isOn)
            {
                pointLight.GetComponent<Light>().enabled = false;
                isOn = false;
            }
            else
            {
                pointLight.GetComponent<Light>().enabled = true;
                isOn = true;
            }
        }


    }
}
