/* VRManagerScript
 * MiddleVR
 * (c) MiddleVR
 */

#if UNITY_5_6_OR_NEWER && !UNITY_EDITOR
using System; 
using System.Linq; 
#endif
using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
[HelpURL("http://www.middlevr.com/doc/current/#vrmanager_options")]
public class VRManagerScript : MonoBehaviour
{
    public enum ENavigation{
        None,
        Joystick,
        Elastic,
        GrabWorld
    }

    public enum EVirtualHandMapping{
        Direct,
        Gogo
    }

    public enum EManipulation{
        None,
        Ray,
        Homer
    }

    // Public readable parameters
    [HideInInspector]
    public float WandAxisHorizontal = 0.0f;

    [HideInInspector]
    public float WandAxisVertical = 0.0f;

    [HideInInspector]
    public bool WandButton0 = false;

    [HideInInspector]
    public bool WandButton1 = false;

    [HideInInspector]
    public bool WandButton2 = false;

    [HideInInspector]
    public bool WandButton3 = false;

    [HideInInspector]
    public bool WandButton4 = false;

    [HideInInspector]
    public bool WandButton5 = false;

    [HideInInspector]
    public double DeltaTime = 0.0f;

    private vrCommand m_startParticlesCommand = null;

    // Exposed parameters:
    public string ConfigFile = "c:\\config.vrx";
    public GameObject VRSystemCenterNode = null;
    public GameObject TemplateCamera     = null;

    public bool ShowWand = true;

    [SerializeField]
    private bool m_UseVRMenu = false;
    public bool UseVRMenu
    {
        get
        {
            return m_UseVRMenu;
        }
        set
        {
            _EnableVRMenu(value);
        }
    }

    public ENavigation         Navigation         = ENavigation.Joystick;
    public EManipulation       Manipulation       = EManipulation.Ray;
    public EVirtualHandMapping VirtualHandMapping = EVirtualHandMapping.Direct;

    [SerializeField]
    private bool m_ShowScreenProximityWarnings = false;
    public bool ShowScreenProximityWarnings
    {
        get
        {
            return m_ShowScreenProximityWarnings;
        }
        set
        {
            _EnableProximityWarning(value);
        }
    }

    [SerializeField]
    private bool m_Fly = false;
    public bool Fly
    {
        get
        {
            return m_Fly;
        }
        set
        {
            _EnableNavigationFly(value);
        }
    }

    [SerializeField]
    private bool m_NavigationCollisions = false;
    public bool NavigationCollisions
    {
        get
        {
            return m_NavigationCollisions;
        }
        set
        {
            _EnableNavigationCollision(value);
        }
    }

    [SerializeField]
    private bool m_ManipulationReturnObjects = false;
    public bool ManipulationReturnObjects
    {
        get
        {
            return m_ManipulationReturnObjects;
        }
        set
        {
            _EnableManipulationReturnObjects(value);
        }
    }

    [SerializeField]
    private bool m_ShowFPS = true;
    public bool ShowFPS
    {
        get
        {
            return m_ShowFPS;
        }
        set
        {
            _EnableFPSDisplay(value);
        }
    }

    public bool              DisableExistingCameras      = true;
    public bool              GrabExistingNodes           = false;
    public bool              DebugNodes                  = false;
    public bool              DebugScreens                = false;
    public bool              LogsToUnityConsole          = true;
    public bool              QuitOnEsc                   = true;
    public bool              DontChangeWindowGeometry    = false;
    public bool              SimpleCluster               = true;
    public bool              SimpleClusterParticles      = true;
    public bool              ForceQuality                = false;
    public int               ForceQualityIndex           = 3;
    public bool              CustomLicense               = false;
    public string            CustomLicenseName;
    public string            CustomLicenseCode;

    // Private members
    private vrKernel         m_Kernel     = null;
    private vrDeviceManager  m_DeviceMgr  = null;
    private vrDisplayManager m_DisplayMgr = null;
    private vrClusterManager m_ClusterMgr = null;

    private GameObject m_Wand   = null;
    private GameObject m_VRMenu = null;

    private bool m_isInit        = false;
    private bool m_isGeometrySet = false;
    private bool m_displayLog    = false;

    private int  m_AntiAliasingLevel = 0;

    private GUIText m_GUI = null;

    private bool[] mouseButtons = new bool[3];

    private uint m_FirstFrameAfterReset = 0;

    private bool m_InteractionsInitialized = false;

    // Public methods

    public void Log(string text)
    {
        MVRTools.Log(text);
    }

    public bool IsKeyPressed(uint iKey)
    {
        return m_DeviceMgr.IsKeyPressed(iKey);
    }

    public bool IsMouseButtonPressed(uint iButtonIndex)
    {
        return m_DeviceMgr.IsMouseButtonPressed(iButtonIndex);
    }

    public float GetMouseAxisValue(uint iAxisIndex)
    {
        return m_DeviceMgr.GetMouseAxisValue(iAxisIndex);
    }


    // Private methods

    private void InitializeVR()
    {
        mouseButtons[0] = mouseButtons[1] = mouseButtons[2] = false;

        if (m_displayLog)
        {
            var guiGO = new GameObject();
            m_GUI = guiGO.AddComponent<GUIText>();
            guiGO.transform.localPosition = new UnityEngine.Vector3(0.5f, 0.0f, 0.0f);
            m_GUI.pixelOffset = new UnityEngine.Vector2(15.0f, 0.0f);
            m_GUI.anchor = TextAnchor.LowerCenter;
        }

        if( MiddleVR.VRKernel != null )
        {
            MVRTools.Log(3, "[ ] VRKernel already alive, reset Unity Manager.");
            MVRTools.VRReset();
            m_isInit = true;
            // Not needed because this is the first execution of this script instance
            // m_isGeometrySet = false;
            m_FirstFrameAfterReset = MiddleVR.VRKernel.GetFrame();
        }
        else
        {
            if( CustomLicense )
            {
                MVRTools.CustomLicense = true;
                MVRTools.CustomLicenseName = CustomLicenseName;
                MVRTools.CustomLicenseCode = CustomLicenseCode;
            }

            m_isInit = MVRTools.VRInitialize(ConfigFile);
        }


        if (SimpleClusterParticles)
        {
            _SetParticlesSeeds();
        }

        // Get AA from vrx configuration file
        m_AntiAliasingLevel = (int)MiddleVR.VRDisplayMgr.GetAntiAliasing();

        DumpOptions();

        if (!m_isInit)
        {
            var guiGO = new GameObject();
            m_GUI = guiGO.AddComponent<GUIText>();
            guiGO.transform.localPosition = new UnityEngine.Vector3(0.2f, 0.0f, 0.0f);
            m_GUI.pixelOffset = new UnityEngine.Vector2(0.0f, 0.0f);
            m_GUI.anchor = TextAnchor.LowerLeft;

            string txt = m_Kernel.GetLogString(true);
            print(txt);
            m_GUI.text = txt;

            return;
        }

        m_Kernel = MiddleVR.VRKernel;
        m_DeviceMgr = MiddleVR.VRDeviceMgr;
        m_DisplayMgr = MiddleVR.VRDisplayMgr;
        m_ClusterMgr = MiddleVR.VRClusterMgr;

        if (SimpleCluster)
        {
            SetupSimpleCluster();
        }

        if (DisableExistingCameras)
        {
            Camera[] cameras = GameObject.FindObjectsOfType(typeof(Camera)) as Camera[];

            foreach (Camera cam in cameras)
            {
                if (cam.targetTexture == null)
                {
                    cam.enabled = false;
                }
            }
        }

        MVRNodesCreator.Instance.CreateNodes(
            VRSystemCenterNode,
            DebugNodes, DebugScreens,
            GrabExistingNodes, TemplateCamera);

        MVRTools.CreateViewportsAndCameras(DontChangeWindowGeometry, true);

        MVRTools.Log(4, "[<] End of VR initialization script");
    }

    protected void Awake()
    {
        // Attempt to collect objects from previous level
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        // Second call to work around a possible mono bug (see https://bugzilla.xamarin.com/show_bug.cgi?id=20503 )
        System.GC.WaitForPendingFinalizers();

        MVRNodesMapper.CreateInstance();
        MVRNodesCreator.CreateInstance();

        MVRTools.RedirectingLogsToUnityConsole = LogsToUnityConsole;

        InitializeVR();
    }

    protected void Start ()
    {
        MVRTools.Log(4, "[>] VR Manager Start.");

        m_Kernel.DeleteLateObjects();

        // Reset Manager's position so text display is correct.
        transform.position = new UnityEngine.Vector3(0, 0, 0);
        transform.rotation = new Quaternion();
        transform.localScale = new UnityEngine.Vector3(1, 1, 1);

        m_Wand = GameObject.Find("VRWand");

        m_VRMenu = GameObject.Find("VRMenu");

        ShowWandGeometry(ShowWand);

        _EnableProximityWarning(m_ShowScreenProximityWarnings);

        _EnableFPSDisplay(m_ShowFPS);

        _EnableNavigationFly(m_Fly);

        _EnableNavigationCollision(m_NavigationCollisions);

        _EnableManipulationReturnObjects(m_ManipulationReturnObjects);

        _EnableVRMenu(m_UseVRMenu);

        if (ForceQuality)
        {
            QualitySettings.SetQualityLevel(ForceQualityIndex);
        }

        // Manage VSync after the quality settings
        MVRTools.ManageVSync();

        // Set AA from vrx configuration file
        QualitySettings.antiAliasing = m_AntiAliasingLevel;

        MVRTools.RegisterCommands(this);

        MVRTools.Log(4, "[<] End of VR Manager Start.");
    }

    private vrValue StartParticlesCommandHandler(vrValue iValue)
    {
        // We get all the randomSeed / playOnAwake of the master's particle systems
        // to sync the slaves.
        ParticleSystem[] particles = GameObject.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[];
        for (uint i = 0, iEnd = iValue.GetListItemsNb(), particlesCnt = (uint)particles.GetLength(0); i < iEnd && i < particlesCnt; ++i)
        {
            particles[i].randomSeed = (uint)iValue.GetListItem(i).GetListItem(0).GetInt();
            if (iValue.GetListItem(i).GetListItem(1).GetBool())
            {
                particles[i].Play();
            }
        }
        return null;
    }

    private void _SetParticlesSeeds()
    {
        if (MiddleVR.VRClusterMgr.IsCluster())
        {
            // Creating the list of randomSeed / is playOnAwake to sync the seeds
            // of each particle systems to the master
            vrValue particlesDataList = vrValue.CreateList();

            foreach (ParticleSystem particle in GameObject.FindObjectsOfType(typeof(ParticleSystem)) as ParticleSystem[])
            {
                if (MiddleVR.VRClusterMgr.IsServer())
                {
                    vrValue particleData = vrValue.CreateList();

                    particleData.AddListItem((int)particle.randomSeed);
                    particleData.AddListItem(particle.playOnAwake);

                    particlesDataList.AddListItem(particleData);
                }
                // We reset the particle systems to sync them in every nodes of the cluster
                particle.Clear();
                particle.Stop();
                particle.time = .0f;
            }

            m_startParticlesCommand = new vrCommand("startParticleCommand", StartParticlesCommandHandler);

            if (MiddleVR.VRClusterMgr.IsServer())
            {
                m_startParticlesCommand.Do(particlesDataList);
            }
        }

    }

    private void _SetNavigation(ENavigation iNavigation)
    {
        Navigation = iNavigation;

        VRInteractionNavigationWandJoystick navigationWandJoystick = m_Wand.GetComponent<VRInteractionNavigationWandJoystick>();
        VRInteractionNavigationElastic      navigationElastic      = m_Wand.GetComponent<VRInteractionNavigationElastic>();
        VRInteractionNavigationGrabWorld    navigationGrabWorld    = m_Wand.GetComponent<VRInteractionNavigationGrabWorld>();
        if (navigationWandJoystick == null || navigationElastic == null || navigationGrabWorld == null)
        {
            MVRTools.Log(2, "[~] Some navigation scripts are missing on the Wand.");
            return;
        }

        if (navigationWandJoystick.GetInteraction() == null || navigationElastic.GetInteraction() == null || navigationGrabWorld.GetInteraction() == null)
        {
            MVRTools.Log(2, "[~] Some navigation interactions are not initialized.");
            return;
        }

        switch (Navigation)
        {
            case ENavigation.None:
                MiddleVR.VRInteractionMgr.Deactivate(navigationWandJoystick.GetInteraction());
                MiddleVR.VRInteractionMgr.Deactivate(navigationElastic.GetInteraction());
                MiddleVR.VRInteractionMgr.Deactivate(navigationGrabWorld.GetInteraction());
                break;

            case ENavigation.Joystick:
                MiddleVR.VRInteractionMgr.Activate(navigationWandJoystick.GetInteraction());
                break;

            case ENavigation.Elastic:
                MiddleVR.VRInteractionMgr.Activate(navigationElastic.GetInteraction());
                break;

            case ENavigation.GrabWorld:
                MiddleVR.VRInteractionMgr.Activate(navigationGrabWorld.GetInteraction());
                break;

            default:
                break;
        }
    }

    private void _SetManipulation(EManipulation iManpulation)
    {
        Manipulation = iManpulation;

        VRInteractionManipulationRay   manipulationRay   = m_Wand.GetComponent<VRInteractionManipulationRay>();
        VRInteractionManipulationHomer manipulationHomer = m_Wand.GetComponent<VRInteractionManipulationHomer>();
        if (manipulationRay == null || manipulationHomer == null)
        {
            MVRTools.Log(2, "[~] Some manipulation scripts are missing on the Wand.");
            return;
        }

        switch (Manipulation)
        {
            case EManipulation.None:
                MiddleVR.VRInteractionMgr.Deactivate(manipulationRay.GetInteraction());
                MiddleVR.VRInteractionMgr.Deactivate(manipulationHomer.GetInteraction());
                break;

            case EManipulation.Ray:
                MiddleVR.VRInteractionMgr.Activate(manipulationRay.GetInteraction());
                break;

            case EManipulation.Homer:
                MiddleVR.VRInteractionMgr.Activate(manipulationHomer.GetInteraction());
                break;

            default:
                break;
        }
    }

    private void _SetVirtualHandMapping(EVirtualHandMapping iVirtualHandMapping)
    {
        VirtualHandMapping = iVirtualHandMapping;

        VRInteractionVirtualHandGogo virtualHandGogo = m_Wand.GetComponent<VRInteractionVirtualHandGogo>();
        if (virtualHandGogo == null)
        {
            MVRTools.Log(2, "[~] The virtual hand  Gogo script is missing on the Wand.");
            return;
        }

        switch (VirtualHandMapping)
        {
            case EVirtualHandMapping.Direct:
                MiddleVR.VRInteractionMgr.Deactivate(virtualHandGogo.GetInteraction());
                break;

            case EVirtualHandMapping.Gogo:
                MiddleVR.VRInteractionMgr.Activate(virtualHandGogo.GetInteraction());
                break;

            default:
                break;
        }
    }

    private void _EnableProximityWarning(bool iShow)
    {
        m_ShowScreenProximityWarnings = iShow;

        VRInteractionScreenProximityWarning proximityWarning = m_Wand.GetComponent<VRInteractionScreenProximityWarning>();

        if (proximityWarning != null)
        {
            proximityWarning.enabled = m_ShowScreenProximityWarnings;
        }
    }

    private void _EnableFPSDisplay(bool iEnable)
    {
        m_ShowFPS = iEnable;

        this.GetComponent<GUIText>().enabled = m_ShowFPS;
    }

    private void _EnableVRMenu(bool iEnable)
    {
        m_UseVRMenu = iEnable;

        if (m_VRMenu != null)
        {
            m_VRMenu.GetComponent<VRMenuManager>().UseVRMenu(m_UseVRMenu);
        }
    }

    private void _EnableNavigationFly(bool iEnable)
    {
        m_Fly = iEnable;

        VRInteractionNavigationElastic navigationElastic = m_Wand.GetComponent<VRInteractionNavigationElastic>();
        if (navigationElastic != null)
        {
            navigationElastic.Fly = m_Fly;
        }

        VRInteractionNavigationWandJoystick navigationWandJoystick = m_Wand.GetComponent<VRInteractionNavigationWandJoystick>();
        if (navigationWandJoystick != null)
        {
            navigationWandJoystick.Fly = m_Fly;
        }

        vrInteractionManager interactionMgr = vrInteractionManager.GetInstance();

        for (uint i = 0, iEnd = interactionMgr.GetInteractionsNb(); i < iEnd; ++i)
        {
            vrProperty flyProp = interactionMgr.GetInteractionByIndex(i).GetProperty("Fly");
            if (flyProp != null)
            {
                flyProp.SetBool(m_Fly);
            }
        }
    }

    private void _EnableNavigationCollision(bool iEnable)
    {
        m_NavigationCollisions = iEnable;

        VRNavigationCollision navigationCollision = m_Wand.GetComponent<VRNavigationCollision>();

        if (navigationCollision != null)
        {
            navigationCollision.enabled = m_NavigationCollisions;
        }
    }

    private void _EnableManipulationReturnObjects(bool iEnable)
    {
        m_ManipulationReturnObjects = iEnable;

        VRInteractionManipulationReturnObjects returnObjects = m_Wand.GetComponent<VRInteractionManipulationReturnObjects>();

        if (returnObjects != null)
        {
            returnObjects.enabled = m_ManipulationReturnObjects;
        }
    }

    public void ShowWandGeometry(bool iShow)
    {
        if (m_Wand != null)
        {
            m_Wand.GetComponent<VRWand>().Show(iShow);
        }
    }

    private void UpdateInput()
    {
        vrButtons wandButtons = m_DeviceMgr.GetWandButtons();

        if (wandButtons != null)
        {
            uint buttonNb = wandButtons.GetButtonsNb();
            if (buttonNb > 0)
            {
                WandButton0 = wandButtons.IsPressed(m_DeviceMgr.GetWandButton0());
            }
            if (buttonNb > 1)
            {
                WandButton1 = wandButtons.IsPressed(m_DeviceMgr.GetWandButton1());
            }
            if (buttonNb > 2)
            {
                WandButton2 = wandButtons.IsPressed(m_DeviceMgr.GetWandButton2());
            }
            if (buttonNb > 3)
            {
                WandButton3 = wandButtons.IsPressed(m_DeviceMgr.GetWandButton3());
            }
            if (buttonNb > 4)
            {
                WandButton4 = wandButtons.IsPressed(m_DeviceMgr.GetWandButton4());
            }
            if (buttonNb > 5)
            {
                WandButton5 = wandButtons.IsPressed(m_DeviceMgr.GetWandButton5());
            }
        }

        WandAxisHorizontal = m_DeviceMgr.GetWandHorizontalAxisValue();
        WandAxisVertical = m_DeviceMgr.GetWandVerticalAxisValue();
    }

    // Update is called once per frame
    protected void Update()
    {
        // Initialize interactions
        if( !m_InteractionsInitialized )
        {
            _SetNavigation(Navigation);
            _SetManipulation(Manipulation);
            _SetVirtualHandMapping(VirtualHandMapping);

            m_InteractionsInitialized = true;
        }

        MVRNodesMapper nodesMapper = MVRNodesMapper.Instance;

        nodesMapper.UpdateNodesUnityToMiddleVR();

        if (m_isInit)
        {
            MVRTools.Log(4, "[>] Unity Update - Start");

            if (m_Kernel.GetFrame() >= m_FirstFrameAfterReset+1 && !m_isGeometrySet && !Application.isEditor)
            {
                if (!DontChangeWindowGeometry)
                {
                    m_DisplayMgr.SetUnityWindowGeometry();
                }
                m_isGeometrySet = true;
            }

            if (m_Kernel.GetFrame() == 0)
            {
                // Set the random seed in kernel for dispatching only during start-up.
                // With clustering, a client will be set by a call to kernel.Update().
                if (!m_ClusterMgr.IsCluster() ||
                    (m_ClusterMgr.IsCluster() && m_ClusterMgr.IsServer()))
                {
                    // The cast is safe because the seed is always positive.
                    uint seed = (uint) UnityEngine.Random.seed;
                    m_Kernel._SetRandomSeed(seed);
                }
            }

            var onPreUpdateNetworkHook = typeof(UnityEngine.Networking.NetworkIdentity).GetMethod("OnPreMiddleVRUpdate");
            if (onPreUpdateNetworkHook != null)
            {
                onPreUpdateNetworkHook.Invoke(null, null);
            }

            m_Kernel.Update();

            var onPostUpdateNetworkHook = typeof(UnityEngine.Networking.NetworkIdentity).GetMethod("OnPostMiddleVRUpdate");
            if (onPostUpdateNetworkHook != null)
            {
                onPostUpdateNetworkHook.Invoke(null, null);
            }

            if (m_Kernel.GetFrame() == 0)
            {
                // Set the random seed in a client only during start-up.
                if (m_ClusterMgr.IsCluster() && m_ClusterMgr.IsClient())
                {
                    // The cast is safe because the seed comes from
                    // a previous value of Unity.
                    int seed = (int) m_Kernel.GetRandomSeed();
                    UnityEngine.Random.seed = seed;
                }
            }

            UpdateInput();

            if (ShowFPS)
            {
                this.GetComponent<GUIText>().text = m_Kernel.GetFPS().ToString("f2");
            }

            nodesMapper.UpdateNodesMiddleVRToUnity(false);

            MVRTools.UpdateCameraProperties(m_Kernel, m_DisplayMgr, nodesMapper);

            if (m_displayLog)
            {
                string txt = m_Kernel.GetLogString(true);
                print(txt);
                m_GUI.text = txt;
            }

            vrKeyboard keyb = m_DeviceMgr.GetKeyboard();

            if (keyb != null)
            {
                if (keyb.IsKeyPressed(MiddleVR.VRK_LSHIFT) || keyb.IsKeyPressed(MiddleVR.VRK_RSHIFT))
                {

                    if (keyb.IsKeyToggled(MiddleVR.VRK_D))
                    {
                        ShowFPS = !ShowFPS;
                    }

                    if (keyb.IsKeyToggled(MiddleVR.VRK_W) || keyb.IsKeyToggled(MiddleVR.VRK_Z))
                    {
                        ShowWand = !ShowWand;
                        ShowWandGeometry(ShowWand);
                    }

                    // Toggle Fly mode on interactions
                    if (keyb.IsKeyToggled(MiddleVR.VRK_F))
                    {
                        Fly = !Fly;
                    }

                    // Navigation mode switch
                    if (keyb.IsKeyToggled(MiddleVR.VRK_N))
                    {
                        vrInteraction navigation = _GetNextInteraction("ContinuousNavigation");
                        if (navigation != null)
                        {
                            MiddleVR.VRInteractionMgr.Activate(navigation);
                        }
                    }
                }
            }

            DeltaTime = m_Kernel.GetDeltaTime();

            MVRTools.Log(4, "[<] Unity Update - End");
        }
        else
        {
            //Debug.LogWarning("[ ] If you have an error mentioning 'DLLNotFoundException: MiddleVR_CSharp', please restart Unity. If this does not fix the problem, please make sure MiddleVR is in the PATH environment variable.");
        }
    }

    private void AddClusterScripts(GameObject iObject)
    {
        MVRTools.Log(2, "[ ] Adding cluster sharing scripts to " + iObject.name);
        if (iObject.GetComponent<VRShareTransform>() == null)
        {
            iObject.AddComponent<VRShareTransform>();
        }
    }

    private void SetupSimpleCluster()
    {
        if (m_ClusterMgr.IsCluster())
        {
            // Rigid bodies
            Rigidbody[] bodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
            foreach (Rigidbody body in bodies)
            {
                if (!body.isKinematic)
                {
                    GameObject iObject = body.gameObject;
                    AddClusterScripts(iObject);
                }
            }

            // Character controller
            CharacterController[] ctrls = FindObjectsOfType(typeof(CharacterController)) as CharacterController[];
            foreach (CharacterController ctrl in ctrls)
            {
                GameObject iObject = ctrl.gameObject;
                AddClusterScripts(iObject);
            }
        }
    }

    private void DumpOptions()
    {
        MVRTools.Log(3, "[ ] Dumping VRManager's options:");
        MVRTools.Log(3, "[ ] - Config File : " + ConfigFile);
        MVRTools.Log(3, "[ ] - System Center Node : " + VRSystemCenterNode);
        MVRTools.Log(3, "[ ] - Template Camera : " + TemplateCamera);
        MVRTools.Log(3, "[ ] - Show Wand : " + ShowWand);
        MVRTools.Log(3, "[ ] - Show FPS  : " + ShowFPS);
        MVRTools.Log(3, "[ ] - Disable Existing Cameras : " + DisableExistingCameras);
        MVRTools.Log(3, "[ ] - Grab Existing Nodes : " + GrabExistingNodes);
        MVRTools.Log(3, "[ ] - Debug Nodes : " + DebugNodes);
        MVRTools.Log(3, "[ ] - Debug Screens : " + DebugScreens);
        MVRTools.Log(3, "[ ] - Quit On Esc : " + QuitOnEsc);
        MVRTools.Log(3, "[ ] - Don't Change Window Geometry : " + DontChangeWindowGeometry);
        MVRTools.Log(3, "[ ] - Simple Cluster : " + SimpleCluster);
        MVRTools.Log(3, "[ ] - Simple Cluster Particles : " + SimpleClusterParticles );
        MVRTools.Log(3, "[ ] - Force Quality : " + ForceQuality );
        MVRTools.Log(3, "[ ] - Force QualityIndex : " + ForceQualityIndex );
        MVRTools.Log(3, "[ ] - Anti-Aliasing Level : " + m_AntiAliasingLevel );
        MVRTools.Log(3, "[ ] - Custom License : " + CustomLicense );
        MVRTools.Log(3, "[ ] - Custom License Name : " + CustomLicenseName );
    }

    private vrInteraction _GetNextInteraction(string iTag)
    {
        vrInteraction nextInteraction = null;

        vrInteraction activeInteraction = MiddleVR.VRInteractionMgr.GetActiveInteractionByTag("ContinuousNavigation");
        if (activeInteraction != null)
        {
            // Found active interaction, search for the next one
            uint interactionsNb = MiddleVR.VRInteractionMgr.GetInteractionsNb();
            uint index = MiddleVR.VRInteractionMgr.GetInteractionIndex(activeInteraction);

            for (uint i = 0; i < interactionsNb - 1; ++i)
            {
                // We loop in the interactions list to find the next interaction with the right tag
                uint nextIndex = (index + 1 + i) % interactionsNb;

                vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteractionByIndex(nextIndex);

                if (interaction != null && interaction.TagsContain("ContinuousNavigation"))
                {
                    nextInteraction = interaction;
                    break;
                }
            }
        }
        else
        {
            // No active interaction, try to activate the first if existing
            nextInteraction = MiddleVR.VRInteractionMgr.GetInteractionByTag("ContinuousNavigation", 0);
        }

        return nextInteraction;
    }

    public void QuitApplication()
    {
        MVRTools.Log(3,"[ ] Execute QuitCommand.");

        // Call cluster command so that all cluster nodes quit
        MiddleVR.VRKernel.ExecuteCommand("VRManager.QuitApplicationCommand");
    }

    [VRCommand("VRManager.QuitApplicationCommand")]
    private void _QuitApplicationCommandHandler()
    {
        MVRTools.Log(3, "[ ] Received QuitApplicationCommand");

        MVRTools.Log("[ ] Unity says we're quitting.");
        MiddleVR.VRKernel.SetQuitting();
        Application.Quit();
    }

    protected void OnApplicationQuit()
    {
        MVRNodesCreator.DestroyInstance();
        MVRNodesMapper.DestroyInstance();

        MiddleVR.DisposeObject(ref m_startParticlesCommand);

        MVRTools.VRDestroy(Application.isEditor);

        // Unity 5.6 players crash on exit when any "-force-*" parameter is on the command line.
        // When launching from middlevr configuration, one of these parameters is always set,
        // so we kill the process as a workaround.
        // The environment variable MIDDLEVR_FORCE_KILL_PLAYER can be set to "false" (case insensitive)
        // to disable this behaviour.
#if UNITY_5_6_OR_NEWER && !UNITY_EDITOR
        string[] crashingArguments = new string[] {"-force-d3d11", "-force-d3d9", "-force-opengl"};

        bool commandLineHasCrashingArgument = Environment.GetCommandLineArgs().Any(
            commandLineArg => crashingArguments.Any(
                crashingArg => String.Equals(commandLineArg, crashingArg, StringComparison.OrdinalIgnoreCase)));

        if (commandLineHasCrashingArgument)
        {
            bool forceKill = true;

            string forceKillEnvVar = Environment.GetEnvironmentVariable("MIDDLEVR_FORCE_KILL_PLAYER");
            if (forceKillEnvVar != null)
            {
                Boolean.TryParse(forceKillEnvVar, out forceKill);
            }

            if (forceKill)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
#endif
    }

    protected void OnDestroy()
    {
        MiddleVR.DisposeObject(ref m_startParticlesCommand);
    }
}
