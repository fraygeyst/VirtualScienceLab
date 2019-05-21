/* VRVirtualTrackerMapping
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/Virtual Tracker Mapping")]
public class VRVirtualTrackerMapping : MonoBehaviour
{
    public string m_SourceTrackerName="VRPNTracker0.Tracker0";
    public string m_DestinationVirtualTrackerName="MyTracker";

    public bool UsePositionX = true;
    public bool UsePositionY = true;
    public bool UsePositionZ = true;

    public bool  UsePositionScale   = false;
    public float PositionScaleValue = 1.0f;

    public bool UseYaw       = true;
    public bool UsePitch     = true;
    public bool UseRoll      = true;

    private bool m_IsInit = false;

    // The trackers
    private vrTracker m_SourceTracker = null;
    private vrTracker m_DestinationVirtualTracker = null;

    protected void Start()
    {
        // Retrieve trackers by name
        m_SourceTracker             = MiddleVR.VRDeviceMgr.GetTracker(m_SourceTrackerName);
        m_DestinationVirtualTracker = MiddleVR.VRDeviceMgr.GetTracker(m_DestinationVirtualTrackerName);

        if (m_SourceTracker == null)
        {
            MVRTools.Log("[X] VirtualTrackerMapping: Error : Can't find tracker '"
                + m_SourceTrackerName + "'.");
        }
        if (m_DestinationVirtualTracker == null)
        {
            MVRTools.Log("[X] VirtualTrackerMapping: Error : Can't find tracker '" +
                m_DestinationVirtualTrackerName + "'.");
        }

        if (m_SourceTracker != null && m_DestinationVirtualTracker != null)
        {
            m_IsInit = true;
        }
    }

    protected void Update()
    {
        if (m_IsInit)
        {
            float scale = 1.0f;

            if (UsePositionScale)
            {
                scale = PositionScaleValue;
            }

            // Position
            //
            // Show how coordinates values can be changed when feeding a virtual tracker.
            //
            if (UsePositionX)
            {
                m_DestinationVirtualTracker.SetX(scale * m_SourceTracker.GetX());
            }
            if (UsePositionY)
            {
                m_DestinationVirtualTracker.SetZ(scale * m_SourceTracker.GetZ());
            }
            if (UsePositionZ)
            {
                m_DestinationVirtualTracker.SetY(scale * m_SourceTracker.GetY());
            }

            // Orientation
            //
            // Note that it is suggested to use quaternions if you do not need
            // to decompose a rotation.
            //
            if (UseYaw)
            {
                m_DestinationVirtualTracker.SetYaw(m_SourceTracker.GetYaw());
            }
            if (UsePitch)
            {
                m_DestinationVirtualTracker.SetPitch(m_SourceTracker.GetPitch());
            }
            if (UseRoll)
            {
                m_DestinationVirtualTracker.SetRoll(m_SourceTracker.GetRoll());
            }
        }
    }
}
