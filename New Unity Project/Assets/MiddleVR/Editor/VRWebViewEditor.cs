/* VRGUIScriptEditor
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using UnityEditor;
using System.Collections;
using MiddleVR_Unity3D;
using UnityEditor.Callbacks;

[CustomEditor(typeof(VRWebView))]
public class VRWebViewEditor : Editor
{
	//This will just be a shortcut to the target, ex: the object you clicked on.
	private VRWebView m_VRWebViewScript;

	void Awake()
	{
		m_VRWebViewScript = (VRWebView)target;
	}
	
	void Start()
	{
	}
	
	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical();

		if (GUILayout.Button("Pick html file"))
		{
			string path = EditorUtility.OpenFilePanel("Please choose a HTML file", "", "html");
			if (path.Length != 0)
			{
				// Don't use Path.DirectorySeparatorChar because
				// Unity always returns paths with slashes
				string basePath = Application.dataPath + "/";
				if(path.StartsWith(basePath))
				{
					path = path.Substring(basePath.Length);
				}
				
				MVRTools.Log("[+] Picked " + path );
				m_VRWebViewScript.URL = path;
				EditorUtility.SetDirty(m_VRWebViewScript);
			}
		}
		
		DrawDefaultInspector();
		GUILayout.EndVertical();
	}
}
