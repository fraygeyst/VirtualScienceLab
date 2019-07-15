using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


public class Styropor : MonoBehaviour {

    public GameObject styropor;
    public GameObject aceton;

    public bool collision = false;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if(collision == true)
        {
            Destroy(styropor,1f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == aceton)
        {
            collision = true;
        }
    }
}
