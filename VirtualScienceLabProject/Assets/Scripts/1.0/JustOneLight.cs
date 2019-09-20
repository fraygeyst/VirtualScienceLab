using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/**
 * Script um das einzelne Licht für den Versuch in Lab3
 */

public class JustOneLight : MonoBehaviour
{

    public GameObject pointLight;
    private bool isOn;
    private bool collisionHappend = false;

    // Use this for initialization
    //Beim Start wird gepruepft ob das Licht an oder aus ist und entsprechend gesetzt
    void Start()
    {

    }

    //sobald man sich z.B. mit dem Controller in dem Bereich befinden wird die Variable auf true gesetzt
    private void OnTriggerEnter(Collider other)
    {
        collisionHappend = true;
        Debug.Log("collisionHappend: " + collisionHappend);
    }

    //sobald man sich z.B. mit dem Controller aus dem Bereich entfernt wird die Variable auf false gesetzt
    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("collisionHappend: " + collisionHappend);
    }

    // Update is called once per frame
    void Update()
    {

        //Sollte man sich in dem Bereich befinden und auf den Trigger Button druecken wird das Licht je nach Zustand an oder ausgeschlatet
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
