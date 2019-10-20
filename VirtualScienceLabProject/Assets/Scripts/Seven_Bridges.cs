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

    private void OnTriggerEnter(Collider col)
    {
        if (Load_Publics.bridges_active)
        {
            if (Load_Publics.sev_bridges_counter < 7)
            {
                Load_Publics.sev_bridges_counter++;
                StartCoroutine(waiter());
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                StartCoroutine(reset());
                Load_Publics.sev_bridges_counter = 0;
            }
            Sev_Seg_Counter counti = new Sev_Seg_Counter();
            counti.setSevSegCount(Load_Publics.sev_bridges_counter);
        }
    }

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

    IEnumerator waiter()
    {
        Load_Publics.bridges_active = false;

        yield return new WaitForSeconds(2);    //Wait 2 Seconds

        Load_Publics.bridges_active = true;
    }
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