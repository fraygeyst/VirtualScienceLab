/* VRPhysicsConstraintFixed
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Constraints/Fixed")]
[HelpURL("http://www.middlevr.com/doc/current/#fixed-constraint")]
[RequireComponent(typeof(VRPhysicsBody))]
public class VRPhysicsConstraintFixed : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private GameObject m_ConnectedBody = null;

    private vrPhysicsConstraintFixed m_PhysicsConstraint = null;
    private string m_PhysicsConstraintName = "";

    private bool m_AttemptedToAddConstraint = false;

    private vrEventListener m_MVREventListener = null;

    #endregion

    #region Member Properties

    public vrPhysicsConstraintFixed PhysicsConstraint
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
            MiddleVRTools.Log(0, "[X] No PhysicsManager found when creating a fixed constraint.");
            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return;
        }

        if (m_PhysicsConstraint == null)
        {
            m_PhysicsConstraint = physicsEngine.CreateConstraintFixedWithUniqueName(name);

            if (m_PhysicsConstraint == null)
            {
                MiddleVRTools.Log(0, "[X] Could not create a fixed physics constraint for '"
                    + name + "'.");
            }
            else
            {
                GC.SuppressFinalize(m_PhysicsConstraint);

                m_MVREventListener = new vrEventListener(OnMVRNodeDestroy);
                m_PhysicsConstraint.AddEventListener(m_MVREventListener);

                m_PhysicsConstraintName = m_PhysicsConstraint.GetName();
            }
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

    protected void Update()
    {
        if (!m_AttemptedToAddConstraint)
        {
            AddConstraint();

            m_AttemptedToAddConstraint = true;
        }
    }

    #endregion

    #region VRPhysicsConstraintFixed Functions

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
                "' for the fixed physics constraint '" + m_PhysicsConstraintName +
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
