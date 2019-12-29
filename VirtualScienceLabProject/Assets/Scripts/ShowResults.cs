using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class ShowResults : MonoBehaviour {

    public GameObject graph;
    public GameObject table;

    private bool isOn;
    private bool collisionHappend = false;

    // Use this for initialization
    void Start()
    {
        graph.SetActive(false);
        table.SetActive(false);
        isOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionHappend = true;
        Debug.Log("show Images collisionHappend: " + collisionHappend);
    }

    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("show Images collisionHappend: " + collisionHappend);
    }

    // Update is called once per frame
    void Update()
    {
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {
            if (isOn)
            {
                isOn = false;
                graph.SetActive(false);
                table.SetActive(false);
            }
            else
            {
                isOn = true;
                graph.SetActive(true);
                table.SetActive(true);
            }
        }

    }
}