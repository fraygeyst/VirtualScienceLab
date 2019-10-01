using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sev_Seg_Counter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void setSevSegCount(int seconds)
    {
        List<int> digits = new List<int>();
        int sizeOfList = digits.Count;
        if(sizeOfList == 1)
        {
            set_n_1(0);
            set_n_2(0);
            set_n_3(digits[0]);
        } else if(sizeOfList == 2)
        {
            set_n_1(0);
            set_n_2(digits[0]);
            set_n_3(digits[1]);
        } else if(sizeOfList == 3)
        {
            set_n_1(digits[0]);
            set_n_2(digits[1]);
            set_n_3(digits[2]);
        }
    }
    // eventuell gameobjecte einzeln initialisieren? funktioniert aber auch nicht 
    /*public GameObject n_1_lt;
    public GameObject n_1_rt;
    public GameObject n_1_lb;
    public GameObject n_1_rb;
    public GameObject n_1_m;
    public GameObject n_1_b;
    public GameObject n_1_t;


    public GameObject n_2_lt;
    public GameObject n_2_rt;
    public GameObject n_2_lb;
    public GameObject n_2_rb;
    public GameObject n_2_m;
    public GameObject n_2_b;
    public GameObject n_2_t;


    public GameObject n_3_lt;
    public GameObject n_3_rt;
    public GameObject n_3_lb;
    public GameObject n_3_rb;
    public GameObject n_3_m;
    public GameObject n_3_b;
    public GameObject n_3_t;
    */

    private void set_n_1(int number)
    {
        switch (number)
        {
            case 0:
                /*setColorP(n_1_lt);
                setColorP(n_1_lt);
                setColorP(n_1_lb);
                setColorP(n_1_rb);
                setColorN(n_1_m);
                setColorP(n_1_b);
                setColorP(n_1_t);
                */
                setColorP(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorP(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorN(GameObject.Find("n_1_m"));
                setColorP(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
            case 1:
                setColorN(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorN(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorN(GameObject.Find("n_1_m"));
                setColorN(GameObject.Find("n_1_b"));
                setColorN(GameObject.Find("n_1_t"));
                break;
            case 2:
                setColorN(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorP(GameObject.Find("n_1_lb"));
                setColorN(GameObject.Find("n_1_rb"));
                setColorP(GameObject.Find("n_1_m"));
                setColorP(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
            case 3:
                setColorN(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorN(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorP(GameObject.Find("n_1_m"));
                setColorP(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
            case 4:
                setColorP(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorN(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorP(GameObject.Find("n_1_m"));
                setColorN(GameObject.Find("n_1_b"));
                setColorN(GameObject.Find("n_1_t"));
                break;
            case 5:
                setColorP(GameObject.Find("n_1_lt"));
                setColorN(GameObject.Find("n_1_rt"));
                setColorN(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorP(GameObject.Find("n_1_m"));
                setColorP(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
            case 6:
                setColorP(GameObject.Find("n_1_lt"));
                setColorN(GameObject.Find("n_1_rt"));
                setColorP(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorP(GameObject.Find("n_1_m"));
                setColorP(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
            case 7:
                setColorN(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorN(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorN(GameObject.Find("n_1_m"));
                setColorN(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
            case 8:
                setColorP(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorP(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorP(GameObject.Find("n_1_m"));
                setColorP(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
            case 9:
                setColorP(GameObject.Find("n_1_lt"));
                setColorP(GameObject.Find("n_1_rt"));
                setColorN(GameObject.Find("n_1_lb"));
                setColorP(GameObject.Find("n_1_rb"));
                setColorP(GameObject.Find("n_1_m"));
                setColorP(GameObject.Find("n_1_b"));
                setColorP(GameObject.Find("n_1_t"));
                break;
        }
    }

    private void set_n_2(int number)
    {
        switch (number)
        {
            case 0:
               /* setColorP(n_2_lt);
                setColorP(n_2_lt);
                setColorP(n_2_lb);
                setColorP(n_2_rb);
                setColorN(n_2_m);
                setColorP(n_2_b);
                setColorP(n_2_t);
                */
                setColorP(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorP(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorN(GameObject.Find("n_2_m"));
                setColorP(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
            case 1:
                setColorN(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorN(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorN(GameObject.Find("n_2_m"));
                setColorN(GameObject.Find("n_2_b"));
                setColorN(GameObject.Find("n_2_t"));
                break;
            case 2:
                setColorN(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorP(GameObject.Find("n_2_lb"));
                setColorN(GameObject.Find("n_2_rb"));
                setColorP(GameObject.Find("n_2_m"));
                setColorP(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
            case 3:
                setColorN(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorN(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorP(GameObject.Find("n_2_m"));
                setColorP(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
            case 4:
                setColorP(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorN(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorP(GameObject.Find("n_2_m"));
                setColorN(GameObject.Find("n_2_b"));
                setColorN(GameObject.Find("n_2_t"));
                break;
            case 5:
                setColorP(GameObject.Find("n_2_lt"));
                setColorN(GameObject.Find("n_2_rt"));
                setColorN(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorP(GameObject.Find("n_2_m"));
                setColorP(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
            case 6:
                setColorP(GameObject.Find("n_2_lt"));
                setColorN(GameObject.Find("n_2_rt"));
                setColorP(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorP(GameObject.Find("n_2_m"));
                setColorP(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
            case 7:
                setColorN(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorN(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorN(GameObject.Find("n_2_m"));
                setColorN(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
            case 8:
                setColorP(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorP(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorP(GameObject.Find("n_2_m"));
                setColorP(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
            case 9:
                setColorP(GameObject.Find("n_2_lt"));
                setColorP(GameObject.Find("n_2_rt"));
                setColorN(GameObject.Find("n_2_lb"));
                setColorP(GameObject.Find("n_2_rb"));
                setColorP(GameObject.Find("n_2_m"));
                setColorP(GameObject.Find("n_2_b"));
                setColorP(GameObject.Find("n_2_t"));
                break;
        }
    }

    private void set_n_3(int number)
    {
        switch (number)
        {
            case 0:
                setColorP(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorP(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorN(GameObject.Find("n_3_m"));
                setColorP(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
            case 1:
                setColorN(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorN(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorN(GameObject.Find("n_3_m"));
                setColorN(GameObject.Find("n_3_b"));
                setColorN(GameObject.Find("n_3_t"));
                break;
            case 2:
                setColorN(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorP(GameObject.Find("n_3_lb"));
                setColorN(GameObject.Find("n_3_rb"));
                setColorP(GameObject.Find("n_3_m"));
                setColorP(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
            case 3:
                setColorN(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorN(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorP(GameObject.Find("n_3_m"));
                setColorP(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
            case 4:
                setColorP(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorN(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorP(GameObject.Find("n_3_m"));
                setColorN(GameObject.Find("n_3_b"));
                setColorN(GameObject.Find("n_3_t"));
                break;
            case 5:
                setColorP(GameObject.Find("n_3_lt"));
                setColorN(GameObject.Find("n_3_rt"));
                setColorN(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorP(GameObject.Find("n_3_m"));
                setColorP(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
            case 6:
                setColorP(GameObject.Find("n_3_lt"));
                setColorN(GameObject.Find("n_3_rt"));
                setColorP(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorP(GameObject.Find("n_3_m"));
                setColorP(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
            case 7:
                setColorN(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorN(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorN(GameObject.Find("n_3_m"));
                setColorN(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
            case 8:
                setColorP(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorP(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorP(GameObject.Find("n_3_m"));
                setColorP(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
            case 9:
                setColorP(GameObject.Find("n_3_lt"));
                setColorP(GameObject.Find("n_3_rt"));
                setColorN(GameObject.Find("n_3_lb"));
                setColorP(GameObject.Find("n_3_rb"));
                setColorP(GameObject.Find("n_3_m"));
                setColorP(GameObject.Find("n_3_b"));
                setColorP(GameObject.Find("n_3_t"));
                break;
        }
    }

    private void setColorP(GameObject gameObject)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
    private void setColorN(GameObject gameObject)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.grey;
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
