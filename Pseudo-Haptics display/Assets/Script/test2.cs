using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    ExperimentManager experimentManager;
    GameObject controller;
    VIVEController viveController;
    Target target;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        experimentManager = GameObject.Find("Manager").GetComponent<ExperimentManager>();
        controller = GameObject.FindGameObjectWithTag("Controller");
        viveController = controller.GetComponent<VIVEController>();
        target = GameObject.Find("Target").GetComponent<Target>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ControlMovement();
    }

    void ControlMovement()
    {
        Vector3 TransformVector = Vector3.zero;

        Debug.Log(viveController.GetMovingVector());
                
        //ターゲット接触時にエージェント側の移動量を操作
        if (target.isInteract)
        {

            //CD比を反映した移動ベクトル
            TransformVector = viveController.GetMovingVector() * experimentManager.CDratio;

            //Debug.Log(TransformVector);

            rb.position += TransformVector;
        }
        else
        {
            rb.position += viveController.GetMovingVector();
        }

        //回転はコントローラに同期
        rb.rotation = controller.transform.rotation;

       // Debug.Log(viveController.GetMovingVector().ToString("F4"));

    }
}
