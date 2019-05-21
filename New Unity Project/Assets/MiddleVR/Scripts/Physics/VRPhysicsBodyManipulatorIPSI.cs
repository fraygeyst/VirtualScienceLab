/* VRPhysicsBodyManipulatorIPSI
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Body Manipulator IPSI")]
[HelpURL("http://www.middlevr.com/doc/current/#haption-haptics-associating-rigid-body")]
[RequireComponent(typeof(VRPhysicsBody))]
public class VRPhysicsBodyManipulatorIPSI : MonoBehaviour {

    public enum AttachPointType
    {
        GEOMETRIC_CENTER,
        AABB_CENTER,
        ARBITRARY_POINT
    }

    #region Member Variables

    [SerializeField]
    private int m_ManipulationDeviceId = 0;
    [SerializeField]
    private AttachPointType m_AttachPointType = AttachPointType.AABB_CENTER;
    [SerializeField]
    private Vector3 m_OffsetTranslation = Vector3.zero;
    [SerializeField]
    private Vector3 m_OffsetRotation = Vector3.zero;
    [SerializeField]
    private float m_GizmoSphereRadius = 0.1f;

    private bool m_ScriptStarted = false;
    private string m_PhysicsBodyName = "";
    private bool m_DidFirstAttachment = false;

    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        m_ScriptStarted = true;

        if (MiddleVR.VRClusterMgr.IsCluster() && !MiddleVR.VRClusterMgr.IsServer())
        {
            enabled = false;
            return;
        }

        vrPhysicsBody physicsBody = GetComponent<VRPhysicsBody>().PhysicsBody;

        if (physicsBody == null)
        {
            MiddleVRTools.Log(0, "[X] PhysicsBodyManipulator: No PhysicsBody found in '"
                + name + "'.");

            enabled = false;

            return;
        }

        // Fix incoherent settings.
        if (physicsBody.IsStatic())
        {
            MiddleVRTools.Log(0, "[X] PhysicsBodyManipulator: The body '" +
                physicsBody.GetName() + "' cannot be manipulated because it is static.");

            // And so we stop to use this script.
            enabled = false;

            return;
        }

        m_PhysicsBodyName = physicsBody.GetName();

        m_DidFirstAttachment = false;
    }

    protected void OnEnable()
    {
        if (!m_ScriptStarted)
        {
            // Start() must be executed first to find physics bodies.
            return;
        }

        AttachOrDetachBody(true);
    }

    protected void OnDisable()
    {
        AttachOrDetachBody(false);
    }

    protected void Update()
    {
        if (!m_DidFirstAttachment)
        {
            var physicsMgr = MiddleVR.VRPhysicsMgr;

            if (physicsMgr == null)
            {
                MiddleVRTools.Log(0, "[X] PhysicsBodyManipulator: No PhysicsManager found.");
                enabled = false;
                return;
            }

            vrPhysicsEngine physicsEngine = physicsMgr.GetPhysicsEngine();

            if (physicsEngine == null)
            {
                return;
            }

            if (physicsEngine.IsStarted())
            {
                AttachOrDetachBody(true);

                m_DidFirstAttachment = true;
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        if (enabled &&
            m_AttachPointType == AttachPointType.ARBITRARY_POINT)
        {
            Gizmos.color = Color.green;

            var q = Quaternion.Euler(m_OffsetRotation);
            var localOffset = q * m_OffsetTranslation;
            Vector3 origin = transform.TransformPoint(localOffset);

            Gizmos.DrawSphere(origin, m_GizmoSphereRadius);
        }
    }

    #endregion

    #region VRPhysicsBodyManipulatorIPSI Functions

    protected void AttachOrDetachBody(bool doAttachement)
    {
        VRPhysicsBody physicsBodyComponent = GetComponent<VRPhysicsBody>();

        if (physicsBodyComponent == null)
        {
            return;
        }

        vrPhysicsBody physicsBody = null;

        if (MiddleVR.VRPhysicsMgr != null)
        {
            vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

            if (physicsEngine != null)
            {
                // Prefer to find the object by its name so we won't access
                // a dangling pointer when the body was destroyed by MiddleVR.
                physicsBody = physicsEngine.GetBody(m_PhysicsBodyName);
            }
        }

        if (physicsBody == null)
        {
            return;
        }

        var kernel = MiddleVR.VRKernel;

        var physicsBodyId = physicsBody.GetId();

        if (doAttachement)
        {
            // Do "+1" because "0" means "unknown attach point type".
            uint attachPointType = (uint)m_AttachPointType + 1;

            var scaleShearMatrix = MVRTools.ComputeScaleShearMatrixWorld(transform);
            var offsetTrans = scaleShearMatrix * m_OffsetTranslation;
            var offsetRot = Quaternion.Euler(m_OffsetRotation);

            var attachManipDeviceToBodyPrmsValue = vrValue.CreateList();
            attachManipDeviceToBodyPrmsValue.AddListItem(m_ManipulationDeviceId);
            attachManipDeviceToBodyPrmsValue.AddListItem(physicsBodyId);
            attachManipDeviceToBodyPrmsValue.AddListItem(attachPointType);
            attachManipDeviceToBodyPrmsValue.AddListItem(
                MiddleVRTools.FromUnity(offsetTrans));
            attachManipDeviceToBodyPrmsValue.AddListItem(
                MiddleVRTools.FromUnity(offsetRot));

            kernel.ExecuteCommand(
                "Haption.IPSI.AttachManipulationDeviceToBody",
                attachManipDeviceToBodyPrmsValue);
        }
    }

    #endregion
}
