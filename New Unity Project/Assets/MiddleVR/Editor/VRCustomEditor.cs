/* VRCustomEditor
 * MiddleVR
 * (c) MiddleVR
 */

// In versions prior Unity 4.6 the fullscreen mode parameter does not exist.
// In the Unity 4.6 the full screen mode for DirectX 9 was added and the support
// for full screen mode in DirectX 11 was added in Unity 5.0.
#if !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5
#define UNITY_D3D9_FULLSCREEN_MODE
#if !UNITY_4_6 && !UNITY_4_7
#define UNITY_D3D11_FULLSCREEN_MODE
#endif
#endif
 
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using MiddleVR_Unity3D;
using UnityEditor.Callbacks;

[CustomEditor(typeof(VRManagerScript))]
public class VRCustomEditor : Editor
{
    //This will just be a shortcut to the target, ex: the object you clicked on.
    private VRManagerScript mgr;

    static private bool m_SettingsApplied = false;

    void Awake()
    {
        mgr = (VRManagerScript)target;

        if( !m_SettingsApplied )
        {
            ApplyVRSettings();
            m_SettingsApplied = true;
        }
    }

    void Start()
    {
        Debug.Log("MT: " + PlayerSettings.MTRendering);
    }

    public void ApplyVRSettings()
    {
        PlayerSettings.defaultIsFullScreen = false;
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
        PlayerSettings.runInBackground = true;
        PlayerSettings.captureSingleScreen = false;
        PlayerSettings.MTRendering = false;
        //PlayerSettings.usePlayerLog = false;
        //PlayerSettings.useDirect3D11 = false;
#if UNITY_D3D9_FULLSCREEN_MODE
        PlayerSettings.d3d9FullscreenMode = D3D9FullscreenMode.ExclusiveMode;
#endif
#if UNITY_D3D11_FULLSCREEN_MODE
        PlayerSettings.d3d11FullscreenMode = D3D11FullscreenMode.ExclusiveMode;
#endif

        MVRTools.Log("VR Player settings changed:");
        MVRTools.Log("- DefaultIsFullScreen = false");
        MVRTools.Log("- DisplayResolutionDialog = Disabled");
        MVRTools.Log("- RunInBackground = true");
        MVRTools.Log("- CaptureSingleScreen = false");
        //MVRTools.Log("- UsePlayerLog = false");

        string[] names = QualitySettings.names;
        int qualityLevel = QualitySettings.GetQualityLevel();

        // Disable VSync on all quality levels
        for( int i=0 ; i<names.Length ; ++i )
        {
            QualitySettings.SetQualityLevel( i );
            QualitySettings.vSyncCount = 0;
        }

        QualitySettings.SetQualityLevel( qualityLevel );

        MVRTools.Log("Quality settings changed for all quality levels:");
        MVRTools.Log("- VSyncCount = 0");
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();

        if (GUILayout.Button("Re-apply VR player settings"))
        {
            ApplyVRSettings();
        }

        if (GUILayout.Button("Pick configuration file"))
        {
            string path = EditorUtility.OpenFilePanel("Please choose MiddleVR configuration file", "C:/Program Files (x86)/MiddleVR/data/Config", "vrx");
            MVRTools.Log("[+] Picked " + path );
            mgr.ConfigFile = path;
            EditorUtility.SetDirty(mgr);
        }

        DrawDefaultInspector();
        GUILayout.EndVertical();
    }

    private static void CopyFolderIfExists(string iFolderName, string iSrcBasePath, string iDstBasePath)
    {
        string sourcePath = Path.Combine(iSrcBasePath, iFolderName);
        if (Directory.Exists(sourcePath))
        {
            // The player executable file and the data folder share the same base name
            string destinationPath = Path.Combine(iDstBasePath, iFolderName);
            FileSystemTools.DirectoryCopy(sourcePath, destinationPath, true, true);
        }
    }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
    {
        string pathToSourceData = Application.dataPath;
        string pathToPlayerData = Path.Combine(Path.GetDirectoryName(pathToBuiltProject), Path.GetFileNameWithoutExtension(pathToBuiltProject) + "_Data");

        CopyFolderIfExists("WebAssets", pathToSourceData, pathToPlayerData);  // Copy web assets 
        CopyFolderIfExists(".WebAssets", pathToSourceData, pathToPlayerData); // Copy web assets in hidden directory

        string pathToSourceDataMiddleVR = Path.Combine(pathToSourceData, "MiddleVR");
        string pathToPlayerDataMiddleVR = Path.Combine(pathToPlayerData, "MiddleVR");

        CopyFolderIfExists("WebAssets", pathToSourceDataMiddleVR, pathToPlayerDataMiddleVR);  // Copy web assets 
        CopyFolderIfExists(".WebAssets", pathToSourceDataMiddleVR, pathToPlayerDataMiddleVR); // Copy web assets in hidden directory

        string pathToPlayerAssemblyCSharp = Path.Combine(Path.Combine(pathToPlayerData, "Managed"), "Assembly-CSharp.dll");
        //UnityEngine.Networking.AssemblyPostProcessor.Process(pathToPlayerAssemblyCSharp);

        // Sign Application
        MVRTools.SignApplication( pathToBuiltProject );
    }
}

// InitializeOnLoad will run the static contructor of this class when
// starting the editor and after every re-compilation of this script
[InitializeOnLoad]
public class MiddleVRDeprecatedAssetsCleaner
{
    static MiddleVRDeprecatedAssetsCleaner()
    {
        Run();
    }

    internal static void Run()
    {
        EditorApplication.update += CleanAssets;
    }

    private static void CleanAssets()
    {
        // Ensure this is called only once
        EditorApplication.update -= CleanAssets;

        if (!File.Exists(Path.Combine(Application.dataPath, "MiddleVR_Source_Project.txt")))
        {
            // Clean old deprecated assets from previous MiddleVR versions
            string[] assetsToDelete = { "Assets/Editor/VRCustomEditor.cs",
                                        "Assets/MiddleVR/Resources/OVRLensCorrectionMat.mat",
                                        "Assets/MiddleVR/Scripts/Internal/VRCameraCB.cs",
                                        "Assets/MiddleVR/Assets/Materials/WandRayMaterial.mat",
                                        "Assets/Plugins/MiddleVR_UnityRendering.dll",
                                        "Assets/Plugins/MiddleVR_UnityRendering_x64.dll" };

            int filesDeleted = 0;

            foreach (string assetToDelete in assetsToDelete)
            {
                if (AssetDatabase.DeleteAsset(assetToDelete))
                {
                    filesDeleted++;
                    MVRTools.Log(3, "[ ] Deleting deprecated MiddleVR asset '" + assetToDelete + "'.");
                }
            }

            if (filesDeleted > 0)
            {
                MVRTools.Log(3, "[ ] Deleted " + filesDeleted.ToString() + " deprecated MiddleVR asset(s).");
                AssetDatabase.Refresh();
            }
        }
    }
}

public class AdditionnalImports : AssetPostprocessor
{
    public static void OnPostprocessAllAssets(string[] iImportedAssets,
                                              string[] iDeletedAssets,
                                              string[] iMovedAssets,
                                              string[] iMovedFromAssetPaths)
    {
        MiddleVRDeprecatedAssetsCleaner.Run();
    }
}

public class FileSystemTools
{
    public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite)
    {
        // Get the subdirectories for the specified directory
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: '"
                + sourceDirName + "'.");
        }

        // If the destination directory doesn't exist, create it
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            if (!file.Name.ToLower().EndsWith(".meta"))
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwrite);
            }
        }

        // If copying subdirectories, copy them and their contents to new location
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs, overwrite);
            }
        }
    }
}
