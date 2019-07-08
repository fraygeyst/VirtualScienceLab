using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.ColliderEvent;


public class Bunsenbrenner_an_aus : MonoBehaviour {

    public GameObject Anschalter;
    public bool Brenneran = false;
    public ParticleSystem Feuer;
    public ParticleSystem Funken;
    public AudioSource Audio;

	// Use this for initialization
	void Start () {
        Feuer.Stop();
        Funken.Stop();
        Audio.Stop();

	}
	
	// Update is called once per frame
	void Update () {
     if (Brenneran == false) {
            Feuer.Stop();
            Funken.Stop();
            Audio.Stop();
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

}
