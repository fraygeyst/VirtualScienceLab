#if UNITY_5_6_OR_NEWER
using System;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using System.IO;
using Debug = UnityEngine.Debug;

[InitializeOnLoad]
public class MiddleVR_UnityEngine_Networking_Check
{
    private const string CompanyName = "MiddleVR";

#if UNITY_5_6
    private const string FileVersion = "1.7.1.1";
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer5.6.exe";
#elif UNITY_2017_1
    private const string FileVersion = "1.7.1.1";
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer2017.1.exe";
#elif UNITY_2017_2
    private const string FileVersion = "1.7.1.1";
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer2017.2.exe";
#elif UNITY_2017_3 || UNITY_2017_4
    private const string FileVersion = "1.7.1.1";
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer2017.3.exe";
#elif UNITY_2018_1
    private const string FileVersion = "1.7.1.1";
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer2018.1.exe";
#elif UNITY_2018_2_OR_NEWER
    private const string FileVersion = "1.7.1.1";
    private const string InstallerName = "MiddleVR_UnityEngine.Networking_Installer2018.2.exe";
#endif
    private const string UninstallerName = "MiddleVR_UnityEngine.Networking_Uninstaller.exe";

    static MiddleVR_UnityEngine_Networking_Check()
    {
        if (UnityEditorInternal.InternalEditorUtility.isHumanControllingUs)
        {
            string prefKey = InstallerName + "_version_" + FileVersion + "_unity_" + Application.unityVersion;

            if (!EditorPrefs.GetBool(prefKey, false))
            {
                CheckUnityNetworking();
                EditorPrefs.SetBool(prefKey, true);
            }
        }
    }

    private static bool RunExe(string exePath, string arguments)
    {
        var uninstallerProcess = new Process();
        uninstallerProcess.StartInfo = new ProcessStartInfo(exePath.Replace("/", "\\"), arguments);

        uninstallerProcess.Start();
        uninstallerProcess.WaitForExit();

        return uninstallerProcess.ExitCode == 0;
    }

    [MenuItem("MiddleVR/Install MiddleVR Networking version")]
    public static void InstallMiddleVRNetworking()
    {
        CheckUnityNetworking(true);
    }

    [MenuItem("MiddleVR/Uninstall MiddleVR Networking version")]
    public static void UninstallMiddleVRNetworking()
    {
        var installFolder = Path.Combine(EditorApplication.applicationContentsPath, "UnityExtensions/Unity/Networking");
        var uninstallerPath = Path.Combine(installFolder, UninstallerName);

        if (File.Exists(uninstallerPath))
        {
            if (RunExe(uninstallerPath, "/S"))
            {
                EditorUtility.DisplayDialog("MiddleVR", "MiddleVR Networking has been successfully uninstalled!", "OK");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("MiddleVR", "MiddleVR Networking is not installed for this version of Unity Editor.", "OK");
        }
    }

	private static void RestartUnity()
	{
		string applicationPath = EditorApplication.applicationPath;
		string projectPath = Application.dataPath;
		DirectoryInfo parentDir = Directory.GetParent(projectPath);
		ProcessStartInfo info = new ProcessStartInfo();
		Debug.Log(parentDir.FullName);
		info.Arguments = "-projectPath \"" + parentDir.FullName + "\"";
		info.FileName = applicationPath;
		Process.Start(info);
		Process.GetCurrentProcess().Kill();
	}

    public static void CheckUnityNetworking(bool displayAlreadyInstalledMessage = false)
    {
        var installFolder = Path.Combine(EditorApplication.applicationContentsPath, "UnityExtensions/Unity/Networking");
        var localFolder = Path.Combine(Application.dataPath, "MiddleVR/Editor");

        System.Diagnostics.FileVersionInfo versionInfo = null;

        var targetNetworkingAssembly = Path.Combine(installFolder, "UnityEngine.Networking.dll");

        try
        {
            versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(targetNetworkingAssembly);
        }
        catch (System.Exception)
        {
            Debug.Log("[X] MiddleVR failed to check version of '" + targetNetworkingAssembly + "'.");
            return;
        }
        
        var installerPath = Path.Combine(Path.Combine(Application.dataPath, "MiddleVR/Editor"), InstallerName);

        if (versionInfo.CompanyName != CompanyName)
        {
            var dialogMessage =
                "MiddleVR Networking is not present in your Unity installation, would you like to install it? Note: this will add MiddleVR Networking features to your Unity installation. There will be no impact on your existing applications, only new applications using MiddleVR Networking. See documentation for more information.";
            if (EditorUtility.DisplayDialog("MiddleVR", dialogMessage, "Install MiddleVR Networking", "Cancel"))
            {
                try
                {
                    if (RunExe(installerPath, "/S /D=" + installFolder.Replace("/", "\\")))
                    {
						bool result = EditorUtility.DisplayDialog("MiddleVR", "MiddleVR Networking has successfully been installed! Unity will restart to applied the changes.", "OK");

						if (result)
						{
							RestartUnity();
						}
					}
                    else
                    {
                        EditorUtility.DisplayDialog("MiddleVR", "An error has occured while installing MiddleVR Networking! Please contact MiddleVR Support.", "OK");
                    }
                }
                catch (Win32Exception)
                {
                    EditorUtility.DisplayDialog("MiddleVR", "The MiddleVR Networking installer was not able to run.", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("MiddleVR", "You can install MiddleVR Networking at any time from the MiddleVR menu in the Unity Editor.", "OK");
            }
        }
        else if (versionInfo.FileVersion != FileVersion)
        {
            var dialogMessage =
                "Your installation uses a different version of MiddleVR Networking (" + versionInfo.FileVersion + "), would you like to replace it with the current version ("+ FileVersion + ")?";
            if (EditorUtility.DisplayDialog("MiddleVR", dialogMessage, "Replace MiddleVR Networking", "Cancel"))
            {
                try
                {
                    var uninstallerPath = Path.Combine(installFolder, UninstallerName);
                    if (RunExe(uninstallerPath, "/S"))
                    {
                        try
                        {
                            if (RunExe(installerPath, "/S /D=" + installFolder.Replace("/", "\\")))
                            {
								bool result = EditorUtility.DisplayDialog("MiddleVR", "MiddleVR Networking has successfully been installed! Unity will restart to applied the changes.", "OK");

								if (result)
								{
									RestartUnity();
								}
							}
                            else
                            {
                                EditorUtility.DisplayDialog("MiddleVR", "An error has occured while installing MiddleVR Networking! Please contact MiddleVR Support.", "OK");
                            }
                        }
                        catch (Win32Exception)
                        {
                            EditorUtility.DisplayDialog("MiddleVR", "The MiddleVR Networking installer was not able to run. MiddleVR Networking is not installed.", "OK");
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("MiddleVR", "An error has occured while uninstalling MiddleVR Networking! Please contact MiddleVR Support.", "OK");
                    }
                }
                catch (Win32Exception)
                {
                    EditorUtility.DisplayDialog("MiddleVR", "The MiddleVR Networking uninstaller was not able to run.", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("MiddleVR", "You can replace MiddleVR Networking at any time from the MiddleVR menu in the Unity Editor.", "OK");
            }
        }
        else if(displayAlreadyInstalledMessage)
        {
            EditorUtility.DisplayDialog("MiddleVR", "MiddleVR Networking is already installed for this version of Unity Editor.", "OK");
        }
    }
}
#endif
