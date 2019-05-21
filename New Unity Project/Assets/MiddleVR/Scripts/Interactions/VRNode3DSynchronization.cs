using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/VRNode3DSynchronization")]
public class VRNode3DSynchronization : MonoBehaviour
{
    #region Data members

    [SerializeField]
    private string m_NodeName = "";

    private vrNode3D m_Node = null;
    private bool m_IsNodeGrabbed = false;
    private MVRNodesMapper.ENodesSyncDirection m_SyncDirection = MVRNodesMapper.ENodesSyncDirection.NoSynchronization;
    private vrEventListener m_EventListener = null;

    #endregion

    #region Properties

    public string NodeName
    {
        get
        {
            return m_NodeName;
        }
    }

    public vrNode3D Node
    {
        get
        {
            return m_Node ?? InitNode();
        }
    }

    public MVRNodesMapper.ENodesSyncDirection SyncDirection
    {
        get
        {
            return m_SyncDirection;
        }
    }

    #endregion

    #region Public methods

    public vrNode3D InitNode()
    {
        // Check for grabbed nodes
        MVRNodesMapper nodesMapper = MVRNodesMapper.Instance;
        if (nodesMapper != null)
        {
            vrNode3D node = nodesMapper.GetNode(gameObject);
            if (node != null)
            {
                m_Node = node;
                m_NodeName = m_Node.GetName();
                m_SyncDirection = MVRNodesMapper.ENodesSyncDirection.MiddleVRToUnity;
                m_IsNodeGrabbed = true;
                return m_Node;
            }
        }

        return CreateNode(m_NodeName, MVRNodesMapper.ENodesSyncDirection.MiddleVRToUnity);
    }

    public vrNode3D CreateNode(string iNodeName, MVRNodesMapper.ENodesSyncDirection iSyncDirection)
    {
        vrNode3D node = null;

        if (iNodeName != "")
        {
            vrDisplayManager displayManager = MiddleVR.VRDisplayMgr;
            if (displayManager.GetNode(iNodeName) == null)
            {
                node = displayManager.CreateNode(iNodeName);
                // Copy transformation from gameObject to node3d
                _SetNode(node, MVRNodesMapper.ENodesSyncDirection.UnityToMiddleVR, iSyncDirection);
            }
        }

        return node;
    }

    public void SetNode(vrNode3D iNode, MVRNodesMapper.ENodesSyncDirection iSyncDirection)
    {
        // Copy transformation from gameObject to node3d
        _SetNode(iNode, MVRNodesMapper.ENodesSyncDirection.MiddleVRToUnity, iSyncDirection);
    }

    public void SetSyncDirection(MVRNodesMapper.ENodesSyncDirection iSyncDirection)
    {
        MVRNodesMapper nodesMapper = MVRNodesMapper.Instance;

        if (nodesMapper != null && m_Node != null)
        {
            if (m_SyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
            {
                nodesMapper.RemoveMapping(gameObject, false);
            }

            if (iSyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
            {
                nodesMapper.AddMapping(gameObject, m_Node, iSyncDirection);
            }
        }

        m_SyncDirection = iSyncDirection;
    }

    #endregion

    #region Unity events

    public void Reset()
    {
        // Generate a unique name so that the object can be identified across scenes/runtime versions
        m_NodeName = gameObject.name + "_" + System.Guid.NewGuid();
    }

    private void OnDestroy()
    {
        if (m_EventListener != null)
        {
            m_EventListener.Dispose();
            m_EventListener = null;
        }

        if (m_Node != null && !m_IsNodeGrabbed)
        {
            // Test for VRDisplayMgr existence in case VRManagerScript is destroyed before this script
            if (MiddleVR.VRDisplayMgr != null)
            {
                if (m_SyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
                {
                    MVRNodesMapper.Instance.RemoveMapping(gameObject, false);
                }

                MiddleVR.VRDisplayMgr.DestroyNode(m_Node);
            }

            m_Node = null;
        }
    }

    #endregion

    #region Private

    private void _SetNode(vrNode3D iNode3D, MVRNodesMapper.ENodesSyncDirection iInitialSyncDirection, MVRNodesMapper.ENodesSyncDirection iSyncDirection)
    {
        // Check if iNode3D and m_Node3D are the same
        if (m_Node == iNode3D || (m_Node != null && iNode3D != null && m_Node.GetId() == iNode3D.GetId()))
        {
            // Same node, return.
            return;
        }

        MVRNodesMapper nodesMapper = MVRNodesMapper.Instance;

        // Cleanup old node
        if (m_Node != null && !m_IsNodeGrabbed)
        {
            if (m_EventListener != null)
            {
                m_Node.RemoveEventListener(m_EventListener);
            }

            if (m_SyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
            {
                nodesMapper.RemoveMapping(gameObject, true);
            }
        }

        m_IsNodeGrabbed = false;
        m_Node = iNode3D;

        // Setup new node
        if (m_Node != null)
        {
            // First mapping will copy the object's transform
            if (iInitialSyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
            {
                nodesMapper.AddMapping(gameObject, m_Node, iInitialSyncDirection);
            }

            if (iInitialSyncDirection != iSyncDirection)
            {
                if (iInitialSyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
                {
                    nodesMapper.RemoveMapping(gameObject, false);
                }

                if (iSyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
                {
                    nodesMapper.AddMapping(gameObject, m_Node, iSyncDirection);
                }
            }

            m_NodeName = m_Node.GetName();
            m_SyncDirection = iSyncDirection;

            if (m_EventListener == null)
            {
                m_EventListener = new vrEventListener(_OnNodeEvent);
            }

            m_Node.AddEventListener(m_EventListener);
        }
        else
        {
            m_NodeName = "";
        }
    }

    private bool _OnNodeEvent(vrEvent iEvent)
    {
        if (!m_IsNodeGrabbed)
        {
            vrObjectEvent objectEvent = vrObjectEvent.Cast(iEvent);
            if (objectEvent != null && objectEvent.GetEventType() == (int) VRObjectEventEnum.VRObjectEvent_Destroy)
            {
                MVRNodesMapper nodesMapper = MVRNodesMapper.Instance;

                if (nodesMapper != null && m_SyncDirection != MVRNodesMapper.ENodesSyncDirection.NoSynchronization)
                {
                    nodesMapper.RemoveMapping(gameObject, false);
                }

                SetSyncDirection(MVRNodesMapper.ENodesSyncDirection.NoSynchronization);
                m_Node = null;

                // Destroys the object later this frame, before rendering
                UnityEngine.Object.Destroy(gameObject);
            }
        }
        return true;
    }

    #endregion
}
