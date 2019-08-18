using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VIVEController : MonoBehaviour
{
    public SteamVR_Input_Sources HandType;
    public SteamVR_Action_Boolean Trigger;

    public VelocityEstimator VE;

    private int frameCount;
    private float time;
    //フレームレートを更新する時間定数
    [SerializeField]
    private float interval;
    //フレームレート
    [SerializeField]
    private int fps = 0;

    void Start()
    {
        VE = this.GetComponent<VelocityEstimator>();

        //再生と同時にコントローラー速度の計測を開始
        if (!VE.estimateOnAwake)
        {
            VE.estimateOnAwake = true;
        }

        frameCount = 0;
    }

    void Update()
    {
        //フレームレートの計算
        frameCount++;
        time += Time.deltaTime;
        if (time > interval)
        {
            //整数値でFPS算出
            fps = (int)(frameCount / time);
            //VelocityEstimatorスクリプトの速度計算を行うフレームレート数に渡す
            VE.velocityAverageFrames = fps;

            frameCount = 0;
            time = 0;
        }
    }

    //トリガーの入力判定
    [SerializeField]
    bool pull = false;
    public bool TriggerState()
    {

        if (Trigger.GetStateDown(HandType))
        {
            pull = true;
        }
        else if (Trigger.GetStateUp(HandType))
        {
            pull = false;
        }

        return pull;
    }

    //コントローラーの速度（mm/s）取得
    public float GetVelosityMagnitude()
    {
        float VelocityMagnitude = VE.GetVelocityEstimate().magnitude * 1000; //単位: mm/s

        return VelocityMagnitude;
    }

    [SerializeField]
    const int bufferSize = 10;
    
    float[] velocityBuffer = new float[bufferSize];

    void UpdateBuffer(float currentVelocity)
    {
        for(int i=0; i<bufferSize - 1; i++)
        {
            velocityBuffer[bufferSize - 1 - i] = velocityBuffer[bufferSize - 1 - (i + 1)];
        }
        velocityBuffer[0] = currentVelocity;
    }


    /// <summary>
    /// 毎フレームのコントローラの移動ベクトル取得
    /// </summary>

    Vector3[] posBuffer = new Vector3[2];

    public Vector3 GetMovingVector()
    {
        Vector3 MovingVec = Vector3.zero;
                
        posBuffer[1] = posBuffer[0]; 
        posBuffer[0] = this.transform.position;

        MovingVec = posBuffer[0] - posBuffer[1]; //現フレームの位置-前フレームの位置

        return MovingVec;
    } 
    
}
