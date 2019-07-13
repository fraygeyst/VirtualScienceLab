using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class Lab3TurnWaterlightOn : MonoBehaviour {

    public GameObject pointLight;
    private bool isOn;
    private bool collisionHappend = false;
    public GameObject water;
    public GameObject Gluehbirne;
    public Material materialGluehbirneOFF;
    public Material materialGluehbirneON;


    // Use this for initialization
    //Beim Start wird gepruepft ob das Licht an oder aus ist und entsprechend gesetzt
    void Start()
    {
        
    }

    //sobald man sich z.B. mit dem Controller in dem Bereich befinden wird die Variable auf true gesetzt
    private void OnTriggerEnter(Collider other)
    {
        collisionHappend = true;
        Debug.Log("Lab3TurnWaterlightOn collisionHappend: " + collisionHappend);
    }

    //sobald man sich z.B. mit dem Controller aus dem Bereich entfernt wird die Variable auf false gesetzt
    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("Lab3TurnWaterlightOn collisionHappend: " + collisionHappend);
    }

    // Update is called once per frame
    void Update()
    {
        
       


        //Sollte man sich in dem Bereich befinden und auf den Trigger Button druecken wird das Licht je nach Zustand an oder ausgeschlatet
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {
            if (isOn && water.GetComponent<Lab3WasserScript>().getIsSalzwasser())
            {
                pointLight.GetComponent<Light>().enabled = false;
                isOn = false;
                Gluehbirne.GetComponent<Renderer>().material = materialGluehbirneOFF;
            }
            else
            {
                //prüfen ob wasser leitet
                if (water.GetComponent<Lab3WasserScript>().getIsSalzwasser())
                {
                    pointLight.GetComponent<Light>().enabled = true;
                    isOn = true;
                    Gluehbirne.GetComponent<Renderer>().material = materialGluehbirneON;
                }
            }
        }


    }
}
