using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerInput : MonoBehaviour
{
    [SerializeField]
    private HandAgent hand;

    public SteamVR_Input_Sources HandType;
    public SteamVR_Action_Boolean Trigger;

    //[HideInInspector]
    public bool grip = false;

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Agent").GetComponent<HandAgent>();

        //if (GameObject.Find("RightHand").activeInHierarchy == true)
        //{
        //    agent = GameObject.Find("RightHand").GetComponent<Agent>();
        //}
        //else {
        //    agent = GameObject.FindGameObjectWithTag("Agent").GetComponent<Agent>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Trigger.GetStateDown(HandType))
        {
            Debug.Log("Pull Trigger Down");
            hand.GrabBox();
        }
        else if (Trigger.GetStateUp(HandType))
        {
            Debug.Log("Pull Trigger Up");
            hand.ReleaseBox();
        }
    }
}
