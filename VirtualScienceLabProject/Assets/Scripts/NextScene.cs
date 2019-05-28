using UnityEngine;

/// <summary>
/// Wechseln einer Szene (Pausetaste!)
/// </summary>
public class NextScene   : MonoBehaviour {
   public System.String sceneName;
 

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("changing Scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
       
    }


    
    
        
}
