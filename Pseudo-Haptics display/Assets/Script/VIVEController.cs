using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VIVEController : MonoBehaviour
{

    public VelocityEstimator VE;
    public SteamVR_Behaviour_Pose SBP;
    public Target target;

    private int frameCount;
    private float time;
    private float prevTime = 0;
    //フレームレートを更新する時間定数
    private float interval = 0.5f;
    //フレームレート
    private int fps = 0;

    private Vector3 _moveVector;
    private Vector3 _currentVector = Vector3.zero;

    class PosHistory
    {
        public float time;
        public Vector3 position;

        public PosHistory(float _t, Vector3 _p)
        {
            time = _t;
            position = _p;
        }
        public PosHistory()
        {
            time = 0;
            position = new Vector3();
        }
    }

    void Start()
    {
        VE = this.GetComponent<VelocityEstimator>();
        SBP = this.GetComponent<SteamVR_Behaviour_Pose>();
        target = GameObject.Find("Target").GetComponent<Target>();

        //再生と同時にコントローラー速度の計測を開始
        if (!VE.estimateOnAwake)
        {
            VE.estimateOnAwake = true;
        }

        frameCount = 0;
    }

    private void FixedUpdate()
    {
        _moveVector = this.transform.position - _currentVector;
        _currentVector = this.transform.position;
    }

    void Update()
    {
        //フレームレートの計算
        frameCount++;
        time = Time.time - prevTime;

        if (time > interval)
        {
            //整数値でFPS算出
            fps = (int)(frameCount / time);
            //VelocityEstimatorスクリプトの速度計算を行うフレームレート数に渡す
            VE.velocityAverageFrames = fps;

            frameCount = 0;
            prevTime = Time.time;
        }

        UpdateBuffer();

        target.UpdateControllerSpeed();

        //speed = SBP.GetVelocity().magnitude * 1000;
        //Debug.Log(VE.GetVelocityEstimate());
        //Debug.Log(GetMovingVector().magnitude * 1000);
    }
    
    [SerializeField]
    const int bufferSize = 44; //fpsの値を拝借
    //const int bufferSize = 10;

    PosHistory[] posBuffer = new PosHistory[bufferSize];

    void UpdateBuffer()
    {
        for(int i=0; i<bufferSize - 1; i++)
        {
            posBuffer[bufferSize - 1 - i] = posBuffer[bufferSize - 1 - (i + 1)];
        }
        posBuffer[0] = new PosHistory(Time.time, this.gameObject.transform.position);
    }

    /// <summary>
    /// 速度ベクトルの取得
    /// </summary>
    /// <returns>vector/sec</returns>
    public Vector3 GetSpeedVector()
    {
        Vector3 speedVec = new Vector3();
        speedVec = (posBuffer[0].position - posBuffer[bufferSize - 1].position) / (posBuffer[0].time - posBuffer[bufferSize - 1].time);
        return speedVec;
    }

    //----------------------------------------------------------
    //VelocityEstimatorScriptの関数を利用

    //コントローラーの速度（mm/s）取得
    public float GetVelosityMagnitude()
    {
        float VelocityMagnitude = VE.GetVelocityEstimate().magnitude * 1000; //単位: mm/s

        return VelocityMagnitude;
    }

    //----------------------------------------------------------

    /// <summary>
    /// 毎フレームのコントローラの移動ベクトル取得
    /// </summary>

    public Vector3 GetMovingVector()
    {
        return _moveVector;
    }


}
