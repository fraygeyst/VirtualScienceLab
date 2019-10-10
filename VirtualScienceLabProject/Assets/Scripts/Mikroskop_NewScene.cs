using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class Mikroskop_NewScene : MonoBehaviour
{

    public GameObject pflanze;
    public GameObject blut;
    public GameObject zelle;

 
    private void OnTriggerEnter(Collider other)
    {
        Load_Publics.bio_collision_happened = true;
       Debug.Log("collisionHappend: " + Load_Publics.bio_collision_happened);

        if (other.gameObject.name == "pflanze_com" || other.gameObject.name == "pflanze" || other.gameObject.name == "pflanze_scheibe")
        {
            Debug.Log("Pflanze eingelegt");
            Load_Publics.scene_change = "pflanze";
        }

        else if (other.gameObject.name == "blut_com" || other.gameObject.name == "blut" || other.gameObject.name == "blut_scheibe")
        {
            Debug.Log("Blut eingelegt");
            Load_Publics.scene_change = "blut";
        }

        else if (other.gameObject.name == "zelle_com" || other.gameObject.name == "zelle" || other.gameObject.name == "zelle_scheibe")
        {
            Debug.Log("Zelle eingelegt");
            Load_Publics.scene_change = "zelle";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "pflanze_com" || collision.gameObject.name == "pflanze" || collision.gameObject.name == "pflanze_scheibe")
        {
            Debug.Log("Pflanze eingelegt");
            Load_Publics.scene_change = "pflanze";
        }

        else if (collision.gameObject.name == "blut_com" || collision.gameObject.name == "blut" || collision.gameObject.name == "blut_scheibe")
        {
            Debug.Log("Blut eingelegt");
            Load_Publics.scene_change = "blut";
        }

        else if (collision.gameObject.name == "zelle_com" || collision.gameObject.name == "zelle" || collision.gameObject.name == "zelle_scheibe")
        {
            Debug.Log("Zelle eingelegt");
            Load_Publics.scene_change = "zelle";
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Load_Publics.bio_collision_happened = false;
        Debug.Log("collisionHappend: " + Load_Publics.bio_collision_happened);
    }


    private void Update()
    {
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && Load_Publics.bio_collision_happened && Load_Publics.scene_change == "pflanze") || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && Load_Publics.bio_collision_happened && Load_Publics.scene_change == "pflanze"))
        {
            Debug.Log("changing Scene zu Blatt");
            Load_Publics.scene_change = "";
            UnityEngine.SceneManagement.SceneManager.LoadScene("Blatt");
        }
        else if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && Load_Publics.scene_change == "blut" && Load_Publics.bio_collision_happened) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && Load_Publics.bio_collision_happened && Load_Publics.scene_change == "blut"))
        {
            Debug.Log("changing Scene zu Blut");
            Load_Publics.scene_change = "";
            UnityEngine.SceneManagement.SceneManager.LoadScene("Blut");
        }
        else if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && Load_Publics.bio_collision_happened && Load_Publics.scene_change == "zelle") || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && Load_Publics.bio_collision_happened && Load_Publics.scene_change == "zelle"))
        {
            Debug.Log("changing Scene zu Zelle");
            Load_Publics.scene_change = "";
            UnityEngine.SceneManagement.SceneManager.LoadScene("Zelle");
        }
    }
}


