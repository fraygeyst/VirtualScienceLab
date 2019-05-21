/* VRMenuManager
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRMenuManager : MonoBehaviour
{
    public int                  HideShowWandButton = 2;
    public GameObject           MenuGUI;
    public VRMenuMaximizeButton MaximizeButton;

    private GameObject m_SystemCenterNode;
    private GameObject m_HeadNode;

    private bool m_IsMenuUsed = true;
    private bool m_IsMenuOpen = true;
    private bool m_Initializing = true;

    private enum EMenuState
    {
        eHidden,
        eShowing,
        eVisible,
        eHiding
    }
    private EMenuState m_MenuState = EMenuState.eHidden;

    // Animation parameters
    private GameObject m_Wand;
    private float m_TransitionDuration = 0.5f;
    private float m_StartTime;
    private Vector3 m_NormalScale;
    private Vector3 m_StartScale;

    // Positions and rotations are VRSystemCenterNode-relative for navigation compatibility
    private Vector3 m_StartPosition;
    private Vector3 m_TargetPosition;
    private Quaternion m_StartRotation;

    protected void Start()
    {
        m_NormalScale = this.transform.localScale;
        this.transform.localScale = Vector3.zero;

        _RefreshMenu();
    }

    protected void Update()
    {
        if (!m_IsMenuUsed)
        {
            return;
        }

        if (m_Initializing)
        {
            // Attach to VRSystemCenterNode
            if (m_SystemCenterNode == null)
            {
                vrNode3D systemCenterNode = MiddleVR.VRDisplayMgr.GetNodeByTag("VRSystemCenter");
                m_SystemCenterNode = MVRNodesMapper.Instance.GetNode(systemCenterNode);
                if (m_SystemCenterNode == null)
                {
                    return;
                }
            }

            transform.parent = m_SystemCenterNode.transform;

            // Retrieve Head
            if (m_HeadNode == null)
            {
                m_HeadNode = GameObject.Find("HeadNode");
                if (m_HeadNode == null)
                {
                    return;
                }
            }

            // Retrieve Wand
            if (m_Wand == null)
            {
                m_Wand = GameObject.Find("VRWand");
                if (m_Wand == null)
                {
                    return;
                }
            }

            m_Initializing = false;
        }

        // Toggle visibility
        if (MiddleVR.VRDeviceMgr.IsWandButtonToggled((uint)HideShowWandButton))
        {
            ToggleVisiblity();
        }

        _VisibilityAnimation();
    }

    public void ToggleMenuGUI()
    {
        m_IsMenuOpen = !m_IsMenuOpen;
        _RefreshMenu();
    }

    private void _RefreshMenu()
    {
        MaximizeButton.Toggle(m_IsMenuOpen);
        MenuGUI.GetComponent<MeshRenderer>().enabled = m_IsMenuOpen;
        Collider menuCollider = MenuGUI.GetComponent<Collider>();
        if( menuCollider != null )
        {
            menuCollider.enabled = m_IsMenuOpen;
        }
    }

    public void ToggleVisiblity()
    {
        if (m_MenuState == EMenuState.eHidden || m_MenuState == EMenuState.eHiding)
        {
            m_StartTime = Time.time;

            Vector3 wandLocalPosition = m_SystemCenterNode.transform.InverseTransformPoint(m_Wand.transform.position);
            Quaternion wandLocalRotation = Quaternion.Inverse(m_SystemCenterNode.transform.rotation) * m_Wand.transform.rotation;
            Vector3 wandLocalForward = m_SystemCenterNode.transform.InverseTransformDirection(m_Wand.transform.forward);

            if (m_MenuState == EMenuState.eHiding)
            {
                m_StartRotation = this.transform.localRotation;
                m_StartPosition = this.transform.localPosition;
                m_StartScale = this.transform.localScale;
            }
            else
            {
                m_StartRotation = wandLocalRotation;
                m_StartPosition = wandLocalPosition;
                m_StartScale = Vector3.zero;
            }

            m_TargetPosition = wandLocalPosition + 0.5f * wandLocalForward;
            m_MenuState = EMenuState.eShowing;
        }
        else if (m_MenuState == EMenuState.eVisible || m_MenuState == EMenuState.eShowing)
        {
            m_StartTime = Time.time;
            m_StartPosition = this.transform.localPosition;
            m_StartRotation = this.transform.localRotation;
            m_StartScale = this.transform.localScale;
            m_MenuState = EMenuState.eHiding;
        }
    }

    private void _VisibilityAnimation()
    {
        switch (m_MenuState)
        {
            case EMenuState.eHidden:
                {
                    break;
                }
            case EMenuState.eHiding:
                {
                    float state = (Time.time - m_StartTime) / m_TransitionDuration;

                    if (state >= 1.0f)
                    {
                        this.transform.localScale = Vector3.zero;
                        this.transform.position = m_Wand.transform.position;
                        this.transform.rotation = m_Wand.transform.rotation;
                        m_MenuState = EMenuState.eHidden;
                    }
                    else
                    {
                        this.transform.localScale = Vector3.Slerp(m_StartScale, Vector3.zero, state);
                        Vector3 wandLocalPosition = m_SystemCenterNode.transform.InverseTransformPoint(m_Wand.transform.position);
                        this.transform.localPosition = Vector3.Slerp(m_StartPosition, wandLocalPosition, state);
                        Quaternion wandLocalRotation = Quaternion.Inverse(m_SystemCenterNode.transform.rotation) * m_Wand.transform.rotation;
                        this.transform.localRotation = Quaternion.Slerp(m_StartRotation, wandLocalRotation, state);
                    }

                    break;
                }
            case EMenuState.eShowing:
                {
                    float state = (Time.time - m_StartTime) / m_TransitionDuration;

                    Vector3 forward = m_SystemCenterNode.transform.InverseTransformDirection((this.transform.position - m_HeadNode.transform.position).normalized);
                    Quaternion targetRotation = Quaternion.LookRotation(forward);

                    if (state >= 1.0f)
                    {
                        this.transform.parent = m_SystemCenterNode.transform;
                        this.transform.localScale = m_NormalScale;
                        this.transform.localPosition = m_TargetPosition;
                        this.transform.localRotation = targetRotation;
                        m_MenuState = EMenuState.eVisible;
                    }
                    else
                    {
                        this.transform.localScale = Vector3.Slerp(m_StartScale, m_NormalScale, state);
                        this.transform.localPosition = Vector3.Slerp(m_StartPosition, m_TargetPosition, state);
                        this.transform.localRotation = Quaternion.Slerp(m_StartRotation, targetRotation, state);
                    }

                    break;
                }
            case EMenuState.eVisible:
                {
                    break;
                }
        }
    }

    public void UseVRMenu(bool iUseVRMenu)
    {
        m_IsMenuUsed = iUseVRMenu;

        // Hide and deactivate
        this.GetComponent<Renderer>().enabled = m_IsMenuUsed;
        this.GetComponent<Collider>().enabled = m_IsMenuUsed;

        int childrenNb = this.transform.childCount;
        for (int i = 0; i < childrenNb; ++i )
        {
            this.transform.GetChild(i).gameObject.SetActive(iUseVRMenu);
        }
    }
}
