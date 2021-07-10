using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    private Text text;
    private GameObject Dandelion;
    private float timer;
    public float timeGap = 1.0f;

    private int hour;
    private int minute;
    private int second;

    private string filePath;
    private string info;
    void Start()
    {
        text = GetComponentInChildren<Text>();
        Debug.Log(Application.dataPath);
        filePath = Application.dataPath + "/" + GetCurTime();
        Dandelion = GameObject.FindGameObjectWithTag("Dandelion");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Dandelion != null)
        {
            var speed = Dandelion.GetComponent<Rigidbody2D>().velocity.magnitude.ToString();
            text.text = "当前速度" + speed;
            if (timer > timeGap)
            {
                info += speed + "\r\n";
                timer = 0;
            }
        }
        

    }

    private void OnApplicationQuit()
    {
        FileStream fs = new FileStream(filePath, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(info);
        sw.Flush();
        sw.Close();
        fs.Close();
    }


    private string GetCurTime()
    {
        hour = DateTime.Now.Hour;
        minute = DateTime.Now.Minute;
        second = DateTime.Now.Second;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
    }
}
