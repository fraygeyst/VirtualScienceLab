using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/**
 * Dieses Script ermoeglicht es den Player in einem bestimmten Bereich nach oben Springen zu lassen. 
 * Dabei wird dem VROrigin kurzzeitig die Rigidbody-Komponente hinzugefuegt und Mithilfe einer Physikalischen Kraft nach oben geschoben.
 * */
public class JumpScript : MonoBehaviour {

    public GameObject VROrigin;
    private bool collisionHappend = false;

    


    // Use this for initialization
    void Start()
    {
       
    }

 

    private void OnTriggerStay(Collider other)
    {
        collisionHappend = true;

    }

 
    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
    

    }

   
   
    // Update is called once per frame
    void Update()
    {


        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {
            

            if (VROrigin.GetComponent<Rigidbody>() == null)
            {
                VROrigin.AddComponent<Rigidbody>();
                
            }
            if (VROrigin.GetComponent<BoxCollider>() == null)
            {
                VROrigin.AddComponent<BoxCollider>();
      
            }
           
            VROrigin.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1000, 0));
           
           


        }


    }

 

   
}
