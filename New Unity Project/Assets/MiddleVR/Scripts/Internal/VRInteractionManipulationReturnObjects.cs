/* VRInteractionManipulationReturnObjects
 * MiddleVR
 * (c) MiddleVR
 *
 * Note: Made to be attached to the Wand
 */

using UnityEngine;
using System.Collections.Generic;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRInteractionManipulationReturnObjects : MonoBehaviour {

    public float ObjectReturnSpeed = 1.4f; // Meters per second

    private struct ReturningObject
    {
        public GameObject Object;
        public Vector3 TargetLocalPosition;
        public Quaternion TargetLocalRotation;
        public Vector3 StartLocalPosition;
        public Quaternion StartLocalRotation;
        public bool InstantReturn;
        public bool WasGrabbable;
        public bool WasCollidable;
    }

    private List<ReturningObject> m_ReturningObjects = new List<ReturningObject>();

    protected void Update()
    {
        // Do the return
        _ReturnObjects();
    }

    private void _ReturnObjects()
    {
        for (int i = 0; i < m_ReturningObjects.Count; ++i)
        {
            ReturningObject currentObject = m_ReturningObjects[i];

            // Move directly to final position if object asked for it or if transition speed is null
            bool finalizeReturn = currentObject.InstantReturn || (ObjectReturnSpeed < 0.0f) || Mathf.Approximately(ObjectReturnSpeed, 0.0f);

            if (!finalizeReturn)
            {
                float distanceToMove = ObjectReturnSpeed * (float)(MiddleVR.VRKernel.GetDeltaTime());

                Vector3 vectorToTarget = currentObject.TargetLocalPosition - currentObject.Object.transform.localPosition;

                if (vectorToTarget.magnitude > distanceToMove)
                {
                    // Apply translation
                    currentObject.Object.transform.localPosition += vectorToTarget.normalized * distanceToMove;

                    float state = (currentObject.Object.transform.localPosition - currentObject.StartLocalPosition).magnitude
                                / (currentObject.TargetLocalPosition - currentObject.StartLocalPosition).magnitude;
                    Quaternion rotation = Quaternion.Lerp(currentObject.StartLocalRotation, currentObject.TargetLocalRotation, state);

                    // Apply rotation
                    currentObject.Object.transform.localRotation = rotation;
                }
                else
                {
                    finalizeReturn = true;
                }
            }

            if (finalizeReturn)
            {
                _FinalizeReturn(i);
            }
        }
    }

    private void _FinalizeReturn( int iObjectId )
    {
        ReturningObject currentObject = m_ReturningObjects[iObjectId];

        currentObject.Object.transform.localPosition = currentObject.TargetLocalPosition;
        currentObject.Object.transform.localRotation = currentObject.TargetLocalRotation;

        _ResetObjectProperties(currentObject);

        m_ReturningObjects.Remove(m_ReturningObjects[iObjectId]);
    }

    public void AddReturningObject(GameObject iObject, Vector3 iLocalPosition, Quaternion iLocalRotation, bool iInstantReturn)
    {
        ReturningObject newObject;
        newObject.Object = iObject;
        newObject.TargetLocalPosition = iLocalPosition;
        newObject.TargetLocalRotation = iLocalRotation;
        newObject.StartLocalPosition = iObject.transform.localPosition;
        newObject.StartLocalRotation = iObject.transform.localRotation;
        newObject.InstantReturn      = iInstantReturn;
        newObject.WasGrabbable       = false;
        newObject.WasCollidable      = false;

        // Not grabbable during return
        VRActor actor = iObject.GetComponent<VRActor>();
        if( actor != null )
        {
            newObject.WasGrabbable = actor.Grabable;
            actor.Grabable = false;
        }

        // No collisions during return
        Collider collider = iObject.GetComponent<Collider>();
        if (collider != null)
        {
            newObject.WasCollidable = collider.enabled;
            collider.enabled = false;
        }
        
        m_ReturningObjects.Add(newObject);
    }

    private void _ResetObjectProperties(ReturningObject iObject)
    {
        // Reset Grabbable
        VRActor actor = iObject.Object.GetComponent<VRActor>();
        if (actor != null)
        {
            actor.Grabable = iObject.WasGrabbable;
        }

        // Reset collisions
        Collider collider = iObject.Object.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = iObject.WasCollidable;
        }
    }

    protected void OnEnable()
    {
        MiddleVR.VRLog(3, "[ ] VRInteractionManipulationReturnObjects: enabled");
    }

    protected void OnDisable()
    {
        MiddleVR.VRLog(3, "[ ] VRInteractionManipulationReturnObjects: disabled");

        // Reset all objects and release them
        foreach (ReturningObject returningObject in m_ReturningObjects)
        {
            _ResetObjectProperties(returningObject);
        }

        m_ReturningObjects.Clear();
    }
}
