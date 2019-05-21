/* VRPhysicsDisableCollisions
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Disable Collisions")]
[HelpURL("http://www.middlevr.com/doc/current/#haption-haptics-managing-collisions")]
[RequireComponent(typeof(VRPhysicsBody))]
public class VRPhysicsDisableCollisions : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private GameObject m_ConnectedBody = null;

    private bool m_InitialActionProcessed = false;

    #endregion

    #region Member Properties

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
            MiddleVRTools.Log(0, "[X] PhysicsDisableCollisions: No PhysicsManager found.");
            enabled = false;

            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            MiddleVRTools.Log(0, "[X] PhysicsDisableCollisions: No PhysicsEngine found.");
            enabled = false;

            return;
        }
    }

    protected void OnEnable()
    {
        if (m_InitialActionProcessed)
        {
            vrPhysicsBody physicsBody = GetPhysicsBodyInSimulation();

            if (physicsBody != null)
            {
                EnableCollisions(physicsBody, false);
            }
        }
    }

    protected void OnDisable()
    {
        vrPhysicsBody physicsBody = GetPhysicsBodyInSimulation();

        if (physicsBody != null)
        {
            EnableCollisions(physicsBody, true);
        }
    }

    protected void Update()
    {
        if (!m_InitialActionProcessed)
        {
            vrPhysicsBody physicsBody = GetPhysicsBodyInSimulation();

            if (physicsBody != null)
            {
                m_InitialActionProcessed = true;

                EnableCollisions(physicsBody, false);
            }
        }
    }

    #endregion

    #region VRPhysicsDisableCollisions Functions

    protected vrPhysicsBody GetPhysicsBodyInSimulation()
    {
        if (MiddleVR.VRPhysicsMgr == null)
        {
            return null;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return null;
        }

        vrPhysicsBody physicsBody = physicsEngine.GetBody(GetComponent<VRPhysicsBody>().PhysicsBodyName);

        if (physicsBody != null && physicsBody.IsInSimulation())
        {
            return physicsBody;
        }
        else
        {
            return null;
        }
    }

    protected void EnableCollisions(vrPhysicsBody physicsBody0, bool iEnabled)
    {
        bool operationDone = false;

        vrPhysicsBody physicsBody1 = null;

        if (m_ConnectedBody != null)
        {
            if (MiddleVR.VRPhysicsMgr == null)
            {
                return;
            }

            vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

            if (physicsEngine == null)
            {
                return;
            }

            physicsBody1 = physicsEngine.GetBody(m_ConnectedBody.GetComponent<VRPhysicsBody>().PhysicsBodyName);

            if (physicsBody1 == null)
            {
                MiddleVRTools.Log(0, "[X] PhysicsDisableCollisions: No PhysicsBody found in the connected body.");
                return;
            }

            operationDone = physicsBody0.EnableCollisionsWith(physicsBody1, iEnabled);
        }
        else
        {
            operationDone = physicsBody0.EnableAllCollisions(iEnabled);
        }

        if (operationDone)
        {
            string againstTxt = (physicsBody1 != null ?
                " against object '" + physicsBody1.GetName() + "'" :
                " against the world scene"
                );

            if (iEnabled)
            {
                MiddleVRTools.Log(2, "[+] PhysicsDisableCollisions: Enabled collisions for '" +
                    physicsBody0.GetName() + "'" + againstTxt + ".");
            }
            else
            {
                MiddleVRTools.Log(2, "[+] PhysicsDisableCollisions: Disabled collisions for '" +
                    physicsBody0.GetName() + "'" + againstTxt + ".");
            }
        }
        else
        {
            if (iEnabled)
            {
                MiddleVRTools.Log(0, "[X] PhysicsDisableCollisions: Failed to enable collisions for '" +
                    physicsBody0.GetName() + "'.");
            }
            else
            {
                MiddleVRTools.Log(0, "[X] PhysicsDisableCollisions: Failed to disable collisions for '" +
                    physicsBody0.GetName() + "'.");
            }
        }
    }

    #endregion
}
