using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class Seven_Bridges : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

	// COntroller Input abfangen
    private void OnTriggerEnter(Collider col)
    {
		// Falls aktiv
        if (Load_Publics.bridges_active)
        {
			// Wenn Counter kleiner 7
            if (Load_Publics.sev_bridges_counter < 7)
            {
				// COunter erhöhen
                Load_Publics.sev_bridges_counter++;
				// Corountine Input deaktivieren
                StartCoroutine(waiter());
				// Brücke einfärben
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
				// Neustart
                StartCoroutine(reset());
                Load_Publics.sev_bridges_counter = 0;
            }
            Sev_Seg_Counter counti = new Sev_Seg_Counter();
            counti.setSevSegCount(Load_Publics.sev_bridges_counter, "Zähler");
        }
    }

	// Alle Brücken färben
    private void colorAll(bool isRed)
    {
        if (isRed)
        {
            GameObject.Find("Bridge_1").GetComponent<Renderer>().material.color = Color.red;
            GameObject.Find("Bridge_2").GetComponent<Renderer>().material.color = Color.red;
            GameObject.Find("Bridge_3").GetComponent<Renderer>().material.color = Color.red;
            GameObject.Find("Bridge_4").GetComponent<Renderer>().material.color = Color.red;
            GameObject.Find("Bridge_5").GetComponent<Renderer>().material.color = Color.red;
            GameObject.Find("Bridge_6").GetComponent<Renderer>().material.color = Color.red;
            GameObject.Find("Bridge_7").GetComponent<Renderer>().material.color = Color.red;
        } else
        {
            GameObject.Find("Bridge_1").GetComponent<Renderer>().material.color = Color.white;
            GameObject.Find("Bridge_2").GetComponent<Renderer>().material.color = Color.white;
            GameObject.Find("Bridge_3").GetComponent<Renderer>().material.color = Color.white;
            GameObject.Find("Bridge_4").GetComponent<Renderer>().material.color = Color.white;
            GameObject.Find("Bridge_5").GetComponent<Renderer>().material.color = Color.white;
            GameObject.Find("Bridge_6").GetComponent<Renderer>().material.color = Color.white;
            GameObject.Find("Bridge_7").GetComponent<Renderer>().material.color = Color.white;
        }
        
    }

	// Deaktiviert für 2 Sekunden die Eingabe
    IEnumerator waiter()
    {
        Load_Publics.bridges_active = false;

        yield return new WaitForSeconds(2);    //Wait 2 Seconds

        Load_Publics.bridges_active = true;
    }
	// Setzt alles zurück und lässt Brücken rot blinken
    IEnumerator reset()
    {
        Load_Publics.bridges_active = false;
        for (int i = 0; i < 5; i++)
        {
            colorAll(true);
            yield return new WaitForSeconds(0.5f);    //Wait 0.5 Seconds
            colorAll(false);
            yield return new WaitForSeconds(0.5f);    //Wait 0.5 Seconds
        }
        Load_Publics.bridges_active = true;
    }
}