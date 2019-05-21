/* VRMenu
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRMenu : MonoBehaviour
{
    private VRManagerScript m_VRManager;

    private vrGUIRendererWeb m_GUIRendererWeb = null;
    private vrWidgetMenu     m_Menu = null;

    public vrGUIRendererWeb guiRendererWeb
    {
        get
        {
            return m_GUIRendererWeb;
        }
    }

    public vrWidgetMenu menu
    {
        get
        {
            return m_Menu;
        }
    }

    // Navigation
    private vrWidgetMenu         m_NavigationOptions;

    // Manipulation
    private vrWidgetMenu         m_ManipulationOptions;

    // Virtual Hand
    private vrWidgetMenu        m_VirtualHandOptions;

    // Bind with Interaction events
    private Dictionary<string, vrWidgetToggleButton> m_Buttons = new Dictionary<string, vrWidgetToggleButton>();

    private bool EventListener(vrEvent iEvent)
    {
        // Catch interaction events
        vrInteractionEvent interactionEvt = vrInteractionEvent.Cast(iEvent);
        if (interactionEvt != null)
        {
            vrInteraction interaction = interactionEvt.GetInteraction();

            bool needLabelRefresh = false;

            // Identify interaction
            // If existing in the Menu, update the menu
            if (interactionEvt.GetEventType() == (int)VRInteractionEventEnum.VRInteractionEvent_Activated)
            {
                vrWidgetToggleButton interactionButton;

                if (m_Buttons.TryGetValue(interaction.GetName(), out interactionButton))
                {
                    interactionButton.SetChecked(true);
                }

                needLabelRefresh = true;
            }
            else if (interactionEvt.GetEventType() == (int)VRInteractionEventEnum.VRInteractionEvent_Deactivated)
            {
                vrWidgetToggleButton interactionButton;

                if (m_Buttons.TryGetValue(interaction.GetName(), out interactionButton))
                {
                    interactionButton.SetChecked(false);
                }

                needLabelRefresh = true;
            }

            // Refresh interaction menu label if activated or deactivated
            if (needLabelRefresh)
            {
                if (interaction.TagsContain("ContinuousNavigation"))
                {
                    _RefreshNavigationMenuName();
                }
                else if (interaction.TagsContain("ContinuousManipulation"))
                {
                    _RefreshManipulationMenuName();
                }
                else if (interaction.TagsContain("VirtualHand"))
                {
                    _RefreshVirtualHandMenuName();
                }
            }
        }

        return true;
    }

    // General
    [VRCommand]
    private void ResetCurrentButtonHandler()
    {
        MVRTools.Log("[ ] Reload current level.");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    [VRCommand]
    private void ResetZeroButtonHandler()
    {
        MVRTools.Log("[ ] Reload level zero.");
        SceneManager.LoadScene(0);
    }

    [VRCommand]
    private void ExitButtonHandler()
    {
        MVRTools.Log("[ ] Exit simulation.");
        m_VRManager.QuitApplication();
    }

    [VRCommand]
    private void FramerateCheckboxHandler(bool iValue)
    {
        m_VRManager.ShowFPS = iValue;
        MVRTools.Log("[ ] Show Frame Rate: " + iValue);
    }

    [VRCommand]
    private void ProxiWarningCheckboxHandler(bool iValue)
    {
        m_VRManager.ShowScreenProximityWarnings = iValue;
        MVRTools.Log("[ ] Show proximity warnings: " + iValue);
    }

    [VRCommand]
    private void NavigationJoystickHandler(bool iValue)
    {
        // Activate Joystick Navigation
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteraction("InteractionNavigationWandJoystick");

        bool activate = iValue;
        if (activate)
        {
            MiddleVR.VRInteractionMgr.Activate(interaction);
            MVRTools.Log("[ ] Navigation Joystick activated.");
        }
        else
        {
            MiddleVR.VRInteractionMgr.Deactivate(interaction);
            MVRTools.Log("[ ] Navigation Joystick deactivated.");
        }
    }

    [VRCommand]
    private void NavigationElasticHandler(bool iValue)
    {
        // Activate Elastic Navigation
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteraction("InteractionNavigationElastic");

        bool activate = iValue;
        if (activate)
        {
            MiddleVR.VRInteractionMgr.Activate(interaction);
            MVRTools.Log("[ ] Navigation Elastic activated.");
        }
        else
        {
            MiddleVR.VRInteractionMgr.Deactivate(interaction);
            MVRTools.Log("[ ] Navigation Elastic deactivated.");
        }
    }

    [VRCommand]
    private void NavigationGrabWorldHandler(bool iValue)
    {
        // Activate Grab World Navigation
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteraction("InteractionNavigationGrabWorld");

        bool activate = iValue;
        if (activate)
        {
            MiddleVR.VRInteractionMgr.Activate(interaction);
            MVRTools.Log("[ ] Navigation Grab World activated.");
        }
        else
        {
            MiddleVR.VRInteractionMgr.Deactivate(interaction);
            MVRTools.Log("[ ] Navigation Grab World deactivated.");
        }
    }

    [VRCommand]
    private void FlyCheckboxHandler(bool iValue)
    {
        m_VRManager.Fly = iValue;
        MVRTools.Log("[ ] Fly mode: " + iValue);
    }

    [VRCommand]
    private void CollisionsCheckboxHandler(bool iValue)
    {
        m_VRManager.NavigationCollisions = iValue;
        MVRTools.Log("[ ] Navigation Collisions: " + iValue);
    }

    [VRCommand]
    private void ManipulationRayHandler(bool iValue)
    {
        // Activate Ray Manipulation
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteraction("InteractionManipulationRay");

        bool activate = iValue;
        if (activate)
        {
            MiddleVR.VRInteractionMgr.Activate(interaction);
            MVRTools.Log("[ ] Manipulation Ray activated.");
        }
        else
        {
            MiddleVR.VRInteractionMgr.Deactivate(interaction);
            MVRTools.Log("[ ] Manipulation Ray deactivated.");
        }
    }

    [VRCommand]
    private void ManipulationHomerHandler(bool iValue)
    {
        // Activate Homer Manipulation
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteraction("InteractionManipulationHomer");

        bool activate = iValue;
        if (activate)
        {
            MiddleVR.VRInteractionMgr.Activate(interaction);
            MVRTools.Log("[ ] Manipulation Homer activated.");
        }
        else
        {
            MiddleVR.VRInteractionMgr.Deactivate(interaction);
            MVRTools.Log("[ ] Manipulation Homer deactivated.");
        }
    }

    [VRCommand]
    private void ReturnObjectsCheckboxHandler(bool iValue)
    {
        m_VRManager.ManipulationReturnObjects = iValue;
        MVRTools.Log("[ ] Manipulation return objects: " + iValue);
    }

    [VRCommand]
    private void VirtualHandGogoButtonHandler(bool iValue)
    {
        // Activate Gogo Virtual Hand
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteraction("InteractionVirtualHandGogo");

        bool activate = iValue;
        if (activate)
        {
            MiddleVR.VRInteractionMgr.Activate(interaction);
            MVRTools.Log("[ ] Virtual Hand Gogo activated.");
        }
        else
        {
            MiddleVR.VRInteractionMgr.Deactivate(interaction);
            MVRTools.Log("[ ] Virtual Hand Gogo deactivated.");
        }
    }

    protected void Start ()
    {
        // Retrieve the VRManager
        VRManagerScript[] foundVRManager = FindObjectsOfType(typeof(VRManagerScript)) as VRManagerScript[];
        if (foundVRManager.Length != 0)
        {
            m_VRManager = foundVRManager[0];
        }
        else
        {
            MVRTools.Log("[X] VRMenu: impossible to retrieve the VRManager.");
            return;
        }

        // Start listening to MiddleVR events
        var listener = new vrEventListener(EventListener);
        MiddleVR.VRInteractionMgr.AddEventListener(listener);
        MVRTools.RegisterObject(this, listener);

        // Register commands
        MVRTools.RegisterCommands(this);

        VRWebView webViewScript = GetComponent<VRWebView>();

        if (webViewScript == null)
        {
            MVRTools.Log(1, "[X] VRMenu does not have a WebView.");
            return;
        }

        m_GUIRendererWeb = new vrGUIRendererWeb("", webViewScript.webView);
        MVRTools.RegisterObject(this, m_GUIRendererWeb);

        m_Menu = new vrWidgetMenu("VRMenu.VRManagerMenu", m_GUIRendererWeb);
        MVRTools.RegisterObject(this, m_Menu);

        // Navigation
        m_NavigationOptions = new vrWidgetMenu("VRMenu.NavigationOptions", m_Menu, "Navigation");
        MVRTools.RegisterObject(this, m_NavigationOptions);

        CreateInteractionToggleButton(MiddleVR.VRInteractionMgr.GetInteraction("InteractionNavigationWandJoystick"), "Joystick", m_NavigationOptions, "NavigationJoystickHandler");
        CreateInteractionToggleButton(MiddleVR.VRInteractionMgr.GetInteraction("InteractionNavigationElastic"), "Elastic", m_NavigationOptions, "NavigationElasticHandler");
        CreateInteractionToggleButton(MiddleVR.VRInteractionMgr.GetInteraction("InteractionNavigationGrabWorld"), "Grab The World", m_NavigationOptions, "NavigationGrabWorldHandler");

        var navigationSeparator = new vrWidgetSeparator("VRMenu.NavigationSeparator", m_NavigationOptions);
        MVRTools.RegisterObject(this, navigationSeparator);
        var flyCheckbox         = new vrWidgetToggleButton("VRMenu.FlyCheckbox", m_NavigationOptions, "Fly", MVRTools.GetCommand("FlyCheckboxHandler"), m_VRManager.Fly);
        MVRTools.RegisterObject(this, flyCheckbox);
        var collisionsCheckbox  = new vrWidgetToggleButton("VRMenu.CollisionsCheckbox", m_NavigationOptions, "Navigation Collisions", MVRTools.GetCommand("CollisionsCheckboxHandler"), m_VRManager.NavigationCollisions);
        MVRTools.RegisterObject(this, collisionsCheckbox);

        // Manipulation
        m_ManipulationOptions = new vrWidgetMenu("VRMenu.ManipulationOptions", m_Menu, "Manipulation");
        MVRTools.RegisterObject(this, m_ManipulationOptions);

        CreateInteractionToggleButton(MiddleVR.VRInteractionMgr.GetInteraction("InteractionManipulationRay"), "Ray", m_ManipulationOptions, "ManipulationRayHandler");
        CreateInteractionToggleButton(MiddleVR.VRInteractionMgr.GetInteraction("InteractionManipulationHomer"), "Homer", m_ManipulationOptions, "ManipulationHomerHandler");

        var manipulationSeparator = new vrWidgetSeparator("VRMenu.ManipulationSeparator", m_ManipulationOptions);
        MVRTools.RegisterObject(this, manipulationSeparator);
        var returnObjectsCheckbox = new vrWidgetToggleButton("VRMenu.ReturnObjectsCheckbox", m_ManipulationOptions, "Return Objects", MVRTools.GetCommand("ReturnObjectsCheckboxHandler"), m_VRManager.ManipulationReturnObjects);
        MVRTools.RegisterObject(this, returnObjectsCheckbox);

        // Virtual Hand
        m_VirtualHandOptions = new vrWidgetMenu("VRMenu.VirtualHandOptions", m_Menu, "Virtual Hand");
        MVRTools.RegisterObject(this, m_VirtualHandOptions);

        CreateInteractionToggleButton(MiddleVR.VRInteractionMgr.GetInteraction("InteractionVirtualHandGogo"), "Gogo", m_VirtualHandOptions, "VirtualHandGogoButtonHandler");

        // General
        var generalSeparator = new vrWidgetSeparator("VRMenu.GeneralSeparator", m_Menu);
        MVRTools.RegisterObject(this, generalSeparator);
        var generalOptions = new vrWidgetMenu("VRMenu.GeneralOptions", m_Menu, "General Options");
        MVRTools.RegisterObject(this, generalOptions);

        var framerateCheckbox = new vrWidgetToggleButton("VRMenu.FramerateCheckbox", generalOptions, "Show Frame Rate", MVRTools.GetCommand("FramerateCheckboxHandler"), m_VRManager.ShowFPS);
        MVRTools.RegisterObject(this, framerateCheckbox);
        var proxiWarningCheckbox = new vrWidgetToggleButton("VRMenu.ProxiWarningCheckbox", generalOptions, "Show Proximity Warning", MVRTools.GetCommand("ProxiWarningCheckboxHandler"), m_VRManager.ShowScreenProximityWarnings);
        MVRTools.RegisterObject(this, proxiWarningCheckbox);

        // Reset and Exit
        var resetButtonMenu = new vrWidgetMenu("VRMenu.ResetButtonMenu", m_Menu, "Reset Simulation");
        MVRTools.RegisterObject(this, resetButtonMenu);
        var resetCurrentButton = new vrWidgetButton("VRMenu.ResetCurrentButton", resetButtonMenu, "Reload current level", MVRTools.GetCommand("ResetCurrentButtonHandler"));
        MVRTools.RegisterObject(this, resetCurrentButton);
        var resetZeroButton = new vrWidgetButton("VRMenu.ResetZeroButton", resetButtonMenu, "Reload level zero", MVRTools.GetCommand("ResetZeroButtonHandler"));
        MVRTools.RegisterObject(this, resetZeroButton);

        var exitButtonMenu = new vrWidgetMenu("VRMenu.ExitButtonMenu", m_Menu, "Exit Simulation");
        MVRTools.RegisterObject(this, exitButtonMenu);
        var exitButton = new vrWidgetButton("VRMenu.ExitButton", exitButtonMenu, "Yes, Exit Simulation", MVRTools.GetCommand("ExitButtonHandler"));
        MVRTools.RegisterObject(this, exitButton);
    }

    public void CreateInteractionToggleButton(vrInteraction iInteraction, string iButtonName, vrWidgetMenu iParentMenu, string iButtonHandlerName)
    {
        string itName = iInteraction.GetName();

        vrWidgetToggleButton button = new vrWidgetToggleButton("VRMenu." + itName + "ToggleButton", iParentMenu, iButtonName, MVRTools.GetCommand(iButtonHandlerName), iInteraction.IsActive());
        m_Buttons.Add(itName, button);
        MVRTools.RegisterObject(this, button);
    }

    private void _RefreshNavigationMenuName()
    {
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetActiveInteractionByTag("ContinuousNavigation");
        if (interaction != null)
        {
            switch (interaction.GetName())
            {
                case "InteractionNavigationWandJoystick":
                    {
                        m_NavigationOptions.SetLabel("Navigation (Joystick)");
                        break;
                    }
                case "InteractionNavigationElastic":
                    {
                        m_NavigationOptions.SetLabel("Navigation (Elastic)");
                        break;
                    }
                case "InteractionNavigationGrabWorld":
                    {
                        m_NavigationOptions.SetLabel("Navigation (Grab The World)");
                        break;
                    }
                default:
                    {
                        m_NavigationOptions.SetLabel("Navigation (" + interaction.GetName() + ")");
                        break;
                    }
            }
        }
        else
        {
            m_NavigationOptions.SetLabel("Navigation");
        }
    }

    private void _RefreshManipulationMenuName()
    {
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetActiveInteractionByTag("ContinuousManipulation");
        if (interaction != null)
        {
            switch (interaction.GetName())
            {
                case "InteractionManipulationRay":
                    {
                        m_ManipulationOptions.SetLabel("Manipulation (Ray)");
                        break;
                    }
                case "InteractionManipulationHomer":
                    {
                        m_ManipulationOptions.SetLabel("Manipulation (Homer)");
                        break;
                    }
                default:
                    {
                        m_ManipulationOptions.SetLabel("Manipulation (" + interaction.GetName() + ")");
                        break;
                    }
            }
        }
        else
        {
            m_ManipulationOptions.SetLabel("Manipulation");
        }
    }

    private void _RefreshVirtualHandMenuName()
    {
        vrInteraction interaction = MiddleVR.VRInteractionMgr.GetActiveInteractionByTag("VirtualHand");
        if (interaction != null)
        {
            switch (interaction.GetName())
            {
                case "InteractionVirtualHandGogo":
                    {
                        m_VirtualHandOptions.SetLabel("Virtual Hand (Gogo)");
                        break;
                    }
                default:
                    {
                        m_VirtualHandOptions.SetLabel("Virtual Hand (" + interaction.GetName() + ")");
                        break;
                    }
            }
        }
        else
        {
            m_VirtualHandOptions.SetLabel("Virtual Hand");
        }
    }

    private void OnDestroy()
    {
        // Widgets
        m_Buttons.Clear();
    }
}
