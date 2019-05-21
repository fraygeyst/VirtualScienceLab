/* VRCalibrateTracker
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;

[AddComponentMenu("MiddleVR/Samples/Calibrate Tracker")]
public class VRCalibrateTracker : MonoBehaviour {
    public string Tracker = "VRPNTracker0.Tracker0";

    protected void Update()
    {
        vrTracker tracker   = null;
        vrKeyboard keyboard = null;

        var deviceMgr = MiddleVR.VRDeviceMgr;

        if (deviceMgr != null)
        {
            tracker  = deviceMgr.GetTracker(Tracker);
            keyboard = deviceMgr.GetKeyboard();
        }

        if (keyboard != null && keyboard.IsKeyToggled(MiddleVR.VRK_SPACE))
        {
            if( tracker != null )
            {
                tracker.SetNeutralOrientation(tracker.GetOrientation());
            }
        }
    }
}
