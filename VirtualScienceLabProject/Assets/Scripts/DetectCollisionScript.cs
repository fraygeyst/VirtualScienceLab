using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script um zu pruefen ob der Player durch das gruene Feld im Terrain durchgebrochen ist.
 * Die Variable CollisionWithVROriginDetected kann von außen ueberprueft werden.
 * */

public class DetectCollisionScript : MonoBehaviour {

    public GameObject VROrigin;
    private bool CollisionWithVROriginDetected = false;

	// Use this for initialization
	void Start () {
		
	}

  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == VROrigin)
        {
            CollisionWithVROriginDetected = true;
            Debug.Log("DetectionCollisionScript : " + CollisionWithVROriginDetected);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == VROrigin)
        {
            CollisionWithVROriginDetected = false;
            Debug.Log("DetectionCollisionScript : " + CollisionWithVROriginDetected);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public bool getCollisionWithVROriginDetected()
    {
        return CollisionWithVROriginDetected;
    }
}
