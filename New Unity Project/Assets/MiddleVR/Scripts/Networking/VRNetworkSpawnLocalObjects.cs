using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(NetworkIdentity))]
public class VRNetworkSpawnLocalObjects : NetworkBehaviour {

    #region Attributes
    [SerializeField]
    private GameObject m_HeadPrefab = null;
    [SerializeField]
    private GameObject m_HandPrefab = null;
    #endregion

    #region MonoBehaviour implementation
#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (m_HeadPrefab != null &&
            m_HeadPrefab.GetComponent<VRNetworkLocalObject>() == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Head prefab error",
                "Prefab '" + m_HeadPrefab.name + "' is missing a 'VRNetworkLocalObject' script.",
                "Continue");
            m_HeadPrefab = null;
        }

        if (m_HandPrefab != null &&
            m_HandPrefab.GetComponent<VRNetworkLocalObject>() == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Hand prefab error",
                "Prefab '" + m_HandPrefab.name + "' is missing a 'VRNetworkLocalObject' script.",
                "Continue");
            m_HandPrefab = null;
        }
    }
#endif
    #endregion

    #region NetworkBehaviour implementation
    public override void OnStartLocalPlayer()
    {
        SpawnLocalNodes();
    }
    #endregion

    #region Spawn objects
    private void SpawnLocalNodes()
    {
        var mvrDisplayManager = vrDisplayManager.GetInstance();

        uint iEnd = mvrDisplayManager.GetNodesNb();
        for (uint i = 0; i < iEnd; ++i)
        {
            var node = mvrDisplayManager.GetNodeByIndex(i);
            var nodeTag = node.GetTag();
            if (m_HeadPrefab != null && nodeTag == "Head")
            {
                CmdSpawnLocalHead(node.GetName());
            }
            else if (m_HandPrefab != null && nodeTag == "Hand")
            {
                CmdSpawnLocalHand(node.GetName());
            }
        }
    }

    [Command]
    private void CmdSpawnLocalHead(string nodeName)
    {
        if (m_HeadPrefab == null)
        {
            return;
        }

        var head = Instantiate(m_HeadPrefab);
        var localObjectScript = head.GetComponent<VRNetworkLocalObject>();
        if (localObjectScript == null)
        {
            Debug.LogError("Hand Prefab does not have a VRNetworkLocalObject component!");
            return;
        }

        localObjectScript.nodeName = nodeName;
        localObjectScript.owner = gameObject;
        NetworkServer.SpawnWithClientAuthority(head, connectionToClient);
    }

    [Command]
    private void CmdSpawnLocalHand(string nodeName)
    {
        if (m_HandPrefab == null)
        {
            return;
        }

        var hand = Instantiate(m_HandPrefab);
        var localObjectScript = hand.GetComponent<VRNetworkLocalObject>();
        if (localObjectScript == null)
        {
            Debug.LogError("Hand Prefab does not have a VRNetworkLocalObject component!");
            return;
        }

        localObjectScript.nodeName = nodeName;
        localObjectScript.owner = gameObject;
        NetworkServer.SpawnWithClientAuthority(hand, connectionToClient);
    }
    #endregion
}
