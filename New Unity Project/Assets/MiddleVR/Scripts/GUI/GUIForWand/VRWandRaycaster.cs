/* VRWandRaycaster
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent (typeof(RectTransform))]
[RequireComponent (typeof(Canvas))]
public class VRWandRaycaster : BaseRaycaster
{
    private RectTransform m_RectTransform;
    private Camera m_WandCamera;

    public override Camera eventCamera
    {
        get
        {
            if (m_WandCamera == null)
            {
                var wand = FindObjectOfType<VRWand>();
                m_WandCamera = wand.GetComponent<Camera>();
                if (m_WandCamera == null)
                {
                    m_WandCamera = wand.gameObject.AddComponent<Camera>();
                    m_WandCamera.enabled = false;
                }
            }

            return m_WandCamera;
        }
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        if (m_RectTransform == null)
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        var wand = FindObjectOfType<VRWand>();

        Ray ray = new Ray(wand.transform.position, wand.transform.forward);

        Vector3 hitPosition;
        RaycastResult hitResult;
        if (RaycastCanvas(ray, wand.DefaultRayLength, out hitResult, out hitPosition))
        {
            var canvasHitPoint = m_RectTransform.InverseTransformPoint(hitPosition);

            var uiObjects = GetComponentsInChildren<UIBehaviour>();
            for (int j = 0, jEnd = uiObjects.Length; j < jEnd; ++j)
            {
                if (!(uiObjects[j] is ICanvasElement))
                {
                    continue;
                }

                var uiObjectBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(transform, uiObjects[j].GetComponent<RectTransform>());
                uiObjectBounds.max = new Vector3(uiObjectBounds.max.x, uiObjectBounds.max.y, 1.0f);
                uiObjectBounds.min = new Vector3(uiObjectBounds.min.x, uiObjectBounds.min.y, -1.0f);

                if (uiObjectBounds.Contains(canvasHitPoint))
                {
                    RaycastResult uiObjectHitResult = new RaycastResult();

                    uiObjectHitResult.distance = hitResult.distance;
                    uiObjectHitResult.module = this;
                    uiObjectHitResult.gameObject = uiObjects[j].gameObject;

                    resultAppendList.Add(uiObjectHitResult);
                }
            }
        }

        if (resultAppendList.Count > 0)
        {
            resultAppendList.Sort(delegate (RaycastResult r0, RaycastResult r1)
            {
                return r0.distance.CompareTo(r1.distance);
            });
        }
    }

    private bool RaycastCanvas(Ray iRay, float iMaxDist, out RaycastResult oHitResult, out Vector3 oHitPosition)
    {
        Vector3 vertex0 = new Vector3(m_RectTransform.rect.xMin, m_RectTransform.rect.yMin);
        Vector3 vertex1 = new Vector3(m_RectTransform.rect.xMax, m_RectTransform.rect.yMin);
        Vector3 vertex2 = new Vector3(m_RectTransform.rect.xMin, m_RectTransform.rect.yMax);
        Vector3 vertex3 = new Vector3(m_RectTransform.rect.xMax, m_RectTransform.rect.yMax);

        if (RaycastTriangle(m_RectTransform.transform.TransformPoint(vertex0),
                            m_RectTransform.transform.TransformPoint(vertex1),
                            m_RectTransform.transform.TransformPoint(vertex2),
                            iRay, iMaxDist, out oHitResult, out oHitPosition))
        {
            return true;
        }

        return RaycastTriangle(m_RectTransform.transform.TransformPoint(vertex1),
                               m_RectTransform.transform.TransformPoint(vertex2),
                               m_RectTransform.transform.TransformPoint(vertex3),
                               iRay, iMaxDist, out oHitResult, out oHitPosition);
    }

    private bool RaycastTriangle(Vector3 iVertex0, Vector3 iVertex1, Vector3 iVertex2, Ray iRay, float iMaxDist, out RaycastResult oHitResult, out Vector3 oHitPosition)
    {
        oHitResult = new RaycastResult();
        oHitPosition = Vector3.zero;

        // Möller–Trumbore intersection algorithm
        // http://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm

        // edge 1
        Vector3 E1 = iVertex1 - iVertex0;

        // edge 2
        Vector3 E2 = iVertex2 - iVertex0;

        // edge 3 - only for testing degenerate triangles
        Vector3 E3 = iVertex2 - iVertex0;
        if (E1.sqrMagnitude < 0.00001f || E2.sqrMagnitude < 0.00001f || E3.sqrMagnitude < 0.00001f)
        {
            // Skip degenerate triangle
            return false;
        }

        //Begin calculating determinant - also used to calculate u parameter
        Vector3 P = Vector3.Cross(iRay.direction, E2);
        float det = Vector3.Dot(E1, P);
        if (det > -0.00001f && det < 0.00001f)
        {
            //if determinant is near zero, ray lies in plane of triangle
            return false;
        }

        float inv_det = 1.0f / det;
        //Calculate distance from V1 to ray origin
        Vector3 T = iRay.origin - iVertex0;
        //Calculate u parameter and test bound
        float u = Vector3.Dot(T, P) * inv_det;
        if (u < 0.0f || u > 1.0f)
        {
            //The intersection lies outside of the triangle
            return false;
        }

        //Prepare to test v parameter
        Vector3 Q = Vector3.Cross(T, E1);
        //Calculate V parameter and test bound
        float v = Vector3.Dot(iRay.direction, Q) * inv_det;
        if (v < 0.0f || u + v > 1.0f)
        {
            //The intersection lies outside of the triangle
            return false;
        }

        float t = Vector3.Dot(E2, Q) * inv_det;

        if (t <= 0.00001f)
        {
            // Intersection is before origin
            return false;
        }

        oHitPosition = iRay.origin + (iRay.direction * t);
        oHitResult.distance = (oHitPosition - iRay.origin).magnitude;

        Vector3 w = oHitPosition - iVertex0;

        Vector3 vCrossW = Vector3.Cross(E2, w);
        Vector3 vCrossU = Vector3.Cross(E2, E1);

        if (Vector3.Dot(vCrossW, vCrossU) < 0.0f)
        {
            return false;
        }

        Vector3 uCrossW = Vector3.Cross(E1, w);
        Vector3 uCrossV = Vector3.Cross(E1, E2);

        if (Vector3.Dot(uCrossW, uCrossV) < 0.0f)
        {
            return false;
        }

        float denom = uCrossV.magnitude;
        float b1 = vCrossW.magnitude / denom;
        float b2 = uCrossW.magnitude / denom;

        // The VRCanvas is not supposed to have a collider but for the VRWand to work correctly
        // we need the VRCanvas to posses a box collider, thus we need to check if we hit
        // our self.
        RaycastHit hitInfo;
        bool didHitObjectBeforeCanvas = Physics.Raycast(iRay, out hitInfo, oHitResult.distance);

        if ((!didHitObjectBeforeCanvas || hitInfo.collider.gameObject == gameObject) &&
            oHitResult.distance <= iMaxDist &&
            (b1 <= 1.0f) && (b2 <= 1.0f) && (b1 + b2 <= 1.0f))
        {
            oHitResult.module = this;
            oHitResult.gameObject = gameObject;
            return true;
        }

        return false;
    }
}
