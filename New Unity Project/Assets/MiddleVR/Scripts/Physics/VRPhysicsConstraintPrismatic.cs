/* VRPhysicsConstraintPrismatic
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Constraints/Prismatic")]
[HelpURL("http://www.middlevr.com/doc/current/#prismatic-constraint")]
[RequireComponent(typeof(VRPhysicsBody))]
public class VRPhysicsConstraintPrismatic : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private GameObject m_ConnectedBody = null;

    [SerializeField]
    private Vector3 m_Axis = new Vector3(1.0f, 0.0f, 0.0f);

    [SerializeField]
    private VRPhysicsJointLimits m_Limits = new VRPhysicsJointLimits(-1.0, +1.0);

    [SerializeField]
    private double m_ZeroPosition = 0.0;

    [SerializeField]
    private float m_GizmoSphereRadius = 0.1f;

    [SerializeField]
    private float m_GizmoLineLength = 1.0f;

    private vrPhysicsConstraintPrismatic m_PhysicsConstraint = null;
    private string m_PhysicsConstraintName = "";

    private vrEventListener m_MVREventListener = null;

    #endregion

    #region Member Properties

    public vrPhysicsConstraintPrismatic PhysicsConstraint
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

    public Vector3 Axis
    {
        get
        {
            return m_Axis;
        }

        set
        {
            m_Axis = value;
        }
    }

    public VRPhysicsJointLimits Limits
    {
        get
        {
            return m_Limits;
        }

        set
        {
            m_Limits = value;
        }
    }

    public double ReferencePosition
    {
        get
        {
            return m_ZeroPosition;
        }

        set
        {
            m_ZeroPosition = value;
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
            MiddleVRTools.Log(0, "[X] No PhysicsManager found when creating a prismatic constraint.");
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
            m_PhysicsConstraint = physicsEngine.CreateConstraintPrismaticWithUniqueName(name);

            if (m_PhysicsConstraint == null)
            {
                MiddleVRTools.Log(0, "[X] Could not create a prismatic physics constraint for '"
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
            Gizmos.color = Color.green;

            Vector3 origin = transform.TransformPoint(Vector3.zero);
            Vector3 axisDir = transform.TransformDirection(m_Axis);
            axisDir.Normalize();

            Gizmos.DrawSphere(origin, m_GizmoSphereRadius);
            Gizmos.DrawLine(origin, origin + m_GizmoLineLength * axisDir);

            // min limit
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(origin + (float)m_Limits.Min * axisDir, m_GizmoSphereRadius * 0.7f);

            // max limit
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(origin + (float)m_Limits.Max * axisDir, m_GizmoSphereRadius * 0.7f);

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

    #region VRPhysicsConstraintPrismatic Functions

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
            m_PhysicsConstraint.SetAxis(MiddleVRTools.FromUnity(Axis));

            m_PhysicsConstraint.SetLowerLimit(m_Limits.Min);
            m_PhysicsConstraint.SetUpperLimit(m_Limits.Max);

            m_PhysicsConstraint.SetReferencePosition(m_ZeroPosition);

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
                "' for the prismatic physics constraint '" + m_PhysicsConstraintName +
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
