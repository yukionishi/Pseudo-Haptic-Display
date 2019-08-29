using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour
{
    public bool isInteract = false;
    Rigidbody rb;

    [SerializeField]
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        initialPos = GameObject.Find("Target").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            isInteract = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            isInteract = false;
        }
    }

    //targetの位置リセット
    public void ResetAgentPos()
    {
        rb.position = initialPos;

        rb.velocity = Vector3.zero;
    }
}
