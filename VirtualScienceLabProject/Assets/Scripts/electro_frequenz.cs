using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electro_frequenz : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Display_Meter_5_D counti = new Display_Meter_5_D();
        counti.setDisplay(Load_Publics.frequency_Netzteil, "Zähler_Netzteil_v2");
        counti.setDisplay(0, "Zähler_Volt_v2");
        counti.setDisplay(0, "Zähler_Ampere_v2");
        counti.setDisplay(4, "Zähler_Uq_v2");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        float new_freq = Load_Publics.frequency_Netzteil;
        switch (gameObject.name)
        {
            case "Button_links_plus":
                if (Load_Publics.frequency_Netzteil < 100f)
                {
                    new_freq += 10f;
                }
                else if (Load_Publics.frequency_Netzteil >= 100 && Load_Publics.frequency_Netzteil < 1000f)
                {
                    new_freq += 100f;
                }
                else if(Load_Publics.frequency_Netzteil >= 1000f)
                {
                    new_freq += 500f;
                }
                break;
            case "Button_links_minus":
                if (Load_Publics.frequency_Netzteil <= 100f)
                {
                    new_freq -= 10f;
                }
                else if (Load_Publics.frequency_Netzteil > 100 && Load_Publics.frequency_Netzteil <= 1000f)
                {
                    new_freq -= 100f;
                }
                else if (Load_Publics.frequency_Netzteil > 1000f)
                {
                    new_freq -= 500f;
                }
                break;
            case "Button_mitte_plus":
                if (Load_Publics.frequency_Netzteil < 100f)
                {
                    new_freq += 5f;
                }
                else if (Load_Publics.frequency_Netzteil >= 100f && Load_Publics.frequency_Netzteil < 1000f)
                {
                    new_freq += 50f;
                }
                else if (Load_Publics.frequency_Netzteil >= 1000f)
                {
                    new_freq += 100f;
                }
                break;
            case "Button_mitte_minus":
                if (Load_Publics.frequency_Netzteil <= 100f)
                {
                    new_freq -= 5f;
                }
                else if (Load_Publics.frequency_Netzteil > 100f && Load_Publics.frequency_Netzteil <= 1000f)
                {
                    new_freq -= 50f;
                }
                else if (Load_Publics.frequency_Netzteil > 1000f)
                {
                    new_freq -= 100f;
                }
                break;
        }
        if (new_freq < 0)
        {
            new_freq = 0;
        }
        else
        {
            Load_Publics.lampe_netzteil_count = new_freq;
        }

        Display_Meter_5_D counti = new Display_Meter_5_D();
        counti.setDisplay(System.Convert.ToDouble(Load_Publics.frequency_Netzteil), "Zähler_Netzteil_v2");
        counti.setDisplay(getUc(Load_Publics.Uq, Load_Publics.Uq), "Zähler_Volt_v2");
        counti.setDisplay(getUr(Load_Publics.Uq, Load_Publics.Uq), "Zähler_Ampere_v2");
    }

    float getUq(float Ur, float Uc)
    {
        return Mathf.Sqrt(Mathf.Pow(2, Ur) + Mathf.Pow(2, Uc));
    }
    float getUr(float Uq, float Uc)
    {
        return Mathf.Sqrt(Mathf.Pow(2, Uq) - Mathf.Pow(2, Uc));
    }
    float getUc(float Uq, float Ur)
    {
        return Mathf.Sqrt(Mathf.Pow(2, Uq) - Mathf.Pow(2, Ur));
    }
    float getW(float f)
    {
        return 2 * Mathf.PI * f;
    }
    float getAcF(float Uc, float Uq)
    {
        return Mathf.Abs(Uc) / Mathf.Abs(Uq);
    }
    float getAcF2(float Uc, float Uq)
    {
        return 20 * Mathf.Log10(Uc / Uq);
    }
    float getAcF3(float f, float r, float c)
    {
        return 20 * Mathf.Log10(1 / (Mathf.Sqrt(1 + Mathf.Pow(2, 2 * Mathf.PI * f * r * c))));
    }
    float getArF(float Ur, float Uq)
    {
        return 20 * Mathf.Log10(Ur / Uq);
    }
    float getArF2(float f, float r, float c)
    {
        return 20 * Mathf.Log10((2 * Mathf.PI * f * r * c) / (Mathf.Sqrt(1 + Mathf.Pow(2, 2 * Mathf.PI * f * r * c))));
    }
    float getPhiCF(float UrF, float UcF)
    {
        return -1 * Mathf.Atan(UrF / UcF);
    }
    float getPhiCF2(float f, float r, float c)
    {
        return -1 * Mathf.Atan(2 * Mathf.PI * f * r * c);
    }
    float getGamma(float r, float c)
    {
        return r * c;
    }
    float getGamma2(float r, float c)
    {
        return r * c;
    }
    float getWGamma(float r, float c)
    {
        return 1 / (r * c);
    }
}
