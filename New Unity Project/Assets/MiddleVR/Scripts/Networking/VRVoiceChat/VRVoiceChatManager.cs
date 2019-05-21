using UnityEngine;
using UnityEngine.Networking;
using System.Diagnostics;
using System.Collections;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;

[HelpURL("http://www.middlevr.com/doc/current/#voice-communication")]
public class VRVoiceChatManager : NetworkBehaviour
{
    #region Attributes
    private NetworkManager m_NetworkManager = null;
    private Process m_WebRTCServerProcess = null;
    private vrWebView m_VRRTCWebView = null;
    private string m_RTCServerURL = "https://localhost:7778";

    private bool m_ServerReady = false;

    private vrCommand m_RTCConnectionReadyCommand = null;

    public bool m_EnableVoiceChat = true;
    #endregion

    #region MonoBehaviour Integration
    protected void OnDisable()
    {
        KillServerProcess();
    }

    protected void OnApplicationQuit()
    {
        if (isServer)
        {
            KillServerProcess();
        }
    }

    private void OnDestroy()
    {
        MiddleVR.DisposeObject(ref m_RTCConnectionReadyCommand);
    }
    #endregion

    #region NetworkBehaviour Integration
    public override void OnStartServer()
    {
        if (vrClusterManager.GetInstance().IsClient() || !m_EnableVoiceChat)
            return;

        StartCoroutine(LaunchWebRTCServer());
    }

    public override void OnStartClient()
    {
        if (vrClusterManager.GetInstance().IsClient() || !m_EnableVoiceChat)
            return;

        if (!isServer)
        {
            m_NetworkManager =  FindObjectOfType<NetworkManager>();
            m_RTCServerURL = "https://" + m_NetworkManager.networkAddress + ":" + (m_NetworkManager.networkPort + 1).ToString();
        }

        m_RTCConnectionReadyCommand = new vrCommand("RTCConnectionReady", RTCConnectionReadyCommandHandler, null, (uint)VRCommandFlags.VRCommandFlag_DontSynchronizeCluster);

        StartCoroutine(CreateWebView());
    }
    #endregion

    private void CopyDirectory(string iSourceDir, string iDestDir)
    {
        DirectoryInfo dir = new DirectoryInfo(iSourceDir);

        DirectoryInfo[] dirs = dir.GetDirectories();
        if (!Directory.Exists(iDestDir))
        {
            Directory.CreateDirectory(iDestDir);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(iDestDir, file.Name);
            file.CopyTo(tempPath, false);
        }

        foreach (DirectoryInfo subdir in dirs)
        {
            string temppath = Path.Combine(iDestDir, subdir.Name);
            CopyDirectory(subdir.FullName, temppath);
        }
    }

    private void KillServerProcess()
    {
        if (m_WebRTCServerProcess != null)
        {
            Process.Start("cmd", "/C taskkill /f /t /pid " + m_WebRTCServerProcess.Id);
            m_WebRTCServerProcess = null;
        }
    }

    private void CopyWebRTCServer()
    {
        string pathToAppDataWebRTCServer = Path.GetFullPath(MiddleVR.VRKernel.GetAppDataFolder() + "\\MVRRTCServer");
        if (!Directory.Exists(pathToAppDataWebRTCServer))
        {
            string pathToInstalledWebRTCServer = Path.GetFullPath(MiddleVR.VRKernel.GetModuleFolder() + "\\MVRRTCServer");
            CopyDirectory(pathToInstalledWebRTCServer, pathToAppDataWebRTCServer);
        }

        m_ServerReady = true;
    }

    private IEnumerator LaunchWebRTCServer()
    {
        MiddleVR.VRLog(1, "[>] Starting copy of the WebRTC server.");

        // We currently have to copy the WebRTC server in %appdata%/MiddleVR
        // due to Windows not allowing a process to start an other process
        // from "Program Files" without admin right.
        var copyingServerThread = new Thread(CopyWebRTCServer);
        copyingServerThread.Start();

        while (!m_ServerReady)
        {
            yield return null;
        }

        copyingServerThread.Join();

        MiddleVR.VRLog(1, "[<] Ending copy of the WebRTC server.");

        m_NetworkManager = FindObjectOfType<NetworkManager>();

        var startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;

        string pathToAppDataWebRTCServer = Path.GetFullPath(MiddleVR.VRKernel.GetAppDataFolder() + "\\MVRRTCServer");
        startInfo.WorkingDirectory = pathToAppDataWebRTCServer;
        startInfo.FileName = pathToAppDataWebRTCServer + "\\node.exe";
        startInfo.Arguments = pathToAppDataWebRTCServer + "\\server.js " + (m_NetworkManager.networkPort + 1).ToString();

        m_WebRTCServerProcess = new Process();
        m_WebRTCServerProcess.StartInfo = startInfo;
        try
        {
            if (!m_WebRTCServerProcess.Start())
            {
                MiddleVR.VRLog(1, "[X] An error occured when launching the voice chat server, process already running.");
            }
        }
        catch (Exception e)
        {
            MiddleVR.VRLog(1, "[X] An error occured when launching the voice chat server.\n" + e.Message);
        }

        m_RTCServerURL = "https://" + m_NetworkManager.networkAddress + ":" + (m_NetworkManager.networkPort + 1).ToString();
    }

    private IEnumerator CreateWebView()
    {
        bool serverReady = false;
        float time = Time.time;

        do
        {
            WWW www = new WWW(m_RTCServerURL);
            yield return www;

            if (string.IsNullOrEmpty(www.error) ||
                (www.error.IndexOf("Could not resolve host", 0) < 0 &&
                 www.error.IndexOf("Connection refused", 0) < 0))
            {
                serverReady = true;
            }
            else
            {
                yield return new WaitForSeconds(10.0f);
            }
        } while (!serverReady && Time.time - time < 60.0f);

        if (serverReady)
        {
            m_VRRTCWebView = new vrWebView("", m_RTCServerURL);
        }
    }

    private vrValue RTCConnectionReadyCommandHandler(vrValue iValue)
    {
        if (isServer)
        {
            m_VRRTCWebView.ExecuteJavascript("OpenRoom('MVRAudioChat', '" + m_RTCServerURL + "');");
        }
        else
        {
            m_VRRTCWebView.ExecuteJavascript("JoinRoom('MVRAudioChat', '" + m_RTCServerURL + "');");
        }

        return null;
    }
}
