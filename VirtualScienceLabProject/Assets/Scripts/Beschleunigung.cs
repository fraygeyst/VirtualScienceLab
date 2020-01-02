using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beschleunigung : MonoBehaviour {


    // Use this for initialization
    void Start () {
		// Counter auf 0 setzen
        Sev_Seg_Counter counti = new Sev_Seg_Counter();
        counti.setSevSegCount(0, "Zähler1");
    }
	
	// Update is called once per frame
	void Update () {
		// Counter auf Standard setzen und aktualisieren wenn geändert
        Sev_Seg_Counter counti = new Sev_Seg_Counter();
        counti.setSevSegCount(Load_Publics.Temperatur, "Zähler1");
    }

	// Bei Controller Berührung
    private void OnTriggerEnter(Collider other)
    {
		// Abfrage ob + oder - gedrückt wurde
        switch (gameObject.name)
        {
            case "plus":
			// Wenn Button aktiv ist und Max bzw Min Temp noch nicht erreicht
                if (Load_Publics.plus_act && Load_Publics.Temperatur < Load_Publics.Temp_Max)
                {
					// Temperatur Änderung und Button deaktivieren
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

	// Deaktiviert Button für 1 Sekunde und aktiviert danach wieder
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
        Color color = new Color();

        if (isplus)
        {
            Load_Publics.plus_act = true;
            if(ColorUtility.TryParseHtmlString("#FF0000", out color))
            {
                button_mol.GetComponent<Renderer>().material.color = color;
            }
        }
        else
        {
            Load_Publics.min_act = true;
            if (ColorUtility.TryParseHtmlString("#00FFFF", out color))
            {
                button_mol.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}
