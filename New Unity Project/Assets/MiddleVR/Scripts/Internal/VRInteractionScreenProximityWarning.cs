/* VRInteractionScreenProximityWarning
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiddleVR_Unity3D;

[AddComponentMenu("")]
public class VRInteractionScreenProximityWarning : MonoBehaviour {

    public string Name           = "InteractionScreenProximityWarning";
    public float WarningDistance = 0.4f;
    public List<string> NodesToWatch;
    public GameObject WarningRepresentationPrefab;

    private bool                                m_Initialized  = false;
    private float                               m_TextureScale = 2.5f;
    private Dictionary<string, vrNode3D>        m_NodesToWatch = new Dictionary<string, vrNode3D>();
    private Dictionary<string, GameObject>      m_WarningRepresentationObjects = new Dictionary<string, GameObject>();
    private vrInteractionScreenProximityWarning m_it = null;

    protected void Start()
    {
        if( NodesToWatch.Count <= 0 )
        {
            return;
        }

        m_it = new vrInteractionScreenProximityWarning(Name);

        MiddleVR.VRInteractionMgr.AddInteraction(m_it);
        MiddleVR.VRInteractionMgr.Activate(m_it);

        // Retrieve and start watching nodes to watch
        foreach( string nodeName in NodesToWatch )
        {
            m_NodesToWatch[nodeName] = MiddleVR.VRDisplayMgr.GetNode(nodeName);
            if( m_NodesToWatch[nodeName] != null )
            {
                m_it.StartWatchingNode( m_NodesToWatch[nodeName] );
            }
        }

        m_it.SetNearDistance(WarningDistance);

        m_Initialized = true;
    }

    protected void Update()
    {
        if( !m_Initialized )
        {
            return;
        }
        
        HideOldWarnings();
        ShowNewWarnings();
        m_it.ClearContactsEvents();
    }

    private void CreateWarningRepresentation( vrNode3D iNode, vrScreen iScreen )
    {
        // Generate name and check if warning doesn't exist
        string warningName = iNode.GetName() + "_" + iScreen.GetName() + "_ProxiWarning";
        if( m_WarningRepresentationObjects.ContainsKey(warningName) )
        {
            return;
        }

        // Retrieve the GameObjects
        GameObject nodeGameObject   = GameObject.Find(iNode.GetName());
        GameObject screenGameObject = GameObject.Find(iScreen.GetName());

        // Create and initialize the warning
        GameObject warningRepresentation = GameObject.Instantiate(WarningRepresentationPrefab) as GameObject;
        warningRepresentation.name                    = warningName;
        warningRepresentation.transform.parent        = screenGameObject.transform;
        warningRepresentation.transform.localPosition = Vector3.zero;
        warningRepresentation.transform.localRotation = Quaternion.identity;
        warningRepresentation.transform.localScale    = new Vector3(iScreen.GetWidth(), iScreen.GetHeight(), 1.0f);

        GameObject warningMesh = warningRepresentation.transform.GetChild(0).gameObject;
        Material mat = Material.Instantiate(warningMesh.GetComponent<Renderer>().material) as Material;
        warningMesh.GetComponent<Renderer>().material = mat;
        warningMesh.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(m_TextureScale * 0.8f * iScreen.GetWidth(), m_TextureScale * iScreen.GetHeight()));

        VRScreenWarningAnimation warningScript = warningMesh.GetComponent<VRScreenWarningAnimation>();
        warningScript.SetNodeToWatch(nodeGameObject);
        warningScript.SetNearDistance(WarningDistance);

        // Add the warning to list
        m_WarningRepresentationObjects[warningRepresentation.name] = warningRepresentation;
    }

    private void DeleteWarningRepresentation(vrNode3D iNode, vrScreen iScreen)
    {
        // Generate name and check if warning exists
        string warningName = iNode.GetName() + "_" + iScreen.GetName() + "_ProxiWarning";
        if (!m_WarningRepresentationObjects.ContainsKey(warningName))
        {
            return;
        }

        // Remove from list
        m_WarningRepresentationObjects.Remove(warningName);

        // Destroy warning object
        GameObject warningObject = GameObject.Find(warningName);
        warningObject.SetActive(false);
        GameObject.Destroy(warningObject);
    }

    private void ShowNewWarnings()
    {
        uint newContactsNB = m_it.GetAddedContactsNb();

        for( uint i = 0 ; i < newContactsNB ; ++i )
        {
            vrNode3D contactNode   = m_it.GetAddedContactNode(i);
            vrScreen contactScreen = m_it.GetAddedContactScreen(i);

            // Show corresponding warning representation
            CreateWarningRepresentation(contactNode, contactScreen);
        }
    }

    private void HideOldWarnings()
    {
        uint oldContactsNB = m_it.GetRemovedContactsNb();

        for( uint i = 0 ; i < oldContactsNB ; ++i )
        {
            vrNode3D contactNode   = m_it.GetRemovedContactNode(i);
            vrScreen contactScreen = m_it.GetRemovedContactScreen(i);

            // Hide corresponding warning representation
            DeleteWarningRepresentation(contactNode, contactScreen);
        }
    }

    protected void OnEnable()
    {
        if (m_it != null)
        {
            MiddleVR.VRInteractionMgr.Activate(m_it);
        }
    }

    protected void OnDisable()
    {
        if (MiddleVR.VRKernel != null && m_it != null)
        {
            MiddleVR.VRInteractionMgr.Deactivate(m_it);
        }

        // Delete all representations
        foreach (KeyValuePair<string, GameObject> warningRepresentation in m_WarningRepresentationObjects)
        {
            GameObject.Destroy(warningRepresentation.Value);
        }
        m_WarningRepresentationObjects.Clear();
    }

    protected void OnApplicationQuit()
    {
        if( m_it != null )
        {
            m_it = null;
        }
    }
}
