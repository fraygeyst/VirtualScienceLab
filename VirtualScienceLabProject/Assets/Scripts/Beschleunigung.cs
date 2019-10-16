using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beschleunigung : MonoBehaviour {


    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        Sev_Seg_Counter counti = new Sev_Seg_Counter();
        counti.setSevSegCount(Load_Publics.Temperatur);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (gameObject.name)
        {
            case "plus":
                if (Load_Publics.plus_act && Load_Publics.Temperatur < Load_Publics.Temp_Max)
                {
                    Load_Publics.Temperatur += 25;
                    Debug.Log("wärmer");
                    StartCoroutine(waiter(true));
                }
                break;
            case "minus":
                if (Load_Publics.min_act && Load_Publics.Temperatur > Load_Publics.Temp_Min)
                {
                    Load_Publics.Temperatur -= 25;
                    Debug.Log("kälter");
                    StartCoroutine(waiter(false));
                }
                break;
        }
    }

    IEnumerator waiter(bool isplus)
    {
        GameObject button_mol = null;
        if (isplus)
        {
            button_mol = GameObject.Find("plus");
            Load_Publics.plus_act = false;
        } else
        {
            button_mol = GameObject.Find("minus");
            Load_Publics.min_act = false;
        }
        button_mol.GetComponent<Renderer>().material.color = Color.yellow;

        yield return new WaitForSeconds(1);    //Wait 1 Second

        if (isplus)
        {
            Load_Publics.plus_act = true;
        }
        else
        {
            Load_Publics.min_act = true;
        }
        button_mol.GetComponent<Renderer>().material.color = Color.green;
    }
}
