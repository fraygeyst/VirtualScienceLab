/* VRTouch
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class VRTouch {
    public enum ETouchParameter
    {
        NoTouchEvents,
        SendTouchEvents,
        ReceiveTouchEvents
    }

    private GameObject m_TouchObject = null;
    private GameObject m_TouchedObject = null;

    /// <summary>
    /// Object that initiated the touch event
    /// </summary>
    public GameObject TouchObject
    {
        get
        {
            return m_TouchObject;
        }
        set
        {
            m_TouchObject = value;
        }
    }

    /// <summary>
    /// Object receiving the touch event
    /// </summary>
    public GameObject TouchedObject
    {
        get
        {
            return m_TouchedObject;
        }
        set
        {
            m_TouchedObject = value;
        }
    }
}
