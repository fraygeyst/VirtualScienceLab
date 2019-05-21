/* VRWebView
 * MiddleVR
 * (c) MiddleVR
 */

// Rendering plugin test
// ---------------------
//
// VRWebView can be used without MiddleVR_UnityRendering.dll but it will
// be much slower.
// - Unity 4 can only use rendering plugins with a professionnal license.
//   However, the UNITY_PRO_LICENSE preprocessor condition only exists
//   starting from Unity 4.5 so for Unity 4.2 and 4.3 we always assume
//   it's a Professional edition.
// - Starting with Unity 5 plugins are not limited anymore.
#if (UNITY_4_5 || UNITY_4_6) && !UNITY_PRO_LICENSE
#define VRWEBVIEW_UNITY_FREE
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
public class VRWebViewDirectoryProvider
{
    public VRWebViewDirectoryProvider()
    {
        m_URL = "";
        m_DirectoryPath = "";
    }

    public VRWebViewDirectoryProvider(string iURL, string iDirectoryPath)
    {
        m_URL = iURL;
        m_DirectoryPath = iDirectoryPath;
    }

    public string m_URL;
    public string m_DirectoryPath;
}

[Serializable]
public class VRWebViewArchiveProvider
{
    public VRWebViewArchiveProvider()
    {
        m_URL = "";
        m_ArchivePath = "";
        m_Password = "";
    }

    public VRWebViewArchiveProvider(string iURL, string iArchivePath, string iPassword)
    {
        m_URL = iURL;
        m_ArchivePath = iArchivePath;
        m_Password = iPassword;
    }

    public string m_URL;
    public string m_ArchivePath;
    public string m_Password;
}

[AddComponentMenu("MiddleVR/GUI/Web View")]
public class VRWebView : MonoBehaviour
{
    public int Width
    {
        get { return m_Width; }
        set { m_Width = value; }
    }

    public int Height
    {
        get { return m_Height; }
        set { m_Height = value; }
    }

    public string URL
    {
        get { return m_URL; }
        set { m_URL = value; }
    }

    public float Zoom
    {
        get { return m_Zoom; }
        set { m_Zoom = value; }
    }

    // If true and there is no collider on the object,
    // automatically create a trigger box collider.
    // This is useful since Unity 5.0 because it does not support
    // non-convex trigger mesh colliders anymore.
    public bool AutomaticTriggerCollider
    {
        get { return m_AutomaticTriggerCollider; }
        set { m_AutomaticTriggerCollider = value; }
    }

    public bool MouseInteractionsIn2D
    {
        get { return m_MouseInteractionsIn2D; }
        set { m_MouseInteractionsIn2D = value; }
    }

    public bool MouseInteractionsIn3D
    {
        get { return m_MouseInteractionsIn3D; }
        set { m_MouseInteractionsIn3D = value; }
    }

    public bool UseTouchEvents
    {
        get { return m_UseTouchEvents; }
        set { m_UseTouchEvents = value; }
    }

    public VRWebViewDirectoryProvider[] DirectoryProviders
    {
        set { m_DirectoryProviders = value; }
    }

    public VRWebViewArchiveProvider[] ArchiveProviders
    {
        set { m_ArchiveProviders = value; }
    }

    public vrImage image
    {
        get { return m_Image; }
    }

    public vrWebView webView
    {
        get { return m_WebView; }
    }

    [SerializeField]
    private int m_Width = 1024;

    [SerializeField]
    private int m_Height = 768;

    [SerializeField]
    private string m_URL = "http://www.middlevr.com/";

    [SerializeField]
    private float m_Zoom = 1.0f;

    [SerializeField]
    private bool m_AutomaticTriggerCollider = true;

    [SerializeField]
    private bool m_MouseInteractionsIn2D = true;

    [SerializeField]
    private bool m_MouseInteractionsIn3D = false;

    [SerializeField]
    private bool m_UseTouchEvents = false;

    [SerializeField]
    public VRWebViewDirectoryProvider[] m_DirectoryProviders = new VRWebViewDirectoryProvider[]
    {
        new VRWebViewDirectoryProvider("http://internal/WebAssets/","WebAssets"),
        new VRWebViewDirectoryProvider("http://internal/MiddleVR/WebAssets/","MiddleVR/WebAssets")
    };

    [SerializeField]
    public VRWebViewArchiveProvider[] m_ArchiveProviders = new VRWebViewArchiveProvider[] { };

    // Internal Web View management
    private vrWebView m_WebView = null;
    private vrImage m_Image     = null;
    private Texture2D m_Texture = null;

    // Wand interaction
    private bool m_WandRayWasVisible = true;
    private static byte ALPHA_LIMIT = 50;

    // Virtual Mouse (Used by Wand and Mouse interactions)
    private Vector2 m_VirtualMousePosition = new Vector2(0.0f, 0.0f);
    private bool m_IsVirtualMouseButtonPressed = false;
    private const int SIMULATED_TOUCH_ID = 0;

    // Mouse interactions in 3D
    private List<Camera> m_Cameras = null;
    private bool m_IgnorePhysicalMouseInput = false;

    // Automatic trigger collider
    private Vector3[] m_RaycastMeshVertices = null;
    private Vector2[] m_RaycastMeshUV = null;
    private int[] m_RaycastMeshTriangles = null;

#if VRWEBVIEW_UNITY_FREE
    // Unity Free texture management
    private Color32[] m_Pixels;
    private GCHandle m_PixelsHandle;
#else
    private IntPtr m_NativeTexturePtr = IntPtr.Zero;
#endif

    protected void SetVirtualMouseButtonPressed()
    {
        m_IsVirtualMouseButtonPressed = true;

        if (m_WebView != null)
        {
            if(m_UseTouchEvents)
            {
                m_WebView.SendTouchEvent(VRWebViewTouchTypeEnum.VRWebViewTouchType_Started, SIMULATED_TOUCH_ID, m_VirtualMousePosition.x, m_VirtualMousePosition.y);
            }
            else
            {
                m_WebView.SendMouseButtonPressed((int)m_VirtualMousePosition.x, (int)m_VirtualMousePosition.y);
            }
        }
    }

    protected void SetVirtualMouseButtonReleased()
    {
        m_IsVirtualMouseButtonPressed = false;

        if (m_WebView != null)
        {
            if (m_UseTouchEvents)
            {
                m_WebView.SendTouchEvent(VRWebViewTouchTypeEnum.VRWebViewTouchType_Ended, SIMULATED_TOUCH_ID, m_VirtualMousePosition.x, m_VirtualMousePosition.y);
            }
            else
            {
                m_WebView.SendMouseButtonReleased((int)m_VirtualMousePosition.x, (int)m_VirtualMousePosition.y);
            }
        }
    }

    // pos: texture coordinate of raycast hit
    protected void SetVirtualMousePosition(Vector2 pos)
    {
        if (m_WebView != null)
        {
            m_VirtualMousePosition = new Vector2(pos.x * m_Texture.width, (float)m_WebView.GetHeight() - (pos.y * m_Texture.height));

            if (m_UseTouchEvents)
            {
                if (m_IsVirtualMouseButtonPressed)
                {
                    m_WebView.SendTouchEvent(VRWebViewTouchTypeEnum.VRWebViewTouchType_Moved, SIMULATED_TOUCH_ID, m_VirtualMousePosition.x, m_VirtualMousePosition.y);
                }
            }
            else
            {
                m_WebView.SendMouseMove((int)m_VirtualMousePosition.x, (int)m_VirtualMousePosition.y);
            }
        }
    }

    public void IgnorePhysicalMouseInput()
    {
        m_IgnorePhysicalMouseInput = true;
    }

    public bool IsPixelEmpty(Vector2 iTextureCoord)
    {
        byte alpha = image.GetAlphaAtPoint((int)(iTextureCoord.x * m_Width), (int)(iTextureCoord.y * m_Height));
        return alpha < ALPHA_LIMIT;
    }

    protected void Awake()
    {
        if( GetComponent<GUITexture>() == null )
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if( meshFilter == null )
            {
                enabled = false;
                Debug.Log("VRWebView must be attached to a GameObject with a MeshFilter component.");
                return;
            }

            Mesh mesh = meshFilter.sharedMesh;
            if( mesh == null )
            {
                enabled = false;
                Debug.Log("VRWebView must be attached to a GameObject with a valid Mesh.");
                return;
            }

            m_RaycastMeshVertices = mesh.vertices;
            m_RaycastMeshUV = mesh.uv;
            m_RaycastMeshTriangles = mesh.triangles;

            if( m_AutomaticTriggerCollider )
            {
                Collider collider = GetComponent<Collider>();

                if (collider == null)
                {
                    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                    _UpdateBoxCollider(boxCollider);
                }
            }
        }
    }

    protected void Start ()
    {
        // Check if we are running MiddleVR
        if(MiddleVR.VRKernel == null)
        {
            Debug.Log("[X] VRManager is missing from the scene !");
            enabled = false;
            return;
        }

        if (SystemInfo.graphicsDeviceID == 0)
        {
            Debug.Log("[~] Running in batchmode - disabling VRWebView script");
            enabled = false;
            return;
        }

        m_VirtualMousePosition = new Vector2(0, 0);

        if (Application.isEditor)
        {
            // Get the vrCameras corresponding Cameras
            m_Cameras = new List<Camera>();

            uint camerasNb = MiddleVR.VRDisplayMgr.GetCamerasNb();
            for (uint i = 0; i < camerasNb; ++i)
            {
                vrCamera vrcamera = MiddleVR.VRDisplayMgr.GetCameraByIndex(i);
                GameObject cameraObj = GameObject.Find(vrcamera.GetName());
                Camera camera = cameraObj.GetComponent<Camera>();
                if (camera != null)
                {
                    m_Cameras.Add(camera);
                }
            }
        }

        m_Texture = new Texture2D(m_Width, m_Height, TextureFormat.ARGB32, false);
        m_Texture.wrapMode = TextureWrapMode.Clamp;

        // Create vrImage and Texture2D
#if VRWEBVIEW_UNITY_FREE
        // Texture2D.SetPixels takes RGBA.
        m_Image = new vrImage("", (uint)m_Width, (uint)m_Height, VRImagePixelFormat.VRImagePixelFormat_RGBA);
        m_Pixels = m_Texture.GetPixels32 (0);
        m_PixelsHandle = GCHandle.Alloc(m_Pixels, GCHandleType.Pinned);
#else
        // OpenGL and Direct3D 9: Memory order for texture upload is BGRA (little-endian representation of ARGB32)
        // Direct3D 11: Unity seems to ignore TextureFormat.ARGB32 and always creates an RGBA texture.
        // We let vrImage do the pixel format conversion because this operation is done in another thread.
        if (SystemInfo.graphicsDeviceVersion.Contains("Direct3D 11"))
        {
            m_Image = new vrImage("", (uint)m_Width, (uint)m_Height, VRImagePixelFormat.VRImagePixelFormat_RGBA);
        }
        else
        {
            m_Image = new vrImage("", (uint)m_Width, (uint)m_Height, VRImagePixelFormat.VRImagePixelFormat_BGRA);
        }
#endif

        // Fill texture
        Color32[] colors = new Color32[(m_Width * m_Height)];
        
        for (int i = 0; i < (m_Width * m_Height); i++)
        {
            colors[i].r = 0;
            colors[i].g = 0;
            colors[i].b = 0;
            colors[i].a = 0;
        }
        
        m_Texture.SetPixels32(colors);
        m_Texture.Apply(false, false);

#if !VRWEBVIEW_UNITY_FREE
        m_NativeTexturePtr = m_Texture.GetNativeTexturePtr();
#endif

        // Attach texture
        if (gameObject != null && gameObject.GetComponent<GUITexture>() == null && gameObject.GetComponent<Renderer>() != null)
        {
            var renderer = gameObject.GetComponent<Renderer>();

            // Assign the material/shader to the object attached
            renderer.material = Resources.Load("VRWebViewMaterial", typeof(Material)) as Material;
            renderer.material.mainTexture = this.m_Texture;
        }
        else if (gameObject != null && gameObject.GetComponent<GUITexture>() != null )
        {
            gameObject.GetComponent<GUITexture>().texture = this.m_Texture;
        }
        else
        {
            MiddleVR.VRLog(2, "VRWebView must be attached to a GameObject with a renderer or a GUITexture !");
            enabled = false;
            return;
        }
        
        // Handle Cluster
        if ( MiddleVR.VRClusterMgr.IsServer() && ! MiddleVR.VRKernel.GetEditorMode() )
        {
            MiddleVR.VRClusterMgr.AddSynchronizedObject( m_Image );
        }
        
        if( ! MiddleVR.VRClusterMgr.IsClient() )
        {
            if (m_DirectoryProviders.Length == 0 && m_ArchiveProviders.Length == 0)
            {
                m_WebView = new vrWebView("", GetAbsoluteURL(m_URL), (uint)m_Width, (uint)m_Height, m_Image);
            }
            else
            {
                m_WebView = new vrWebView("", "", (uint)m_Width, (uint)m_Height, m_Image);

                int order = 0;

                foreach(VRWebViewDirectoryProvider directoryProvider in m_DirectoryProviders)
                {
                    m_WebView.AddDirectoryProvider(directoryProvider.m_URL, Application.dataPath + System.IO.Path.DirectorySeparatorChar + directoryProvider.m_DirectoryPath, order, "VRWebViewProvider" + order);
                    order++;
                }

                foreach(VRWebViewArchiveProvider archiveProvider in m_ArchiveProviders)
                {
                    m_WebView.AddArchiveProvider(archiveProvider.m_URL, Application.dataPath + System.IO.Path.DirectorySeparatorChar + archiveProvider.m_ArchivePath, archiveProvider.m_Password, order, "VRWebViewProvider" + order);
                    order++;
                }

                m_WebView.SetURL(GetAbsoluteURL(m_URL));
            }
            m_WebView.SetZoom( m_Zoom );
        }
    }

#if !VRWEBVIEW_UNITY_FREE
    // Hex code for "MVR1"
    const int MVR_RENDEREVENT_COPYBUFFERSTOTEXTURES = 0x4D565231;

    [DllImport("MiddleVR_UnityRendering")]
    private static extern void MiddleVR_AsyncCopyBufferToTexture(IntPtr iBuffer, IntPtr iNativeTexturePtr, uint iWidth, uint iHeight);

    [DllImport("MiddleVR_UnityRendering")]
    private static extern void MiddleVR_CancelCopyBufferToTexture(IntPtr iNativeTexturePtr);

    [DllImport("MiddleVR_UnityRendering")]
    private static extern IntPtr MiddleVR_CopyBuffersToTextures();
#endif

    protected void Update ()
    {
        // Handle mouse input
        if (!MiddleVR.VRClusterMgr.IsClient())
        {
            if (!m_IgnorePhysicalMouseInput)
            {
                Vector2 mouseHit = new Vector2(0, 0);
                bool hasMouseHit = false;

                GUITexture guiTexture = gameObject.GetComponent<GUITexture>();

                if (m_MouseInteractionsIn2D && guiTexture != null)
                {
                    // GUITexture mouse input
                    Rect r = guiTexture.GetScreenRect();
                
                    if( Input.mousePosition.x >= r.x && Input.mousePosition.x < (r.x + r.width) &&
                        Input.mousePosition.y >= r.y && Input.mousePosition.y < (r.y + r.height) )
                    {

                        float x = (Input.mousePosition.x - r.x) / r.width;
                        float y = (Input.mousePosition.y - r.y) / r.height;
                    
                        mouseHit = new Vector2(x, y);
                        hasMouseHit = true;
                    }
                }
                else if(m_MouseInteractionsIn3D && gameObject.GetComponent<Renderer>() != null)
                {
                    // 3D object mouse input
                    mouseHit = GetClosestMouseHit();

                    if (mouseHit.x != -1 && mouseHit.y != -1)
                    {
                        hasMouseHit = true;
                    }
                }

                if(hasMouseHit)
                {
                    bool isMouseButtonPressed = Input.GetMouseButton(0);

                    if(!m_IsVirtualMouseButtonPressed && isMouseButtonPressed)
                    {
                        SetVirtualMousePosition(mouseHit);
                        SetVirtualMouseButtonPressed();
                    }
                    else if(m_IsVirtualMouseButtonPressed && !isMouseButtonPressed)
                    {
                        SetVirtualMouseButtonReleased();
                        SetVirtualMousePosition(mouseHit);
                    }
                    else
                    {
                        SetVirtualMousePosition(mouseHit);
                    }
                }
            }

            m_IgnorePhysicalMouseInput = false;
        }

        // Handle texture update
        if ( m_Image.HasChanged() )
        {
            using (vrImageFormat format = m_Image.GetImageFormat())
            {
                if ((uint)m_Texture.width != format.GetWidth() || (uint)m_Texture.height != format.GetHeight())
                {
#if VRWEBVIEW_UNITY_FREE
                    m_PixelsHandle.Free();
#endif
                    m_Texture.Resize((int)format.GetWidth(), (int)format.GetHeight());
                    m_Texture.Apply(false, false);
#if VRWEBVIEW_UNITY_FREE
                    m_PixelsHandle.Free();
                    m_Pixels = m_Texture.GetPixels32 (0);
                    m_PixelsHandle = GCHandle.Alloc(m_Pixels, GCHandleType.Pinned);
#else
                    MiddleVR_CancelCopyBufferToTexture(m_NativeTexturePtr);
                    m_NativeTexturePtr = m_Texture.GetNativeTexturePtr();
#endif
                }

                if (format.GetWidth() > 0 && format.GetHeight() > 0)
                {
#if VRWEBVIEW_UNITY_FREE
                    m_Image.GetReadBufferData( m_PixelsHandle.AddrOfPinnedObject() );
                    m_Texture.SetPixels32(m_Pixels, 0);
                    m_Texture.Apply(false, false);
#else
                    MiddleVR_AsyncCopyBufferToTexture(m_Image.GetReadBuffer(), m_NativeTexturePtr, format.GetWidth(), format.GetHeight());
                    GL.IssuePluginEvent(MiddleVR_CopyBuffersToTextures(), MVR_RENDEREVENT_COPYBUFFERSTOTEXTURES);
#endif
				}
			}
        }
    }

    private void OnDestroy ()
    {
#if VRWEBVIEW_UNITY_FREE
        m_PixelsHandle.Free();
#else
        MiddleVR_CancelCopyBufferToTexture( m_NativeTexturePtr );
#endif
        MiddleVR.DisposeObject(ref m_Image);
        MiddleVR.DisposeObject(ref m_WebView);
    }

    private Vector2 GetClosestMouseHit()
    {
        foreach( Camera camera in m_Cameras )
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            VRRaycastHit hit = RaycastMesh(ray.origin, ray.direction);
            if( hit != null )
            {
                return hit.textureCoord;
            }
        }

        return new Vector2(-1,-1);
    }

    private string GetAbsoluteURL( string iUrl )
    {
        string url = iUrl;
        
        // If url does not start with http/https we assume it's a file
        if( !url.StartsWith( "http://" ) && !url.StartsWith( "https://" ) )
        {
            if( url.StartsWith( "file://" ) )
            {
                url = url.Substring(7, url.Length-7 );
                
                if( Application.platform == RuntimePlatform.WindowsPlayer && url.StartsWith( "/" ) )
                {
                    url = url.Substring(1, url.Length-1);
                }
            }
            
            if( ! System.IO.Path.IsPathRooted( url ) )
            {
                url = Application.dataPath + System.IO.Path.DirectorySeparatorChar + url;
            }
            
            if( Application.platform == RuntimePlatform.WindowsPlayer )
            {
                url = "/" + url;
            }
            
            url = "file://" + url;
        }
        
        return url;
    }


    public VRRaycastHit RaycastMesh(Vector3 rayOrigin, Vector3 rayDirection)
    {
        for (int i = 0; i < m_RaycastMeshTriangles.Length; i += 3)
        {
            int i0 = m_RaycastMeshTriangles[i];
            int i1 = m_RaycastMeshTriangles[i + 1];
            int i2 = m_RaycastMeshTriangles[i + 2];

            VRRaycastHit raycastHit = _RaycastTriangle(i0, i1, i2, rayOrigin, rayDirection);

            if (raycastHit != null)
            {
                return raycastHit;
            }
        }

        return null;
    }

    private VRRaycastHit _RaycastTriangle(int i0, int i1, int i2, Vector3 rayOrigin, Vector3 rayDirection)
    {
        Vector3 p0 = transform.TransformPoint(m_RaycastMeshVertices[i0]);
        Vector3 p1 = transform.TransformPoint(m_RaycastMeshVertices[i1]);
        Vector3 p2 = transform.TransformPoint(m_RaycastMeshVertices[i2]);

        // Möller–Trumbore intersection algorithm
        // http://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm

        // edge 1
        Vector3 E1 = p1 - p0;

        // edge 2
        Vector3 E2 = p2 - p0;

        // edge 3 - only for testing degenerate triangles
        Vector3 E3 = p2 - p0;
        if( E1.sqrMagnitude < 0.00001f || E2.sqrMagnitude < 0.00001f || E3.sqrMagnitude < 0.00001f )
        {
            // Skip degenerate triangle
            return null;
        }

        //Begin calculating determinant - also used to calculate u parameter
        Vector3 P = Vector3.Cross(rayDirection, E2);
        float det = Vector3.Dot(E1, P);
        if (det > -0.00001f && det < 0.00001f)
        {
            //if determinant is near zero, ray lies in plane of triangle
            return null;
        }

        float inv_det = 1.0f / det;
        //Calculate distance from V1 to ray origin
        Vector3 T = rayOrigin - p0;
        //Calculate u parameter and test bound
        float u = Vector3.Dot(T, P) * inv_det;
        if (u < 0.0f || u > 1.0f)
        {
            //The intersection lies outside of the triangle
            return null;
        }

        //Prepare to test v parameter
        Vector3 Q = Vector3.Cross(T, E1);
        //Calculate V parameter and test bound
        float v = Vector3.Dot(rayDirection, Q) * inv_det;
        if (v < 0.0f || u + v > 1.0f)
        {
            //The intersection lies outside of the triangle
            return null;
        }

        float t = Vector3.Dot(E2, Q) * inv_det;

        if (t <= 0.00001f)
        {
            // Intersection is before origin
            return null;
        }

        // We have an intersection, now compute uv

        Vector3 hitPoint = rayOrigin + (rayDirection * t);

        Vector3 w = hitPoint - p0;

        Vector3 vCrossW = Vector3.Cross(E2, w);
        Vector3 vCrossU = Vector3.Cross(E2, E1);

        if (Vector3.Dot(vCrossW, vCrossU) < 0.0f)
        {
            return null;
        }

        Vector3 uCrossW = Vector3.Cross(E1, w);
        Vector3 uCrossV = Vector3.Cross(E1, E2);

        if (Vector3.Dot(uCrossW, uCrossV) < 0.0f)
        {
            return null;
        }

        float denom = uCrossV.magnitude;
        float b1 = vCrossW.magnitude / denom;
        float b2 = uCrossW.magnitude / denom;

        if ((b1 <= 1.0f) && (b2 <= 1.0f) && (b1 + b2 <= 1.0f))
        {
            float b0 = 1.0f - b1 - b2;
            Vector2 uv0 = m_RaycastMeshUV[i0];
            Vector2 uv1 = m_RaycastMeshUV[i1];
            Vector2 uv2 = m_RaycastMeshUV[i2];
            Vector2 uv = new Vector2(b0 * uv0.x + b1 * uv1.x + b2 * uv2.x, b0 * uv0.y + b1 * uv1.y + b2 * uv2.y);

            VRRaycastHit raycastHit = new VRRaycastHit();
            raycastHit.collider = GetComponent<Collider>();
            raycastHit.distance = t;
            raycastHit.normal = vCrossU.normalized;
            raycastHit.point = hitPoint;
            raycastHit.textureCoord = uv;
            return raycastHit;
        }
        else
        {
            return null;
        }
    }

    private void _UpdateBoxCollider(BoxCollider boxCollider)
    {
        if (boxCollider != null)
        {
            Vector3 lowerBoxLimits = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 upperBoxLimits = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int i = 0; i < m_RaycastMeshVertices.Length; ++i)
            {
                Vector3 vertex = m_RaycastMeshVertices[i];
                lowerBoxLimits.x = Mathf.Min(lowerBoxLimits.x, vertex.x);
                lowerBoxLimits.y = Mathf.Min(lowerBoxLimits.y, vertex.y);
                lowerBoxLimits.z = Mathf.Min(lowerBoxLimits.z, vertex.z);
                upperBoxLimits.x = Mathf.Max(upperBoxLimits.x, vertex.x);
                upperBoxLimits.y = Mathf.Max(upperBoxLimits.y, vertex.y);
                upperBoxLimits.z = Mathf.Max(upperBoxLimits.z, vertex.z);
            }

            boxCollider.center = (upperBoxLimits + lowerBoxLimits) / 2.0f;
            boxCollider.size = upperBoxLimits - lowerBoxLimits;
            boxCollider.isTrigger = true;
        }
    }

    protected void OnMVRWandEnter(VRSelection iSelection)
    {
        // Force show ray and save state
        m_WandRayWasVisible = iSelection.SourceWand.IsRayVisible();
        iSelection.SourceWand.ShowRay(true);
    }

    protected void OnMVRWandHover(VRSelection iSelection)
    {
        SetVirtualMousePosition(iSelection.TextureCoordinate);
    }

    protected void OnMVRWandButtonPressed(VRSelection iSelection)
    {
        SetVirtualMousePosition(iSelection.TextureCoordinate);
        SetVirtualMouseButtonPressed();
    }

    protected void OnMVRWandButtonReleased(VRSelection iSelection)
    {
        SetVirtualMouseButtonReleased();
        SetVirtualMousePosition(iSelection.TextureCoordinate);
    }

    protected void OnMVRWandExit(VRSelection iSelection)
    {
        // Unforce show ray
        iSelection.SourceWand.ShowRay(m_WandRayWasVisible);
    }

    protected void OnMVRTouchEnd(VRTouch iTouch)
    {
        SetVirtualMouseButtonPressed();
        SetVirtualMouseButtonReleased();
    }

    protected void OnMVRTouchMoved(VRTouch iTouch)
    {
        Vector3 fwd = -transform.TransformDirection(Vector3.up);
        Ray ray = new Ray(iTouch.TouchObject.transform.position, fwd);

        var collider = GetComponent<Collider>();

        RaycastHit hit;
        if (collider != null && collider.Raycast(ray, out hit, 1.0F))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Vector2 mouseCursor = hit.textureCoord;
                SetVirtualMousePosition(mouseCursor);
            }
        }
    }

    protected void OnMVRTouchBegin(VRTouch iTouch)
    {
        Vector3 fwd = -transform.TransformDirection(Vector3.up);
        Ray ray = new Ray(iTouch.TouchObject.transform.position, fwd);

        var collider = GetComponent<Collider>();

        RaycastHit hit;
        if (collider != null && collider.Raycast(ray, out hit, 1.0F))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Vector2 mouseCursor = hit.textureCoord;
                SetVirtualMousePosition(mouseCursor);
            }
        }
    }
}

public class VRRaycastHit
{
    public Collider collider;
    public float distance;
    public Vector3 normal;
    public Vector3 point;
    public Vector2 textureCoord;

    public VRRaycastHit()
    {
        collider = null;
        distance = 0.0f;
        normal = new Vector3();
        point = new Vector3();
        textureCoord = new Vector2();
    }

    public VRRaycastHit(RaycastHit raycastHit)
    {
        collider = raycastHit.collider;
        distance = raycastHit.distance;
        normal = raycastHit.normal;
        point = raycastHit.point;
        textureCoord = raycastHit.textureCoord;
    }
}
