using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class WaterScript : MonoBehaviour {

    public GameObject water;
    private bool isOn;
    private bool collisionHappend = false;
    public GameObject waterParticleSystem;
    

    // Use this for initialization
    void Start () {
        
        water.SetActive(false);
        waterParticleSystem.SetActive(false);
        isOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
            collisionHappend = true;
            Debug.Log("WaterScript collisionHappend: " + collisionHappend);
        
    }

    //sobald man sich z.B. mit dem Controller aus dem Bereich entfernt wird die Variable auf false gesetzt
    private void OnTriggerExit(Collider other)
    {
          collisionHappend = false;
          Debug.Log("WaterScript collisionHappend" + collisionHappend);
        
    }

    // Update is called once per frame
    void Update () {
       

        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {
            if (isOn)
            {
                water.SetActive(false);
                waterParticleSystem.SetActive(false);
                isOn = false;
            }
            else
            {
                water.SetActive(true);
                waterParticleSystem.SetActive(true);
                isOn = true;
            }
        }
    }
}
