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
        float move_speed = Load_Publics.RemapTemp((float)Load_Publics.Temperatur, Load_Publics.Temp_Min, Load_Publics.Temp_Max, Load_Publics.Map_Temp_Min, Load_Publics.Map_Temp_Max) * Load_Publics.move_speed_multi;
        //update the position
        transform.Translate(direction * move_speed * Time.deltaTime);
    }
}
