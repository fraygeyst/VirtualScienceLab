using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class BubbleSort : MonoBehaviour {

    public bool start = false;

    public GameObject sicht1;
    public GameObject sicht2;


    private void OnTriggerEnter(Collider other)
    {

        start = true;
    }


    // Use this for initialization
    void Start () {
		
	}


    // Update is called once per frame
    void Update()
    {
            if (start == true)
            {
                sicht1.SetActive(false);
                sicht2.SetActive(false);
            }
            else

            {

            }
        }
    }
 
