/* VRPhysicsBody
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Physics/Body")]
[HelpURL("http://www.middlevr.com/doc/current/#haption-haptics-rigid-body")]
[RequireComponent(typeof(VRClusterObject))]
public class VRPhysicsBody : MonoBehaviour
{
    #region Member Variables

    [SerializeField]
    private bool m_Static = false;
    [SerializeField]
    private float m_Mass = 0.0f;
    [SerializeField]
    private double m_Margin = 0.0;
    [SerializeField]
    private double m_RotationDamping = 0.0;
    [SerializeField]
    private double m_TranslationDamping = 0.0;
    [SerializeField]
    private bool m_MergeChildGeometries = false;

    private vrPhysicsGeometry m_Geometry = null;
    private vrPhysicsBody m_PhysicsBody = null;
    private string m_PhysicsBodyName = "";

    private vrEventListener m_MVREventListener = null;

    #endregion

    #region Member Properties

    public float Mass
    {
        get
        {
            return m_Mass;
        }

        set
        {
            m_Mass = value;
        }
    }

    public vrPhysicsBody PhysicsBody
    {
        get
        {
            return m_PhysicsBody;
        }
    }

    public string PhysicsBodyName
    {
        get
        {
            return m_PhysicsBodyName;
        }
    }

    #endregion

    #region MonoBehaviour Member Functions

    protected void Start()
    {
        if (MiddleVR.VRClusterMgr.IsCluster() && !MiddleVR.VRClusterMgr.IsServer())
        {
            enabled = false;
            return;
        }

        if (MiddleVR.VRPhysicsMgr == null)
        {
            MiddleVRTools.Log(0, "[X] PhysicsBody: No PhysicsManager found.");
            enabled = false;
            return;
        }

        vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();

        if (physicsEngine == null)
        {
            MiddleVRTools.Log(0, "[X] PhysicsBody: Failed to access a physics engine for body '" + name + "'.");
            enabled = false;
            return;
        }

        if (m_PhysicsBody == null)
        {
            m_PhysicsBody = physicsEngine.CreateBodyWithUniqueName(name);

            if (m_PhysicsBody == null)
            {
                MiddleVRTools.Log(0, "[X] PhysicsBody: Failed to create a physics body for '" + name + "'.");
                enabled = false;
                return;
            }
            else
            {
                GC.SuppressFinalize(m_PhysicsBody);

                m_MVREventListener = new vrEventListener(OnMVRNodeDestroy);
                m_PhysicsBody.AddEventListener(m_MVREventListener);

                var nodesMapper = MVRNodesMapper.Instance;

                nodesMapper.AddMapping(
                    gameObject, m_PhysicsBody,
                    MVRNodesMapper.ENodesSyncDirection.MiddleVRToUnity,
                    MVRNodesMapper.ENodesInitValueOrigin.FromUnity);

                m_PhysicsBodyName = m_PhysicsBody.GetName();

                m_Geometry = CreateGeometry(m_MergeChildGeometries);
                if (m_Geometry != null)
                {
                    GC.SuppressFinalize(m_Geometry);
                }

                m_PhysicsBody.SetGeometry(m_Geometry);

                m_PhysicsBody.SetStatic(m_Static);
                m_PhysicsBody.SetMass(m_Mass);
                m_PhysicsBody.SetRotationDamping(m_RotationDamping);
                m_PhysicsBody.SetTranslationDamping(m_TranslationDamping);

                m_PhysicsBody.SetMargin(m_Margin);

                if (physicsEngine.AddBody(m_PhysicsBody))
                {
                    MiddleVRTools.Log(3, "[ ] PhysicsBody: The physics body '" + m_PhysicsBodyName +
                        "' was added to the physics simulation.");
                }
                else
                {
                    MiddleVRTools.Log(0, "[X] PhysicsBody: Failed to add the body '" +
                         m_PhysicsBodyName + "' to the physics simulation.");
                }
            }
        }
    }

    protected void Update()
    {
        if (m_Geometry != null && m_PhysicsBody.IsInSimulation())
        {
            // The geometry was used for creation so it can be deleted.

            // Clear now content to avoid a delayed memory deallocation.
            m_Geometry.Clear();
            m_Geometry.Dispose();

            m_Geometry = null;
        }
    }

    protected void OnDestroy()
    {
        if (m_PhysicsBody != null)
        {
            if (MVRNodesMapper.HasInstance())
            {
                var nodesMapper = MVRNodesMapper.Instance;
                nodesMapper.RemoveMapping(gameObject);
            }

            if (MiddleVR.VRPhysicsMgr != null)
            {
                vrPhysicsEngine physicsEngine = MiddleVR.VRPhysicsMgr.GetPhysicsEngine();
                if (physicsEngine != null)
                {
                    physicsEngine.DestroyBody(m_PhysicsBodyName);
                }
            }

            m_PhysicsBody.Dispose();

            m_PhysicsBody = null;
        }

        m_PhysicsBodyName = "";

        if (m_MVREventListener != null)
        {
            m_MVREventListener.Dispose();
        }
    }

    // The following method is for debug purpose. It displays one Gizmo sphere
    // per vertex of the current geometry. Rendering in Unity editor will become
    // very slow if many vertices need to be drawn.
    // If you need to display the content of the current vrPhysicsGeometry,
    // then you first have to avoid destruction of the 'm_Geometry' variable:
    // modify the Update() method accordingly (for example, just comment code
    // of the destruction -- you will get a memory leak but it is probably
    // fine to accept it for a debugging session).
    /*
    protected void OnDrawGizmosSelected()
    {
        if (m_Geometry != null)
        {
            var topMatrix = Matrix4x4.TRS(
                transform.position,
                transform.rotation,
                Vector3.one);

            var g = m_Geometry;

            for (uint i = 0, iEnd = m_Geometry.GetSubGeometriesNb(); i < iEnd; ++i)
            {
                if (i == 0)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.blue;
                }

                var verticesNb = g.GetVerticesNb(i);
                var vertices = new double[3 * verticesNb];
                g.GetVertices(vertices, i);

                for (uint j = 0; j < verticesNb; j++)
                {
                    uint start = j * 3;
                    // Change coordinate space from MVR to Unity.
                    var p = new Vector3(
                        (float)vertices[start + 0],
                        (float)vertices[start + 2],
                        (float)vertices[start + 1]);

                    var ret = topMatrix.MultiplyPoint3x4(p);
                    // Spheres may need to be resized here.
                    Gizmos.DrawSphere(ret, 0.005f);
                }
            }
        }
    }
    */

    #endregion

    #region VRPhysicsBody Functions

    private bool OnMVRNodeDestroy(vrEvent iBaseEvt)
    {
        vrObjectEvent e = vrObjectEvent.Cast(iBaseEvt);
        if (e != null)
        {
            if (e.ComesFrom(m_PhysicsBody))
            {
                if (e.eventType == (int)VRObjectEventEnum.VRObjectEvent_Destroy)
                {
                    // Killed in MiddleVR so delete it in C#.
                    m_PhysicsBody.Dispose();
                }
            }
        }

        return true;
    }

    private vrPhysicsGeometry CreateGeometry(bool iAreChildGeometriesMerged)
    {
        string geometryName = m_PhysicsBodyName + ".Geometry";

        MiddleVRTools.Log(4,
            "[>] PhysicsBody: Creation of the physics geometry '" +
            geometryName + "'.");

        var rootMatrixWorld = transform.localToWorldMatrix;

        vrPhysicsGeometry ret = new vrPhysicsGeometry(geometryName);

        uint geometryIndex = 0;

        var queue = new Queue<GameObject>();
        queue.Enqueue(gameObject);

        while (queue.Count > 0)
        {
            var go = queue.Dequeue();

            Mesh mesh = null;

            var meshCollider = go.GetComponent<MeshCollider>();

            if (meshCollider != null && meshCollider.enabled)
            {
                mesh = meshCollider.sharedMesh;

                if (mesh != null)
                {
                    MiddleVRTools.Log(2,
                        "[ ] PhysicsBody: the GO '" + go.name +
                        "' will furnish a physics geometry for '" + geometryName +
                        "' from its MeshCollider.");
                }
            }

            // No mesh from collider was found so let's try from the mesh filter.
            if (mesh == null)
            {
                var meshFilter = go.GetComponent<MeshFilter>();

                if (meshFilter != null)
                {
                    mesh = meshFilter.sharedMesh;

                    if (mesh != null)
                    {
                        MiddleVRTools.Log(2,
                            "[ ] PhysicsBody: the GO '" + go.name +
                            "' will furnish a physics geometry for '" + geometryName +
                            "' from its MeshFilter.");
                    }
                }
            }

            if (mesh != null)
            {
                // The top geometry keep its name, others will get the word "sub".
                if (geometryIndex != 0)
                {
                    ret.SetSubGeometryName(geometryIndex, geometryName + ".sub." + go.name);
                }

                ConvertGeometry(rootMatrixWorld, go.transform, mesh, ret, geometryIndex);

                ++geometryIndex;

                MiddleVRTools.Log(4,
                    "[ ] PhysicsBody: Physics geometry created from GO '" +
                    go.name + "'.");
            }

            if (iAreChildGeometriesMerged)
            {
                foreach (Transform child in go.transform)
                {
                    if (child.gameObject.activeSelf)
                    {
                        queue.Enqueue(child.gameObject);
                    }
                }
            }
        }

        var scaleShearMatrix = MVRTools.ComputeScaleShearMatrixWorld(transform);
        ret.TransformStoredVertices(MVRTools.FromUnity(scaleShearMatrix));

        MiddleVRTools.Log(4,
            "[<] PhysicsBody: Creation of the physics geometry '" +
            geometryName + "' ended.");

        return ret;
    }

    private void ConvertGeometry(Matrix4x4 iTopMatrixWorld, Transform iTransform, Mesh iMesh, vrPhysicsGeometry ioGeometry, uint iGeometryIndex)
    {
        var vertices = iMesh.vertices;
        var triangles = iMesh.triangles;

        MiddleVRTools.Log(3,
            "[ ] PhysicsBody: Number of vertices in sub-geometry '" +
            iGeometryIndex + "': " + vertices.Length);
        MiddleVRTools.Log(3,
            "[ ] PhysicsBody: Number of triangles in sub-geometry '" +
            iGeometryIndex + "': " + triangles.Length);

        var m = iTopMatrixWorld.inverse * iTransform.localToWorldMatrix;

        // We will reuse the same vector to avoid many memory allocations.
        vrVec3 vPos = new vrVec3();

        MiddleVRTools.Log(6, "[>] PhysicsBody: Adding vertices.");

        foreach (Vector3 vertex in vertices)
        {
            var vertexPos = m.MultiplyPoint3x4(vertex);
            MiddleVRTools.FromUnity(vertexPos, ref vPos);
            ioGeometry.AddVertex(vPos, iGeometryIndex);

            MiddleVRTools.Log(6,
                "[ ] PhysicsBody: Adding a vertex at position (" +
                vPos.x() + ", " + vPos.y() + ", " + vPos.z() +
                ") to sub-geometry '" + iGeometryIndex + "'.");
        }

        MiddleVRTools.Log(6, "[<] PhysicsBody: End of adding vertices.");
        MiddleVRTools.Log(6, "[>] PhysicsBody: Adding triangles.");

        for (int i = 0, iEnd = triangles.Length; i < iEnd; i += 3)
        {
            var index0 = (uint)triangles[i + 0];
            var index1 = (uint)triangles[i + 1];
            var index2 = (uint)triangles[i + 2];

            ioGeometry.AddTriangle(index0, index1, index2, iGeometryIndex);

            MiddleVRTools.Log(6,
                "[ ] PhysicsBody: Adding a triangle with vertex indexes (" +
                index0 + ", " + index1 + ", " + index2 + ") to sub-geometry '" +
                iGeometryIndex + "'.");
        }

        MiddleVRTools.Log(6, "[<] PhysicsBody: End of adding triangles.");
    }

    #endregion
}
