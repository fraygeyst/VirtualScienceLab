/* VRInteractionNavigationGrabWorld
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRInteractionNavigationGrabWorld : VRInteraction {

    public string Name            = "InteractionNavigationGrabWorld";
    public string ReferenceNode   = "HandNode";
    public uint  WandActionButton = 1;

    private vrNode3D m_ReferenceNode              = null;
    private vrInteractionNavigationGrabWorld m_it = null;

    private GameObject m_NavRefNode   = null;
    private vrNode3D   m_NavRefVrNode = null;

    private bool m_Initialized = false;


    protected void Start()
    {
        // Make sure the base interaction is started
        InitializeBaseInteraction();

        m_it = new vrInteractionNavigationGrabWorld(Name);
        // Must tell base class about our interaction
        SetInteraction(m_it);

        MiddleVR.VRInteractionMgr.AddInteraction(m_it);
        MiddleVR.VRInteractionMgr.Activate(m_it);

        m_ReferenceNode = MiddleVR.VRDisplayMgr.GetNode(ReferenceNode);

        if ( m_ReferenceNode!= null )
        {
            m_it.SetActionButton(WandActionButton);
            m_it.SetReferenceNode(m_ReferenceNode);
        }
        else
        {
            MiddleVR.VRLog( 2, "[X] VRInteractionNavigationGrabWorld: One or several nodes are missing." );
        }
    }

    protected void Update()
    {
        if (IsActive())
        {
            if (!m_Initialized)
            {
                Initialize();
            }
        }
    }

    protected void Initialize()
    {
        GameObject vrSystemCenterNode = null;
        if (GameObject.Find("VRManager").GetComponent<VRManagerScript>().VRSystemCenterNode != null)
        {
            vrSystemCenterNode = GameObject.Find("VRManager").GetComponent<VRManagerScript>().VRSystemCenterNode;
        }
        else
        {
            vrNode3D vrSystemMVRNode = MiddleVR.VRDisplayMgr.GetNodeByTag(MiddleVR.VR_SYSTEM_CENTER_NODE_TAG);
            if (vrSystemMVRNode != null)
            {
                vrSystemCenterNode = GameObject.Find(vrSystemMVRNode.GetName());
            }
        }

        if (vrSystemCenterNode == null)
        {
            return;
        }

        // If our navigation node has a parent, we want to move relatively to this vehicule
        if (vrSystemCenterNode.transform.parent != null)
        {
            m_NavRefNode = vrSystemCenterNode.transform.parent.gameObject;
            if (m_NavRefNode != null)
            {
                VRNode3DSynchronization nodeSyncComponent = m_NavRefNode.AddComponent<VRNode3DSynchronization>();
                m_it.SetNavigationReferentialNode(nodeSyncComponent.Node);
            }
        }

        m_Initialized = true;
    }

    protected void OnEnable()
    {
        MiddleVR.VRLog( 3, "[ ] VRInteractionNavigationGrabWorld: enabled" );
        if( m_it != null )
        {
            MiddleVR.VRInteractionMgr.Activate( m_it );
        }
    }

    protected void OnDisable()
    {
        MiddleVR.VRLog( 3, "[ ] VRInteractionNavigationGrabWorld: disabled" );

        if( m_it != null && MiddleVR.VRInteractionMgr != null )
        {
            MiddleVR.VRInteractionMgr.Deactivate( m_it );
        }
    }
}
