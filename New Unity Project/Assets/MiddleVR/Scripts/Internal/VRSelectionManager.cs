/* VRSelectionManager
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class VRSelectionManager : MonoBehaviour {

    VRSelection m_Selection = null;

    public void SetSelection(VRSelection iSelection)
    {
        m_Selection = iSelection;
    }

    public VRSelection GetSelection()
    {
        return m_Selection;
    }
}

public class VRSelection
{
    public GameObject SelectedObject    = null;
    public VRWand     SourceWand        = null;
    public Vector2    TextureCoordinate = Vector2.zero;
    // Result of raycast hit distance: Distance from origin of wand to point of intersection
    public float      SelectionDistance = 0.0f;

    // Needed later? (all RaycastHit structure?)
    public Vector3 SelectionContact = Vector3.zero;
    public Vector3 SelectionNormal  = Vector3.zero;

    public static bool Compare(VRSelection iFirst, VRSelection iSecond)
    {
        if (iFirst != null && iSecond != null)
        {
            return iFirst.SelectedObject == iSecond.SelectedObject;
        }
        else if (iFirst == null && iSecond == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
