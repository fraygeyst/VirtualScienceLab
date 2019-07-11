using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/**
 * Script um das Licht z.B. an der Decke an/aus zuschalten
 */

public class TurnLightOn : MonoBehaviour
{

    public GameObject pointLight1up;
    public GameObject pointLight1down;
    public GameObject pointLight2up;
    public GameObject pointLight2down;
    public GameObject pointLight3up;
    public GameObject pointLight3down;
    public GameObject directLight;
    private bool isOn;
    private bool collisionHappend = false;

    // Use this for initialization
    //Beim Start wird gepruepft ob das Licht an oder aus ist und entsprechend gesetzt
    void Start()
    {
        isOn = directLight.GetComponent<Light>().enabled;
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
                pointLight1up.GetComponent<Light>().enabled = false;
                pointLight1down.GetComponent<Light>().enabled = false;
                pointLight2up.GetComponent<Light>().enabled = false;
                pointLight2down.GetComponent<Light>().enabled = false;
                pointLight3up.GetComponent<Light>().enabled = false;
                pointLight3down.GetComponent<Light>().enabled = false;
                directLight.GetComponent<Light>().enabled = false;
                isOn = false;
            }
            else
            {
                pointLight1up.GetComponent<Light>().enabled = true;
                pointLight1down.GetComponent<Light>().enabled = true;
                pointLight2up.GetComponent<Light>().enabled = true;
                pointLight2down.GetComponent<Light>().enabled = true;
                pointLight3up.GetComponent<Light>().enabled = true;
                pointLight3down.GetComponent<Light>().enabled = true;
                directLight.GetComponent<Light>().enabled = true;
                isOn = true;
            }
        }


    }
}
