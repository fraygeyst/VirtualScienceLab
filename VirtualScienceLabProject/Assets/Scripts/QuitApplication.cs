using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/// <summary>
/// Erstes Beispiel einer C# Klasse in einer Unity-Anwendung
/// </summary>
public class QuitApplication : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Input.GetKeyUp(KeyCode.Escape) ||
        if (
            ( ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Menu) && ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.HairTrigger) ) ||
            ( ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Menu) && ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.HairTrigger) )  )
        {
            Application.Quit();
            // Esc is ignored in Editor playback mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }


}
