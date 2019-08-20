using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    private GameObject target;
    private GameObject camera2;
    private GameObject displayScreen;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Target");
        camera2 = target.transform.GetChild(0).gameObject;
        displayScreen = GameObject.Find("DisplayScreen");
    }

    // Update is called once per frame
    void Update()
    {
        if(camera2.activeInHierarchy == true)
        {
            displayScreen.transform.position = target.transform.position;
        }
    }
}
