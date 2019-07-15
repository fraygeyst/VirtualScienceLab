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
    public GameObject cam;

    //CollisionsBereich damit der Player nicht durch das Terrain durchbricht.
    public GameObject collisionAreaGO;
    private bool collisionHappend = false;
    private Transform transform;
    


    // Use this for initialization
    void Start()
    {
       
    }

    //Entfernt Rigidbody und BoxCollider falls der Spieler durch das Terrain bricht und stoppt ihn somit rechtzeitig.
    private void RemoveComponents()
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

    private void OnTriggerStay(Collider other)
    {
        collisionHappend = true;
        //Debug.Log("JumpScript collisionHappend: " + collisionHappend);

      

    }

 
    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("JumpScript collisionHappend: " + collisionHappend);
        transform = VROrigin.GetComponent<Transform>();
        

       // if (VROrigin.GetComponent<Transform>().position.y <= 25.0f)
        {
       //     RemoveComponents();
            
        }

    }

   
   
    // Update is called once per frame
    void Update()
    {

        
        //Sobald es zur Kollision mit dem Boden kommt werden die Komponenten entfernt und der Player wird auf seien Urspruengliche Position zurueckgesetzt
        if (collisionAreaGO.GetComponent<DetectCollisionScript>().getCollisionWithVROriginDetected()) {
            RemoveComponents();
            Vector3 oldPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            Quaternion rot = new Quaternion(0,0,0,0);
            VROrigin.GetComponent<Transform>().position = oldPos;
            VROrigin.GetComponent<Transform>().rotation = rot;
        }






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
