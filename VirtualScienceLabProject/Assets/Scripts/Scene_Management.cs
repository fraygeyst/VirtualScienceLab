using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Management : MonoBehaviour {
    GameObject camera_obj;
    Transform transform;

    // Use this for initialization
    void Start () {
        Scene scene = SceneManager.GetActiveScene();
        camera_obj = GameObject.Find("ViveRig");
        transform = camera_obj.GetComponent<Transform>();
        Vector3 vec = new Vector3();
        vec = new Vector3(3.291f, 0f, 4.134f);

        if (scene.name == "NeuerFlur")
        {
            switch (Globals.last_scene)
            {
                case "BioLab":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "Elektro":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "GeoLab":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "grosserRaum":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "Lab1":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "Lab2":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "Lab3":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "Teilchenlabor":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
                case "VRLab":
                    vec = new Vector3(3.291f, 0f, 4.134f);
                    break;
            }
        } else
        {
            Globals.last_scene = scene.name;
        }
        camera_obj.transform.Translate(vec);
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Test: " + transform);
	}
}
