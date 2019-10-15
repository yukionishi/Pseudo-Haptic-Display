using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    ExperimentManager experimentManager;
    GameObject controller;
    VIVEController viveController;
    Target target;

    Test test;
    
    private bool isCalibrate = false;
    private float DistanceCameraToHand = 0;
    [SerializeField]
    private Vector3 offsetPos = Vector3.zero;
    private Rigidbody rb;
    private Animator anim;

    private Vector3 _previousVector = Vector3.zero;
    private Vector3 _transformVector = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        experimentManager = GameObject.Find("Manager").GetComponent<ExperimentManager>();
        controller = GameObject.FindGameObjectWithTag("Controller");
        viveController = controller.GetComponent<VIVEController>();
        target = GameObject.Find("Target").GetComponent<Target>();
        anim = this.GetComponent<Animator>();

        test = GameObject.Find("Manager").GetComponent<Test>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CaliblateHandPos();
        }
    }

    private void LateUpdate()
    {       
        _transformVector = viveController.GetMovingVector(); //コントローラの移動ベクトル取得

        ControlMovement(experimentManager);
    }

    //エージェントの位置キャリブレーション（オフセットの計算）
    void CaliblateHandPos()
    {

        //キャリブレーションモードの判定
        if (experimentManager.isCalibrate)
        {
            //GameViewでマウスがクリックした座標を初期位置
            Vector3 HandPos = Input.mousePosition;
            HandPos.z = experimentManager.DistanceCameraToHand;

            Vector3 _HandPos = Camera.main.ScreenToWorldPoint(HandPos);

            //オフセットの算出
            offsetPos = _HandPos - controller.transform.position;

            //オフセット分を加味したカーソル位置
            this.transform.position = controller.transform.position + offsetPos;         
        }
       

    }

    /// <summary>
    /// エージェントの動作制御
    /// </summary>
    /// <param name="experimentManager"></param>
    void ControlMovement(ExperimentManager experimentManager)
    {

        //VisualPhysical_: カーソル側のCD比を操作しない条件
        if (experimentManager.expCondition == ExperimentManager.ExpCondition.VisualPhysical_)
        {
            //コントローラとエージェントの位置・回転を同期
            this.transform.position += _transformVector;
            this.transform.rotation = controller.transform.rotation;
        }
        //Visual_, Physical_, Visual_Physical, Visual_Physical_: CD比を操作する条件
        else
        {
            //ターゲット接触時にエージェント側の移動量を操作
            if (target._isInteract)
            {
                this.transform.position +=_transformVector * experimentManager.CDratio;
            }
            else
            {
                this.transform.position += _transformVector;
            }

            //回転はコントローラに同期
            this.transform.rotation = controller.transform.rotation;
        }

    }

    //エージェントの位置リセット
    public void ResetAgentPos()
    {
        transform.position = controller.transform.position + offsetPos;
    }


    //デバック用
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            test.controllerPos[0]=controller.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            test.controllerPos[1]=controller.transform.position;

            test.CalculateDistance();

            Debug.Log("Distance of Roomba Movement: " + test.CalculateDistance());
        }
    }

}
