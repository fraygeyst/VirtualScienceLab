﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.SceneManagement;

public class TurnBeamerOn : MonoBehaviour
{

    public GameObject beamerLight;
    public GameObject imageOnTheWall;
   
   

    private bool isOn;

    private bool collisionHappend = false;

    // Use this for initialization
    void Start()
    {
        beamerLight.SetActive(false);
        isOn = false;
    }

 

    private void OnTriggerEnter(Collider other)
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(other.name);
        if (scene.name == "Lab2")
        {
            if (other.name == "SphereCollider") {
                collisionHappend = true;
                Debug.Log("TurnBeamerOn collisionHappend: " + collisionHappend);
            }
        }
        else {
            collisionHappend = true;
            Debug.Log("TurnBeamerOn collisionHappend: " + collisionHappend);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("TurnBeamerOn collisionHappend: " + collisionHappend);
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
