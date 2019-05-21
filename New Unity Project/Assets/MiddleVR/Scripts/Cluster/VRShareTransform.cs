/* VRShareTransform
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;

using MiddleVR_Unity3D;

// Share a GameObject transformation using MiddleVR Cluster Command.
[AddComponentMenu("MiddleVR/Cluster/Share Transform")]
public class VRShareTransform : MonoBehaviour {
    static private uint g_shareID = 1;

    private vrClusterManager m_ClusterMgr = null;
    private vrCommand m_Command = null;

    // Create cluster command on script start
    // For more information, refer to the MiddleVR User Guide and the VRShareTransform script
    protected void Start()
    {
        uint shareID = g_shareID++;
        string shareName = "VRShareTransform_" + shareID.ToString();

        // Create the command with cluster flag
        m_Command = new vrCommand(shareName, _CommandHandler);

        m_ClusterMgr = MiddleVR.VRClusterMgr;
    }

    protected void OnDisable()
    {
        if( m_Command != null )
        {
            m_Command.Dispose();
            m_Command = null;
        }
    }

    // On the server, call the cluster command with a list of [position, rotation] every update
    // On all nodes, _CommandHandler will be called the next time there is a synchronization update :
    // either during VRManagerScript Update() or VRManagerPostFrame Update() (see script ordering)
    protected void Update()
    {
        if (m_ClusterMgr.IsServer())
        {
            // put position and orientation in a vrValue as a list
            Vector3 p = transform.position;
            Quaternion q = transform.rotation;

            vrValue val = vrValue.CreateList();
            val.AddListItem( new vrVec3(p.x, p.y, p.z) );
            val.AddListItem( new vrQuat(q.x, q.y, q.z, q.w) );

            // Do the actual call
            // This returns immediately
            m_Command.Do( val );
        }
    }

    // On clients, handle the command call
    private vrValue _CommandHandler(vrValue iValue)
    {
        if (m_ClusterMgr.IsClient())
        {
            // extract position and orientation from the vrValue
            vrVec3 pos = iValue[0].GetVec3();
            vrQuat orient = iValue[1].GetQuat();

            Vector3 p = new Vector3( pos.x(), pos.y(), pos.z() );
            Quaternion q = new Quaternion( orient.x(), orient.y(), orient.z(), orient.w() );

            transform.position = p;
            transform.rotation = q;
        }

        return null;
    }
}
