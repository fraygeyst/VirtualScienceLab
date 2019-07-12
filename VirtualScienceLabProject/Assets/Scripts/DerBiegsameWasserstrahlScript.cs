using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerBiegsameWasserstrahlScript : MonoBehaviour {

    private bool touchedPullover = false;
    private bool touchedWaschbecken = false;
    private GameObject pullover;
    private GameObject waschbecken;

	// Use this for initialization
	void Start () {
        pullover = GameObject.FindGameObjectWithTag("Pullover");
        waschbecken = GameObject.FindGameObjectWithTag("Waschbecken");

    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == pullover)
        {
            touchedPullover = true;
            Debug.Log("Pullover mit Stab beruehrt");
        }

      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == waschbecken)
        {
            touchedWaschbecken = true;
            Debug.Log("Stab im Waschbecken");
        }
    }

    private void OnTriggerExit(Collider other)
    {
          if (other.gameObject == waschbecken)
        {
            touchedWaschbecken = false; 
            Debug.Log("Stab NICHT mehr im Waschbecken");
        }
    }


    // Update is called once per frame
    void Update () {
       
		
	}

   public bool isTouchedWaschbecken()
    {
        return touchedWaschbecken;
    }

    public bool isTouchedPullover()
    {
        return touchedPullover;
    }
}
