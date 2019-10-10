using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class BubbleSort : MonoBehaviour
{
    GameObject sicht_1;
    GameObject sicht_2;
    GameObject sicht_3;
    GameObject sicht_4;
    GameObject sicht_5;
    GameObject sicht_6;
    GameObject sicht_7;


    // Use this for initialization
    void Start()
    {
        sicht_1 = GameObject.Find("sicht_1");
        sicht_2 = GameObject.Find("sicht_2");
        sicht_3 = GameObject.Find("sicht_3");
        sicht_4 = GameObject.Find("sicht_4");
        sicht_5 = GameObject.Find("sicht_5");
        sicht_6 = GameObject.Find("sicht_6");
        sicht_7 = GameObject.Find("sicht_7");
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (gameObject.name)
        {
            case "bubble_start":
                if (Load_Publics.bubble_active)
                {
                    switch (Load_Publics.b_state)
                    {
                        case 0:
                            Load_Publics.b_state = 1;
                            activate(sicht_1);
                            activate(sicht_2);
                            deactivate(sicht_6);
                            deactivate(sicht_7);
                            break;
                        case 1:
                            Load_Publics.b_state = 2;
                            activate(sicht_2);
                            activate(sicht_3);
                            deactivate(sicht_1);
                            break;
                        case 2:
                            Load_Publics.b_state = 3;
                            activate(sicht_3);
                            activate(sicht_4);
                            deactivate(sicht_2);
                            break;
                        case 3:
                            Load_Publics.b_state = 4;
                            activate(sicht_4);
                            activate(sicht_5);
                            deactivate(sicht_3);
                            break;
                        case 4:
                            Load_Publics.b_state = 5;
                            activate(sicht_5);
                            activate(sicht_6);
                            deactivate(sicht_4);
                            break;
                        case 5:
                            Load_Publics.b_state = 0;
                            activate(sicht_6);
                            activate(sicht_7);
                            deactivate(sicht_5);
                            break;
                    }
                    StartCoroutine(waiter());
                    Debug.Log("State: " + Load_Publics.b_state);
                    break;
                }
                break;
        }
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Act: " + gameObject.name + ", col: " + col.gameObject.name);
        switch (gameObject.name)
        {
            case "b_act_1":
                if(col.gameObject.name == "b_1")
                {
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_2":
                if (col.gameObject.name == "b_2")
                {
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_3":
                if (col.gameObject.name == "b_3")
                {
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_4":
                if (col.gameObject.name == "b_4")
                {
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_5":
                if (col.gameObject.name == "b_5")
                {
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_6":
                if (col.gameObject.name == "b_6")
                {
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_7":
                if (col.gameObject.name == "b_7")
                {
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
        }
    }

    private void activate(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, (float)-3.1);
    }
    private void deactivate(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, (float)-3.412225);
    }

    IEnumerator waiter()
    {
        GameObject button_bub = GameObject.Find("bubble_start");
        button_bub.GetComponent<Renderer>().material.color = Color.yellow;
        Load_Publics.bubble_active = false;

        yield return new WaitForSeconds(2);    //Wait 2 Seconds

        Load_Publics.bubble_active = true;
        button_bub.GetComponent<Renderer>().material.color = Color.green;
    }
}


