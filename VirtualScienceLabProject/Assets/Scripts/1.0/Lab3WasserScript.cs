using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab3WasserScript : MonoBehaviour
{

    private bool isSalzwasser = false;
    public GameObject salzstein;
    public GameObject stein1;
    public GameObject stein2;
    public GameObject nachricht;

    // Use this for initialization
    void Start()
    {
       
    }


    //Coroutine um den Salzstein zu zersetzen
    IEnumerator salzsteinZersetzen(int sec)
    {
        //6 sekunden warten
        yield return new WaitForSeconds(sec);
        Debug.Log("Salzstein im Wasserbecken in TriggerArea");
           
        //Salzstein obj zerstören, danach haben wir gut leitbares Salzwasser
        Destroy(salzstein);
        isSalzwasser = true;

        //Nachricht einblenden
        nachricht.SetActive(true);
        StartCoroutine(nachrichtAusblenden(6));
    }


    //Nachricht soll nach ein paar sekunden wieder verschwinden
    IEnumerator nachrichtAusblenden(int sec)
    {
        yield return new WaitForSeconds(sec);
        nachricht.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

    }



    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == salzstein)
        {
            //Salzstein soll sich nach 3 sekunden zersetzt haben
            StartCoroutine(salzsteinZersetzen(3));
           
        }else if (col.gameObject == stein1)
        {
            Destroy(stein1);
        }else if (col.gameObject == stein2)
        {
            Destroy(stein2);
        }
        
    }

    public bool getIsSalzwasser()
    {
        return isSalzwasser;
    }

}
