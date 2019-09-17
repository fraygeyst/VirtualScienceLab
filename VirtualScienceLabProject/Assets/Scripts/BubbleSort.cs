using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class BubbleSort : MonoBehaviour {

    public bool start = false;
    private bool collisionHappend = false;

    public GameObject sicht1;
    public GameObject sicht2;


    private void OnTriggerEnter(Collider other)
    {

        collisionHappend = true;
        Debug.Log("startButton collisionHappend: " + collisionHappend);
    }

    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("startButton collisionHappend: " + collisionHappend);
    }


    // Use this for initialization
    void Start () {
		
	}


    // Update is called once per frame
    void Update()
    {
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {

            if (start == true)
            {
                startSort();
            }
            else

            {

            }
        }
    }
    public void startSort()
    {
            sicht1.SetActive(false);
            sicht2.SetActive(false);
      
    }
}
