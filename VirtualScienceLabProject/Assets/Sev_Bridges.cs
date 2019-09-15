using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sev_Bridges : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "player")
        {
            switch (gameObject.name)
            {
                case "Bridge_1":
                    bridge_1_act = true;
                    bridgecount++;
                    break;
                case "Bridge_2":
                    bridge_2_act = true;
                    bridgecount++;
                    break;
                case "Bridge_3":
                    bridge_3_act = true;
                    bridgecount++;
                    break;
                case "Bridge_4":
                    bridge_4_act = true;
                    bridgecount++;
                    break;
                case "Bridge_5":
                    bridge_5_act = true;
                    bridgecount++;
                    break;
                case "Bridge_6":
                    bridge_6_act = true;
                    bridgecount++;
                    break;
                case "Bridge_7":
                    bridge_7_act = true;
                    bridgecount++;
                    break;
            }
            Debug.Log("Bridgecount: " + gameObject.name + " - " + bridgecount);
        }
    }
}
