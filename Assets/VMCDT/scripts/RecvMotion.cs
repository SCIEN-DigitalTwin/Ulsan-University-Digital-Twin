using System.Threading;
using UnityEngine;
using System;

public class RecvMotion : MonoBehaviour
{
    Client client;
    MotionController motionController;
    // MotionController2 motionController2;
    public Transform body;
    // public Transform body1;

    private bool isRunning = false;
    private Thread recvThread;
    private string receivedData = "";
    string data = "";

    void Start()
    {
        client = FindObjectOfType<Client>();
        motionController = FindObjectOfType<MotionController>();
        // motionController2 = FindObjectOfType<MotionController2>();

        recvThread = new Thread(new ThreadStart(RecvThreadLoop));
    }

    private void RecvThreadLoop()
    {
        while (isRunning)
        { 
            string[] motions;
            do
            {
                data += client.Receive(256);
                motions = data.Split(new char[] { '$' }, System.StringSplitOptions.RemoveEmptyEntries);
            } while (motions.Length <= 1);
            data = motions[1];
            receivedData = motions[0];
            DateTime now = DateTime.Now;
            // 转换为字符串，包括小时、分钟、秒和毫秒
            string timeString = now.ToString("HH:mm:ss.fff");
            // 输出当前时间
            Debug.Log("Current time: " + timeString);
            // print(receivedData);
        }
    }

    void Update()
    {
        isRunning = true;
        if (!recvThread.IsAlive)
        {
            recvThread.Start();
        }
        if (!string.IsNullOrEmpty(receivedData))
        {
            ParseMessage();
        }
    }

    public void ParseMessage()
    {
        string[] pose_and_tran_p1 = null;
        float cameraID = 1f;
        pose_and_tran_p1 = receivedData.Split(new char[] { '#' }, System.StringSplitOptions.RemoveEmptyEntries);
        motionController.SetPose(pose_and_tran_p1[0]);   // body == p1
        motionController.SetRootPosition(new Vector3(180f, 0f, 0f));

        string vectorString = pose_and_tran_p1[1];
        Vector3 vector = StringToVector3(vectorString);
        Vector3 RTvector = vector;
        RTvector.y = 0f;
        RTvector.x = vector.z - 6.508f;
        RTvector.z = 5.741f - vector.x;
        body.transform.localPosition = RTvector;
        motionController.SetRootPosition(new Vector3(180f, -90f, 0f));
        // // motionController2.SetPose(pose_and_tran_c2_p1[0]);  // body1 == p2
        // if ((cameraID - 1) < 0.01) {
        //     motionController.SetRootPosition(new Vector3(180f, 45f, 0f));
        // }
        // else {
        //     motionController.SetRootPosition(new Vector3(180f, -90f, 0f));
        // }
        
        // motionController2.SetRootPosition(new Vector3(180f, 45f, 0f));

    }

    Vector3 StringToVector3(string s)
    {
        // 在这里检查字符串是否为空或者格式不正确的情况
        if (string.IsNullOrEmpty(s))
            return Vector3.zero;

        // 分割字符串
        string[] splitString = s.Split(',');
        // 解析字符串为float类型
        if (splitString.Length != 3)
            return Vector3.zero;

        float x, y, z;
        if (float.TryParse(splitString[0], out x) && float.TryParse(splitString[1], out y) && float.TryParse(splitString[2], out z))
        {
            return new Vector3(x, y, z);
        }

        return Vector3.zero;
    }

    void OnDestroy()
    {
        isRunning = false;
        if (recvThread != null)
        {
            recvThread.Join();
        }
    }
}