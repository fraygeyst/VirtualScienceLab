using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/**
 * Script um das Licht z.B. an der Decke an/aus zuschalten
 */

public class TurnLightOn : MonoBehaviour
{
    void Start()
    {
        
    }

    //sobald man sich z.B. mit dem Controller in dem Bereich befinden wird die Variable auf true gesetzt
    private void OnTriggerEnter(Collider other)
    {
        Load_Publics.light_col = true;
        Debug.Log("collisionHappend: " + Load_Publics.light_col);
    }

    //sobald man sich z.B. mit dem Controller aus dem Bereich entfernt wird die Variable auf false gesetzt
    private void OnTriggerExit(Collider other)
    {
        Load_Publics.light_col = false;
        Debug.Log("collisionHappend: " + Load_Publics.light_col);
    }

    // Update is called once per frame
    void Update()
    {
        //Sollte man sich in dem Bereich befinden und auf den Trigger Button druecken wird das Licht je nach Zustand an oder ausgeschlatet
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && Load_Publics.light_col) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && Load_Publics.light_col))
        {
            if (Load_Publics.light_on)
            {
                Load_Publics.light_on = false;
            }
            else
            {
                Load_Publics.light_on = true;
            }
            foreach (string light in Load_Publics.lightnames)
            {
                testLight(light);
            }
        }
    }

    private void testLight(string lightname)
    {
        try
        {
            GameObject.Find(lightname).GetComponent<Light>().enabled = Load_Publics.light_on;   
        } catch
        {
            Debug.Log("No Light with name " + lightname + " found in this Scene");
        }
    }
}

