using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savetest : MonoBehaviour
{


    public int a = 5;
    public int[] b = { 1, 2, 3, 4, 5 };
    public float c = 10f;
    public string str = "string";
    public



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            a = 0;
            b = new int[5];
            c = 0f;
            str = "";

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ES3.Save("a", a);
            ES3.Save("b", b);
            ES3.Save("c", c);
            ES3.Save("str", str);


            Debug.Log("Save Data");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

            a = ES3.Load<int>("a");
            b = ES3.Load<int[]>("b");
            c = ES3.Load<float>("c");
            str = ES3.Load<string>("str");

           

            Debug.Log("Load Data");
        }

    }
}
