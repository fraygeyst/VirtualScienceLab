/* VRActor
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Interactions/Actor")]
[RequireComponent(typeof(VRNode3DSynchronization))]
public class VRActor : MonoBehaviour
{
    public bool Grabable = true;
    public bool AddCollider = true;

    [HideInInspector]
    public VRTouch.ETouchParameter TouchEvents = VRTouch.ETouchParameter.ReceiveTouchEvents;

    private List<VRTouch> m_Touches = new List<VRTouch>();

    protected void Start()
    {
        // Check for VRSyncNode3D even if it is required because Unity
        // doesn't add it automatically on old objects.
        if (gameObject.GetComponent<VRNode3DSynchronization>() == null)
        {
            gameObject.AddComponent<VRNode3DSynchronization>().Reset();
        }

        if (AddCollider && gameObject.GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    protected void OnDestroy()
    {
        foreach (VRTouch touch in m_Touches)
        {
            _SendVRTouchEnd(touch,true);
        }
    }

    public List<VRTouch> GetTouches()
    {
        return new List<VRTouch>(m_Touches);
    }

    // ***** RECEIVE TOUCH
    protected void _OnMVRTouchBegin(VRTouch iTouch)
    {
        if (TouchEvents == VRTouch.ETouchParameter.ReceiveTouchEvents)
        {
            m_Touches.Add(iTouch);
            //MiddleVRTools.Log(2, "Touch begin: " + iSrc.name + ". NbTouch: " + m_Touches.Count );
            SendMessage("OnMVRTouchBegin", iTouch,SendMessageOptions.DontRequireReceiver);
        }
    }

    protected void _OnMVRTouchMoved(VRTouch iTouch)
    {
        if (TouchEvents == VRTouch.ETouchParameter.ReceiveTouchEvents)
        {
            //MiddleVRTools.Log(2, "Touch end: " + iSrc.name + ".NbTouch: " + m_Touches.Count);
            SendMessage("OnMVRTouchMoved", iTouch, SendMessageOptions.DontRequireReceiver);
        }
    }

    protected void _OnMVRTouchEnd(VRTouch iTouch)
    {
        if (TouchEvents == VRTouch.ETouchParameter.ReceiveTouchEvents)
        {
            m_Touches.Remove(iTouch);
            //MiddleVRTools.Log(2, "Touch end: " + iSrc.name + ".NbTouch: " + m_Touches.Count);
            SendMessage("OnMVRTouchEnd", iTouch, SendMessageOptions.DontRequireReceiver);
        }
    }

    // ***** SEND TOUCH
    protected void OnTriggerEnter(Collider collider)
    {
        if (TouchEvents == VRTouch.ETouchParameter.SendTouchEvents)
        {
            GameObject touchedObject = collider.gameObject;

            // Other actor
            VRActor otherActor = touchedObject.GetComponent<VRActor>();

            if (otherActor != null)
            {
                VRTouch touch = new VRTouch();
                touch.TouchedObject = touchedObject;
                touch.TouchObject = this.gameObject;

                // Send message to touched object
                otherActor._OnMVRTouchBegin(touch);

                // Send message to ourself so attached script can also react
                this.SendMessage("OnMVRTouchBegin", touch, SendMessageOptions.DontRequireReceiver);
                m_Touches.Add(touch);
            }
        }
    }

    protected VRTouch _FindVRTouch(GameObject iTouchedObject)
    {
        VRTouch touch = null;

        // Find existing VRTouch
        foreach (VRTouch touchInList in m_Touches)
        {
            if (touchInList.TouchedObject == iTouchedObject)
            {
                touch = touchInList;
            }
        }

        return touch;
    }

    protected void OnTriggerStay(Collider collider)
    {
        if (TouchEvents == VRTouch.ETouchParameter.SendTouchEvents)
        {
            // We are the touch emitter
            GameObject touchedObject = collider.gameObject;
            VRActor actor = touchedObject.GetComponent<VRActor>();

            if (actor != null)
            {
                VRTouch touch = _FindVRTouch(touchedObject);

                if (touch != null)
                {
                    // Send message to touched object
                    actor._OnMVRTouchMoved(touch);
                    // Send message to ourself
                    this.SendMessage("OnMVRTouchMoved", touch, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    protected void OnTriggerExit(Collider collider)
    {
        if (TouchEvents == VRTouch.ETouchParameter.SendTouchEvents)
        {
            // We are the touch emitter
            GameObject touchedObject = collider.gameObject;

            VRTouch touch = _FindVRTouch(touchedObject);

            if (touch != null)
            {
                _SendVRTouchEnd(touch, false);
            }
        }
    }

    protected void _SendVRTouchEnd(VRTouch iTouch, bool iOnDestroy)
    {
        // Other actor
        VRActor actor = iTouch.TouchedObject.GetComponent<VRActor>();

        if (actor != null)
        {
            // Send message to touched object
            actor._OnMVRTouchEnd(iTouch);
            // Send message to ourself
            this.SendMessage("OnMVRTouchEnd", iTouch, SendMessageOptions.DontRequireReceiver);

            // XXX Cb: in OnDestroy we are iterating over the m_Touches list
            // here we are removing an element, so in OnDestroy, foreach
            // complains that the list has been modified
            // There is probably a cleaner way to do this !!
            if (!iOnDestroy)
            {
                // Ourself
                m_Touches.Remove(iTouch);
            }
        }
    }
}
