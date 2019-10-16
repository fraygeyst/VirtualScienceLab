using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beschleunigung : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        Sev_Seg_Counter temp = new Sev_Seg_Counter();
        temp.setSevSegCount(Load_Publics.Temperatur);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (gameObject.name)
        {
            case "plus":
                Load_Publics.Temperatur += 25;
                Debug.Log("wärmer");
                break;
            case "minus":
                Load_Publics.Temperatur -= 25;
                Debug.Log("kälter");
                break;
        }
    }
}
