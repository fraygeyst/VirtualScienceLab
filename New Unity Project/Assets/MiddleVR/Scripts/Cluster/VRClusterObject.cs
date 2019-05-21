/* VRClusterObject
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Cluster/Cluster Object")]
public class VRClusterObject : MonoBehaviour {
    public bool IncludeChildren = true;

    protected void OnEnable()
    {
        // Not very clear whether OnEnable will be always called before Start.
        AddClusterScripts(gameObject, IncludeChildren);
        EnableClusterScripts(gameObject, IncludeChildren);
    }

    protected void OnDisable()
    {
        DisableClusterScripts(gameObject, IncludeChildren);
    }

    protected void Start()
    {
        // Not very clear whether OnEnable will be always called before Start.
        AddClusterScripts(gameObject, IncludeChildren);
        EnableClusterScripts(gameObject, IncludeChildren);
    }

    /**
     * @brief Add scripts that are needed by the synchronization with clusters.
     *
     * This method must be called before EnableClusterScripts.
     */
    private void AddClusterScripts(GameObject iObject, bool iChildren)
    {
        //MVRTools.Log("AddCluster to " + iObject);
        //print("AddCluster to " + iObject);

        if (iObject.GetComponent<VRShareTransform>() == null)
        {
            iObject.AddComponent<VRShareTransform>();
        }

        if( iChildren == true )
        {
            foreach (Transform child in iObject.transform)
            {
                GameObject childObject = child.gameObject;

                //print("Child : " + childObject);
                AddClusterScripts(childObject, true);
            }
        }
    }

    private void EnableClusterScripts(GameObject iObject, bool iChildren)
    {
        EnableOrDisableClusterScripts(true, iObject, iChildren);
    }

    private void DisableClusterScripts(GameObject iObject, bool iChildren)
    {
        EnableOrDisableClusterScripts(false, iObject, iChildren);
    }

    private void EnableOrDisableClusterScripts(bool iEnabling, GameObject iObject, bool iChildren)
    {
        string enableOpStr = ( iEnabling ? "[ ] Enabling" : "[ ] Disabling" );
        MVRTools.Log(enableOpStr + " cluster on " + iObject);

        VRShareTransform shareTransformObj = iObject.GetComponent<VRShareTransform>();
        if( shareTransformObj != null )
        {
            shareTransformObj.enabled = iEnabling;
            MVRTools.Log(enableOpStr + " cluster on " + iObject + " with VRShareTransform.");
        }

        if( iChildren == true )
        {
            foreach (Transform child in iObject.transform)
            {
                GameObject childObject = child.gameObject;

                EnableOrDisableClusterScripts(iEnabling, childObject, true);
            }
        }
    }
}
