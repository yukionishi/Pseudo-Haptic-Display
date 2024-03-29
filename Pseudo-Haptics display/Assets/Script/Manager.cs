﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;
using Valve.VR.InteractionSystem;

public class Manager : MonoBehaviour
{
    private GameObject camera1;
    private GameObject camera2;
    private GameObject cameraRig;
    private GameObject controllerModel_left;
    private GameObject controllerModel_right;

    /// <summary>
    /// 実験条件
    /// </summary>
    public enum ExpCondition
    {
        Visual, //視覚情報CD比＝１（ベースライン）
        Visual_, //視覚情報をCD比操作，ディスプレイ固定
        Physical_, //視覚情報固定，ディスプレイ動作をCD比操作
        Visual_Physical, //視覚情報CD比操作，ディスプレイ動作CD比＝１
        VisualPhysical_, //視覚情報CD比＝１，ディスプレイ動作CD比操作
        Visual_Physical_ //視覚情報CD比操作，ディスプレイ動作CD比操作
    }
    public ExpCondition expCondition;

    [HideInInspector]
    public Target target;
    private Agent agent;
    private TrackerPosition trackerPos;

    /// <summary>
    /// 各クラスのパラメータ操作用
    /// </summary>
    //カーソルのキャリブレーションモード切替
    public bool isCalibrate = false;
    public float DistanceCameraToHand = 0;
    public float CDratio = 1;

    /// <summary>
    /// ルンバの移動距離計測用
    /// </summary>
    public float RoombaMoveDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera1 = GameObject.Find("Camera1");
        camera2 = GameObject.Find("Target").transform.GetChild(0).gameObject;
        cameraRig = GameObject.Find("[CameraRig]");
        controllerModel_left = cameraRig.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        controllerModel_right = cameraRig.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        target = GameObject.FindGameObjectWithTag("Target").GetComponent<Target>();
        agent = GameObject.FindGameObjectWithTag("Agent").GetComponent<Agent>();
        trackerPos = GameObject.FindGameObjectWithTag("Tracker").GetComponent<TrackerPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateControllerModel();

        ResetTargetPos();

        ChangeUseCamera();

        MesureDistance();
    }

    //ターゲットとエージェントの位置をリセット
    void ResetTargetPos()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            target.ResetTargetPos();
            agent.ResetAgentPos();
        }
    }

    //キャリブレーションモード = falseの時コントローラのrender modelを消去
    void ActivateControllerModel()
    {
        if (!isCalibrate)
        {
            controllerModel_left.SetActive(false);
            controllerModel_right.SetActive(false);
        }
        else
        {
            controllerModel_left.SetActive(true);
            controllerModel_right.SetActive(true);
        }
    }

    //使用するカメラの位置を切替え
    void ChangeUseCamera()
    {
        switch (expCondition)
        {
            //Physical: ターゲットにカメラが追従，GameSceneではターゲットが固定されて見える
            case ExpCondition.Physical_: 
                camera1.SetActive(false);
                camera2.SetActive(true);
                break;

            //VisualOnly,VisualPhysical: カメラ位置は固定，スクリーン上ではターゲットが移動している様子を映す
            default:
                camera1.SetActive(true);
                camera2.SetActive(false);
                break;
        }
    }

    //ルンバの移動距離を計測
    void MesureDistance()
    {
        RoombaMoveDistance = trackerPos.CalculateDistance();
    }


}
