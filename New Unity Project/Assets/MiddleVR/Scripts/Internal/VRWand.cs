/* VRWand
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRWand : MonoBehaviour
{
    public float DefaultRayLength = 10;
    public Color DefaultRayColor = Color.white;

    private GameObject m_WandCube = null;
    private GameObject m_WandRay = null;
    private Renderer   m_WandRayRenderer = null;
    private float m_RayLength = 0;
    private VRSelectionManager m_SelectionMgr = null;

    public bool SendWandEvents = true;
    public bool RepeatVRAction = false;

    protected void Start()
    {
        m_SelectionMgr = this.GetComponent<VRSelectionManager>();
        if (m_SelectionMgr == null)
        {
            MVRTools.Log(0, "[X] VRWand: impossible to retrieve VRSelectionManager.");
            enabled = false;
            return;
        }

        _FindWandGeometry();

        SetRayLength(DefaultRayLength);
    }

    private void _FindWandGeometry()
    {
        m_WandCube = transform.Find("DefaultWandRepresentation/WandCube").gameObject;
        m_WandRay = transform.Find("DefaultWandRepresentation/WandRay").gameObject;
        m_WandRayRenderer = transform.Find("DefaultWandRepresentation/WandRay/RayMesh").GetComponent<Renderer>();
    }

    protected void Update()
    {
        VRSelection selection = m_SelectionMgr.GetSelection();

        // Send action if selection not null
        if (selection != null && selection.SelectedObject != null)
        {
            if (SendWandEvents)
            {
                // VRAction
                if ((!RepeatVRAction && MiddleVR.VRDeviceMgr.IsWandButtonToggled(0))
                     || (RepeatVRAction && MiddleVR.VRDeviceMgr.IsWandButtonPressed(0)))
                {
                    selection.SelectedObject.SendMessage("VRAction", selection, SendMessageOptions.DontRequireReceiver);
                }

                // Wand button pressed/released
                if (MiddleVR.VRDeviceMgr.IsWandButtonToggled(0))
                {
                    selection.SelectedObject.SendMessage("OnMVRWandButtonPressed", selection, SendMessageOptions.DontRequireReceiver);
                }
                else if (MiddleVR.VRDeviceMgr.IsWandButtonToggled(0, false))
                {
                    selection.SelectedObject.SendMessage("OnMVRWandButtonReleased", selection, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    public void Show( bool iValue )
    {
        if (m_WandRay == null || m_WandRayRenderer == null || m_WandCube == null)
        {
            _FindWandGeometry();
        }

        if (m_WandRayRenderer != null && m_WandCube != null)
        {
            m_WandRayRenderer.enabled = iValue;
            m_WandCube.GetComponent<Renderer>().enabled = iValue;
        }
    }

    public void ShowRay(bool iValue)
    {
        m_WandRayRenderer.enabled = iValue;
    }

    public bool IsRayVisible()
    {
        return m_WandRayRenderer.enabled;
    }

    public VRSelection GetSelection()
    {
        // Find Selection Mgr
        VRSelectionManager selectionManager = this.GetComponent<VRSelectionManager>();

        // Return selection
        return selectionManager.GetSelection();
    }

    public float GetDefaultRayLength()
    {
        return DefaultRayLength;
    }

    private float GetRayLength()
    {
        return m_RayLength;
    }

    public void SetRayLength(float iLenght)
    {
        m_RayLength = iLenght;
        m_WandRay.transform.localScale = new Vector3(1.0f, 1.0f, m_RayLength);
    }

    public void SetRayColor(Color iColor)
    {
        m_WandRayRenderer.material.color = iColor;
    }
}
