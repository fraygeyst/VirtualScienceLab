using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class Mikroskop_NewScene : MonoBehaviour
{

    private bool collisionHappend = false;
    private string counter;

    public GameObject pflanze;
    public GameObject blut;
    public GameObject zelle;

 
    private void OnTriggerEnter(Collider other)
    {
       collisionHappend = true;
       Debug.Log("collisionHappend: " + collisionHappend);

        /*if (other.gameObject.tag.Equals(pflanze.tag))
        {
            Debug.Log("Pflanze eingelegt");
            counter = "pflanze";
        }

        else if (other.gameObject.tag.Equals(blut.tag))
        {
            Debug.Log("Blut eingelegt");
            counter = "blut";
        }

        else if (other.gameObject.tag.Equals(zelle.tag))
        {
            Debug.Log("Zelle eingelegt");
            counter = "zelle";
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(pflanze.tag))
        {
            Debug.Log("Pflanze eingelegt");
            counter = "pflanze";
        }

        else if (collision.gameObject.tag.Equals(blut.tag))
        {
            Debug.Log("Blut eingelegt");
            counter = "blut";
        }

        else if (collision.gameObject.tag.Equals(zelle.tag))
        {
            Debug.Log("Zelle eingelegt");
            counter = "zelle";
        }

    }

    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("collisionHappend: " + collisionHappend);
    }


    private void Update()
    {
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend && (counter == "pflanze")) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend && (counter == "pflanze")))
        {
            Debug.Log("changing Scene zu Blatt");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Blatt");
        }
        else if (((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && (counter == "blut") && collisionHappend)) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend && (counter == "blut")))
        {
            Debug.Log("changing Scene zu Blut");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Blut");
        }
        else if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend && (counter == "zelle")) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend && (counter == "zelle")))
        {
            Debug.Log("changing Scene zu Zelle");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Zelle");
        }
    }
}


