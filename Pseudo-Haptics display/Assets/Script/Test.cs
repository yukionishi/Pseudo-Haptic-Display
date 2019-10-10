using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Test : MonoBehaviour
{
    public SteamVR_Input_Sources HandType;
    public SteamVR_Action_Boolean Trigger;
    public SteamVR_Action_Boolean Grip;
    public SteamVR_Action_Boolean TrackPad;
    public SteamVR_Action_Vibration HapticAction;

    public VIVEController VC;
    VelocityEstimator VE;
    RoombaControllerScript roomba;
    ExperimentManager manager;

    public GameObject controller;

    [Header("Controller Speed")]
    public float inputSpeed;
    public float outputSpeed;
    public float CDratio;

    private Vector3 _previousVector;
    private Vector3 _currentVector = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        roomba = this.GetComponent<RoombaControllerScript>();
        manager = GameObject.Find("Manager").GetComponent<ExperimentManager>();

        controller = GameObject.FindGameObjectWithTag("Controller");
        VC = controller.GetComponent<VIVEController>();
        VE = controller.GetComponent<VelocityEstimator>();

    }

    // Update is called once per frame
    void Update()
    {
        _previousVector = _currentVector;
        _currentVector = this.transform.position;
        Debug.Log("AAA; " + (_currentVector - _previousVector));

        if (Trigger.GetStateDown(HandType))
        {
            Debug.Log("Pull Trigger Down");

            if (roomba.open)
            {
                roomba.MoveForward();
            }
            
        }
        else if (Trigger.GetStateUp(HandType))
        {
            Debug.Log("Pull Trigger Up");

            if (roomba.open)
            {
                roomba.Stop();
            }
            
        }
        else if (Trigger.GetState(HandType))
        {
            Debug.Log("Pull Trigger");
            HapticAction.Execute(1,0,100,75,HandType);
        }
        else if (Grip.GetStateUp(HandType))
        {
            Debug.Log("Grip");

            if (roomba.open)
            {
                roomba.MoveBack();
            }
            
        }
        else if (TrackPad.GetStateUp(HandType))
        {
            Debug.Log("Click TrackPad");

            if (roomba.open)
            {
                roomba.Stop();
            }
            
        }

        /* 
        Debug.Log(VC.GetMovingVector().magnitude*1000);

        inputSpeed = (int)VC.GetVelosityMagnitude();
        outputSpeed = (int)(inputSpeed * CDratio);
        */
    }

    public Vector3[] controllerPos = new Vector3[2]; 
    public float CalculateDistance()
    {
        float controllerDistance = 0;
        float roombaDistance = 0;

        controllerDistance = controllerPos[1].z - controllerPos[0].z;
        roombaDistance = controllerDistance * manager.CDratio * 100;

        return roombaDistance;
    }

}
