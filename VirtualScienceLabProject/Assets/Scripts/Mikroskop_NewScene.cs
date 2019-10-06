using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class Mikroskop_NewScene : MonoBehaviour
{

    private bool collisionHappend = false;
    private int counter = 0;

    public GameObject pflanze;
    public GameObject blut;
    public GameObject zelle;


    private void OnTriggerEnter(Collider other)
    {
       collisionHappend = true;
       Debug.Log("collisionHappend: " + collisionHappend);

        if (other.gameObject.tag.Equals(pflanze))
        {
            Debug.Log("Pflanze eingelegt");
            counter = 1;
        }

        else if (other.gameObject.tag.Equals("blut"))
        {
            Debug.Log("Blut eingelegt");
            counter = 2;
        }

        else if (other.gameObject.tag.Equals(zelle))
        {
            Debug.Log("changing Scene zu Zelle");
            counter = 3;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("collisionHappend: " + collisionHappend);
    }


    private void Update()
    {
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend && (counter == 1)) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend && (counter == 1)))
        {
            Debug.Log("changing Scene zu Blatt");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Blatt");
        }
        else if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend && (counter == 2)) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend && (counter == 2)))
        {
            Debug.Log("changing Scene zu Blut");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Blut");
        }
        else if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend && (counter == 3)) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend && (counter == 3)))
        {
            Debug.Log("changing Scene zu Zelle");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Zelle");
        }
    }
}


