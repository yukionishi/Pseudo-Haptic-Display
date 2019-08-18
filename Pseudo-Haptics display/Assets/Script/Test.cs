﻿using System.Collections;
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

    [SerializeField]
    private VelocityEstimator VE;

    RoombaControllerScript roomba;

    // Start is called before the first frame update
    void Start()
    {
        roomba = this.GetComponent<RoombaControllerScript>();
        
        VE = GameObject.FindGameObjectWithTag("Controller").GetComponent<VelocityEstimator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        
    }

    

}
