using UnityEngine;
using System.Collections;

public class TriggerHapticPulseOnVRAction : MonoBehaviour
{
    [SerializeField]
    private float m_VibrationTime = 1.0f;

    void VRAction(VRSelection iSelection)
    {
        StartCoroutine(MakeVibrate());
    }

    IEnumerator MakeVibrate()
    {
        float time = .0f;

        while (time < m_VibrationTime)
        {
            time += Time.deltaTime;

            // The parameters for the "vrDriverOpenVRSDK.TriggerHapticPulse" vrCommand are:
            // - ControllerId: int
            //   It is the controller we want to make vibrate. The first controller is
            //   the controller 0. If ControllerId is -1 then all the
            //   connected controllers will receive the haptic pulse.
            // - Axis: uint
            //   It is the axis we want to make vibrate on the controller. Controllers
            //   usually have only one axis but they can have more. The first
            //   axis is the axis 0.
            // - VibrationTime: uint
            //   It is the time in microseconds the pulse will last. It can last
            //   up to 3 milliseconds.

            // Note that after this call the application may not trigger another haptic
            // pulse on this controller and axis combination for 5 ms.
            var value = vrValue.CreateList();

            value.AddListItem(new vrValue(-1));
            value.AddListItem(new vrValue(0));
            value.AddListItem(new vrValue(3000));

            MiddleVR.VRKernel.ExecuteCommand("vrDriverOpenVRSDK.TriggerHapticPulse", value);

            yield return new WaitForSeconds(.002f);
        }
    }
}
