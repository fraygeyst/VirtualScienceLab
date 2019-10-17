using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_Publics : MonoBehaviour {

    // Informatiklab
    // Dijkstra
    internal static readonly bool reset_clicked;
    public static string Dijkstra_Word;
    public static bool s_active = true;
    public static bool a_active = false;
    public static bool b_active = false;
    public static bool g_active = false;
    public static bool c_active = false;
    public static bool d_active = false;
    public static bool e_active = false;
    public static bool f_active = false;
    public static bool z_active = false;
    public static bool r_active = false;

    public static bool s_clicked = false;
    public static bool a_clicked = false;
    public static bool b_clicked = false;
    public static bool g_clicked = false;
    public static bool c_clicked = false;
    public static bool d_clicked = false;
    public static bool e_clicked = false;
    public static bool f_clicked = false;
    public static bool z_clicked = false;
    public static bool r_clicked = false;

    public static int counter = 0;
    public static int maximum = 21;
    public static string last_clicked = "";


    // Bubblesort
    public static int b_state = 0;
    public static bool bubble_active = true;
    public static bool s_1_act = true;
    public static bool s_2_act = true;
    public static bool s_3_act = true;
    public static bool s_4_act = true;
    public static bool s_5_act = true;
    public static bool s_6_act = true;
    public static bool s_7_act = true;

    // BioLab
    public static string scene_change = "";
    public static bool bio_collision_happened = false;

    // Teilchenlabor
    // Molekuele
    public static int Temperatur = 25;
    public static bool min_act = true;
    public static bool plus_act = true;
    public static float Temp_Max = 200f;
    public static float Temp_Min = 0f;
    public static float Map_Temp_Max = 0.25f;
    public static float Map_Temp_Min = 0f;
    public static float move_speed_multi = 15;
    public static float RemapTemp(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;
        var normal = fromAbs / fromMaxAbs;
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
        var to = toAbs + toMin;

        return to;
    }

    // Mathelab
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
