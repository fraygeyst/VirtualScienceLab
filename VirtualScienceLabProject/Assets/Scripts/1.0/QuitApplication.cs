using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/**
 * Dieses Script ermoeglicht es den Player in einem bestimmten Bereich nach oben Springen zu lassen und die Anwendung zu Beenden. 
 * Dabei wird dem VROrigin kurzzeitig die Rigidbody-Komponente hinzugefuegt und Mithilfe einer Physikalischen Kraft nach oben geschoben.
 * Oben angekommen Beendet sich die Anwendung.
 * */
public class QuitApplication : MonoBehaviour {

    public GameObject VROrigin;
    private bool collisionHappend = false;

    

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
            Application.Quit();
            // Esc is ignored in Editor playback mode
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        //Input.GetKeyUp(KeyCode.Escape) ||
        if (
            (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Menu) && ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger)) ||
            (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Menu) && ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger)))
        {
            Application.Quit();
            // Esc is ignored in Editor playback mode
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

    }

 

   
}
