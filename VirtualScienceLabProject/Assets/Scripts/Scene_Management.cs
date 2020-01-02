using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Management : MonoBehaviour {
    GameObject camera_obj;

    // Use this for initialization
    void Start () {
		// Aktive Szene auslesen
        Scene scene = SceneManager.GetActiveScene();
		// Player initialisieren
        camera_obj = GameObject.Find("ViveRig");
        Vector3 vec = new Vector3();

// Überprüfen ob Szene == NeuerFlur
        if (scene.name == "NeuerFlur" && Globals.last_scene != "")
        {
			// Player Position ändern
            Debug.Log("Switching Cam from: " + Globals.last_scene);
            switch (Globals.last_scene)
            {
                case "BioLab":
                    vec = new Vector3(12.0f, 0f, -1.4f);
                    break;
                case "grosserRaum": // Mathe
                    vec = new Vector3(9.2f, 0f, 0f);
                    break;
                case "Lab1": // Chemie
                    vec = new Vector3(22.0f, 0f, -1.4f);
                    break;
                case "Lab2": // Physik
                    vec = new Vector3(22.0f, 0f, 1.4f);
                    break;
                case "Lab3": // Informatik
                    vec = new Vector3(16.9f, 0f, 1.4f);
                    break;
                case "Teilchenlabor":
                    vec = new Vector3(12.0f, 0f, 1.4f);
                    break;
                case "VRLab":
                    vec = new Vector3(16.9f, 0f, 1.4f);
                    break;
            }
            Debug.Log("Changing Position: " + vec);
            camera_obj.transform.position = vec;
        } else
        {
			// Last Szene setzen
            Globals.last_scene = scene.name;
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    


}
