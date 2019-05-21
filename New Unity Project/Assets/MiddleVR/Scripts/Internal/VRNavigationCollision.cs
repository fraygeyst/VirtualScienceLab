/* VRNavigationCollision
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRNavigationCollision : MonoBehaviour {

    public float  CollisionDistance = 0.20f;
    public string CollisionNodeName = "HeadNode";

    GameObject m_CollisionNode;
    GameObject m_NavigationNode;
    vrNode3D   m_VRNavigationNode;

    bool m_IsFlying = false;

    bool m_Initialized = false;

    Vector3 m_LastCollisionNodePosition;

    public void SetCollisionNode( GameObject iCollisionNode )
    {
        m_CollisionNode = iCollisionNode;
    }

    public void SetNavigationNode( vrNode3D iNavigationNode )
    {
        if (m_VRNavigationNode == null)
        {
            MVRTools.Log(2, "[X] VRNavigationCollision: MiddleVR navigation node is null.");
        }

        m_VRNavigationNode = iNavigationNode;

        GameObject unityNavigationNode = MVRNodesMapper.Instance.GetNode(iNavigationNode);
        if (unityNavigationNode == null)
        {
            MVRTools.Log(2, "[X] VRNavigationCollision: impossible to retrieve navigation node GameObject.");
            return;
        }

        m_NavigationNode = unityNavigationNode;
    }

    public void SetFly( bool iFly )
    {
        m_IsFlying = iFly;
    }

    // Use this public method from interaction scripts to initialize and start collision
    public void Initialize()
    {
        if( m_CollisionNode!=null && m_NavigationNode!=null && m_VRNavigationNode!=null )
        {
            m_LastCollisionNodePosition = m_CollisionNode.transform.position;
            m_Initialized = true;
            MVRTools.Log( 2, "[ ] VRHeadCollision: initialized" );
        }
        else
        {
            MVRTools.Log( 2, "[X] VRHeadCollision: impossible to retrieve specified navigation or collision nodes." );
        }
    }

    protected void InitializeFromActiveNavigation()
    {
        // Search for navigation interaction parameters
        uint interactionNb = MiddleVR.VRInteractionMgr.GetInteractionsNb();

        if( interactionNb == 0 )
        {
            MVRTools.Log( 4, "[~] VRHeadCollision: no interaction found in Interaction Manager." );
            return;
        }

        bool fly = true;
        vrNode3D navNodeMVR = null;

        for( uint i=0 ; i<interactionNb ; ++i )
        {
            vrInteraction interaction = MiddleVR.VRInteractionMgr.GetInteractionByIndex(i);
            if( interaction.IsActive() && interaction.TagsContain("Navigation") )
            {
                // Get fly mode
                vrProperty flyProp = interaction.GetProperty("Fly");
                if( flyProp != null )
                {
                    fly = flyProp.GetBool();
                }

                // Get navigation node
                vrProperty navNodeProp = interaction.GetProperty("NavigationNode");
                if( navNodeProp != null )
                {
                    navNodeMVR = MiddleVR.VRDisplayMgr.GetNode( navNodeProp.GetObject().GetName() );
                }

                break;
            }
        }

        if (navNodeMVR == null)
        {
            MVRTools.Log(2, "[X] VRNavigationCollision: impossible to retrieve navigation node.");
            return;
        }

        // Initialize parameters from found ones
        SetCollisionNode (GameObject.Find(CollisionNodeName));
        SetNavigationNode(navNodeMVR);
        SetFly( fly );

        // Try to start the collisions
        Initialize();
    }

    protected Vector3 ComputeReactionMovement( Vector3 iStartPosition, Vector3 iMovement )
    {
        Vector3 reactionMovement = Vector3.zero;

        RaycastHit hit;
        if( Physics.SphereCast( iStartPosition, CollisionDistance, iMovement.normalized, out hit, iMovement.magnitude ) )
        {
            // Compute reaction vector
            Vector3 collisionNormal = hit.normal;
            if (!m_IsFlying)
            {
                collisionNormal.y = 0.0f;
            }

            if( Vector3.Dot( iMovement, collisionNormal ) < 0.0f )
            {
                reactionMovement = -Vector3.Project( iMovement, collisionNormal.normalized );
            }
        }

        return reactionMovement;
    }

    protected void Update()
    {
        if( !m_Initialized )
        {
            InitializeFromActiveNavigation();
        }

        if( m_NavigationNode == null || m_VRNavigationNode == null || m_CollisionNode == null )
        {
            return;
        }

        // Check if moved

        Vector3 startPos = m_LastCollisionNodePosition;
        Vector3 endPos   = m_CollisionNode.transform.position;
        Vector3 lastMovement = endPos-startPos;

        if( lastMovement.magnitude > 0.0f )
        {
            Vector3 reactionMovement = ComputeReactionMovement( startPos, lastMovement );

            // Update Unity and MVR versions of navigation node
            m_NavigationNode.transform.position += reactionMovement;
            m_VRNavigationNode.SetPositionWorld( MVRTools.FromUnity(m_NavigationNode.transform.position) );
        }

        m_LastCollisionNodePosition = m_CollisionNode.transform.position;
    }
}
