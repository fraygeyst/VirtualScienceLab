/* VRApplySharedTransform
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;

using MiddleVR_Unity3D;

// THIS SCRIPT IS DEPRECATED
// Starting from MiddleVR 1.6.0 you only have to use the VRShareTransform script

[AddComponentMenu("")] // Hide it.
public class VRApplySharedTransform : MonoBehaviour {

    protected void Start()
    {
        MVRTools.Log("[X] Using deprecated script VRApplySharedTransform");
    }
}
