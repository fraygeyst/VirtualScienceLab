using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_Publics : MonoBehaviour {

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

    // BioLab
    public static string scene_change = "";
    public static bool bio_collision_happened = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
