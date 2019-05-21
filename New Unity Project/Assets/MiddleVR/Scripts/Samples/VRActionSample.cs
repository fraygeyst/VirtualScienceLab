/* VRActionSample
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;

[AddComponentMenu("MiddleVR/Samples/Action")]
public class VRActionSample : MonoBehaviour {

    protected void VRAction(VRSelection iSelection)
    {
        print(name + ": VRAction.");
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    protected void OnMVRWandEnter(VRSelection iSelection)
    {
        print(name + ": OnMVRWandEnter.");
   }

    protected void OnMVRWandHover(VRSelection iSelection)
    {
        //print(name + ": OnMVRWandHover.");
    }

    protected void OnMVRWandExit(VRSelection iSelection)
    {
        print(name + ": OnMVRWandExit.");
    }

    protected void OnMVRWandButtonPressed(VRSelection iSelection)
    {
        print(name + ": OnMVRWandButtonPressed.");
    }

    protected void OnMVRWandButtonReleased(VRSelection iSelection)
    {
        print(name + ": OnMVRWandButtonReleased.");
    }

    protected void OnMVRTouchBegin(VRTouch iTouch)
    {
        print(name + ": OnMVRTouchBegin by, touch object=" + iTouch.TouchObject);
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    protected void OnMVRTouchMoved(VRTouch iTouch)
    {
        print(name + ": OnMVRTouchMoved, touch object=" + iTouch.TouchObject);
    }

    protected void OnMVRTouchEnd(VRTouch iTouch)
    {
        print(name + ": OnMVRTouchBegin, touch object=" + iTouch.TouchObject);
        this.GetComponent<Renderer>().material.color = Color.white;
    }
}
