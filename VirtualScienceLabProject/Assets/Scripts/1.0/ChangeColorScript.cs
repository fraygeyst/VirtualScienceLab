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
        //falls sich die Zeit nicht geaendert hat soll die Farbe beibehalten werden
        if (seconds != sec)
        {
            gameObject.GetComponent<Renderer>().material = Materialcolor2;
           
        }else
        {
            //Ansonsten soll es kurz aufblitzen
            gameObject.GetComponent<Renderer>().material = Materialcolor1;
            
        }
        seconds = sec;
    }
}

/**
 * public class ChangeColorScript : MonoBehaviour
{

    //Farbauswahl
    public Material Materialcolor1;
    public Material Materialcolor2;
    private bool isMaterialcolor1;
   


    // Use this for initialization
    void Start()
    {
        //Erste Farbe setzten
        gameObject.GetComponent<Renderer>().material = Materialcolor1;
        isMaterialcolor1 = true;
    }

    void Update()
    {
       
       
        if (isMaterialcolor1)
        {
            
            StartCoroutine(waitAndSetOtherMaterial(1, Materialcolor2));
        }
        else
        {
            StartCoroutine(waitAndSetOtherMaterial(1, Materialcolor1));

        }
       
    }

    IEnumerator waitAndSetOtherMaterial(int sec, Material m)
    {
        yield return new WaitForSeconds(sec);
        //Ansonsten soll es kurz aufblinzeln
       gameObject.GetComponent<Renderer>().material = m;
        isMaterialcolor1 = !isMaterialcolor1;
       
    }
}

 * 
 * 
 * */
