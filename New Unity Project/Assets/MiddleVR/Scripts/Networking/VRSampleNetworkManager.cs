using UnityEngine;
using UnityEngine.Networking;

public class VRCustomNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("VRCustomNetworkManager.OnClientConnect(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("VRCustomNetworkManager.OnClientDisconnect(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("VRCustomNetworkManager.OnClientError(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnClientNotReady(NetworkConnection conn)
    {
        Debug.Log("VRCustomNetworkManager.OnClientNotReady(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("VRCustomNetworkManager.OnClientSceneChanged(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("VRCustomNetworkManager.OnServerAddPlayer(" + conn.hostId + "[" + conn.address + "], " + playerControllerId + ")");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        Debug.Log("VRCustomNetworkManager.OnServerAddPlayer(" + conn.hostId + "[" + conn.address + "], " + playerControllerId + ")");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("VRCustomNetworkManager.OnServerConnect(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("VRCustomNetworkManager.OnServerDisconnect(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("VRCustomNetworkManager.OnServerError(" + conn.hostId + "[" + conn.address + "], " + errorCode + ")");
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("VRCustomNetworkManager.OnServerReady(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        Debug.Log("VRCustomNetworkManager.OnServerRemovePlayer(" + conn.hostId + "[" + conn.address + "])");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("VRCustomNetworkManager.OnServerSceneChanged(" + sceneName + ")");
    }

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("VRCustomNetworkManager.OnStartClient()");
    }

    public override void OnStartHost()
    {
        Debug.Log("VRCustomNetworkManager.OnStartHost()");
    }

    public override void OnStartServer()
    {
        Debug.Log("VRCustomNetworkManager.OnStartServer()");
    }

    public override void OnStopClient()
    {
        Debug.Log("VRCustomNetworkManager.OnStopClient()");
    }

    public override void OnStopHost()
    {
        Debug.Log("VRCustomNetworkManager.OnStopHost()");
    }

    public override void OnStopServer()
    {
        Debug.Log("VRCustomNetworkManager.OnStopServer()");
    }
}
