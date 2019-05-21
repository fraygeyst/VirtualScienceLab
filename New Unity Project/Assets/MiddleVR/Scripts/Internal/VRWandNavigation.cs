/* VRWandNavigation
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
//[RequireComponent (typeof(VRWandInteraction))]
public class VRWandNavigation : MonoBehaviour {
    public string NodeToMove = "VRSystemCenterNode";
    public string DirectionReferenceNode = "HandNode";
    public string TurnAroundNode = "HeadNode";

    public float NavigationSpeed = 1.0f;
    public float RotationSpeed = 1.0f;

    public bool Fly = true;

    private GameObject m_VRMgr            = null;
    private GameObject m_DirectionRefNode = null;
    private GameObject m_NodeToMove       = null;
    private GameObject m_TurnNode         = null;

    private bool m_SearchedRefNode        = false;
    private bool m_SearchedNodeToMove     = false;
    private bool m_SearchedRotationNode   = false;

    protected void Update()
    {
        // Get nodes when created
        if (m_DirectionRefNode == null)
        {
            m_DirectionRefNode = GameObject.Find(DirectionReferenceNode);
        }
        if (m_NodeToMove == null)
        {
            m_NodeToMove = GameObject.Find(NodeToMove);
        }
        if (m_TurnNode == null)
        {
            m_TurnNode = GameObject.Find(TurnAroundNode);
        }

        if ( m_SearchedRefNode == false && m_DirectionRefNode == null )
        {
            MVRTools.Log("[X] VRWandNavigation: Couldn't find '" + DirectionReferenceNode + "'");
            m_SearchedRefNode = true;
        }

        if (m_SearchedNodeToMove == false && m_NodeToMove == null)
        {
            MVRTools.Log("[X] VRWandNavigation: Couldn't find '" + NodeToMove + "'");
            m_SearchedNodeToMove = true;
        }

        if (m_SearchedRotationNode == false && TurnAroundNode.Length > 0 && m_TurnNode == null)
        {
            MVRTools.Log("[X] VRWandNavigation: Couldn't find '" + TurnAroundNode + "'");
            m_SearchedRotationNode = true;
        }

        if (m_DirectionRefNode != null && m_NodeToMove != null )
        {
            float speed = 0.0f;
            float speedR = 0.0f;

            if (m_VRMgr == null)
            {
                m_VRMgr = GameObject.Find("VRManager");
            }

            if( m_VRMgr != null )
            {
                VRManagerScript script = m_VRMgr.GetComponent<VRManagerScript>();

                if( script != null )
                {
                    /// FORWARD
                    float forward = script.WandAxisVertical;

                    //MVRTools.Log("Forward: " + forward);

                    float deltaTime = (float)MiddleVR.VRKernel.GetDeltaTime();

                    if (Math.Abs(forward) > 0.1) speed = forward * NavigationSpeed * deltaTime;

                    /// ROTATION
                    float rotation = script.WandAxisHorizontal;

                    if (Math.Abs(rotation) > 0.1) speedR = rotation * RotationSpeed * deltaTime;

                    /// Computing direction
                    Vector3 translationVector = new Vector3(0, 0, 1);
                    Vector3 tVec = translationVector;
                    Vector3 nVec = new Vector3(tVec.x * speed, tVec.y * speed, tVec.z * speed );

                    Vector3 mVec = m_DirectionRefNode.transform.TransformDirection(nVec);

                    if( Fly == false )
                    {
                        mVec.y = 0.0f;
                    }

                    m_NodeToMove.transform.Translate(mVec,Space.World);

                    if (m_TurnNode != null)
                    {
                        m_NodeToMove.transform.RotateAround(m_TurnNode.transform.position, new Vector3(0, 1, 0), speedR);
                    }
                    else
                    {
                        m_NodeToMove.transform.Rotate(new Vector3(0, 1, 0), speedR);
                    }
                }
            }
        }
    }
}
