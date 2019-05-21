/* VRShortcutInvertEyes
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/Shortcut Invert-Eyes")]
public class VRShortcutInvertEyes : MonoBehaviour
{
    protected void Update()
    {
        vrKeyboard keyboard = MiddleVR.VRDeviceMgr.GetKeyboard();

        // Invert eyes.
        if (keyboard != null &&
            keyboard.IsKeyToggled(MiddleVR.VRK_I) &&
            (keyboard.IsKeyPressed(MiddleVR.VRK_LSHIFT) || keyboard.IsKeyPressed(MiddleVR.VRK_RSHIFT)))
        {
            var displayMgr = MiddleVR.VRDisplayMgr;

            // For each vrCameraStereo, invert inter eye distance.
            for (uint i = 0, iEnd = displayMgr.GetCamerasNb(); i < iEnd; ++i)
            {
                vrCamera cam = displayMgr.GetCameraByIndex(i);
                if (cam.IsA("CameraStereo"))
                {
                    vrCameraStereo stereoCam = displayMgr.GetCameraStereoById(cam.GetId());
                    stereoCam.SetInterEyeDistance( -stereoCam.GetInterEyeDistance() );
                }
            }
        }
    }
}
