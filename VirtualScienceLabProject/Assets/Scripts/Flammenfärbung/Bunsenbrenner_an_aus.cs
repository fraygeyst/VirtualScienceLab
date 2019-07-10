using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;


public class Bunsenbrenner_an_aus : MonoBehaviour {

    public GameObject Anschalter;
    public bool Brenneran = false;
    public ParticleSystem Feuer;
    public ParticleSystem Funken;
    public AudioSource Audio;

    // Variablen für die Flammenfärbung
    public GameObject spoon;
    public GameObject Calcium;
    public GameObject Kupfer;
    public GameObject Lithium;
    public GameObject Natrium;
    public ParticleSystem Ca;
    public ParticleSystem Cu;
    public ParticleSystem Li;
    public ParticleSystem Na;

    int counter = 0;


    // Use this for initialization
    void Start () {
        Feuer.Stop();
        Funken.Stop();
        Audio.Stop();
        Ca.Stop();
        Cu.Stop();
        Li.Stop();
        Na.Stop();

    }
	
	// Update is called once per frame
	void Update () {
     if (Brenneran == false) {
            Feuer.Stop();
            Funken.Stop();
            Audio.Stop();
            Ca.Stop();
            Cu.Stop();
            Li.Stop();
            Na.Stop();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (Brenneran == false)
        {
            Brenneran = true;
            Feuer.Play();
            Funken.Play();
            Audio.Play();
        }
        else
        {
            Brenneran = false;
            Feuer.Stop();
            Funken.Stop();
            Audio.Stop();
        }
    }

    //Setze den Counter je nach Collision
    private void OnCollisionEnter(Collision spoon)
    {

        if(spoon.collider.name == "Calcium")
        {
            counter = 1;
        }
        if (spoon.collider.name == "Kupfer")
        {
            counter = 2;
        }
        if (spoon.collider.name == "Lithium")
        {
            counter = 3;
        }
        if (spoon.collider.name == "Natrium")
        {
            counter = 4;
        }
        else
        {
            counter = 0;
        }
    }

    //Ändere die Flammenfärbung je nach Counter
    public void OnCollisionFeuer(Collision collision)
    {

        if (Brenneran == true)
        {
            if (counter == 1)
            {
                Feuer.Stop();
                Ca.Play();
                Cu.Stop();
                Li.Stop();
                Na.Stop();
            }

            if (counter == 2)
            {
                Feuer.Stop();
                Ca.Stop();
                Cu.Play();
                Li.Stop();
                Na.Stop();
            }

            if (counter == 3)
            {
                Feuer.Stop();
                Ca.Stop();
                Cu.Stop();
                Li.Play();
                Na.Stop();
            }

            if (counter == 1)
            {
                Feuer.Stop();
                Ca.Stop();
                Cu.Stop();
                Li.Stop();
                Na.Play();
            }

            else{ }


        }
    }

}
