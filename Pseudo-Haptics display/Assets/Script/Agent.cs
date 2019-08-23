using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    ExperimentManager experimentManager;
    GameObject controller;
    VIVEController viveController;
    Target target;
    Animator animator;
    
    private bool isCalibrate = false;
    private float DistanceCameraToHand = 0;
    [SerializeField]
    private Vector3 offsetPos = Vector3.zero;
    private Rigidbody rigidbody;
        
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        experimentManager = GameObject.Find("Manager").GetComponent<ExperimentManager>();
        controller = GameObject.FindGameObjectWithTag("Controller");
        viveController = controller.GetComponent<VIVEController>();
        target = GameObject.Find("Target").GetComponent<Target>();

        if (GameObject.Find("RightHand").activeInHierarchy == true)
        {
            animator = GameObject.Find("RightHand").GetComponent<Animator>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {       
        CaliblateHandPos();

        ControlMovement(experimentManager);
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
            this.transform.position = controller.transform.position + offsetPos;
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
            rigidbody.position = controller.transform.position + offsetPos;
            rigidbody.rotation = controller.transform.rotation;
        }
        //Visual_, Physical_, Visual_Physical, Visual_Physical_: CD比を操作する条件
        else
        {
            //ターゲット接触時にエージェント側の移動量を操作
            if (target.isInteract)
            {
                //CD比を反映した移動ベクトル
                TransformVector = viveController.GetMovingVector() * experimentManager.CDratio;

                rigidbody.position += TransformVector;
            }
            else
            {
                rigidbody.position += viveController.GetMovingVector();
            }

            //回転はコントローラに同期
            rigidbody.rotation = controller.transform.rotation;
        }

    }

    //エージェントの位置リセット
    public void ResetAgentPos()
    {
        rigidbody.position = controller.transform.position + offsetPos;
        rigidbody.rotation = controller.transform.rotation;
    }

    //Handモデルのジェスチャの変更
    public void ChangeHandAnimation(bool grip)
    {
        if (grip)
        {
            animator.SetBool("Point", true);
        }
        else
        {
            animator.SetBool("Point", false);
        }
    }
}
