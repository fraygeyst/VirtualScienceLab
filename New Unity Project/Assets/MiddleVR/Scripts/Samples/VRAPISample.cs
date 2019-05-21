/* VRInteractionTest
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/API")]
public class VRInteractionTest : MonoBehaviour {

    protected void Update()
    {
        TestWand();
        TestKeyboardMouse();
        TestDevices();
        TestDisplay();
    }

    private void TestWand()
    {
        var deviceMgr = MiddleVR.VRDeviceMgr;

        if (deviceMgr != null)
        {
            // Getting wand horizontal axis
            float x = deviceMgr.GetWandHorizontalAxisValue();
            // Getting wand vertical axis
            float y = deviceMgr.GetWandVerticalAxisValue();

            // Getting state of primary wand button
            bool b0 = deviceMgr.IsWandButtonPressed(0);

            // Getting toggled state of primary wand button
            // bool t0 = deviceMgr.IsWandButtonToggled(0);

            if (b0 == true)
            {
                // If primary button is pressed, display wand horizontal axis value
                MVRTools.Log("WandButton 0 pressed! HAxis value: " + x + ", VAxis value: " + y );
            }
        }
    }

    private void TestKeyboardMouse()
    {
        var deviceMgr = MiddleVR.VRDeviceMgr;

        if (deviceMgr != null)
        {
            // Testing mouse button
            if (deviceMgr.IsMouseButtonPressed(0))
            {
                MVRTools.Log("Mouse Button pressed!");
                MVRTools.Log("VRMouseX : " + deviceMgr.GetMouseAxisValue(0));
            }

            // Testing keyboard key
            if (deviceMgr.IsKeyPressed(MiddleVR.VRK_SPACE))
            {
                MVRTools.Log("Space!");
            }
        }
    }

    private void TestDevices()
    {
        vrTracker tracker = null;
        vrJoystick    joy = null;
        vrAxis       axis = null;
        vrButtons buttons = null;

        var deviceMgr = MiddleVR.VRDeviceMgr;

        // Getting a reference to different device types
        if (deviceMgr != null)
        {
            tracker = deviceMgr.GetTracker("VRPNTracker0.Tracker0");
            joy     = deviceMgr.GetJoystickByIndex(0);
            axis    = deviceMgr.GetAxis("VRPNAxis0.Axis");
            buttons = deviceMgr.GetButtons("VRPNButtons0.Buttons");
        }

        // Getting tracker data
        if( tracker != null )
        {
              MVRTools.Log("TrackerX : " + tracker.GetPosition().x() );
        }

        // Testing joystick button
        if (joy != null && joy.IsButtonPressed(0))
        {
            MVRTools.Log("Joystick!");
        }

        // Testing axis value
        if( axis != null && axis.GetValue(0) > 0 )
        {
            MVRTools.Log("Axis Value: " + axis.GetValue(0));
        }

        // Testing button state
        if (buttons != null)
        {
            if (buttons.IsToggled(0))
            {
                MVRTools.Log("Button 0 pressed !");
            }

            if (buttons.IsToggled(0, false))
            {
                MVRTools.Log("Button 0 released !");
            }
        }
    }

    private void TestDisplay()
    {
        var displayMgr = MiddleVR.VRDisplayMgr;

        if (displayMgr != null)
        {
            vrNode3D node = displayMgr.GetNode("HeadNode");
            if (node != null)
            {
                MVRTools.Log("Found HeadNode");
            }

            vrCamera cam = displayMgr.GetCamera("Camera0");
            if (cam != null)
            {
                MVRTools.Log("Found Camera0");
            }

            vrCameraStereo sCam = displayMgr.GetCameraStereo("CameraStereo0");
            if (sCam != null)
            {
                MVRTools.Log("Found CameraStereo0");
            }

            vrScreen screen = displayMgr.GetScreen("Screen0");
            if (screen != null)
            {
                MVRTools.Log("Found Screen0");
            }

            vrViewport vp = displayMgr.GetViewport("Viewport0");
            if (vp != null)
            {
                MVRTools.Log("Found Viewport0");
            }
        }
    }
}
