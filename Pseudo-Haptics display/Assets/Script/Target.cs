﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Target : MonoBehaviour
{
    ExperimentManager experimentManager;
    RoombaControllerScript roomba;
    VIVEController controller;
    Rigidbody rb;

    [Header("Controller Infomation")]
    public int _inputSpeed = 0; 
    [SerializeField]
    private int _outputSpeed = 0;
    //[SerializeField]
    public float _CDratio = 1;
    [SerializeField]
    private int _offset = 25;

    [Header("Target Infomation")]
    private Vector3 initialPos;

    //ターゲットとエージェントの衝突判定
    public bool _isInteract = false;
    //接触状態の遷移を記録
    private bool _previousState = false;
    private bool _currentState = false;

    // Start is called before the first frame update
    void Start()
    {
        experimentManager = GameObject.Find("Manager").GetComponent<ExperimentManager>();
        roomba = GameObject.Find("Manager").GetComponent<RoombaControllerScript>();
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<VIVEController>();
        rb = this.GetComponent<Rigidbody>();

        initialPos = GameObject.Find("Target").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCollisionState();

        ChangeTargetStatus(experimentManager);

    }

    //inpuSpeed,OutputSpeedの更新
    public void UpdateControllerSpeed()
    {
        _CDratio = experimentManager.CDratio;
        _inputSpeed = (int)(controller.GetSpeedVector().magnitude * 1000) - _offset; //VIVEコントローラの速度（mm）
        _outputSpeed = (int)(_inputSpeed * _CDratio); //CD比を反映後のエージェントの速度
    }

    
    void UpdateCollisionState()
    {
        _previousState = _currentState;
        _currentState = _isInteract;        
    }

    //実験条件に応じてターゲットとルンバのふるまいを変更
    void ChangeTargetStatus(ExperimentManager experimentManager)
    {
        switch (experimentManager.expCondition)
        {
            //Visual: スクリーン内のオブジェクトはCD比＝１，ディスプレイは動かない
            case ExperimentManager.ExpCondition.Visual:
                break;

            //Visual_: スクリーン内のオブジェクトはCD比制御，ディスプレイは動かない
            case ExperimentManager.ExpCondition.Visual_:                 
                break;
                
            //Visual_Physical: スクリーン内オブジェクトはCD比制御，ディスプレイはCD比＝１
            case ExperimentManager.ExpCondition.Visual_Physical:
                MoveDisplay_staticCD();
                break;

            //Visual_Physical_: オブジェクト，ディスプレイ共にCD比制御
            case ExperimentManager.ExpCondition.Visual_Physical_:
                MoveDisplay_variableCD((int)(_inputSpeed * (_CDratio / 2)));
                break;

            //VisualPhysical_: スクリーン内のオブジェクトはCD比＝１，ディスプレイはCD比制御
            //Physical_: オブジェクトはスクリーン内で固定＋ディスプレイはCD比制御
            default:
                MoveDisplay_variableCD(_outputSpeed);

                break;
        }
    }

    //ディスプレイ（ルンバ）の動作制御（CD比制御）
    void MoveDisplay_variableCD(int roombaSpeed)
    {
        if (roomba.open)
        {
            if (_currentState == true) //両者が接触状態のとき
            {
                roomba.MoveBack(roombaSpeed);
                
            }
            else if (_currentState == false && _previousState == true) //前フレームでは接触していたが現フレームで離れたとき
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
            if (_currentState == true) //両者が接触状態のとき
            {
                roomba.MoveBack(_inputSpeed);

            }
            else if (_currentState == false && _previousState == true) //前フレームでは接触していたが現フレームで離れたとき
            {
                roomba.Stop();
            }
        }
    }

    //targetの位置リセット
    public void ResetTargetPos()
    {
        rb.position = initialPos;
        rb.velocity = Vector3.zero;
    }

    //-------------------------------------------------------------------------------------------

    //エージェントとターゲットの衝突判定->ディスプレイを動かすトリガー
    //デバック用
    private Vector3 startPos = Vector3.zero;
    private Vector3 finPos = Vector3.zero;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            _isInteract = true;

            //デバック用
            startPos = controller.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            _isInteract = false;

            //デバック用
            finPos = controller.transform.position;
            Debug.Log((finPos - startPos).magnitude * 100); //ルンバが動いた距離（cm）
        }
    }

    
    
}
