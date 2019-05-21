/* VRPhysicsConstraintPlanar
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Constraints/Planar")]
[HelpURL("http://www.middlevr.com/doc/current/#planar-constraint")]
[RequireComponent(typeof(VRPhysicsBody))]
public class VRPhysicsConstraintPlanar : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private GameObject m_ConnectedBody = null;

    [SerializeField]
    private Vector3 m_Axis0 = new Vector3(1.0f, 0.0f, 0.0f);

    [SerializeField]
    private Vector3 m_Axis1 = new Vector3(0.0f, 1.0f, 0.0f);

    [SerializeField]
    private float m_GizmoLineLength = 1.0f;

    private vrPhysicsConstraintPlanar m_PhysicsConstraint = null;
    private string m_PhysicsConstraintName = "";

    private vrEventListener m_MVREventListener = null;

    #endregion

    #region Member Properties

    public vrPhysicsConstraintPlanar PhysicsConstraint
    {
        get
        {
            return m_PhysicsConstraint;
        }
    }

    public string PhysicsConstraintName
    {
        get
        {
            return m_PhysicsConstraintName;
        }
    }

    public GameObject ConnectedBody
    {
        get
        {
            return m_ConnectedBody;
        }

        set
        {
            m_ConnectedBody = value;
        }
    }

    public Vector3 Axis0
    {
        get
        {
            return m_Axis0;
        }

        set
        {
            m_Axis0 = value;
        }
    }

    public Vector3 Axis1
    {
        get
        {
            return m_Axis1;
        }

        set
        {
            m_Axis1 = value;
        }
    }

    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        if (MiddleVR.VRClusterMgr.IsCluster() && !MiddleVR.VRClusterMgr.IsServer())
        {
            enabled = false;
            return;
        }

        if (MiddleVR.VRPhysicsMgr == null)
        {
            MiddleVRTools.Log(0, "[X] No PhysicsManager found when creating a planar constraint.");
            enabled = false;
            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return;
        }

        if (m_PhysicsConstraint == null)
        {
            m_PhysicsConstraint = physicsEngine.CreateConstraintPlanarWithUniqueName(name);

            if (m_PhysicsConstraint == null)
            {
                MiddleVRTools.Log(0, "[X] Could not create a planar physics constraint '"
                    + name + "'.");
            }
            else
            {
                GC.SuppressFinalize(m_PhysicsConstraint);

                m_MVREventListener = new vrEventListener(OnMVRNodeDestroy);
                m_PhysicsConstraint.AddEventListener(m_MVREventListener);

                m_PhysicsConstraintName = m_PhysicsConstraint.GetName();

                AddConstraint();
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        if (enabled)
        {
            Vector3 origin = transform.TransformPoint(Vector3.zero);

            Gizmos.color = Color.yellow;
            Vector3 axis0Dir = transform.TransformDirection(m_Axis0);
            axis0Dir.Normalize();
            Gizmos.DrawLine(origin, origin + m_GizmoLineLength * axis0Dir);

            Gizmos.color = Color.gray;
            Vector3 axis1Dir = transform.TransformDirection(m_Axis1);
            axis1Dir.Normalize();
            Gizmos.DrawLine(origin, origin + m_GizmoLineLength * axis1Dir);
        }
    }

    protected void OnDestroy()
    {
        if (m_PhysicsConstraint == null)
        {
            return;
        }

        if (MiddleVR.VRPhysicsMgr == null)
        {
            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return;
        }

        physicsEngine.DestroyConstraint(m_PhysicsConstraintName);

        m_PhysicsConstraint = null;
        m_PhysicsConstraintName = "";
    }

    #endregion

    #region VRPhysicsConstraintPlanar Functions

    protected bool AddConstraint()
    {
        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return false;
        }

        if (m_PhysicsConstraint == null)
        {
            return false;
        }

        bool addedToSimulation = false;

        // Cannot fail since we require this component.
        VRPhysicsBody body0 = GetComponent<VRPhysicsBody>();

        VRPhysicsBody body1 = null;
        if (m_ConnectedBody != null)
        {
            body1 = m_ConnectedBody.GetComponent<VRPhysicsBody>();
        }

        if (body0.PhysicsBody != null)
        {
            m_PhysicsConstraint.SetAxis0(MiddleVRTools.FromUnity(Axis0));
            m_PhysicsConstraint.SetAxis1(MiddleVRTools.FromUnity(Axis1));

            m_PhysicsConstraint.SetBody(0, body0.PhysicsBody);
            m_PhysicsConstraint.SetBody(1, body1 != null ? body1.PhysicsBody : null);

            addedToSimulation = physicsEngine.AddConstraint(m_PhysicsConstraint);

            if (addedToSimulation)
            {
                MiddleVRTools.Log(3, "[ ] The constraint '" + m_PhysicsConstraintName +
                    "' was added to the physics simulation.");
            }
            else
            {
                MiddleVRTools.Log(3, "[X] Failed to add the constraint '" +
                    m_PhysicsConstraintName + "' to the physics simulation.");
            }
        }
        else
        {
            MiddleVRTools.Log(0, "[X] The PhysicsBody of '" + name +
                "' for the planar physics constraint '" + m_PhysicsConstraintName +
                "' is null.");
        }

        return addedToSimulation;
    }

    private bool OnMVRNodeDestroy(vrEvent iBaseEvt)
    {
        vrObjectEvent e = vrObjectEvent.Cast(iBaseEvt);
        if (e != null)
        {
            if (e.ComesFrom(m_PhysicsConstraint))
            {
                if (e.eventType == (int)VRObjectEventEnum.VRObjectEvent_Destroy)
                {
                    // Killed in MiddleVR so delete it in C#.
                    m_PhysicsConstraint.Dispose();
                }
            }
        }

        return true;
    }

    #endregion
}
