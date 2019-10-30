using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardioideScript : MonoBehaviour {

    // Instantiates prefabs in a circle formation
    public GameObject prefab_kleine_wurfel;
    public int numberOfObjects = 40;
    public float radius = 120f;
    public GameObject prefab_hslogo;
    public GameObject prefab_hskl_schriftzug;
    public GameObject prefab_flameParticleSystem;

    //verschiebt das Gesamtbild
    private Vector3 verschiebungsVector = new Vector3(60, 0, -500);

    void Start()
    {

        //Erzeugt einen Kreis mit dem eingestellten radius mit den HSKL logo prefabs
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;

            //Kreis
            //float x = Mathf.Cos(angle) * radius;
            //float y = Mathf.Sin(angle) * radius;


            //Kardioide
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float x = radius * cos * (1 - cos);
            float y = radius * sin * (1 - cos);

            Vector3 pos = transform.position + new Vector3(x, y, 0) + verschiebungsVector;
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            Instantiate(prefab_kleine_wurfel, pos, rot);



        }



        //hskl_logo_ oben erzeugen
        //fuer Kreis
        //Vector3 pos2 = transform.position + new Vector3(0, 40, 0) + verschiebungsVector;

        //fuer Kardioide
        Vector3 pos2 = transform.position + new Vector3(-100, 40, 0) + verschiebungsVector;

        float angle2 = Mathf.PI * 2 / numberOfObjects;
        float angleDegrees2 = -angle2 * Mathf.Rad2Deg;
        Quaternion rot2 = Quaternion.Euler(0, angleDegrees2, 0);
        Instantiate(prefab_hslogo, pos2, rot2);




        //hskl schriftzug prefab unten erzeugen
        //fuer Kreis
        //Vector3 pos3 = transform.position + new Vector3(0, -40, 0) + verschiebungsVector;

        //fuer Kardioide
        Vector3 pos3 = transform.position + new Vector3(-100, -40, 0) + verschiebungsVector;

        Instantiate(prefab_hskl_schriftzug, pos3, rot2);





        //FlameParticleSystem Feuer bei der Kardioide erzeugen
        Vector3 pos1 = transform.position + new Vector3(-100, 60, 0) + verschiebungsVector;
        Instantiate(prefab_flameParticleSystem, pos1, rot2);

    }
}
