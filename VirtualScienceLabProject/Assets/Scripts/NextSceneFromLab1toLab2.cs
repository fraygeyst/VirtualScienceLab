
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class NextSceneFromLab1toLab2 : MonoBehaviour {

    public System.String sceneName;
    private bool collisionHappend = false;

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        collisionHappend = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionHappend = false;
    }


    // Update is called once per frame
    void Update()
    {

        // get trigger down
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) && collisionHappend || ViveInput.GetPressDownEx(HandRole.LeftHand, ControllerButton.Trigger) && collisionHappend)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
        }
    }
        
}
