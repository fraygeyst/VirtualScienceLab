/* VRMenuMaximizeButton
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class VRMenuMaximizeButton : MonoBehaviour
{
    public VRMenuManager MenuManager;
    public GameObject    ToggleIcon;

    public Texture ClosedTexture;
    public Texture OpenTexture;

    protected void VRAction()
    {
        if (MenuManager != null)
        {
            MenuManager.ToggleMenuGUI();
        }
    }

    public void Toggle(bool iIsOpen)
    {
        if (iIsOpen)
        {
            ToggleIcon.GetComponent<Renderer>().material.SetTexture("_MainTex", OpenTexture);
        }
        else
        {
            ToggleIcon.GetComponent<Renderer>().material.SetTexture("_MainTex", ClosedTexture);
        }
    }
}
