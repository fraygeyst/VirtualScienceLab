/* VRInteraction
 * MiddleVR
 * (c) MiddleVR
 */

using System.Linq;
using UnityEngine;
using MiddleVR_Unity3D;
using UnityEngine.Networking;

[AddComponentMenu("")]
public class VRInteraction : MonoBehaviour
{
    public enum AuthorityRequestState
    {
        None,
        Accepted,
        Pending,
        Denied
    }

    private AuthorityRequestState m_AuthorityRequestState = AuthorityRequestState.None;
    private vrInteraction   m_Interaction = null;
    private vrEventListener m_Listener = null;

    private bool m_HasSearchedForNetworkInteractionsHandler = false;
    private VRNetworkInteractionsHandler m_NetworkInteractionsHandler = null;

    private VRNetworkInteractionsHandler GetNetworkInteractionsHandler()
    {
        if (!m_HasSearchedForNetworkInteractionsHandler)
        {
            m_NetworkInteractionsHandler = FindObjectsOfType<VRNetworkInteractionsHandler>()
                .FirstOrDefault(handlerComponent => handlerComponent.isLocalPlayer);

            if (m_NetworkInteractionsHandler == null)
            {
                MVRTools.Log(1,
                    "[X] VRInteraction: Cannot find an instance of VRNetworkInteractionsHandler on a local player.");
            }

            m_HasSearchedForNetworkInteractionsHandler = true;
        }

        return m_NetworkInteractionsHandler;
    }

    private bool m_IsActive = false;

    private void OnDestroy()
    {
        MiddleVR.DisposeObject(ref m_Listener);
        MiddleVR.DisposeObject(ref m_Interaction);
    }

    private bool EventListener(vrEvent iEvent)
    {
        vrInteractionEvent evt = vrInteractionEvent.Cast(iEvent);
        if (evt == null)
        {
            return false;
        }

        vrInteraction evtInteraction = evt.GetInteraction();

        if (m_Interaction != null && evtInteraction != null && evt != null &&
            evtInteraction.GetId() == m_Interaction.GetId())
        {
            var eventType = evt.GetEventType();

            if (eventType == (int)VRInteractionEventEnum.VRInteractionEvent_Activated)
            {
                Activate();
            }
            else if (eventType == (int)VRInteractionEventEnum.VRInteractionEvent_Deactivated)
            {
                Deactivate();
            }
        }

        return true;
    }

    public void Activate()
    {
        if (!m_IsActive)
        {
            m_IsActive = true;
            MiddleVR.VRInteractionMgr.Activate(m_Interaction);

            OnActivate();
        }
    }

    public void Deactivate()
    {
        if (m_IsActive)
        {
            m_IsActive = false;
            MiddleVR.VRInteractionMgr.Deactivate(m_Interaction);

            OnDeactivate();
        }
    }

    protected virtual void OnActivate()
    {
        MVRTools.Log(3, "[ ] VRInteraction: Activating '" + m_Interaction.GetName() + "'.");
    }

    protected virtual void OnDeactivate()
    {
        MVRTools.Log(3, "[ ] VRInteraction: Deactivating '" + m_Interaction.GetName() + "'.");
    }

    public bool IsActive()
    {
        return m_IsActive;
    }

    public void InitializeBaseInteraction()
    {
        m_Listener = new vrEventListener(EventListener);
        MiddleVR.VRInteractionMgr.AddEventListener(m_Listener);
    }

    public vrInteraction CreateInteraction(string iName)
    {
        if (m_Interaction == null)
        {
            // Create the requested interaction
            m_Interaction = new vrInteraction(iName);
            MiddleVR.VRInteractionMgr.AddInteraction(m_Interaction);
        }
        else
        {
            // Interaction already existing, rename it
            m_Interaction.SetName(iName);
        }

        return m_Interaction;
    }

    public void SetInteraction(vrInteraction iInteraction)
    {
        m_Interaction = iInteraction;
    }

    public vrInteraction GetInteraction()
    {
        return m_Interaction;
    }

    private MVRNodesMapper.ENodesSyncDirection m_ObjectPreviousSyncDir = MVRNodesMapper.ENodesSyncDirection.NoSynchronization;

    protected AuthorityRequestState RequestAssignClientAuthority(GameObject iGameObject)
    {
        if (m_AuthorityRequestState == AuthorityRequestState.None)
        {
            var networkTransform = iGameObject.GetComponent<NetworkTransform>();
            var networkTransformChild = iGameObject.GetComponent<NetworkTransformChild>();

            if (networkTransformChild == null && networkTransform == null)
            {
                m_AuthorityRequestState = AuthorityRequestState.Accepted;
            }
            else
            {
                if (MiddleVR.VRClusterMgr.IsClient())
                {
                    m_AuthorityRequestState = AuthorityRequestState.Denied;
                }
                else
                {
                    var authorityHandler = GetNetworkInteractionsHandler();
                    if (authorityHandler == null)
                    {
                        return AuthorityRequestState.Accepted;
                    }

                    authorityHandler.RequestAssignClientAuthority(iGameObject, this);
                    m_AuthorityRequestState = AuthorityRequestState.Pending;
                }
            }
        }

        return m_AuthorityRequestState;
    }

    protected void RequestRemoveClientAuthority(GameObject iGameObject)
    {
        var networkTransform = iGameObject.GetComponent<NetworkTransform>();
        var networkTransformChild = iGameObject.GetComponent<NetworkTransformChild>();

        if (!MiddleVR.VRClusterMgr.IsClient())
        {
            if (networkTransformChild != null || networkTransform != null)
            {
                var authorityHandler = GetNetworkInteractionsHandler();
                if (authorityHandler != null)
                {
                    authorityHandler.RequestRemoveClientAuthority(iGameObject);
                }
            }
        }
    }

    protected void ClearClientAuthorityRequest()
    {
        // Clear interaction from player object
        m_AuthorityRequestState = AuthorityRequestState.None;
    }

    public void SetClientAuthorityRequestState(AuthorityRequestState state)
    {
        m_AuthorityRequestState = state;
    }

    protected vrNode3D AcquireGameObjectNode(GameObject iGameObject, string iTempNodeName)
    {
        var nodeSyncComponent = iGameObject.GetComponent<VRNode3DSynchronization>();
        var node = nodeSyncComponent.Node;
        m_ObjectPreviousSyncDir = nodeSyncComponent.SyncDirection;
        // Hack to force update of vrNode3D beforehand
        nodeSyncComponent.SetSyncDirection( MVRNodesMapper.ENodesSyncDirection.UnityToMiddleVR );
        nodeSyncComponent.SetSyncDirection( MVRNodesMapper.ENodesSyncDirection.MiddleVRToUnity );
        return node;
    }

    protected void ReleaseGameObjectNode(GameObject iGameObject)
    {
        var nodeSyncComponent = iGameObject.GetComponent<VRNode3DSynchronization>();
        nodeSyncComponent.SetSyncDirection( m_ObjectPreviousSyncDir);
    }
}
