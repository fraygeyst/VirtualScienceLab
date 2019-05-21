/* VRAttachToNode
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Interactions/Attach to Node")]
public class VRAttachToNode  : MonoBehaviour {
	public string VRParentNode   = "HandNode";
	
	public bool KeepLocalPosition = true;
	public bool KeepLocalRotation = true;
	public bool KeepLocalScale    = true;
	
	private bool m_Attached = false;
	private bool m_Searched = false;
	
	protected void Update ()
	{
		if (!m_Attached)
		{
			GameObject node = GameObject.Find(VRParentNode);
			
			if( VRParentNode.Length == 0 )
			{
				MVRTools.Log(0, "[X] AttachToNode: Please specify a valid VRParentNode name.");
			}
			
			if (node != null)
			{
				Vector3    oldPos   = transform.localPosition;
				Quaternion oldRot   = transform.localRotation;
				Vector3    oldScale = transform.localScale;
				
				// Setting new parent
				transform.parent = node.transform;
				
				if( !KeepLocalPosition )
				{
					transform.localPosition = new Vector3(0, 0, 0);
				}
				else
				{
					transform.localPosition = oldPos;
				}
				
				if( !KeepLocalRotation )
				{
					transform.localRotation = new Quaternion(0, 0, 0, 1);
				}
				else
				{
					transform.localRotation = oldRot;
				}
				
				if( !KeepLocalScale )
				{
					transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				}
				else
				{
					transform.localScale = oldScale;
				}
				
				
				MVRTools.Log( 2, "[+] AttachToNode: " + this.name + " attached to : " + node.name );
				m_Attached = true;

				// Stop this component now.
				enabled = false;
			}
			else
			{
				if (m_Searched == false)
				{
					MVRTools.Log(0, "[X] AttachToNode: Failed to find Game object '" + VRParentNode + "'");
					m_Searched = true;

					// Stop this component now.
					enabled = false;
				}
			}
		}
	}
}
