/* VRZSpaceUsageSample
 * MiddleVR
 * (c) MiddleVR
 *
 * This code is given as an example. You can do whatever you want with it
 * without any restriction.
 */

using UnityEngine;

using MiddleVR_Unity3D;

/// <summary>
/// Checks head/stylus visibility, and changes the LED color of the stylus or makes it vibrate.
///
/// Usage of keyboard keys:
/// - 'C' (like 'color') to go back and forth between the two colors.
/// - 'L' (like 'light') to turn on the LED.
/// - 'SHIFT' + 'L' to turn off the LED.
/// - 'V' (like 'vibration') to make the stylus vibrating.
/// - 'SHIFT' + 'V' to stop vibration.
/// - 'T' (like trigger) to trigger a vibration with default settings defined
///   in MiddleVRConfig.
///
/// The following vrCommand are introduced:
/// - "zSpace.SetStylusLEDColor"
/// - "zSpace.GetStylusLEDColor"
/// - "zSpace.SetStylusLEDAsTurnedOn"
/// - "zSpace.IsStylusLEDTurnedOn"
/// - "zSpace.StartStylusVibration"
/// - "zSpace.StopStylusVibration"
/// - "zSpace.TriggerDefaultStylusVibration" (legacy code support, prefer "StartStylusVibration").
/// </summary>
[AddComponentMenu("MiddleVR/Samples/zSpace Usage")]
public class VRZSpaceUsageSample : MonoBehaviour {

    [SerializeField]
    private Color m_LEDColor0;

    [SerializeField]
    private Color m_LEDColor1;

    [SerializeField]
    private float m_DurationVibration = 2.0f;

    [SerializeField]
    private float m_DurationBetween = 0.5f;

    [SerializeField]
    private int m_NumberOfVibrations= 3;

    private bool m_UsingLEDColor0 = true;

    private bool headTargetWasVisible = false;
    private bool stylusTargetWasVisible = false;

    protected void Start()
    {
        // LED Color component values are limited to 0 or 1. So let round them.
        m_LEDColor0 = RoundColorTo01(m_LEDColor0);
        m_LEDColor1 = RoundColorTo01(m_LEDColor1);
    }

    protected void Update()
    {
        var deviceMgr = MiddleVR.VRDeviceMgr;

        if (deviceMgr != null)
        {
            var headTracker = deviceMgr.GetTracker("zSpace.Head.Tracker");
            var stylusTracker = deviceMgr.GetTracker("zSpace.Stylus.Tracker");

            if (headTracker == null || stylusTracker == null)
            {
                MiddleVRTools.Log(0, "[X] No head or stylus tracker found for zSpace. Did you load the driver?");
                enabled = false;
                return;
            }

            bool headTargetIsVisible = headTracker.IsTracked();
            if (headTargetWasVisible != headTargetIsVisible)
            {
                MiddleVRTools.Log(2, "[+] Head is " + (headTargetIsVisible ? "visible" : "invisible") +
                    " by the zSpace device.");

                headTargetWasVisible = headTargetIsVisible;
            }

            bool stylusTargetIsVisible = stylusTracker.IsTracked();
            if (stylusTargetWasVisible != stylusTargetIsVisible)
            {
                MiddleVRTools.Log(2, "[+] Stylus is " + (stylusTargetIsVisible ? "visible" : "invisible") +
                    " by the zSpace device.");

                stylusTargetWasVisible = stylusTargetIsVisible;
            }
        }

        var kernel = MiddleVR.VRKernel;

        if (kernel != null && deviceMgr != null)
        {
            if (deviceMgr.IsKeyToggled(MiddleVR.VRK_C))
            {
                Color c = m_UsingLEDColor0 ? m_LEDColor0 : m_LEDColor1;

                vrVec3 proposedColor = new vrVec3(c.r, c.g, c.b);

                MiddleVRTools.Log(2, "[+] Proposed stylus LED color sent to zSpace: ("
                    + proposedColor.x() + ", " + proposedColor.y() + ", " + proposedColor.z() + ").");

                kernel.ExecuteCommand(
                    "zSpace.SetStylusLEDColor", proposedColor);

                m_UsingLEDColor0 = !m_UsingLEDColor0;

                vrVec3 usedColorAsRGB = GetStylusLEDColor();

                MiddleVRTools.Log(2, "[+] Current zSpace stylus LED color is: ("
                    + usedColorAsRGB.x() + ", " + usedColorAsRGB.y() + ", " + usedColorAsRGB.z() + ").");
            }

            if (deviceMgr.IsKeyToggled(MiddleVR.VRK_L))
            {
                bool turnOnLight = true;

                if (deviceMgr.IsKeyPressed(MiddleVR.VRK_LSHIFT) || deviceMgr.IsKeyPressed(MiddleVR.VRK_RSHIFT))
                {
                    turnOnLight = false;
                }

                MiddleVRTools.Log(2, "[+] Trying to turn " +
                    (turnOnLight ? "on" : "off") + " the zSpace stylus LED.");

                kernel.ExecuteCommand(
                    "zSpace.SetStylusLEDAsTurnedOn", turnOnLight);

                vrValue isLEDTurnedOnValue = kernel.ExecuteCommand(
                    "zSpace.IsStylusLEDTurnedOn", null);

                bool isLEDTurnedOn = isLEDTurnedOnValue.GetBool();

                MiddleVRTools.Log(2, "[+] zSpace stylus LED turned on? " +
                    (isLEDTurnedOn ? "Yes" : "No") + ".");

                if (isLEDTurnedOn)
                {
                    vrVec3 usedColorAsRGB = GetStylusLEDColor();

                    MiddleVRTools.Log(2, "[+] Current zSpace stylus LED color is: ("
                        + usedColorAsRGB.x() + ", " + usedColorAsRGB.y() + ", " + usedColorAsRGB.z() + "). If set to black, no light will appear!");
                }
            }

            if (deviceMgr.IsKeyToggled(MiddleVR.VRK_V))
            {
                if (deviceMgr.IsKeyPressed(MiddleVR.VRK_LSHIFT) || deviceMgr.IsKeyPressed(MiddleVR.VRK_RSHIFT))
                {
                    // Stop vibration (nothing will happen if the stylus
                    // is not vibrating).

                    MiddleVRTools.Log(2, "[+] Trying to stop zSpace stylus vibration.");

                    kernel.ExecuteCommand(
                        "zSpace.StopStylusVibration", null);
                }
                else
                {
                    // Start vibration.

                    MiddleVRTools.Log(2, "[+] Trying to start zSpace stylus vibration with parameters:\n" +
                        "- duration vibration: " + m_DurationVibration + " s,\n" +
                        "- duration between two vibrations: " + m_DurationBetween + " s,\n" +
                        "- number of vibrations: " + m_NumberOfVibrations + ".");

                    vrValue vibrationValueList = vrValue.CreateList();
                    vibrationValueList.AddListItem(m_DurationVibration);
                    vibrationValueList.AddListItem(m_DurationBetween);
                    vibrationValueList.AddListItem(m_NumberOfVibrations);
                    // It is also possible to set the intensity of the vibration:
                    // pass then a 4th argument with a float between 0.0 and 1.0.
                    //vibrationValueList.AddListItem(1.0f);

                    kernel.ExecuteCommand(
                        "zSpace.StartStylusVibration", vibrationValueList);
                }
            }

            if (deviceMgr.IsKeyToggled(MiddleVR.VRK_T))
            {
                MiddleVRTools.Log(2, "[+] Trying to trigger a default zSpace stylus vibration.");

                kernel.ExecuteCommand(
                    "zSpace.TriggerDefaultStylusVibration",
                    true);  // or false to stop the vibration (if any).
            }
        }
    }

    protected Color RoundColorTo01(Color c)
    {
        return new Color(
            c.r < 0.5f ? 0.0f : 1.0f,
            c.g < 0.5f ? 0.0f : 1.0f,
            c.b < 0.5f ? 0.0f : 1.0f,
            1.0f // no alpha
            );
    }

    protected vrVec3 GetStylusLEDColor()
    {
        vrValue usedColorValue = MiddleVR.VRKernel.ExecuteCommand(
            "zSpace.GetStylusLEDColor", null);

        return usedColorValue.GetVec3();
    }
}
