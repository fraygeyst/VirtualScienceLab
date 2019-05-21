/* VRCustomizeDefaultMenu

 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/GUI/Customize Default Menu")]
public class VRCustomizeDefaultMenu : MonoBehaviour
{
    // Start waits on VRMenu creation with a coroutine
    protected IEnumerator Start()
    {
        MVRTools.RegisterCommands(this);

        VRMenu MiddleVRMenu = null;
        while (MiddleVRMenu == null || MiddleVRMenu.menu == null)
        {
            // Wait for VRMenu to be created
            yield return null;
            MiddleVRMenu = FindObjectOfType(typeof(VRMenu)) as VRMenu;
        }

        AddButton(MiddleVRMenu);
        //RemoveItem(MiddleVRMenu);
        //MoveItems(MiddleVRMenu);

        // End coroutine
        yield break;
    }

    [VRCommand]
    private void MyItemCommandHandler()
    {
        print("My menu item has been clicked");
    }

    private void AddButton(VRMenu iVRMenu)
    {
        // Add a button at the start of the menu
        var button = new vrWidgetButton("VRMenu.MyCustomButton", iVRMenu.menu, "My Menu Item", MVRTools.GetCommand("MyItemCommandHandler"));
        iVRMenu.menu.SetChildIndex(button, 0);
        MVRTools.RegisterObject(this, button);

        // Add a separator below it
        var separator = new vrWidgetSeparator("VRMenu.MyCustomSeparator", iVRMenu.menu);
        iVRMenu.menu.SetChildIndex(separator, 1);
        MVRTools.RegisterObject(this, separator);
    }

    private void RemoveItem(VRMenu iVRMenu)
    {
        // Remove "Reset" submenu
        for (uint i = 0; i < iVRMenu.menu.GetChildrenNb(); ++i)
        {
            var widget = iVRMenu.menu.GetChild(i);
            if( widget.GetLabel().Contains("Reset"))
            {
                iVRMenu.menu.RemoveChild(widget);
                break;
            }
        }   
    }

    private void MoveItems(VRMenu iVRMmenu)
    {
        // Move every menu item under a sub menu
        var subMenu = new vrWidgetMenu("VRMenu.MyNewSubMenu", null, "MiddleVR Menu");

        while (iVRMmenu.menu.GetChildrenNb() > 0)
        {
            var widget = iVRMmenu.menu.GetChild(0);
            widget.SetParent(subMenu);
        }

        subMenu.SetParent(iVRMmenu.menu);
    }
}
