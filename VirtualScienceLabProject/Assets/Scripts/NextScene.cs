using UnityEngine;
using HTC.UnityPlugin.Vive;

/// <summary>
/// Wechseln einer Szene (Pausetaste!)
/// </summary>
public class NextScene   : MonoBehaviour {
   public System.String sceneName;
   private bool collisionHappend = false;



   

    private void OnTriggerEnter(Collider other)
    {
        collisionHappend = true;
        Debug.Log("collisionHappend: " + collisionHappend);
        

    }

    private void OnTriggerExit(Collider other)
    {
        collisionHappend = false;
        Debug.Log("collisionHappend: " + collisionHappend);
    }

    private void Update()
    {
        if ((ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend) || (ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend))
        {
            Debug.Log("changing Scene");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
        }
    }




}
