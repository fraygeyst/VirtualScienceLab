/* VRInteractionSample
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;

[AddComponentMenu("MiddleVR/Samples/Interaction")]
public class VRInteractionSample : VRInteraction {

    protected void Start()
    {
        // Make sure the base interaction is started
        InitializeBaseInteraction();

        // Must tell base class about our interaction
        CreateInteraction("MyInteraction");
        base.GetInteraction().AddTag("ContinuousNavigation");

        base.Activate();
    }

    protected void Update()
    {
        if (IsActive())
        {
            print("interacting");
        }
    }

    protected override void OnActivate()
    {
        print("Activating MyInteraction");
    }

    protected override void OnDeactivate()
    {
        print("Deactivating MyInteraction");
    }
}
