using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


public class Styropor : MonoBehaviour {

    public GameObject styropor;
    public GameObject aceton;

    public bool collision = false;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if(collision == true)
        {
            StartCoroutine(ScaleOverTime(1));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == aceton)
        {
            collision = true;
        }
    }

    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale = styropor.transform.localScale;
        Vector3 destinationScale = new Vector3(0f, 0f, 0f);

        float currentTime = 0.0f;

        do
        {
            styropor.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

        Destroy(gameObject);
    }
}
