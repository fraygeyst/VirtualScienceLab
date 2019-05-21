using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class VRNetworkAddOptionsToMenu : MonoBehaviour
{
    private vrWidgetButton m_StartHostButton;
    private vrWidgetButton m_ConnectToLocalServerButton;
    private vrWidgetButton m_DisconnectButton;

    // Start waits on VRMenu creation with a coroutine
    protected IEnumerator Start()
    {
       

        MVRTools.RegisterCommands(this);

        // Wait for the menu to be created
        VRMenu MiddleVRMenu = null;
        while (MiddleVRMenu == null || MiddleVRMenu.menu == null)
        {
            // Wait for VRMenu to be created
            yield return null;
            MiddleVRMenu = FindObjectOfType(typeof(VRMenu)) as VRMenu;
        }

        // Add a "Networking" submenu
        var networkMenu = new vrWidgetMenu("VRMenu.Networking", MiddleVRMenu.menu, "Networking");
        MiddleVRMenu.menu.SetChildIndex(networkMenu, 0);
        MVRTools.RegisterObject(this, networkMenu);

        // Add a separator below it
        var separator = new vrWidgetSeparator("", MiddleVRMenu.menu);
        MiddleVRMenu.menu.SetChildIndex(separator, 2);
        MVRTools.RegisterObject(this, separator);

        var networkManager = GetComponent<NetworkManager>();
        var defaultAddress = networkManager.networkAddress + ":" + networkManager.networkPort;

        m_StartHostButton = new vrWidgetButton("VRMenu.Networking.StartNetworkHost", networkMenu, "Start host (Server and client)", MVRTools.GetCommand("NetworkStartHost"));
        MVRTools.RegisterObject(this, m_StartHostButton);

        m_ConnectToLocalServerButton = new vrWidgetButton("VRMenu.Networking.ConnectToDefaultServer", networkMenu, "Connect to default server (" + defaultAddress + ")", MVRTools.GetCommand("NetworkConnectToDefaultServer"));
        MVRTools.RegisterObject(this, m_ConnectToLocalServerButton);

        m_DisconnectButton = new vrWidgetButton("VRMenu.Networking.Disconnect", networkMenu, "Disconnect", MVRTools.GetCommand("NetworkDisconnect"));
        MVRTools.RegisterObject(this, m_DisconnectButton);
    }

    [VRCommand]
    private void NetworkStartHost()
    {
        GetComponent<NetworkManager>().StartHost();
    }

    [VRCommand]
    private void NetworkConnectToDefaultServer()
    {
        GetComponent<NetworkManager>().StartClient();
    }

    [VRCommand]
    private void NetworkDisconnect()
    {
        GetComponent<NetworkManager>().StopHost();
    }
}
