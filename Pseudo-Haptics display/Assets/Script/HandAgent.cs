using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAgent : MonoBehaviour
{
    ExperimentManager experimentManager;
    GameObject controller;
    VIVEController viveController;
    Target target;

    private bool isCalibrate = false;
    private bool grab = false;
    private float DistanceCameraToHand = 0;
    [SerializeField]
    private Vector3 offsetPos = Vector3.zero;
    private Rigidbody rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        experimentManager = GameObject.Find("Manager").GetComponent<ExperimentManager>();
        controller = GameObject.FindGameObjectWithTag("Controller");
        viveController = controller.GetComponent<VIVEController>();
        target = GameObject.Find("Target").GetComponent<Target>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CaliblateHandPos();

        //ControlMovement(experimentManager);
        if (grab)
        {
            ControlHandPosition();
        }
        else
        {

        }
    }

    //エージェントの位置キャリブレーション
    void CaliblateHandPos()
    {
        isCalibrate = experimentManager.isCalibrate;
        DistanceCameraToHand = experimentManager.DistanceCameraToHand;

        //キャリブレーションモードの判定
        if (isCalibrate)
        {
            //GameViewでマウスがクリックした座標を初期位置
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 HandPos = Input.mousePosition;
                HandPos.z = DistanceCameraToHand;

                Vector3 _HandPos = Camera.main.ScreenToWorldPoint(HandPos);

                offsetPos = _HandPos - controller.transform.position;
            }

            //offsetを考慮した位置にカーソルをキャリブレーション
            rb.position = controller.transform.position + offsetPos;
            rb.rotation = controller.transform.rotation;
        }
    }

    /// <summary>
    /// エージェントの動作制御
    /// </summary>
    /// <param name="experimentManager"></param>
    void ControlMovement(ExperimentManager experimentManager)
    {
        Vector3 TransformVector = Vector3.zero;

        //VisualPhysical_: CD比を操作しない条件
        if (experimentManager.expCondition == ExperimentManager.ExpCondition.VisualPhysical_)
        {
            //コントローラとエージェントの位置・回転を同期
            rb.position = controller.transform.position + offsetPos;
            rb.rotation = controller.transform.rotation;
        }
        //Visual_, Physical_, Visual_Physical, Visual_Physical_: CD比を操作する条件
        else
        {
            //ターゲット接触時にエージェント側の移動量を操作
            if (target._isInteract)
            {
                //CD比を反映した移動ベクトル
                TransformVector = viveController.GetMovingVector() * experimentManager.CDratio;

                rb.position += TransformVector;
            }
            else
            {
                rb.position += viveController.GetMovingVector();
            }

            //回転はコントローラに同期
            rb.rotation = controller.transform.rotation;
        }

    }
    //-------------------------------------------------

    //オブジェクトの把持判定
    public void GrabBox()
    {
        anim.SetTrigger("GrabBox");

        CaliblateInitialHandPos();

        grab = true;
    }

    //オブジェクトのリリース判定
    public void ReleaseBox()
    {
        anim.SetTrigger("ReleaseBox");

        grab = false;
    }

    //手モデルの移動
    public void CaliblateInitialHandPos()
    {
        Vector3 targetPos = target.transform.position;
        rb.position = new Vector3(targetPos.x - 0.07f, targetPos.y, targetPos.z - 0.07f);
    }

    //手モデルの位置更新
    public void ControlHandPosition()
    {
        Vector3 TransformVector = Vector3.zero;

        TransformVector = viveController.GetMovingVector();

        rb.position += new Vector3(0, 0, TransformVector.z);
    }

    //-------------------------------------------------

    //エージェントの位置リセット
    public void ResetAgentPos()
    {
        rb.position = controller.transform.position + offsetPos;
        rb.rotation = controller.transform.rotation;
    }

}
