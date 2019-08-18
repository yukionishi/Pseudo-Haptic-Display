using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerPosition : MonoBehaviour {
    
    [SerializeField] private bool depth = false;
    [SerializeField] private bool sideway = false;

    //vector3情報をもつクラス
    class PosHistory
    {
        public Vector3 position;

        public PosHistory(Vector3 _p)
        {
            position = _p;
        }
        public PosHistory()
        {
            position = Vector3.zero;
        }
    }

    [SerializeField]
    const int bufferSize = 20;

    PosHistory[] posBuffer = new PosHistory[bufferSize];


    // Use this for initialization
    void Start()
    {
        posBuffer[bufferSize - 1] = new PosHistory(Vector3.zero); //initialize the element of buffer
    }

    // Update is called once per frame
    void Update()
    {
        //トラッカーの初期位置リセット
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetInitialPosition();
        }
        

        //常にtrackerの位置を配列に格納
        UpdatePosBuffer();
    }
    
    //配列にtrackerのpositionを初期位置として格納する
    public void SetInitialPosition()
    {
        posBuffer[bufferSize - 1] = new PosHistory(this.transform.position); //set initial position in posBuffer[bufferSize - 1]

        //Debug.Log("InitialData" + posBuffer[bufferSize - 1].position);
    } 

    //update buffer elements about position
    void UpdatePosBuffer()
    {
        //posBuffer[bufferSize - 1] is the initial position,
        //so position updating is proceeding except posBuffer[bufferSize - 1]
        for (int i = 1; i < bufferSize - 1; i++)
        {
            posBuffer[bufferSize - 1 - i] = posBuffer[bufferSize - 1 - (i + 1)]; //posBuffer[0] is the newest information
        }

        posBuffer[0] = new PosHistory(this.transform.position);

    }
    
    //calculate the distance between initial position and current position
    [HideInInspector] public float rawPosData = 0; //trackerの初期位置から現在の位置までの移動距離分（Unity座標）,TranslationManagerで引用するためpublic変数化
    public float CalculateDistance()
    {
        //進む方向に合わせて使用するpositionの座標軸を変更
        if (depth)
        {
            rawPosData = posBuffer[0].position.z - posBuffer[bufferSize - 1].position.z;
        }
        else if (sideway)
        {
            rawPosData = posBuffer[0].position.x - posBuffer[bufferSize - 1].position.x;
        }

        //移動の正方向を設定
        if(rawPosData < 0)
        {
            if (sideway)
            {
                rawPosData = Mathf.Abs(rawPosData); //距離rawPosDataが負であれば絶対値を計算
                                                    //進行方向向かって左方向(sideway)が正になるように正負を調整
            }
            else
            {
                //depth方向の時は修正不要（進行方向向かって手前から奥方向が正）
            }
        }
        else
        {
            if (sideway)
            {
                rawPosData = (-1) * rawPosData; //進行方向向かって右方向(sideway)が負になるように正負を調整
            }
            else
            {
                //depth方向の時は修正不要（進行方向向かって奥から手前方向が負）
            }

        }

        //Debug.Log("rawPosData" + rawPosData);

        float distance = (rawPosData * 100); //(rawPosData * 100): rawPosDataはUnity座標なので1.0=1mであるため，cmに変換後intに変換する

        return distance;
    }
}
