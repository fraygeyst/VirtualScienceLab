/* VRChangeAttachedPhysicsBodyIPSISample
 * Written by MiddleVR.
 *
 * This code is given as an example. You can do whatever you want with it
 * without any restriction.
 */

using UnityEngine;
using System.Collections;

using MiddleVR_Unity3D;

/// <summary>
/// Iterate through physics rigid bodies and make them manipulated by a given Haption manipulation device.
///
/// The purpose of this class is to illustrate how to change dynamically
/// the rigid body being manipulated by a Haption (IPSI) manipulation device.
///
/// It also show the usage of the following vrCommands:
/// - "Haption.IPSI.GetManipulationDevicesNb"
/// - "Haption.IPSI.GetManipulationDeviceName"
/// - "Haption.IPSI.AttachManipulationDeviceToBody"
/// - "Haption.IPSI.DetachManipulationDevice"
/// - "Haption.IPSI.DetachBodyFromAManipulationDevice"
/// - "Haption.IPSI.IsManipulationDeviceAttachedToABody"
/// - "Haption.IPSI.IsBodyAttachedToAManipulationDevice"
/// - "Haption.IPSI.GetIdOfManipulationDeviceAttachedToBody"
/// - "Haption.IPSI.GetIdOfBodyAttachedToManipulationDevice"
///
/// Usage:
/// press keyboard keys 'h' and 'c'.
/// The keys have this meaning: 'h' stands for 'haptics' and 'c' for 'change'.
///
/// Programming note: in order to work, this script must be executed after the
/// script that creates a rigid body.
/// </summary>
[AddComponentMenu("MiddleVR/Samples/Physics/Change Attached Physics Body IPSI")]
[HelpURL("http://www.middlevr.com/doc/current/#change-attached-physics-body-ipsi")]
public class VRChangeAttachedPhysicsBodyIPSISample : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private uint m_ManipulationDeviceId = 0;

    private uint m_PhysicsBodyId = 0;

    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        if (MiddleVR.VRClusterMgr.IsCluster() && !MiddleVR.VRClusterMgr.IsServer())
        {
            enabled = false;
            return;
        }
    }

    protected void Update()
    {
        var deviceMgr = MiddleVR.VRDeviceMgr;

        if (deviceMgr != null &&
            deviceMgr.IsKeyPressed(MiddleVR.VRK_H) &&
            deviceMgr.IsKeyToggled(MiddleVR.VRK_C))
        {
            var physicsMgr = MiddleVR.VRPhysicsMgr;

            if (physicsMgr == null)
            {
                MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: No PhysicsManager found.");
                enabled = false;
                return;
            }

            vrPhysicsEngine physicsEngine = physicsMgr.GetPhysicsEngine();

            if (physicsEngine == null)
            {
                return;
            }

            if (!physicsEngine.IsStarted())
            {
                // We have to wait...
                return;
            }

            uint bodiesNb = physicsEngine.GetBodiesNb();

            if (bodiesNb == 0)
            {
                MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: No physics body found!");
                return;
            }

            vrPhysicsBody physicsBody = physicsEngine.GetBody(m_PhysicsBodyId);

            MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: proposed body id: " +
                m_PhysicsBodyId + " ('" + (physicsBody != null ? physicsBody.GetName() : "Null") + "').");

            if (physicsBody != null && physicsBody.IsA("PhysicsBodyIPSI"))
            {
                var objId = physicsBody.GetId();

                // As a reminder: static or frozen physics bodies cannot be manipulated.
                MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: Is the physics body '" +
                    objId + "' static? " + (physicsBody.IsStatic() ? "Yes" : "No") + ".");
                MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: Is the physics body '" +
                    objId + "' frozen? " + (physicsBody.IsFrozen() ? "Yes" : "No") + ".");

                var kernel = MiddleVR.VRKernel;


                // Use of "Haption.IPSI.GetManipulationDevicesNb".
                // Param with one vrValue:
                //  + None, so vrValue.NULL_VALUE can be used.
                // Return:
                //  A vrValue that contains a uint.
                //  In case of problem, the vrValue is invalid.

                var getManipDeviceNbRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.GetManipulationDevicesNb",
                    vrValue.NULL_VALUE);

                if (getManipDeviceNbRetValue.IsNumber())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: " +
                        getManipDeviceNbRetValue.GetUInt() + " Haption device(s) are connected.");
                }
                else
                {
                    MiddleVRTools.Log(0,
                        "[X] VRChangeAttachedPhysicsBodyIPSISample: Failed to detect how many Haption devices are connected.");
                }


                // Use of "Haption.IPSI.GetManipulationDeviceName".
                // Param with one vrValue:
                //  + the id of an Haption device (as uint).
                // Return:
                //  A vrValue that contains a string (the name).
                //  In case of problem, the vrValue is invalid.

                var getManipDeviceNamePrmsValue = new vrValue(m_ManipulationDeviceId);

                var getManipDeviceNameRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.GetManipulationDeviceName",
                    getManipDeviceNamePrmsValue);

                if (getManipDeviceNameRetValue.IsString())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: The name of the Haption device '" +
                        m_ManipulationDeviceId + "' is '" + getManipDeviceNameRetValue.GetString() + "'.");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: Failed to the find the name of the Haption device '" +
                        m_ManipulationDeviceId + "'.");
                }


                // Use of "Haption.IPSI.AttachManipulationDeviceToBody".
                // Params with one vrValue:
                //  + 1st arg: the id of an Haption device (as uint),
                //  + 2st arg: the id of a physics body (returned by GetId() on this object).
                // Return:
                //  A vrValue that contains a boolean: True if successfully attached, False otherwise.
                //  In case of problem, the vrValue is invalid.
                //
                // If the attachment failed, the previous attached physics body
                // will remain attached, otherwise it will be detached.
                //
                // It is also possible to add arguments in order to select how
                // the manipulated object will be attached:
                // + 3rd arg: the type of attachment (as uint),
                // + 4th arg: a translation offset for an arbitrary point of attachment (as vrVec3),
                // + 5th arg: a rotation for offset of an arbitrary point of attachment (as vrQuat).
                //
                // The type of attachment can take the following values:
                // + 0: unknown attach point so no attachment will occur,
                // + 1: attachment at the geometric center,
                // + 2: attachment at the center of the axis-aligned bounding box (AABB),
                // + 3: attachment at an arbitrary point that is an offset in
                //      the object coordinate frame.
                //
                // So the 4th and the 5th arguments will be used with type 3
                // (i.e. arbitrary point) but ignored otherwise.
                // If the type 3 is used but the 4th and the 5th arguments are
                // not given, translation will equal to 0 and rotation to identity.
                // If the type 3 is used but the 5th argument is not given,
                // rotation will equal to identity.

                var attachManipDeviceToBodyPrmsValue = vrValue.CreateList();
                attachManipDeviceToBodyPrmsValue.AddListItem(m_ManipulationDeviceId);
                attachManipDeviceToBodyPrmsValue.AddListItem(objId);

                // The previous manipulated physics body (if any), will be
                // automatically detached.
                var attachManipDeviceToBodyRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.AttachManipulationDeviceToBody",
                    attachManipDeviceToBodyPrmsValue);

                if (attachManipDeviceToBodyRetValue.IsBool())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: Did attachment of the Haption device '" +
                        m_ManipulationDeviceId + "' to the physics body '" + objId + "' succeeded? " +
                        (attachManipDeviceToBodyRetValue.GetBool() == true ? "Yes" : "No") + ".");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: Failed to attach the Haption device '" +
                        m_ManipulationDeviceId + "' to the physics body '" + objId + "'.");
                }

                // Use of "Haption.IPSI.IsManipulationDeviceAttachedToABody".
                // Params with one vrValue:
                //  + the id of an Haption device (as uint).
                // Return:
                //  A vrValue that contains a boolean: True means that the device is attached to a body, False otherwise.
                //  In case of problem, the vrValue is invalid.

                var isManipDeviceAttachedToABodyPrmsValue = new vrValue(m_ManipulationDeviceId);

                var isManipDeviceAttachedToABodyRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.IsManipulationDeviceAttachedToABody",
                    isManipDeviceAttachedToABodyPrmsValue);

                if (isManipDeviceAttachedToABodyRetValue.IsBool())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: Is the Haption device '" +
                        m_ManipulationDeviceId + "' attached to a physics body? " +
                        (isManipDeviceAttachedToABodyRetValue.GetBool() == true ? "Yes" : "No") + ".");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: Failed to check whether the Haption device '" +
                        m_ManipulationDeviceId + "' is attached to a physics body.");
                }


                // Use of "Haption.IPSI.IsBodyAttachedToAManipulationDevice".
                // Params with one vrValue:
                //  + the id of a physics body (returned by GetId() on this object).
                // Return:
                //  A vrValue that contains a boolean: True means that the body is attached to a manipulation device, False otherwise.
                //  In case of problem, the vrValue is invalid.

                var isBodyAttachedToAManipDevicePrmsValue = new vrValue(objId);

                var isBodyAttachedToAManipDeviceRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.IsBodyAttachedToAManipulationDevice",
                    isBodyAttachedToAManipDevicePrmsValue);

                if (isBodyAttachedToAManipDeviceRetValue.IsBool())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: Is the physics body '" +
                        objId + "' attached to a Haption device? " +
                        (isBodyAttachedToAManipDeviceRetValue.GetBool() == true ? "Yes" : "No") + ".");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: Failed to check whether the physics body " +
                        objId + " is attached to a Haption device.");
                }


                // Use of "Haption.IPSI.GetIdOfManipulationDeviceAttachedToBody".
                // Params with one vrValue:
                //  + the id of a physics body (returned by GetId() on this object).
                // Return:
                //  A vrValue that contains the id (a uint) of the attached manipulation device.
                //  In case of problem or if the physics body was not attached, the vrValue is invalid.

                var getIdOfManipDeviceAttachedToBodyPrmsValue = new vrValue(objId);

                var getIdOfManipDeviceAttachedToBodyRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.GetIdOfManipulationDeviceAttachedToBody",
                    getIdOfManipDeviceAttachedToBodyPrmsValue);

                if (getIdOfManipDeviceAttachedToBodyRetValue.IsNumber())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: The physics body '" +
                        objId + "' is attached to the Haption device '" +
                        getIdOfManipDeviceAttachedToBodyRetValue.GetNumber() + "'.");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: The physics body '" +
                        objId + "' does not seem to be attached to a Haption device.");
                }


                // Use of "Haption.IPSI.GetIdOfBodyAttachedToManipulationDevice".
                // Params with one vrValue:
                //  + the id of an Haption device (as uint).
                // Return:
                //  A vrValue that contains the id (returned by vrObject.GetId()) of the attached physics body.
                //  In case of problem or if the device was not attached, the vrValue is invalid.

                var getIdOfBodyAttachedToManipDevicePrmsValue = new vrValue(m_ManipulationDeviceId);

                var getIdOfBodyAttachedToManipDeviceRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.GetIdOfBodyAttachedToManipulationDevice",
                    getIdOfBodyAttachedToManipDevicePrmsValue);

                if (getIdOfBodyAttachedToManipDeviceRetValue.IsNumber())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: The Haption device '" +
                        m_ManipulationDeviceId + "' is attached to the physics body '" +
                        getIdOfBodyAttachedToManipDeviceRetValue.GetNumber() + "'.");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: The Haption device '" +
                        m_ManipulationDeviceId + "' does not seem to be attached to a physics body.");
                }


                // Use of "Haption.IPSI.DetachManipulationDevice".
                // Params with one vrValue:
                //  + the id of an Haption device (as uint).
                // Return:
                //  A vrValue that contains a boolean: True if successfully detached, False otherwise.
                //  In case of problem, the vrValue is invalid.

                // Please uncomment the code following to try...
                /*
                var detachManipDevicePrmsValue = new vrValue(m_ManipulationDeviceId);

                var detachManipDeviceRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.DetachManipulationDevice",
                    detachManipDevicePrmsValue);

                if (detachManipDeviceRetValue.IsBool())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: Did detachment of the Haption device '" +
                        m_ManipulationDeviceId + "' succeeded? " +
                        (detachManipDeviceRetValue.GetBool() == true ? "Yes" : "No") + ".");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: Failed to detach the Haption device '" +
                        m_ManipulationDeviceId + "'.");
                }
                */


                // Use of "Haption.IPSI.DetachBodyFromAManipulationDevice".
                // Params with one vrValue:
                //  + the id of a physics body (returned by GetId() on this object).
                // Return:
                //  A vrValue that contains a boolean: True if successfully detached, False otherwise.
                //  In case of problem, the vrValue is invalid.

                // Please uncomment the code following to try...
                /*
                var detachBodyFromAManipDevicePrmsValue = new vrValue(objId);

                var detachBodyFromAManipDeviceRetValue = kernel.ExecuteCommand(
                    "Haption.IPSI.DetachBodyFromAManipulationDevice",
                    detachBodyFromAManipDevicePrmsValue);

                if (detachBodyFromAManipDeviceRetValue.IsBool())
                {
                    MiddleVRTools.Log(2, "[+] VRChangeAttachedPhysicsBodyIPSISample: Did detachment of the physics body '" +
                        objId + "' succeeded? " +
                        (detachBodyFromAManipDeviceRetValue.GetBool() == true ? "Yes" : "No") + ".");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] VRChangeAttachedPhysicsBodyIPSISample: Failed to detach the physics body '" +
                        objId + "'.");
                }
                */
            }

            m_PhysicsBodyId = (m_PhysicsBodyId + 1) % (bodiesNb);
        }
    }

    #endregion
}
