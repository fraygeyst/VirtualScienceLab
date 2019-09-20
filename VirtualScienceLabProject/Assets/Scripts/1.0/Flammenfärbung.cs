using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script zur Flammenfärbung


public class Flammenfärbung : MonoBehaviour {

   
    public GameObject spoonInhalt;
    public GameObject Anschalter;

    public Material Kupfer;
    public Material Lithium;
    public Material Calcium;
    public Material Natrium;

    // Use this for initialization
    void Start () {
		
	}


    //Sobald der Löffel sich in dem Flammen bereich befindet und zuvor ein entsprechendes Material aufgenommen hat, färbt sich die Flame in eine 
    //entsprechende Farbe
    private void OnTriggerEnter(Collider other)
    {
        Color m = new Color(0,0,0);

        if (other.gameObject.tag.Equals(spoonInhalt.tag))
        {
            Debug.Log("Spooninhalt im Feuer");
            
            m = other.gameObject.GetComponent<Renderer>().material.color;
       

        if (m.r != 0 && m.g != 0 && m.b != 0)
        {
         

            if (m.Equals(Calcium.color))
            {
                Anschalter.GetComponent<BrennerAnAusScript>().TurnCaFlameOn();
                Debug.Log("TurnCaFlameOn");

            }
            else if (m.Equals(Kupfer.color))
            {
                Anschalter.GetComponent<BrennerAnAusScript>().TurnCuFlameOn();
                Debug.Log("TurnCuFlameOn");
            }
            else if (m.Equals(Lithium.color))
            {
                Anschalter.GetComponent<BrennerAnAusScript>().TurnLiFlameOn();
                Debug.Log("TurnLiFlameOn");
            }
            else if (m.Equals(Natrium.color))
            {
                Anschalter.GetComponent<BrennerAnAusScript>().TurnNaFlameOn();
                Debug.Log("TurnNaFlameOn");

            }
            
        }

        }
    }



    //Sobald der Löffel aus dem Bereich wieder entzogen wird, und das Feuer an ist, brennt es normal weiter. 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == spoonInhalt)
        {
            
            Debug.Log("SpoonInhalt NICHT mehr in TriggerArea");
        }

        //falls man den bereich verlässt und das Feuer an war, Feuer anlassen! bzw Normales Feuer anmachen da der Löffen nicht mehr aufliegt
        if (Anschalter.GetComponent<BrennerAnAusScript>().getIsFireOn())
        {
            Anschalter.GetComponent<BrennerAnAusScript>().TurnAllOthersOff();

            //Loeffelinhalt verbrannt // entfernen
            //spoonInhalt.SetActive(false);


            Anschalter.GetComponent<BrennerAnAusScript>().TurnNormalFireOn();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
