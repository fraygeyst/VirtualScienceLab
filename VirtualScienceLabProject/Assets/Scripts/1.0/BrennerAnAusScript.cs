using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;



/** Dieses Script steuert den Bunsenbrenner. Dieser lässt sich über den grünen Button ein und ausschalten.
 * 
 * */

public class BrennerAnAusScript : MonoBehaviour {

    private bool isFireOn = false;
    private bool collisionHappend = false;

    //basis Feuer mit Audio und Funken bei betätigung des grünnen schalters wird dies eingschaltet
    public GameObject Feuer;
    public GameObject Funken;
    public GameObject Audio;

    //verschiedene Flammen die jeweils benötigt weren
    public GameObject Ca;
    public GameObject Cu;
    public GameObject Li;
    public GameObject Na;
    public GameObject smallFires;




    // Use this for initialization
    void Start () {
        Feuer.SetActive(false);
        isFireOn = false;
        Funken.SetActive(false);
        Audio.SetActive(false);
        Ca.SetActive(false);
        Cu.SetActive(false);
        Li.SetActive(false);
        Na.SetActive(false);

        smallFires.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        collisionHappend = true;
        Debug.Log("BrennerAnAus collisionHappend: " + collisionHappend);
    }

    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("BrennerAnAus collisionHappend: " + collisionHappend);
    }


    // Update is called once per frame
    void Update()
    {
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {

            if (isFireOn)
            {
                TurnFireOff();
            }
            else

            {
                TurnNormalFireOn();
               
            }
        }
    }


    //Basis Feuer einschalten + Audio + Funken
    public void TurnNormalFireOn()
    {
        smallFires.SetActive(true);
        

        Audio.SetActive(true);
        Audio.GetComponent<AudioSource>().Play();
        Feuer.SetActive(true);
        Feuer.GetComponent<ParticleSystem>().Play();
        Funken.SetActive(true);
        Funken.GetComponent<ParticleSystem>().Play();

        isFireOn = true;
        Debug.Log("Feuer AN");
    }


    //Schaltet gesamten Bunsenbrenner mit alle Feuervarianten aus
    public void TurnFireOff()
    {
        Feuer.SetActive(false);
        Funken.SetActive(false);
        Audio.SetActive(false);

        Ca.SetActive(false);
        Cu.SetActive(false);
        Li.SetActive(false);
        Na.SetActive(false);

        isFireOn = false;
      


        smallFires.SetActive(false);

        Debug.Log("Feuer AUS");
    }


    //Schaltet nur die zusätzlichen Feuervarianten aus
    public void TurnAllOthersOff()
    {
        Ca.SetActive(false);
        Cu.SetActive(false);
        Li.SetActive(false);
        Na.SetActive(false);
   
    }

    


    //Einschalten des Natriumfeuers
    public void TurnNaFlameOn() {

        smallFires.SetActive(true);

        Feuer.SetActive(false);
        Funken.SetActive(false);
        Ca.SetActive(false);    
        Cu.SetActive(false);
        Li.SetActive(false);

        Audio.SetActive(true);
        Audio.GetComponent<AudioSource>().Play();
        isFireOn = true; 

        Na.SetActive(true);
        Na.GetComponent<ParticleSystem>().Play();

        Debug.Log("Feuer NATRIUM An");
    }

    //Einschalten des Caliumfeuers
    public void TurnCaFlameOn()
    {
        smallFires.SetActive(true);

        Feuer.SetActive(false);
        Funken.SetActive(false);
        Na.SetActive(false);
        Cu.SetActive(false);
        Li.SetActive(false);


        Audio.SetActive(true);
        Audio.GetComponent<AudioSource>().Play();
        isFireOn = true;

        Ca.SetActive(true);
        Ca.GetComponent<ParticleSystem>().Play();

        Debug.Log("Feuer CALIUM An");
    }

    //Einschalten des Kupferfeuers
    public void TurnCuFlameOn()
    {
        smallFires.SetActive(true);

        Feuer.SetActive(false);
        Funken.SetActive(false);
        Ca.SetActive(false);
        Na.SetActive(false);
        Li.SetActive(false);


        Audio.SetActive(true);
        Audio.GetComponent<AudioSource>().Play();
        isFireOn = true;

        Cu.SetActive(true);
        Cu.GetComponent<ParticleSystem>().Play();

        Debug.Log("Feuer KUPFER An");
    }

    //Einschalten des Lithiumfeuers
    public void TurnLiFlameOn()
    {
        smallFires.SetActive(true);

        Feuer.SetActive(false);
        Funken.SetActive(false);
        Ca.SetActive(false);
        Cu.SetActive(false);
        Na.SetActive(false);



        Audio.SetActive(true);
        Audio.GetComponent<AudioSource>().Play();
        isFireOn = true;

        Li.SetActive(true);
        Li.GetComponent<ParticleSystem>().Play();

        Debug.Log("Feuer LITHIUM An");
    }

    //Prüft ob ein Feuer überhaubt an ist
    public bool getIsFireOn()
    {
        return isFireOn;
    }


}
