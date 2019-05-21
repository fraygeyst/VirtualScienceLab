using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
public class VRNetworkLocalObject : NetworkBehaviour
{
    [NonSerialized] [SyncVar] public GameObject owner;
    [NonSerialized] [SyncVar] public string nodeName;

    public override void OnStartClient()
    {
        if (owner == null)
        {
            return;
        }

        var ownerNetworkIdentity = owner.GetComponent<NetworkIdentity>();

        gameObject.name = nodeName + "[" + ownerNetworkIdentity.netId.Value + "]";

        if (ownerNetworkIdentity.hasAuthority)
        {
            // On our client, reparent but don't display

            var parent = GameObject.Find(nodeName);

            if (parent != null)
            {
                var localPosition = gameObject.transform.localPosition;
                var localRotation = gameObject.transform.localRotation;
                var localScale = gameObject.transform.localScale;

                gameObject.transform.parent = parent.transform;

                gameObject.transform.localPosition = localPosition;
                gameObject.transform.localRotation = localRotation;
                gameObject.transform.localScale = localScale;
            }

            var rendererComponent = GetComponent<Renderer>();
            if (rendererComponent)
            {
                rendererComponent.enabled = false;
            }

            var childrenRendererComponents = GetComponentsInChildren<Renderer>();
            foreach (var childrenRendererComponent in childrenRendererComponents)
            {
                childrenRendererComponent.enabled = false;
            }
        }
    }
}
