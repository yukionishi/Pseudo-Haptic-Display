using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Target : MonoBehaviour
{
    Manager manager;
    RoombaControllerScript roomba;
    VIVEController vive;
    Rigidbody rigidbody;

    [SerializeField]
    private int inputSpeed = 0; 
    [SerializeField]
    private int outputSpeed = 0;
    [SerializeField]
    private float CDratio = 1;

    [SerializeField]
    private Vector3 initialPos;

    //ターゲットとエージェントの衝突判定
    public bool isInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        roomba = GameObject.Find("Manager").GetComponent<RoombaControllerScript>();
        vive = GameObject.FindGameObjectWithTag("Controller").GetComponent<VIVEController>();
        rigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CDratio = manager.CDratio;
        inputSpeed = (int)vive.GetVelosityMagnitude(); //VIVEコントローラの速度（mm/s）
        outputSpeed = (int)(inputSpeed * CDratio); //CD比を反映後のエージェントの速度

        UpdateCollisionState();

        ChangeTargetStatus(manager);

    }

    //接触状態の遷移を記録
    private bool previousState = false;
    private bool currentState = false;
    void UpdateCollisionState()
    {
        previousState = currentState;
        currentState = isInteract;        
    }

    //実験条件に応じてターゲットとルンバのふるまいを変更
    void ChangeTargetStatus(Manager manager)
    {
        switch (manager.expCondition)
        {
            //Visual: スクリーン内のオブジェクトはCD比＝１，ディスプレイは動かない
            case Manager.ExpCondition.Visual:
                break;

            //Visual_: スクリーン内のオブジェクトはCD比制御，ディスプレイは動かない
            case Manager.ExpCondition.Visual_:                 
                break;
                
            //Visual_Physical: スクリーン内オブジェクトはCD比制御，ディスプレイはCD比＝１
            case Manager.ExpCondition.Visual_Physical:
                MoveDisplay_staticCD();
                break;

            //VisualPhysical_: スクリーン内のオブジェクトはCD比＝１，ディスプレイはCD比制御
            //Visual_Physical_: スクリーン内のオブジェクトとディスプレイ共にCD比制御
            //Physical_: オブジェクトはスクリーン内で固定＋ディスプレイはCD比制御
            default:
                MoveDisplay_variableCD();

                break;
        }
    }

    //ディスプレイ（ルンバ）の動作制御（CD比制御）
    void MoveDisplay_variableCD()
    {
        if (roomba.open)
        {
            if (currentState == true) //両者が接触状態のとき
            {
                roomba.MoveForward(outputSpeed);
                
            }
            else if (currentState == false && previousState == true) //前フレームでは接触していたが現フレームで離れたとき
            {
                roomba.Stop();
            }
        }
    }
    //ディスプレイ（ルンバ）の動作制御（CD比＝１）
    void MoveDisplay_staticCD()
    {
        if (roomba.open)
        {
            if (currentState == true) //両者が接触状態のとき
            {
                roomba.MoveForward(inputSpeed);

            }
            else if (currentState == false && previousState == true) //前フレームでは接触していたが現フレームで離れたとき
            {
                roomba.Stop();
            }
        }
    }

    //エージェントとターゲットの衝突判定->ディスプレイを動かすトリガー
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Agent")
        {
            isInteract = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Agent")
        {
            isInteract = false;
        }
    }

    //targetの位置リセット
    public void ResetAgentPos()
    {
        this.transform.position = initialPos;
        this.transform.eulerAngles = Vector3.zero;
    }
}
