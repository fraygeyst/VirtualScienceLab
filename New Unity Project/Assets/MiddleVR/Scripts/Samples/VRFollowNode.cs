/* VRFollowNode
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/Follow Node")]
public class VRFollowNode : MonoBehaviour {
    public string VRNodeName = "HeadNode";
    private vrNode3D m_Node = null;

    protected void Update()
    {
        var displayMgr = MiddleVR.VRDisplayMgr;

        if (m_Node == null && displayMgr != null)
        {
            m_Node = displayMgr.GetNode(VRNodeName);
        }

        if (m_Node != null)
        {
            transform.position = MVRTools.ToUnity(m_Node.GetPositionVirtualWorld());
            transform.rotation = MVRTools.ToUnity(m_Node.GetOrientationVirtualWorld());
        }
    }
}
