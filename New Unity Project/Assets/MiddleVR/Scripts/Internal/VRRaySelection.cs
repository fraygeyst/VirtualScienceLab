/* VRRaySelection
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRRaySelection : VRInteraction {

    public Color HoverColor = Color.green;

    private VRSelection m_LastSelection = new VRSelection();

    private VRSelectionManager m_SelectionMgr = null;
    private VRWand m_Wand = null;

    protected void Start()
    {
        // Make sure the base interaction is started and create interaction
        InitializeBaseInteraction();
        CreateInteraction("VRRaySelection");
        base.GetInteraction().AddTag("ContinuousSelection");
        base.Activate();

        m_SelectionMgr = this.GetComponent<VRSelectionManager>();
        if (m_SelectionMgr == null)
        {
            MVRTools.Log(0, "[X] VRRaySelection: impossible to retrieve VRSelectionManager.");
            enabled = false;
            return;
        }

        m_Wand = this.GetComponent<VRWand>();
        if (m_Wand == null)
        {
            MVRTools.Log(0, "[X] VRRaySelection: impossible to retrieve VRWand.");
            enabled = false;
            return;
        }
    }

    protected void Update ()
    {
        if (IsActive())
        {
            _RaySelection();
            _RefreshRayMesh();
            _SendWandEvents();
        }
    }

    private void _SendWandEvents()
    {
        if (!m_Wand.SendWandEvents)
        {
            return;
        }

        VRSelection selection = m_SelectionMgr.GetSelection();

        // Enter/exit events
        if ( !VRSelection.Compare(m_LastSelection, selection) )
        {
            // Selection changed

            // Exit last
            if (m_LastSelection != null)
            {
                m_LastSelection.SelectedObject.SendMessage("OnMVRWandExit", m_LastSelection, SendMessageOptions.DontRequireReceiver);
            }

            // Enter new
            if (selection != null)
            {
                selection.SelectedObject.SendMessage("OnMVRWandEnter", selection, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            // Hover current
            if (selection != null)
            {
                selection.SelectedObject.SendMessage("OnMVRWandHover", selection, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    protected void _RaySelection()
    {
        // Ray picking
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.TransformDirection(Vector3.forward);

        VRSelection newSelection = null;

        foreach (RaycastHit raycastHit in Physics.RaycastAll(rayOrigin, rayDirection, m_Wand.GetDefaultRayLength()))
        {
            if (newSelection != null && raycastHit.distance >= newSelection.SelectionDistance)
            {
                continue;
            }

            GameObject objectHit = raycastHit.collider.gameObject;

            if (objectHit.name != "VRWand")
            {
                // Ignore GameObject without the VRActor component
                if (objectHit.GetComponent<VRActor>() == null)
                {
                    continue;
                }

                VRWebView webView = objectHit.GetComponent<VRWebView>();
                VRRaycastHit completeHit = null;
                if (webView != null)
                {
                    completeHit = webView.RaycastMesh(rayOrigin, rayDirection);
                }
                else
                {
                    completeHit = new VRRaycastHit(raycastHit);
                }

                if (completeHit != null)
                {
                    // Special case : pass through transparent pixels of web views.
                    if (webView != null)
                    {
                        if (!webView.GetComponent<Renderer>().enabled || webView.IsPixelEmpty(completeHit.textureCoord))
                        {
                            continue;
                        }
                    }

                    // Create selection if it does not exist
                    if (newSelection == null)
                    {
                        newSelection = new VRSelection();
                    }

                    newSelection.SourceWand = m_Wand;
                    newSelection.SelectedObject = objectHit;
                    newSelection.TextureCoordinate = completeHit.textureCoord;
                    newSelection.SelectionDistance = completeHit.distance;
                    newSelection.SelectionContact = completeHit.point;
                    newSelection.SelectionNormal = completeHit.normal;
                }
            }
        }

        m_LastSelection = m_SelectionMgr.GetSelection();
        m_SelectionMgr.SetSelection(newSelection);
    }

    protected void _RefreshRayMesh()
    {
        VRSelection selection = m_SelectionMgr.GetSelection();

        if (selection != null)
        {
            m_Wand.SetRayColor(HoverColor);
            m_Wand.SetRayLength(selection.SelectionDistance);
        }
        else
        {
            m_Wand.SetRayColor(m_Wand.DefaultRayColor);
            m_Wand.SetRayLength(m_Wand.DefaultRayLength);
        }
    }
}
