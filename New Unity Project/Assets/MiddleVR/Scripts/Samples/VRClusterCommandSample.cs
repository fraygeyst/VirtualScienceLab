/* VRClusterCommand
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

// Command execution is synchronized across the cluster.
// Command handlers are not executed instantly but the next time the cluster synchronizes,
// in the same order they are called.
[AddComponentMenu("MiddleVR/Samples/Cluster Command")]
public class VRClusterCommand : MonoBehaviour {

    // If m_Name is an empty string, one will be automatically generated
    public string m_Name = "";

    private vrCommand m_Command = null;

    private void Start()
    {
        m_Command = new vrCommand(m_Name, _CommandHandler);
    }

    private void OnDestroy()
    {
        MiddleVR.DisposeObject(ref m_Command);
    }

    // When called on the server (master) node, an asynchronous call
    // to _CommandHandler is triggered on all nodes at next
    // synchronization (VRManagerScript or VRManagerPostFrame)
    public void Call(vrValue val)
    {
        if (m_Command != null)
        {
            // On a single machine, this will immediately call _CommandHandler
            // On a cluster server, this will schedule a call to _CommandHandler
            //   on all cluster nodes at the next synchronization point
            // On a cluster client, this does nothing
            m_Command.Do(val);
        }
    }

    // This method is called on all nodes immediately after
    // the same synchronization point
    private vrValue _CommandHandler(vrValue iValue)
    {
        // Do stuff here
        return null; // Return null/undefined vrValue
    }
}
