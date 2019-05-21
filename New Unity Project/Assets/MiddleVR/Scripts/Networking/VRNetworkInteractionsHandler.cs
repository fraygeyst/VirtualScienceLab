using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class VRNetworkInteractionsHandler : NetworkBehaviour
{
    private Dictionary<GameObject, VRInteraction> m_AssignClientAuthorityRequests = new Dictionary<GameObject, VRInteraction>();

    [Command]
    private void CmdAssignClientAuthority(GameObject iObject)
    {
        if (iObject != null)
        {
            var networkIdentity = iObject.GetComponent<NetworkIdentity>();
            if (networkIdentity.clientAuthorityOwner == null)
            {
                networkIdentity.AssignClientAuthority(connectionToClient);
                RpcAssignClientAuthority(iObject, gameObject, true);
                return;
            }
        }
        RpcAssignClientAuthority(iObject, gameObject, false);
    }

    [Command]
    private void CmdRemoveClientAuthority(GameObject iObject)
    {
        if (iObject != null)
        {
            var networkIdentity = iObject.GetComponent<NetworkIdentity>();
            if (networkIdentity.clientAuthorityOwner == connectionToClient)
            {
                networkIdentity.RemoveClientAuthority(networkIdentity.clientAuthorityOwner);
                RpcRemoveClientAuthority(iObject, gameObject, true);
                return;
            }
        }
        RpcRemoveClientAuthority(iObject, gameObject, false);
    }

    [ClientRpc]
    private void RpcAssignClientAuthority(GameObject iObject, GameObject iRequestPlayer, bool success)
    {
        if (iRequestPlayer == gameObject)
        {
            if (m_AssignClientAuthorityRequests.ContainsKey(iObject))
            {
                var interaction = m_AssignClientAuthorityRequests[iObject];
                m_AssignClientAuthorityRequests.Remove(iObject);
                interaction.SetClientAuthorityRequestState(
                    success
                    ? VRInteraction.AuthorityRequestState.Accepted
                    : VRInteraction.AuthorityRequestState.Denied);
            }
        }
    }

    [ClientRpc]
    private void RpcRemoveClientAuthority(GameObject iObject, GameObject iRequestPlayer, bool success)
    {
    }

    [Client]
    public void RequestAssignClientAuthority(GameObject iObject, VRInteraction interaction)
    {
        if (iObject != null && isLocalPlayer)
        {
            m_AssignClientAuthorityRequests.Add(iObject, interaction);
            CmdAssignClientAuthority(iObject);
        }
    }

    [Client]
    public void RequestRemoveClientAuthority(GameObject iObject)
    {
        if (iObject != null && isLocalPlayer)
        {
            CmdRemoveClientAuthority(iObject);
        }
    }
}
