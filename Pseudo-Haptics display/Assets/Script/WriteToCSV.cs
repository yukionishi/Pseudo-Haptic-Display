using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteToCSV : MonoBehaviour
{
    private StreamWriter sw;
    private FileInfo fi;

    //public string filePath = "defaultUser";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //cite: http://blog.shirai.la/sakakibara/2016/07/07/study1/
    public void WriteData(string fileName, string data)
    {
        fi = new FileInfo(Application.dataPath + "/StreamingAssets/data/" + fileName + ".csv");
        sw = fi.AppendText();
        sw.WriteLine(data); //テキストを書き出した上で改行
        sw.Flush(); //バッファに残る値をすべて書き出す
        sw.Close();
    }

}
