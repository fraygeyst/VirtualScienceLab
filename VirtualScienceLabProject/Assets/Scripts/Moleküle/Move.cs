using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    private float speed = 0.5f;
    private Vector3 direction = new Vector3(1, 1, 1);
    
    // Use this for initialization
	void Start () {

	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            direction = direction * -1;
        }
    }

    

    // Update is called once per frame
    void Update () {

        //update the position
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
