using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sev_Bridges_Start : MonoBehaviour {

    public static GameObject player;
    public static bool start = true;
    public static bool bridge_1_act;
    public static bool bridge_2_act;
    public static bool bridge_3_act;
    public static bool bridge_4_act;
    public static bool bridge_5_act;
    public static bool bridge_6_act;
    public static bool bridge_7_act;
    public static int bridgecount;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "player")
        {
            if (start)
            {
                bridge_1_act = false;
                bridge_2_act = false;
                bridge_3_act = false;
                bridge_4_act = false;
                bridge_5_act = false;
                bridge_6_act = false;
                bridge_7_act = false;
                bridgecount = 0;
            }
        }
    }
}
