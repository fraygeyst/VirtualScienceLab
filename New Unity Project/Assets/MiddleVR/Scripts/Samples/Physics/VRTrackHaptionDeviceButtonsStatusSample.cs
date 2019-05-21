/* VRTrackHaptionDeviceButtonsStatusSample
 * Written by MiddleVR.
 *
 * This code is given as an example. You can do whatever you want with it
 * without any restriction.
 */

using UnityEngine;
using System.Collections;

using MiddleVR_Unity3D;

/// <summary>
/// Print status of Haption device buttons.
///
/// You simply need to attach this Component to one GameObject in the scene.
/// </summary>
[AddComponentMenu("MiddleVR/Samples/Physics/Track Haption Device Buttons Status")]
public class VRTrackHaptionDeviceButtonsStatusSample : MonoBehaviour {

    #region Member Variables
    private vrObject haptionDriver = null;
    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        haptionDriver = MiddleVR.VRKernel.GetObject("Haption Driver");

        if (haptionDriver == null)
        {
            MiddleVRTools.Log(0, "[X] TrackHaptionDeviceButtonsStatus: No driver Haption found!");
            enabled = false;

            return;
        }
    }

    protected void Update()
    {
        int haptionDeviceId = 0;

        string haptionDeviceNameBase = "Haption" + haptionDeviceId;

        vrButtons buttons = MiddleVR.VRDeviceMgr.GetButtons(haptionDeviceNameBase + ".Buttons");

        for (uint i = 0, iEnd = buttons.GetButtonsNb(); i < iEnd; ++i )
        {
            if (buttons.IsToggled(i, true))
            {
                MiddleVRTools.Log(2, "[+] Haption button '" + i + "' is pressed.");
            }
            else if (buttons.IsToggled(i, false))
            {
                MiddleVRTools.Log(2, "[+] Haption button '" + i + "' is released.");
            }
        }
    }

    #endregion
}
