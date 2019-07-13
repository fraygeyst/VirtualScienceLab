using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterialToSpoonScript : MonoBehaviour {

    //Verschiende Materialen die man mit dem Löffel aufnehmen kann
    public Material natrium;
    public Material lithium;
    public Material kupfer;
    public Material calcium;

    public GameObject spoonInhalt;

	// Use this for initialization
	void Start () {
		
	}


    //Sobald man ein Material mit dem Löffel berührt, wird es in dem Löffel angezeigt/dargestellt
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("natrium"))
        {
            spoonInhalt.SetActive(true);
            spoonInhalt.GetComponent<Renderer>().material = natrium;
            Debug.Log(spoonInhalt.GetComponent<Renderer>().material.name);
            
        }
        else if (other.gameObject.CompareTag("lithium")) { 
            spoonInhalt.SetActive(true);
            spoonInhalt.GetComponent<Renderer>().material = lithium;
            Debug.Log(spoonInhalt.GetComponent<Renderer>().material.name);
        }
        else if (other.gameObject.CompareTag("kupfer")) { 
            spoonInhalt.SetActive(true);
            spoonInhalt.GetComponent<Renderer>().material = kupfer;
            Debug.Log(spoonInhalt.GetComponent<Renderer>().material.name);
        }
        else if (other.gameObject.CompareTag("calcium")) { 
            spoonInhalt.SetActive(true);
            spoonInhalt.GetComponent<Renderer>().material = calcium;
            Debug.Log(spoonInhalt.GetComponent<Renderer>().material.name);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
