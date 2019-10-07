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
        if(seconds == 0)
        {
            set_n_1(0);
            set_n_2(0);
            set_n_3(0);
        } else
        {
            List<int> digits = new List<int>();
            while (seconds > 0)
            {
                digits.Add(seconds % 10);
                seconds = seconds / 10;
            }
            digits.Reverse();
            int sizeOfList = digits.Count;
            if (sizeOfList == 1)
            {
                set_n_1(0);
                set_n_2(0);
                set_n_3(digits[0]);
            }
            else if (sizeOfList == 2)
            {
                set_n_1(0);
                set_n_2(digits[0]);
                set_n_3(digits[1]);
            }
            else if (sizeOfList == 3)
            {
                set_n_1(digits[0]);
                set_n_2(digits[1]);
                set_n_3(digits[2]);
            }
        }
    }

    private void set_n_1(int number)
    {
        switch (number)
        {
            case 0:
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
        if(Load_Publics.counter <= Load_Publics.maximum)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        } else
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
