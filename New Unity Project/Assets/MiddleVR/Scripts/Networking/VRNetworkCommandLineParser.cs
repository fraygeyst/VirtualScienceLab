using System;
using UnityEngine;
using UnityEngine.Networking;

public class VRNetworkCommandLineParser : MonoBehaviour
{
    private const string StartHostArgument     = "--mvr-start-host";
    private const string StartServerArgument   = "--mvr-start-server";
    private const string ClientConnectArgument = "--mvr-client-connect";

    private void Start ()
    {
        var networkManager = GetComponent<NetworkManager>();
        if (!networkManager)
        {
            Debug.LogError("[X] VRNetworkCommandLineParser could not find NetworkManager instance!");
            return;
        }

	    var cmdLineArguments = Environment.GetCommandLineArgs();
        for (int i = 0; i < cmdLineArguments.Length; ++i)
        {
            var cmdLineArg = cmdLineArguments[i];

            if (cmdLineArg.StartsWith(StartHostArgument))
            {
                var argumentElements = cmdLineArg.Split('=');
                if (argumentElements.Length > 1)
                {
                    var portString = argumentElements[1];
                    networkManager.networkPort = int.Parse(portString);
                }
                networkManager.StartHost();
                break;
            }
            else if (cmdLineArg.StartsWith(StartServerArgument))
            {
                var argumentElements = cmdLineArg.Split('=');
                if (argumentElements.Length > 1)
                {
                    var portString = argumentElements[1];
                    networkManager.networkPort = int.Parse(portString);
                }
                networkManager.StartServer();
                break;
            }
            else if (cmdLineArg.StartsWith(ClientConnectArgument))
            {
                var argumentElements = cmdLineArg.Split('=');
                if (argumentElements.Length > 1)
                {
                    var addressString = argumentElements[1];
                    var addressElements = addressString.Split(':');
                    networkManager.networkAddress = addressElements[0];
                    if (addressElements.Length > 1)
                    {
                        networkManager.networkPort = int.Parse(addressElements[1]);
                    }
                }
                networkManager.StartClient();
                break;
            }
        }
    }
}
