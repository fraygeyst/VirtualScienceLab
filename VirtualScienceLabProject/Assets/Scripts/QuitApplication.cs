using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
            // Esc is ignored in Editor playback mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }


}
