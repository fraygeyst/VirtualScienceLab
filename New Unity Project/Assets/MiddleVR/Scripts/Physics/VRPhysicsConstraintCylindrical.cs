/* VRPhysicsConstraintCylindrical
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Constraints/Cylindrical")]
[HelpURL("http://www.middlevr.com/doc/current/#cylindrical-constraint")]
[RequireComponent(typeof(VRPhysicsBody))]
public class VRPhysicsConstraintCylindrical : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private GameObject m_ConnectedBody = null;

    [SerializeField]
    private Vector3 m_Anchor = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField]
    private Vector3 m_Axis = new Vector3(1.0f, 0.0f, 0.0f);

    [SerializeField]
    private VRPhysicsJointLimits m_AngularLimits = new VRPhysicsJointLimits(-360.0, +360.0);

    [SerializeField]
    private double m_AngularZeroPosition = 0.0;

    [SerializeField]
    private VRPhysicsJointLimits m_LinearLimits = new VRPhysicsJointLimits(-1.0, +1.0);

    [SerializeField]
    private double m_LinearZeroPosition = 0.0;

    [SerializeField]
    private float m_GizmoSphereRadius = 0.1f;

    [SerializeField]
    private float m_GizmoLineLength = 1.0f;

    private vrPhysicsConstraintCylindrical m_PhysicsConstraint = null;
    private string m_PhysicsConstraintName = "";

    private vrEventListener m_MVREventListener = null;

    #endregion

    #region Member Properties

    public vrPhysicsConstraintCylindrical PhysicsConstraint
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

    public Vector3 Anchor
    {
        get
        {
            return m_Anchor;
        }

        set
        {
            m_Anchor = value;
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

    public VRPhysicsJointLimits AngularLimits
    {
        get
        {
            return m_AngularLimits;
        }

        set
        {
            m_AngularLimits = value;
        }
    }

    public double AngularReferencePosition
    {
        get
        {
            return m_AngularZeroPosition;
        }

        set
        {
            m_AngularZeroPosition = value;
        }
    }

    public VRPhysicsJointLimits LinearLimits
    {
        get
        {
            return m_AngularLimits;
        }

        set
        {
            m_AngularLimits = value;
        }
    }

    public double LinearReferencePosition
    {
        get
        {
            return m_LinearZeroPosition;
        }

        set
        {
            m_LinearZeroPosition = value;
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
            MiddleVRTools.Log(0, "[X] No PhysicsManager found when creating a cylindrical constraint.");
            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return;
        }

        if (m_PhysicsConstraint == null)
        {
            m_PhysicsConstraint = physicsEngine.CreateConstraintCylindricalWithUniqueName(name);

            if (m_PhysicsConstraint == null)
            {
                MiddleVRTools.Log(0, "[X] Could not create a cylindrical physics constraint for '"
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

            Vector3 origin = transform.TransformPoint(m_Anchor);
            Vector3 axisDir = transform.TransformDirection(m_Axis);
            axisDir.Normalize();

            Gizmos.DrawSphere(origin, m_GizmoSphereRadius);
            Gizmos.DrawLine(origin, origin + m_GizmoLineLength * axisDir);

            // min limit
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(origin + (float)m_LinearLimits.Min * axisDir, m_GizmoSphereRadius * 0.7f);

            // max limit
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(origin + (float)m_LinearLimits.Max * axisDir, m_GizmoSphereRadius * 0.7f);
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

    #region VRPhysicsConstraintCylindrical Functions

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
            var scaleShearMatrix = MVRTools.ComputeScaleShearMatrixWorld(transform);
            m_PhysicsConstraint.SetPosition(MiddleVRTools.FromUnity(scaleShearMatrix * Anchor));
            m_PhysicsConstraint.SetAxis(MiddleVRTools.FromUnity(Axis));

            m_PhysicsConstraint.SetAngularLowerLimit(m_AngularLimits.Min);
            m_PhysicsConstraint.SetAngularUpperLimit(m_AngularLimits.Max);

            m_PhysicsConstraint.SetAngularReferencePosition(m_AngularZeroPosition);

            m_PhysicsConstraint.SetLinearLowerLimit(m_LinearLimits.Min);
            m_PhysicsConstraint.SetLinearUpperLimit(m_LinearLimits.Max);

            m_PhysicsConstraint.SetLinearReferencePosition(m_LinearZeroPosition);

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
                MiddleVRTools.Log(0, "[X] Failed to add the constraint '" +
                    m_PhysicsConstraintName + "' to the physics simulation.");
            }
        }
        else
        {
            MiddleVRTools.Log(0, "[X] The PhysicsBody of '" + name +
                "' for the cylindrical physics constraint '" + m_PhysicsConstraintName +
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
