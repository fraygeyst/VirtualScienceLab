using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *Bei Collision wird der Rigidbody und Boxcollider von VROrigin wieder entfernt.
 * */

public class DetectCollisionScript : MonoBehaviour {

    public GameObject VROrigin;
    

    private void OnTriggerEnter(Collider other)
    {

        if (VROrigin.GetComponent<Rigidbody>() != null)
        {
            Destroy(VROrigin.GetComponent<Rigidbody>());
            Debug.Log("rigidbody von VROrigin entfernt");
        }

        if (VROrigin.GetComponent<BoxCollider>() != null)
        {
            Destroy(VROrigin.GetComponent<BoxCollider>());
            Debug.Log("BoxCollider von VROrigin entfernt");
        }


    }

 
}
