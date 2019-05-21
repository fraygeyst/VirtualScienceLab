/* VRApplyForceTorqueSample
 * Written by MiddleVR.
 *
 * This code is given as an example. You can do whatever you want with it
 * without any restriction.
 */

using UnityEngine;

using MiddleVR_Unity3D;

/// <summary>
/// Apply a force or a torque on the physics body relative to the owning GameObject.
/// 
/// The purpose of this class is only to illustrate how to apply a force or a
/// torque on the physics body associated to the GameObject this component is
/// member of.
///
/// Usage:
/// press keyboard keys 'h' and 'f' or 't.
/// The keys are this meaning: 'h' stands for 'haptics', 'f' for 'force',
/// 't' for torque.
/// In addition, pressing a shift key will apply the reverse force or torque.
///
/// Programming note: in order to work, this script must be executed after the
/// script that create a rigid body.
/// </summary>
[AddComponentMenu("MiddleVR/Samples/Physics/Apply Force-Torque")]
[HelpURL("http://www.middlevr.com/doc/current/#apply-forcetorque-sample")]
public class VRApplyForceTorqueSample : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private Vector3 m_Force;
    [SerializeField]
    private Vector3 m_Torque;

    private vrPhysicsBody m_RigidBody = null;

    private vrEventListener m_MVREventListener = null;

    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        if (MiddleVR.VRPhysicsMgr == null)
        {
            MiddleVRTools.Log(0, "[X] ApplyForceTorqueSample: No PhysicsManager found.");
            enabled = false;
            return;
        }

        VRPhysicsBody body = GetComponent<VRPhysicsBody>();

        m_RigidBody = body.PhysicsBody;

        if (m_RigidBody == null)
        {
            MiddleVRTools.Log(0, "[X] ApplyForceTorqueSample: No rigid body given.");
            enabled = false;
            return;
        }

        m_MVREventListener = new vrEventListener(OnMVRNodeDestroy);
        m_RigidBody.AddEventListener(m_MVREventListener);
    }

    protected void Update()
    {
        var deviceMgr = MiddleVR.VRDeviceMgr;

        if (deviceMgr != null &&
            deviceMgr.IsKeyPressed(MiddleVR.VRK_H))
        {
            if (deviceMgr.IsKeyToggled(MiddleVR.VRK_F))
            {
                Vector3 force = m_Force;

                if (deviceMgr.IsKeyPressed(MiddleVR.VRK_LSHIFT) ||
                    deviceMgr.IsKeyPressed(MiddleVR.VRK_RSHIFT))
                {
                    force = -force;
                }

                m_RigidBody.AddForce(MiddleVRTools.FromUnity(force));

                MiddleVRTools.Log(2, "[+] ApplyForceTorqueSample: applied force " +
                    force + " on '" + m_RigidBody.GetName() + "'.");
            }

            if (deviceMgr.IsKeyToggled(MiddleVR.VRK_T))
            {
                Vector3 torque = m_Torque;

                if (deviceMgr.IsKeyPressed(MiddleVR.VRK_LSHIFT) ||
                    deviceMgr.IsKeyPressed(MiddleVR.VRK_RSHIFT))
                {
                    torque = -torque;
                }

                m_RigidBody.AddTorque(MiddleVRTools.FromUnity(torque));

                MiddleVRTools.Log(2, "[+] ApplyForceTorqueSample: applied torque " +
                    m_Torque + " on '" + m_RigidBody.GetName() + "'.");
            }
        }
    }

    private bool OnMVRNodeDestroy(vrEvent iBaseEvt)
    {
        vrObjectEvent e = vrObjectEvent.Cast(iBaseEvt);
        if (e != null)
        {
            if (e.ComesFrom(m_RigidBody))
            {
                if (e.eventType == (int)VRObjectEventEnum.VRObjectEvent_Destroy)
                {
                    // The physics rigid body was killed in MiddleVR so
                    // stop to use it in C#.
                    m_RigidBody = null;

                    // And even stop to use this component.
                    enabled = false;
                }
            }
        }

        return true;
    }

    #endregion
}
