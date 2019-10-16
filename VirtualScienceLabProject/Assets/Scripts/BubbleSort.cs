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
        next_sort(gameObject.name);
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Act: " + gameObject.name + ", col: " + col.gameObject.name);
        switch (gameObject.name)
        {
            case "b_act_1":
                if(col.gameObject.name == "b_1")
                {
                    Load_Publics.s_1_act = false;
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_2":
                if (col.gameObject.name == "b_2")
                {
                    Load_Publics.s_2_act = false;
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_3":
                if (col.gameObject.name == "b_3")
                {
                    Load_Publics.s_3_act = false;
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_4":
                if (col.gameObject.name == "b_4")
                {
                    Load_Publics.s_4_act = false;
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_5":
                if (col.gameObject.name == "b_5")
                {
                    Load_Publics.s_5_act = false;
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_6":
                if (col.gameObject.name == "b_6")
                {
                    Load_Publics.s_6_act = false;
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            case "b_act_7":
                if (col.gameObject.name == "b_7")
                {
                    Load_Publics.s_7_act = false;
                    col.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
        }
    }

    private void next_sort(string obj_name)
    {
        switch (obj_name)
        {
            case "bubble_start":
                if (Load_Publics.bubble_active)
                {
                    switch (Load_Publics.b_state)
                    {
                        case 0:
                            Load_Publics.b_state = 1;
                            deactivate(sicht_6);
                            deactivate(sicht_7);
                            if (Load_Publics.s_1_act && Load_Publics.s_2_act)
                            {
                                activate(sicht_1);
                                activate(sicht_2);
                            } else
                            {
                                next_sort(obj_name);
                            }
                            break;
                        case 1:
                            Load_Publics.b_state = 2;
                            deactivate(sicht_1);
                            if (Load_Publics.s_1_act && Load_Publics.s_2_act)
                            {
                                activate(sicht_2);
                                activate(sicht_3);
                            }
                            else
                            {
                                next_sort(obj_name);
                            }
                            break;
                        case 2:
                            Load_Publics.b_state = 3;
                            deactivate(sicht_2);
                            if (Load_Publics.s_1_act && Load_Publics.s_2_act)
                            {
                                activate(sicht_3);
                                activate(sicht_4);
                            }
                            else
                            {
                                next_sort(obj_name);
                            }
                            break;
                        case 3:
                            Load_Publics.b_state = 4;
                            deactivate(sicht_3);
                            if (Load_Publics.s_1_act && Load_Publics.s_2_act)
                            {
                                activate(sicht_4);
                                activate(sicht_5);
                            }
                            else
                            {
                                next_sort(obj_name);
                            }
                            break;
                        case 4:
                            Load_Publics.b_state = 5;
                            deactivate(sicht_4);
                            if (Load_Publics.s_1_act && Load_Publics.s_2_act)
                            {
                                activate(sicht_5);
                                activate(sicht_6);
                            }
                            else
                            {
                                next_sort(obj_name);
                            }
                            break;
                        case 5:
                            Load_Publics.b_state = 0;
                            deactivate(sicht_5);
                            if (Load_Publics.s_1_act && Load_Publics.s_2_act)
                            {
                                activate(sicht_6);
                                activate(sicht_7);
                            }
                            else
                            {
                                next_sort(obj_name);
                            }
                            break;
                    }
                    if (!all_done())
                    {
                        StartCoroutine(waiter());
                        Debug.Log("State: " + Load_Publics.b_state);
                    }
                    break;
                }
                break;
        }
    }
    private void activate(GameObject obj)
    {
        Transform transform = obj.GetComponent<Transform>();
        Vector3 vec = new Vector3(obj.transform.position.x, obj.transform.position.y, (float)-3.1);
        StartCoroutine(MoveToPosition(transform, vec, 1));
    }
    private void deactivate(GameObject obj)
    {
        Transform transform = obj.GetComponent<Transform>();
        Vector3 vec = new Vector3(obj.transform.position.x, obj.transform.position.y, (float)-3.412225);
        StartCoroutine(MoveToPosition(transform, vec, 1));
    }
    private bool all_done()
    {
        if(!Load_Publics.s_1_act && !Load_Publics.s_2_act && !Load_Publics.s_3_act && !Load_Publics.s_4_act && !Load_Publics.s_5_act && !Load_Publics.s_6_act && !Load_Publics.s_7_act)
        {
            GameObject button_bub = GameObject.Find("bubble_start");
            button_bub.GetComponent<Renderer>().material.color = Color.red;
            Load_Publics.bubble_active = false;
            return true;
        } else
        {
            return false;
        }
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
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}


