using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electro_lampe : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Display_Meter_5_D counti = new Display_Meter_5_D();
        counti.setDisplay(0, "Zähler_Netzteil");
        counti.setDisplay(0, "Zähler_Volt");
        counti.setDisplay(0, "Zähler_Ampere");
        setLight(0, "Glowlight");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        switch (gameObject.name)
        {
            case "Button_links_plus":
                if(Load_Publics.lampe_netzteil_count <= 6)
                {
                    Load_Publics.lampe_netzteil_count += 0.5;
                }
                break;
            case "Button_links_minus":
                if(Load_Publics.lampe_netzteil_count >= 0.5)
                {
                    Load_Publics.lampe_netzteil_count -= 0.5;
                }
                break;
            case "Button_mitte_plus":
                if (Load_Publics.lampe_netzteil_count <= 6.4)
                {
                    Load_Publics.lampe_netzteil_count += 0.1;
                }
                break;
            case "Button_mitte_minus":
                if (Load_Publics.lampe_netzteil_count >= 0.1)
                {
                    Load_Publics.lampe_netzteil_count -= 0.1;
                }
                break;
        }
        Display_Meter_5_D counti = new Display_Meter_5_D();
        counti.setDisplay(Load_Publics.lampe_netzteil_count, "Zähler_Netzteil");
        counti.setDisplay(getVolt(Load_Publics.lampe_netzteil_count), "Zähler_Volt");
        counti.setDisplay(getAmpere(Load_Publics.lampe_netzteil_count), "Zähler_Ampere");

        setLight((float)Load_Publics.lampe_netzteil_count, "Glowlight");
    }

    private double getVolt(double x)
    {
        x = (-0.0490743420159 * Math.Pow(x, 6)) + (1.0680459493943 * Math.Pow(x, 5)) - (9.1705441592139 * Math.Pow(x, 4)) + (39.5654453080791 * Math.Pow(x, 3)) - (92.0907855870632 * Math.Pow(x, 2)) + (148.3179610169378 * x) + 0.1029496480702;
        return x;
    }

    private double getAmpere(double x)
    {
        x = (0.0012809183618 * Math.Pow(x, 4)) - (0.019316637036 * Math.Pow(x, 3)) + (0.106348881997 * Math.Pow(x, 2)) + (0.6547898739782 * x) - 0.0044123552528;
        return x;
    }

    private void setLight(float input, string obj_name)
    {
        GameObject go = GameObject.Find(obj_name);
        Light lt = go.GetComponent<Light>();
        lt.intensity = Load_Publics.RemapLight(input, 0f, 10f, 0f, 5f);
    }
}
