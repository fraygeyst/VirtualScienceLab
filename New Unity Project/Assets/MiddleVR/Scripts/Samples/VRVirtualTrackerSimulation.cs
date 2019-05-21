/* VRVirtualTrackerSimulation
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/Virtual Tracker Simulation")]
public class VRVirtualTrackerSimulation : MonoBehaviour
{
    public string m_VirtualTrackerName = "MyTracker";

    private bool m_IsInit = false;

    // The trackers
    private vrTracker m_Tracker = null;
    private vrAxis    m_Wiimote = null;


    protected void Start()
    {
        // Retrieve trackers by name
        m_Tracker = MiddleVR.VRDeviceMgr.GetTracker(m_VirtualTrackerName);

        m_Wiimote = MiddleVR.VRDeviceMgr.GetAxis("VRPNAxis0.Axis");

        if( m_Tracker == null )
        {
            MVRTools.Log("[X] VirtualTrackerMapping: Error : Can't find tracker '" + m_VirtualTrackerName + "'.");
        }

        if( m_Wiimote == null )
        {
            MVRTools.Log ("[X] Wiimote not found.");
        }

        if (m_Tracker != null && m_Wiimote != null )
        {
            m_IsInit = true;
        }
    }

    protected void Update()
    {
        if (m_IsInit)
        {
            m_Tracker.SetX (0.0f);
            m_Tracker.SetY (0.0f);
            m_Tracker.SetZ (0.0f);

            float yaw   = 0.0f;
            float pitch = MiddleVR.RadToDeg( Mathf.Asin( Mathf.Clamp (m_Wiimote.GetValue(2),-1,1) ) );
            float roll  = MiddleVR.RadToDeg( Mathf.Asin( Mathf.Clamp (m_Wiimote.GetValue(1),-1,1) ) );

            m_Tracker.SetYaw  ( yaw );
            m_Tracker.SetPitch( pitch );
            m_Tracker.SetRoll ( roll );
        }
    }
}
