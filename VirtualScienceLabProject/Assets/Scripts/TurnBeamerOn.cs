using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class TurnBeamerOn : MonoBehaviour
{

    public GameObject beamerLight;
    public GameObject imageOnTheWall;
   
   

    private bool isOn;

    private bool collisionHappend = false;

    // Use this for initialization
    void Start()
    {
        Debug.Log("collisionHappend: " + collisionHappend);
        isOn = beamerLight.activeSelf;
    }

 

    private void OnTriggerEnter(Collider other)
    {
        collisionHappend = true;
        Debug.Log("collisionHappend: " + collisionHappend);

    }

    private void OnTriggerExit(Collider other)
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
                beamerLight.SetActive(false);
                isOn = false;
                imageOnTheWall.SetActive(false);
            }
            else
            {
                beamerLight.SetActive(true);
                isOn = true;
                imageOnTheWall.SetActive(true);
            }
        }

    }
}
