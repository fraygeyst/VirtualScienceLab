/* VRCanvas
 * MiddleVR
 * (c) MiddleVR
 */

// Add this script to the Unity Canvas object to interact with it using a Wand.
// For this to work you need to make sure that the Unity EventSystem script
// is in the scene.

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(Canvas))]
[RequireComponent (typeof(RectTransform))]
[RequireComponent (typeof(VRActor))]
[RequireComponent (typeof(VRWandRaycaster))]
public class VRCanvas : MonoBehaviour
{
    private VRWandRaycaster m_WandRayCaster;

    private List<RaycastResult> m_PreviouslyHoveredObjects = new List<RaycastResult>();
    private List<RaycastResult> m_PreviouslyPressedObjects = new List<RaycastResult>();

    protected void Start()
    {
        m_WandRayCaster = GetComponent<VRWandRaycaster>();

        var rectTransform = GetComponent<RectTransform>();
        var boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(rectTransform.rect.width, rectTransform.rect.height, .01f);

        var vrActor = GetComponent<VRActor>();
        vrActor.Grabable = false;
    }

    protected void Update()
    {
        List<RaycastResult> hoveredObjects = new List<RaycastResult>();
        m_WandRayCaster.Raycast(null, hoveredObjects);

        for (int i = 0, iEnd = hoveredObjects.Count; i < iEnd; ++i)
        {
            int previouslyHoveredObjectNdx = m_PreviouslyHoveredObjects.FindIndex(o => o.gameObject == hoveredObjects[i].gameObject);
            if (previouslyHoveredObjectNdx < 0)
            {
                var pointer = new PointerEventData(null);
                pointer.pointerCurrentRaycast = hoveredObjects[i];
                ExecuteEvents.Execute(hoveredObjects[i].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
            }
            else
            {
                m_PreviouslyHoveredObjects.RemoveAt(previouslyHoveredObjectNdx);
            }

            if (MiddleVR.VRDeviceMgr.IsWandButtonPressed(0))
            {
                var pointer = new PointerEventData(null);
                pointer.button = PointerEventData.InputButton.Left;
                pointer.pointerPress = hoveredObjects[i].gameObject;
                pointer.pointerCurrentRaycast = hoveredObjects[i];
                pointer.pointerPressRaycast = hoveredObjects[i];
                pointer.position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

                ExecuteEvents.Execute(hoveredObjects[i].gameObject, pointer, ExecuteEvents.pointerDownHandler);

                m_PreviouslyPressedObjects.Add(hoveredObjects[i]);
            }
            else
            {
                var pointer = new PointerEventData(null);
                pointer.button = PointerEventData.InputButton.Left;
                pointer.pointerPress = hoveredObjects[i].gameObject;
                pointer.pointerCurrentRaycast = hoveredObjects[i];
                pointer.pointerPressRaycast = hoveredObjects[i];
                pointer.position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

                ExecuteEvents.Execute(hoveredObjects[i].gameObject, pointer, ExecuteEvents.pointerUpHandler);

                int previouslyClickedObjectNdx = m_PreviouslyPressedObjects.FindIndex(o => o.gameObject == hoveredObjects[i].gameObject);
                if (previouslyClickedObjectNdx >= 0)
                {
                    ExecuteEvents.Execute(hoveredObjects[i].gameObject, pointer, ExecuteEvents.pointerClickHandler);
                    m_PreviouslyPressedObjects.RemoveAt(previouslyClickedObjectNdx);
                }
            }
        }

        for (int i = 0, iEnd = m_PreviouslyHoveredObjects.Count; i < iEnd; ++i)
        {
            var pointer = new PointerEventData(null);
            ExecuteEvents.Execute(m_PreviouslyHoveredObjects[i].gameObject, pointer, ExecuteEvents.pointerExitHandler);
        }

        m_PreviouslyHoveredObjects = hoveredObjects;
    }
}
