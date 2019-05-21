/* VRPhysicsShowContactPoints
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Show Contact Points")]
[HelpURL("http://www.middlevr.com/doc/current/#haption-haptics-visualizing-collisions")]
public class VRPhysicsShowContacts : MonoBehaviour {

    #region Member Variables

    [SerializeField]
    private GameObject m_ObjectAtContact = null;

    [SerializeField]
    private int m_MaxContactsNb = 15;

    [SerializeField]
    private Vector3 m_Translation;

    [SerializeField]
    private Vector3 m_Rotation;

    [SerializeField]
    private bool m_RayDebug = false;

    private GameObject[] m_ContactsToShow = new GameObject[0];

    private Quaternion m_RotationAsQuat;

    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        if (MiddleVR.VRClusterMgr.IsCluster() && !MiddleVR.VRClusterMgr.IsServer())
        {
            enabled = false;
            return;
        }

        m_ContactsToShow = new GameObject[m_MaxContactsNb];

        for (int i = 0; i < m_MaxContactsNb; i++)
        {
            GameObject go = null;

            if (m_ObjectAtContact != null)
            {
                go = (GameObject)GameObject.Instantiate(m_ObjectAtContact);
                go.transform.parent = transform;
            }

            m_ContactsToShow[i] = go;
        }

        m_RotationAsQuat.eulerAngles = m_Rotation;
    }

    protected void Update()
    {
        if (MiddleVR.VRPhysicsMgr == null)
        {
            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            return;
        }

        for (int i = 0; i < m_MaxContactsNb; i++)
        {
            GameObject go = m_ContactsToShow[i];

            if (go != null)
            {
                go.SetActive(false);
            }
        }

        // We will reuse the same vectors to avoid many memory allocations.
        Vector3 contactPosition = new Vector3();
        Vector3 contactNormal = new Vector3();

        for (uint i = 0, iEnd = physicsEngine.GetContactInfosNb(); i < iEnd && i < m_MaxContactsNb; i++)
        {
            vrPhysicsContactInfo contactInfo = physicsEngine.GetContactInfo(i);

            MiddleVRTools.ToUnity(contactInfo.GetPositionOnBody0(), ref contactPosition);
            MiddleVRTools.ToUnity(contactInfo.GetNormalOnBody0(), ref contactNormal);

            Quaternion contactQ = Quaternion.FromToRotation(Vector3.up, contactNormal);

            Vector3 p = m_Translation + contactPosition;

            GameObject go = m_ContactsToShow[i];

            if (go != null)
            {
                go.transform.position = p;
                go.transform.rotation = m_RotationAsQuat * contactQ;

                go.SetActive(true);
            }

            if (m_RayDebug)
            {
                Debug.DrawRay(p, m_RotationAsQuat * contactNormal, Color.green);
            }
        }
    }

    #endregion
}
