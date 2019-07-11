using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/**
 * Dieses Skript sorgt für ein Farbenwechsel/ kurzes Aufblinken um den Nutzer auf eine mögliche Interaktion hinzuweisen
 * */

public class ChangeColorScript : MonoBehaviour {

	//Farbauswahl
    public Material Materialcolor1;
    public Material Materialcolor2;
    
    //Aktuelle Sekundenzeit
    private int seconds = System.DateTime.Now.Second;


    // Use this for initialization
    void Start () {
        //Erste Farbe setzten
        gameObject.GetComponent<Renderer>().material = Materialcolor1;
	}

    void Update()
    {
        //Zeit abfragen
        int sec = System.DateTime.Now.Second;
        //falls sich die Zeit nicht geaendert hat soll die Farbei beibehalten werden
        if (seconds != sec)
        {
            gameObject.GetComponent<Renderer>().material = Materialcolor2;
           
        }else
        {
            //Ansonsten soll es kurz aufblinzeln
            gameObject.GetComponent<Renderer>().material = Materialcolor1;
            
        }
        seconds = sec;
    }
}
