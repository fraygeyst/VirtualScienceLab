using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display_Meter_5_D : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setDisplay(double do_number, string parent_name)
    {
        if(do_number < 1000)
        {
            string s_number = String.Format("{0:000.00}", do_number);
            GameObject parent = GameObject.Find(parent_name);

            Debug.Log(s_number);

            set_n(int.Parse(Char.ToString(s_number[0])), 1, parent);
            set_n(int.Parse(Char.ToString(s_number[1])), 2, parent);
            set_n(int.Parse(Char.ToString(s_number[2])), 3, parent);
            set_n(int.Parse(Char.ToString(s_number[4])), 4, parent);
            set_n(int.Parse(Char.ToString(s_number[5])), 5, parent);

            setColorP(parent.transform.Find("decimal_point").gameObject);
        }
    }

    private void set_n(int number, int disp_num, GameObject parent_item)
    {
        string s_lt = "n_" + disp_num + "_lt";
        string s_rt = "n_" + disp_num + "_rt";
        string s_lb = "n_" + disp_num + "_lb";
        string s_rb = "n_" + disp_num + "_rb";
        string s_m = "n_" + disp_num + "_m";
        string s_b = "n_" + disp_num + "_b";
        string s_t = "n_" + disp_num + "_t";

        switch (number)
        {
            case 0:
                setColorP(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorP(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorN(parent_item.transform.Find(s_m).gameObject);
                setColorP(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;
            case 1:
                setColorN(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorN(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorN(parent_item.transform.Find(s_m).gameObject);
                setColorN(parent_item.transform.Find(s_b).gameObject);
                setColorN(parent_item.transform.Find(s_t).gameObject);
                break;
            case 2:
                setColorN(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorP(parent_item.transform.Find(s_lb).gameObject);
                setColorN(parent_item.transform.Find(s_rb).gameObject);
                setColorP(parent_item.transform.Find(s_m).gameObject);
                setColorP(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;              
            case 3:                 
                setColorN(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorN(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorP(parent_item.transform.Find(s_m).gameObject);
                setColorP(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;              
            case 4:                 
                setColorP(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorN(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorP(parent_item.transform.Find(s_m).gameObject);
                setColorN(parent_item.transform.Find(s_b).gameObject);
                setColorN(parent_item.transform.Find(s_t).gameObject);
                break;              
            case 5:                 
                setColorP(parent_item.transform.Find(s_lt).gameObject);
                setColorN(parent_item.transform.Find(s_rt).gameObject);
                setColorN(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorP(parent_item.transform.Find(s_m).gameObject);
                setColorP(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;              
            case 6:                 
                setColorP(parent_item.transform.Find(s_lt).gameObject);
                setColorN(parent_item.transform.Find(s_rt).gameObject);
                setColorP(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorP(parent_item.transform.Find(s_m).gameObject);
                setColorP(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;              
            case 7:                 
                setColorN(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorN(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorN(parent_item.transform.Find(s_m).gameObject);
                setColorN(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;              
            case 8:                 
                setColorP(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorP(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorP(parent_item.transform.Find(s_m).gameObject);
                setColorP(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;              
            case 9:                 
                setColorP(parent_item.transform.Find(s_lt).gameObject);
                setColorP(parent_item.transform.Find(s_rt).gameObject);
                setColorN(parent_item.transform.Find(s_lb).gameObject);
                setColorP(parent_item.transform.Find(s_rb).gameObject);
                setColorP(parent_item.transform.Find(s_m).gameObject);
                setColorP(parent_item.transform.Find(s_b).gameObject);
                setColorP(parent_item.transform.Find(s_t).gameObject);
                break;
        }
    }
    
    private void setColorP(GameObject gameObject)
    {
        if (Load_Publics.counter <= Load_Publics.maximum)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

    }
    private void setColorN(GameObject gameObject)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.black;
    }

    private int[] GetIntArray(int num)
    {
        List<int> listOfInts = new List<int>();
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        listOfInts.Reverse();
        return listOfInts.ToArray();
    }
}
