using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class Client : MonoBehaviour
{
    public string serverIP = "192.168.31.57";
    public int port = 8888;
    public bool connectOnLoad = true;

    private Socket clientSocket;
    private int dataSize = 4096;
    private byte[] data = new byte[4096];


    /// <summary>
    /// Connect to server
    /// </summary>
    public void Connect()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIP), port));
    }

    /// <summary>
    /// Disconnect and close the socket
    /// </summary>
    public void Disconnect()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }

    /// <summary>
    /// Receive a string that ends with a specific character from the server. Note that the char must 
    /// occur at the end rather than in the middle of the string.
    /// </summary>
    /// <param name="end">The character that means the end of the received string.</param>
    /// <returns>The received string before (without) the specific character.</returns>
    public string Receive(char end)
    {
        string message = "";
        do
        {
            int length = clientSocket.Receive(data);
            message += Encoding.UTF8.GetString(data, 0, length);
        } while (message[message.Length - 1] != end);
        //Debug.Log("Received: " + message);
        return message.Substring(0, message.Length - 1);
    }

    /// <summary>
    /// Receive a string with a specified buffer size.
    /// </summary>
    /// <param name="size">The buffer size for the received data.</param>
    /// <returns>The received short string.</returns>
    public string Receive(int size = 4096)
    {
        if (dataSize != size)
        {
            data = new byte[size];
            dataSize = size;
        }
        int length = clientSocket.Receive(data);
        string message = Encoding.UTF8.GetString(data, 0, length);
        //Debug.Log("Received: " + message);
        return message;
    }

    /// <summary>
    /// Send a string to the server.
    /// </summary>
    public void Send(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
        Debug.Log("Sent: " + message);
    }

    void Start()
    {
        if (connectOnLoad) Connect();
    }

    void OnDestroy()
    {
        if (connectOnLoad) Disconnect();
    }
}



//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using UnityEngine;
//using System.Threading;

//public class Client : MonoBehaviour
//{
//    public string serverIP = "192.168.31.57";
//    public int port = 8888;
//    public bool connectOnLoad = true;

//    private Socket clientSocket;
//    private Thread clientThread;
//    private bool isRunning;
//    private int dataSize = 4096;
//    private byte[] data = new byte[4096];

//    public void Connect()
//    {
//        isRunning = true;
//        clientThread = new Thread(ClientThreadLoop);
//        clientThread.Start();
//    }

//    private void ClientThreadLoop()
//    {
//        try
//        {
//            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIP), port));

//            // 没有 while 循环,只连接到服务器
//        }
//        catch (SocketException e)
//        {
//            Debug.LogError("Socket error: " + e.Message);
//        }
//        finally
//        {
//            clientSocket.Shutdown(SocketShutdown.Both);
//            clientSocket.Close();
//        }
//    }

//    private object socketLock = new object();

//    public string Receive(char end)
//    {
//        string message = "";
//        lock (socketLock)
//        {
//            do
//            {
//                int length = clientSocket.Receive(data);
//                message += Encoding.UTF8.GetString(data, 0, length);
//            } while (message[message.Length - 1] != end);
//        }
//        return message.Substring(0, message.Length - 1);
//    }

//    public string Receive(int size = 4096)
//    {
//        lock (socketLock)
//        {
//            if (dataSize != size)
//            {
//                data = new byte[size];
//                dataSize = size;
//            }
//            int length = clientSocket.Receive(data);
//            string message = Encoding.UTF8.GetString(data, 0, length);
//            return message;
//        }
//    }

//    public void Send(string message)
//    {
//        lock (socketLock)
//        {
//            byte[] data = Encoding.UTF8.GetBytes(message);
//            clientSocket.Send(data);
//        }
//    }

//    void Start()
//    {
//        if (connectOnLoad) Connect();
//    }

//    void OnDestroy()
//    {
//        isRunning = false;
//        if (clientThread != null)
//        {
//            clientThread.Join();
//        }
//    }
//}