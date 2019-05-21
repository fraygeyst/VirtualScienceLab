/* VRShortcutReload
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/Shortcut Reload")]
public class VRShortcutReload : MonoBehaviour
{
    protected void Update()
    {
        vrKeyboard keyboard = MiddleVR.VRDeviceMgr.GetKeyboard();

        if (keyboard != null &&
            keyboard.IsKeyToggled(MiddleVR.VRK_R) &&
            (keyboard.IsKeyPressed(MiddleVR.VRK_LSHIFT) || keyboard.IsKeyPressed(MiddleVR.VRK_RSHIFT)))
        {
            if ((keyboard.IsKeyPressed(MiddleVR.VRK_LCONTROL) || keyboard.IsKeyPressed(MiddleVR.VRK_RCONTROL)))
            {
                // Reload Simulation (level 0).
                SceneManager.LoadScene(0);
            }
            else
            {
                // Reload last loaded level.
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }
}
