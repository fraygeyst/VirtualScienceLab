/* VRManagerPostFrame
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRManagerPostFrame : MonoBehaviour {
    public vrKernel kernel = null;
    public vrDeviceManager deviceMgr = null;
    public vrClusterManager clusterMgr = null;

    private bool LoggedNoKeyboard = false;

    private bool ContinuePostFrameUpdate = false;

    #region MonoBehaviour
    private void Start()
    {
        MVRTools.Log(4, "[ ] Unity: StartCoroutine PostFrameUpdate");

        ContinuePostFrameUpdate = true;
        StartCoroutine(PostFrameUpdate());
    }
    
    private void Update()
    {
        MVRTools.Log(4, "[>] Unity: VR EndFrame Update!");

        MiddleVR.VRKernel.EndFrameUpdate();

        MVRTools.Log(4, "[<] Unity: End of VR EndFrame Update!");
    }

    protected void OnApplicationQuit()
    {
        ContinuePostFrameUpdate = false;
    }
    #endregion

    #region VRManagerPostFrame
    private void InitManagers()
    {
        if (kernel == null)
        {
            kernel = MiddleVR.VRKernel;
        }

        if (deviceMgr == null)
        {
            deviceMgr = MiddleVR.VRDeviceMgr;
        }

        if (clusterMgr == null)
        {
            clusterMgr = MiddleVR.VRClusterMgr;
        }
    }

    // This coroutine when started, will wait for every end of frame and then
    // call the PostFrameUpdate of the vrKernel.
    private IEnumerator PostFrameUpdate()
    {
        while (ContinuePostFrameUpdate)
        {
            yield return new WaitForEndOfFrame();

            var mgr = GetComponent<VRManagerScript>();
            if (mgr != null && MiddleVR.VRKernel == null)
            {
                Debug.LogWarning("[ ] If you have an error mentioning 'DLLNotFoundException: MiddleVR_CSharp', please restart Unity. If this does not fix the problem, please make sure MiddleVR is in the PATH environment variable.");
                mgr.GetComponent<GUIText>().text = "[ ] Check the console window to check if you have an error mentioning 'DLLNotFoundException: MiddleVR_CSharp', please restart Unity. If this does not fix the problem, please make sure MiddleVR is in the PATH environment variable.";
            }

            MVRTools.Log(4, "[>] Unity: Start of VR PostFrameUpdate.");

            if (kernel == null || deviceMgr == null || clusterMgr == null)
            {
                InitManagers();
            }

            if (deviceMgr != null)
            {
                var keyboard = deviceMgr.GetKeyboard();
                if (keyboard != null)
                {
                    // Because when the voice chat is on the input are not redirected to MiddleVR we need to check the Input.GetKey(KeyCode.Escape)
                    if (mgr != null && mgr.QuitOnEsc && (keyboard.IsKeyPressed((uint)MiddleVR.VRK_ESCAPE) || Input.GetKey(KeyCode.Escape)))
                    {
                        mgr.QuitApplication();
                    }
                }
                else
                {
                    if (!LoggedNoKeyboard)
                    {
                        MVRTools.Log("[X] No VR keyboard.");
                        LoggedNoKeyboard = true;
                    }
                }
            }

            if (kernel != null)
            {
                kernel.PostFrameUpdate();
            }

            MVRTools.Log(4, "[<] Unity: End of VR PostFrameUpdate.");

            if (kernel != null && kernel.GetFrame() == 2 && !Application.isEditor)
            {
                MVRTools.Log(2, "[ ] If the application is stuck here and you're using Quad-buffer active stereoscopy, make sure that in the Player Settings of Unity, the option 'Run in Background' is checked.");
            }
        }
    }
    #endregion
}
