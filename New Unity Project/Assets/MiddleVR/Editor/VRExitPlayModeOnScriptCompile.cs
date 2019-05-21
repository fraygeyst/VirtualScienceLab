// Copyright Cape Guy Ltd. 2015. http://capeguy.co.uk.
// Provided under the terms of the MIT license -
// http://opensource.org/licenses/MIT. Cape Guy accepts
// no responsibility for any damages, financial or otherwise,
// incurred as a result of using this code.

// This code is strongly based on the code found at the following website:
// https://capeguy.co.uk/2015/06/no-more-unity-hot-reload/
//
// It was modified by MiddleVR to match MiddleVR's coding style and print
// messages related to MiddleVR directly.

using UnityEngine;
using UnityEditor;

/// <summary>
/// This script exits play mode whenever script compilation is detected during
/// an editor update.
/// </summary>
[InitializeOnLoad] // Make static initialiser be called as soon as the scripts
                   // are initialised in the editor (rather than just in play mode).
public class VRExitPlayModeOnScriptCompile
{
    private static VRExitPlayModeOnScriptCompile s_Instance = null;

    // Static initialiser called by Unity Editor whenever scripts are loaded
    // (editor or play mode).
    static VRExitPlayModeOnScriptCompile()
    {
        SilentlyUnused(s_Instance);
        s_Instance = new VRExitPlayModeOnScriptCompile();
    }

    // Called each time the editor updates (approx. 100 times per second).
    private static void OnEditorUpdate()
    {
        if (EditorApplication.isPlaying && EditorApplication.isCompiling)
        {
            EditorApplication.isPlaying = false;

            var msg =
              "MiddleVR does not support compiling scripts when playing a scene." +
              " Stopped playing.";

            Debug.LogWarning(msg);

            EditorUtility.DisplayDialog("Play mode left", msg, "Close");
        }
    }

    // Used to silence the 'is assigned by its value is never used' warning
    // for s_Instance.
    private static void SilentlyUnused<T>(T SilentlyUnusedVariable)
    {
    }

    private VRExitPlayModeOnScriptCompile()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    ~VRExitPlayModeOnScriptCompile()
    {
        EditorApplication.update -= OnEditorUpdate;
        s_Instance = null;
    }
}
