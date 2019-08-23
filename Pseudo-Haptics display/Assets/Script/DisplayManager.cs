using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;

public class DisplayManager : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private GameObject camera2;
    private GameObject displayScreen;

    ExperimentManager experimentManager;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Target");
        displayScreen = GameObject.Find("DisplayScreen");
        experimentManager = GameObject.Find("Manager").GetComponent<ExperimentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlDisplayScreen(experimentManager);
    }

    private void ControlDisplayScreen(ExperimentManager experimentManager)
    {
        if(experimentManager.expCondition == ExperimentManager.ExpCondition.Physical_)
        {
            displayScreen.transform.position = target.transform.position;
        }
    }
}
