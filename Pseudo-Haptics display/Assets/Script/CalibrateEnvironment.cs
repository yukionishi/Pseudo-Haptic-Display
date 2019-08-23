using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateEnvironment : MonoBehaviour
{
    private Transform displayScreen1;
    private Transform camera1;
    private Transform camera2;

    //液晶ディスプレイ位置環境
    [SerializeField]
    private Vector3 Height_displayTop = Vector3.zero;
    [SerializeField]
    private float Height_Display = 0;
    //Unity Scene内のDisplayScreen位置
    private Vector3 DisplayPos = Vector3.zero;

    //ユーザ位置環境
    [SerializeField]
    private float Distance_UserToDisplay = 0;
    [SerializeField]
    private float Height_Gaze = 0;
    //Unity Scene内のCamera位置
    private Vector3 GazePos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        displayScreen1 = GameObject.Find("DisplayScreen1").transform;
        camera1 = GameObject.Find("Camera1").transform;
        camera2 = GameObject.Find("Target").transform.GetChild(0).gameObject.transform;


        DisplayPos = Height_displayTop - new Vector3(0.0f, (Height_Display / 2), 0.0f);
        //現実世界での液晶ディスプレイの位置をUnity Scene内のDisplayScreen位置に反映
        displayScreen1.position = DisplayPos;

        GazePos = new Vector3(0, Height_Gaze, DisplayPos.z - Distance_UserToDisplay);
        //現実世界でのユーザの視点位置をCamera1の位置に反映
        camera1.position = GazePos;
        camera2.position = GazePos;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
