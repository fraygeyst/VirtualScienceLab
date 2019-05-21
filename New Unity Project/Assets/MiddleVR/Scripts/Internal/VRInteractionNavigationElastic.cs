/* VRInteractionNavigationElastic
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRInteractionNavigationElastic : VRInteraction {
    public string Name = "InteractionNavigationElastic";

    public string ReferenceNode  = "HandNode";
    public string TurnAroundNode = "HeadNode";

    vrNode3D m_ReferenceNode  = null;
    vrNode3D m_TurnAroundNode = null;

    public uint  WandActionButton = 1;

    public float TranslationSpeed = 1.0f;
    public float RotationSpeed    = 45.0f;

    public float DistanceThreshold = 0.025f;
    public float AngleThreshold    = 5.0f;

    public bool UseRotationYaw = true;

    public bool Fly = false;

    public GameObject       ElasticRepresentationPrefab;
    GameObject              m_ElasticRepresentationObject;
    VRElasticRepresentation m_ElasticRepresentation;

    vrInteractionNavigationElastic m_it = null;

    bool m_Initialized = false;
    Transform m_VRSystemCenterNode = null;

    protected void Start()
    {
        // Make sure the base interaction is started
        InitializeBaseInteraction();

        m_it = new vrInteractionNavigationElastic(Name);
        // Must tell base class about our interaction
        SetInteraction(m_it);

        MiddleVR.VRInteractionMgr.AddInteraction(m_it);
        MiddleVR.VRInteractionMgr.Activate(m_it);

        m_ReferenceNode  = MiddleVR.VRDisplayMgr.GetNode(ReferenceNode);
        m_TurnAroundNode = MiddleVR.VRDisplayMgr.GetNode(TurnAroundNode);

        if ( m_ReferenceNode!= null && m_TurnAroundNode != null )
        {
            m_it.SetActionButton( WandActionButton );

            m_it.SetReferenceNode(m_ReferenceNode);
            m_it.SetTurnAroundNode(m_TurnAroundNode);

            m_it.SetTranslationSpeed(TranslationSpeed);
            m_it.SetRotationSpeed(RotationSpeed);

            m_it.SetDistanceThreshold( DistanceThreshold );
            m_it.SetAngleThreshold(AngleThreshold);

            m_it.SetUseRotationYaw(UseRotationYaw);

            m_it.SetFly(Fly);
        }
        else
        {
            MiddleVR.VRLog( 2, "[X] VRInteractionNavigationElastic: One or several nodes are missing." );
        }
    }

    protected void Update()
    {
        if (IsActive())
        {
            if (!m_Initialized)
            {
                if (GameObject.Find("VRManager").GetComponent<VRManagerScript>().VRSystemCenterNode != null)
                {
                    m_VRSystemCenterNode = GameObject.Find("VRManager").GetComponent<VRManagerScript>().VRSystemCenterNode.transform;
                }
                else
                {
                    vrNode3D vrSystemMVRNode = MiddleVR.VRDisplayMgr.GetNodeByTag(MiddleVR.VR_SYSTEM_CENTER_NODE_TAG);
                    if (vrSystemMVRNode != null)
                    {
                        m_VRSystemCenterNode = GameObject.Find(vrSystemMVRNode.GetName()).transform;
                    }
                }

                m_Initialized = true;
            }

            if (ElasticRepresentationPrefab == null)
            {
                MVRTools.Log("[X] VRInteractionNavigationElastic error: bad elastic prefab reference");
                return;
            }

            if (m_it.IsNavigationStarted())
            {
                m_ElasticRepresentationObject = (GameObject)GameObject.Instantiate(ElasticRepresentationPrefab);
                m_ElasticRepresentationObject.transform.parent = m_VRSystemCenterNode;
                m_ElasticRepresentation = m_ElasticRepresentationObject.GetComponent<VRElasticRepresentation>();
                UpdateElasticRepresentation();
            }
            else if (m_it.IsNavigationRunning())
            {
                UpdateElasticRepresentation();
            }
            else if (m_it.IsNavigationStopped() && m_ElasticRepresentation != null)
            {
                GameObject.Destroy(m_ElasticRepresentationObject);
            }
        }
    }

    protected void UpdateElasticRepresentation()
    {
        if( m_ElasticRepresentation == null )
        {
            MiddleVR.VRLog( 2, "[X] VRInteractionNavigationElastic error: bad elastic representation reference" );
            return;
        }

        Vector3 startPosition = MVRTools.ToUnity( m_it.GetInteractionStartWorldMatrix().GetTranslation() );
        Vector3 endPosition   = MVRTools.ToUnity( m_ReferenceNode.GetPositionWorld() );
        m_ElasticRepresentation.SetElasticPoints( startPosition, endPosition );
    }

    protected void OnEnable()
    {
        MiddleVR.VRLog( 3, "[ ] VRInteractionNavigationElastic: enabled" );
        if( m_it != null )
        {
            MiddleVR.VRInteractionMgr.Activate( m_it );
        }
    }

    protected void OnDisable()
    {
        MiddleVR.VRLog( 3, "[ ] VRInteractionNavigationElastic: disabled" );
        if( m_it != null && MiddleVR.VRInteractionMgr != null )
        {
            MiddleVR.VRInteractionMgr.Deactivate( m_it );
        }
    }
}
