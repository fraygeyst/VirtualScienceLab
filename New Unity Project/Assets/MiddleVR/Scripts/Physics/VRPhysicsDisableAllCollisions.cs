/* VRPhysicsDisableAllCollisions
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;

using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Disable All Collisions")]
public class VRPhysicsDisableAllCollisions : MonoBehaviour {

    #region Member Variables

    private bool m_AppliedAtStartup = false;

    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        if (MiddleVR.VRPhysicsMgr == null)
        {
            MiddleVRTools.Log(0, "[X] PhysicsDisableAllCollisions: No PhysicsManager found.");
            enabled = false;

            return;
        }
    }

    protected void OnEnable()
    {
        DisableAllCollisions(true);
    }

    protected void OnDisable()
    {
        DisableAllCollisions(false);
    }

    protected void Update()
    {
        if (!m_AppliedAtStartup)
        {
            m_AppliedAtStartup = DisableAllCollisions(true);
        }
    }

    private bool DisableAllCollisions(bool iDisabled)
    {
        if (MiddleVR.VRPhysicsMgr == null)
        {
            return false;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return false;
        }

        if (physicsEngine.IsStarted())
        {
            bool actionApplied = physicsEngine.EnableCollisions(!iDisabled);

            if (actionApplied)
            {
                if (iDisabled)
                {
                    MiddleVRTools.Log(2, "[ ] PhysicsDisableAllCollisions: all collisions disabled.");
                }
                else
                {
                    MiddleVRTools.Log(2, "[ ] PhysicsDisableAllCollisions: all collisions enabled.");
                }
            }

            return actionApplied;
        }

        return false;
    }

    #endregion
}
