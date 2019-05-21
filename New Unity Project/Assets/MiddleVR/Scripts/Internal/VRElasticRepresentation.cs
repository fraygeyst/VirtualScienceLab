/* VRElasticRepresentation
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRElasticRepresentation : MonoBehaviour {

    public GameObject ElasticRoot;
    public GameObject Elastic;

    protected void Start ()
    {
        if( ElasticRoot==null || Elastic==null )
        {
            MiddleVR.VRLog( 2, "[X] VRElasticRepresentation error: bad ElasticRoot or Elastic GameObject reference" );
        }
    }

    void SetElasticLength( float iLength )
    {
        float length = iLength/2.0f;

        Vector3 localScale = Elastic.transform.localScale;
        localScale.y = length;
        Elastic.transform.localScale = localScale;

        Vector3 localPosition = Elastic.transform.localPosition;
        localPosition.z = length;
        Elastic.transform.localPosition = localPosition;
    }

    void SetElasticStartPoint( Vector3 iPosition )
    {
        ElasticRoot.transform.position = iPosition;
    }

    // Need to be called when elastic start point has already been defined
    void SetElasticEndPoint( Vector3 iPosition )
    {
        float elasticLength = (iPosition-ElasticRoot.transform.position).magnitude;
        SetElasticLength( elasticLength );

        Quaternion rotation = Quaternion.FromToRotation( Vector3.forward, (iPosition-ElasticRoot.transform.position).normalized );
        ElasticRoot.transform.rotation = rotation;
    }

    // Only method to be public to simplify usage and force call order
    public void SetElasticPoints( Vector3 iStartPoint, Vector3 iEndPoint )
    {
        SetElasticStartPoint( iStartPoint );
        SetElasticEndPoint( iEndPoint );
    }
}
