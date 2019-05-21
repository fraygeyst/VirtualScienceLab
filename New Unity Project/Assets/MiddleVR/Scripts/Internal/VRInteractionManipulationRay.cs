/* VRInteractionManipulationRay
 * MiddleVR
 * (c) MiddleVR
 *
 * Note: Made to be attached to the Wand
 */

using UnityEngine;
using System.Collections;
using MiddleVR_Unity3D;
using System;

[AddComponentMenu("")]
public class VRInteractionManipulationRay : VRInteraction
{

    public string Name = "InteractionManipulationRay";
    public string HandNode = "HandNode";
    public uint WandGrabButton = 0;

    private vrInteractionManipulationRay m_it = null;

    private vrNode3D m_HandNode = null;
    private VRWand m_Wand = null;

    private GameObject m_CurrentSelectedObject = null;
    private GameObject m_CurrentManipulatedObject = null;

    private Vector3 m_ManipulatedObjectInitialLocalPosition;
    private Quaternion m_ManipulatedObjectInitialLocalRotation;
    private bool m_ManipulatedObjectInitialIsKinematic;

    private vrInteraction m_PausedSelection = null;

    private enum InteractionState
    {
        Inactive,
        AuthorityPending,
        AuthorityDenied,
        Running
    }

    private InteractionState m_State = InteractionState.Inactive;

    protected void Start()
    {
        // Make sure the base interaction is started
        InitializeBaseInteraction();

        m_it = new vrInteractionManipulationRay(Name);
        // Must tell base class about our interaction
        SetInteraction(m_it);

        MiddleVR.VRInteractionMgr.AddInteraction(m_it);
        MiddleVR.VRInteractionMgr.Activate(m_it);

        m_HandNode = MiddleVR.VRDisplayMgr.GetNode(HandNode);

        if (m_HandNode != null)
        {
            m_it.SetGrabWandButton(WandGrabButton);
            m_it.SetManipulatorNode(m_HandNode);
        }
        else
        {
            MiddleVR.VRLog(2, "[X] VRInteractionManipulationRay: One or several nodes are missing.");
        }

        m_Wand = this.GetComponent<VRWand>();
    }

    protected void Update()
    {
        if (IsActive())
        {
            // Retrieve selection result
            VRSelection selection = m_Wand.GetSelection();

            if (selection == null || !selection.SelectedObject.GetComponent<VRActor>().Grabable)
            {
                return;
            }

            m_CurrentSelectedObject = selection.SelectedObject;

            switch (m_State)
            {
                case InteractionState.Inactive:
                    if (m_it.IsManipulationStarted())
                    {
                        GrabInitialPosition(m_CurrentSelectedObject);
                        var authorityRequest = RequestAssignClientAuthority(m_CurrentSelectedObject);
                        if (authorityRequest == AuthorityRequestState.Accepted)
                        {
                            Grab(m_CurrentSelectedObject);
                            m_State = InteractionState.Running;
                        }
                        else
                        {
                            m_State = InteractionState.AuthorityPending;
                        }
                    }
                    break;

                case InteractionState.AuthorityPending:
                    if (m_it.IsManipulationStopped())
                    {
                        ClearClientAuthorityRequest();
                        m_State = InteractionState.Inactive;
                    }
                    else
                    {
                        var authorityRequest = RequestAssignClientAuthority(m_CurrentSelectedObject);
                        if (authorityRequest == AuthorityRequestState.Accepted)
                        {
                            Grab(m_CurrentSelectedObject);
                            m_State = InteractionState.Running;
                        }
                    }
                    break;

                case InteractionState.Running:
                    if (m_it.IsManipulationStopped())
                    {
                        RequestRemoveClientAuthority(m_CurrentSelectedObject);
                        Ungrab();
                        ClearClientAuthorityRequest();
                        m_State = InteractionState.Inactive;
                    }
                    break;

                case InteractionState.AuthorityDenied:
                    if (m_it.IsManipulationStopped())
                    {
                        ClearClientAuthorityRequest();
                        m_State = InteractionState.Inactive;
                    }
                    break;
            }
        }
    }

    protected void OnEnable()
    {
        MiddleVR.VRLog(3, "[ ] VRInteractionManipulationRay: enabled");
        if (m_it != null)
        {
            MiddleVR.VRInteractionMgr.Activate(m_it);
        }
    }

    protected void OnDisable()
    {
        MiddleVR.VRLog(3, "[ ] VRInteractionManipulationRay: disabled");

        if (m_it != null && MiddleVR.VRInteractionMgr != null)
        {
            MiddleVR.VRInteractionMgr.Deactivate(m_it);
        }
    }

    private void GrabInitialPosition(GameObject iGrabbedObject)
    {
        if (iGrabbedObject == null)
        {
            return;
        }

        // Save initial position
        m_ManipulatedObjectInitialLocalPosition = iGrabbedObject.transform.localPosition;
        m_ManipulatedObjectInitialLocalRotation = iGrabbedObject.transform.localRotation;
    }

    private void Grab(GameObject iGrabbedObject)
    {
        // Initialize manipulated node
        m_CurrentManipulatedObject = iGrabbedObject;
        m_it.SetManipulatedNode(AcquireGameObjectNode(m_CurrentManipulatedObject, "VRInteractionManipulationRayNode"));

        // Pause rigidbody acceleration 
        Rigidbody manipulatedRigidbody = iGrabbedObject.GetComponent<Rigidbody>();
        if (manipulatedRigidbody != null)
        {
            m_ManipulatedObjectInitialIsKinematic = manipulatedRigidbody.isKinematic;
            manipulatedRigidbody.isKinematic = true;
        }

        // Deactivate selection during the manipulation
        vrInteraction selection = MiddleVR.VRInteractionMgr.GetActiveInteractionByTag("ContinuousSelection");
        if (selection != null)
        {
            m_PausedSelection = selection;
            MiddleVR.VRInteractionMgr.Deactivate(m_PausedSelection);
        }
    }

    private void Ungrab()
    {
        if (m_CurrentManipulatedObject == null)
        {
            return;
        }

        // Give to return objects script
        VRInteractionManipulationReturnObjects returningObjectScript = this.GetComponent<VRInteractionManipulationReturnObjects>();
        if (returningObjectScript != null)
        {
            if (returningObjectScript.enabled)
            {
                returningObjectScript.AddReturningObject(m_CurrentManipulatedObject, m_ManipulatedObjectInitialLocalPosition,
                                                         m_ManipulatedObjectInitialLocalRotation, false);
            }
        }

        // Reset
        m_it.SetManipulatedNode(null);
        ReleaseGameObjectNode(m_CurrentManipulatedObject);

        // Unpause rigidbody acceleration 
        Rigidbody manipulatedRigidbody = m_CurrentManipulatedObject.GetComponent<Rigidbody>();
        if (manipulatedRigidbody != null)
        {
            manipulatedRigidbody.isKinematic = m_ManipulatedObjectInitialIsKinematic;
        }

        // Reactivate selection after the manipulation
        if (m_PausedSelection != null)
        {
            MiddleVR.VRInteractionMgr.Activate(m_PausedSelection);
            m_PausedSelection = null;
        }

        m_CurrentManipulatedObject = null;
    }
}
