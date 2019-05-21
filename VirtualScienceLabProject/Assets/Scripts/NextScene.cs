using UnityEngine;

/// <summary>
/// Wechseln einer Szene (Pausetaste!)
/// </summary>
public class NextScene   : MonoBehaviour {
    public System.String sceneName;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
    }
}
