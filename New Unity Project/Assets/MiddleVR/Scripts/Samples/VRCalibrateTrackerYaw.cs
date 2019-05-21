/* VRCalibrateTrackerYaw
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;

[AddComponentMenu("MiddleVR/Samples/Calibrate Tracker Yaw")]
public class VRCalibrateTrackerYaw : MonoBehaviour {
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
            if (tracker != null)
            {
                float yaw = tracker.GetYaw();

                vrQuat neutralQ = new vrQuat();
                neutralQ.SetEuler(-yaw, 0.0f, 0.0f);
                vrQuat invNeutralQ = neutralQ.GetInverse();

                tracker.SetNeutralOrientation(invNeutralQ);
            }
        }
    }
}
