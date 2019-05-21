/* VRScreenWarningAnimation
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRScreenWarningAnimation : MonoBehaviour {

    private GameObject m_NodeToWatch;
    private float      m_NearDistance = 0.01f;

    protected void Update()
    {
        if (m_NodeToWatch == null)
        {
            return;
        }

        var rendererMaterial = GetComponent<Renderer>().material;

        var displayMgr = vrDisplayManager.GetInstance();
        bool useWorldScale = displayMgr.GetChangeWorldScale();
        float worldScale = useWorldScale ? displayMgr.GetWorldScale() : 1.0f;

        // Set near distance
        rendererMaterial.SetFloat("_NearDistance", m_NearDistance * worldScale);

        // Halo position
        Vector3 nodePosition = m_NodeToWatch.transform.position;
        rendererMaterial.SetVector("_HeadPosition", new Vector4(nodePosition.x, nodePosition.y, nodePosition.z, 1.0f));

        // Make texture slide
        rendererMaterial.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.08f * Time.time % 1.0f));

        // Make texture blink
        float bright = Mathf.Clamp( 1.5f-(Time.time%1.0f), 0.0f, 1.0f );
        rendererMaterial.SetFloat("_Brightness", bright);
    }

    public void SetNodeToWatch(GameObject iNode)
    {
        m_NodeToWatch = iNode;
    }

    public void SetNearDistance(float iNearDistance)
    {
        m_NearDistance = iNearDistance;
    }
}
