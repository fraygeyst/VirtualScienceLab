/* VRPhysicsDeactivateAllContacts
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;

using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Deactivate All Contacts")]
public class VRPhysicsDeactivateAllContacts : MonoBehaviour {

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        if (MiddleVR.VRPhysicsMgr == null)
        {
            MiddleVRTools.Log(0, "[X] PhysicsDeactivateAllContacts: No PhysicsManager found.");
            enabled = false;

            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            MiddleVRTools.Log(0, "[X] PhysicsDeactivateAllContacts: No PhysicsEngine found.");
            enabled = false;

            return;
        }

        physicsEngine.SetActivateContactInfos(false);

        MiddleVRTools.Log(2, "[+] PhysicsDisableAllContacts: all contacts disabled.");
    }

    #endregion
}
