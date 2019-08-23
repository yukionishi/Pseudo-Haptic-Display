using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerInput : MonoBehaviour
{
    [SerializeField]
    private Agent agent;

    public SteamVR_Input_Sources HandType;
    public SteamVR_Action_Boolean Trigger;

    //[HideInInspector]
    public bool grip = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("RightHand").activeInHierarchy == true)
        {
            agent = GameObject.Find("RightHand").GetComponent<Agent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Trigger.GetStateDown(HandType))
        {
            grip = true;
            agent.ChangeHandAnimation(grip);
            
        }
        else if (Trigger.GetStateUp(HandType))
        {
            grip = false;
            agent.ChangeHandAnimation(grip);
        }
    }
}
