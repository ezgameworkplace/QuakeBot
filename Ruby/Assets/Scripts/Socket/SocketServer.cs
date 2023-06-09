using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SocketServer : MonoBehaviour
{
    public static SocketServer Instance { get; private set; }  // 单例
    private TcpListener listener;
    private Thread listenThread;

    [Serializable]
    public class Command
    {
        public string command;
        public string parameters;
    }

    private List<string> NesscessaryGameObjects = new List<string> { "SocketServer", "EventSystem", "Canvas", "Main Camera" };

    void Awake()
    {
        // 如果已经有一个SocketServer实例，销毁新的
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else  // 否则，这个就是新的实例
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);// Add this line to avoid destory on load
            listener = new TcpListener(IPAddress.Any, 9999);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
        }

    }

    private void ListenForClients()
    {
        this.listener.Start();

        while (true)
        {
            //blocks until a client has connected to the server
            TcpClient client = this.listener.AcceptTcpClient();

            //create a new thread to handle communication
            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
            clientThread.Start(client);
        }
    }

    private void HandleClientComm(object client)
    {
        TcpClient tcpClient = (TcpClient)client;
        NetworkStream clientStream = tcpClient.GetStream();

        byte[] message = new byte[4096];
        int bytesRead;

        while (true)
        {
            bytesRead = 0;

            try
            {
                //blocks until a client sends a message
                bytesRead = clientStream.Read(message, 0, 4096);
            }
            catch
            {
                //a socket error has occured
                break;
            }

            if (bytesRead == 0)
            {
                //the client has disconnected from the server
                break;
            }

            //message has successfully been received
            string msg = Encoding.ASCII.GetString(message, 0, bytesRead);

            Debug.Log("Received: " + msg);

            // Here, you would handle the command received and take action on your GameObjects based on the command
            // You should likely run on the main Unity thread with Unity's main thread dispatcher to update your GameObject
            Command cmd = JsonUtility.FromJson<Command>(msg);
            // 打印结果
            Debug.Log("Command: " + cmd.command + ", Params: " + cmd.parameters);
            if (cmd.command == "FocusSprite")
            {
                string objectName = cmd.parameters;
                MainThreadDispatcher.Enqueue(() =>
                {
                    StartCoroutine(FocusSprite(objectName));
                });
            }
            if (cmd.command == "DisableSpirit")
            {
                string objectName = cmd.parameters;
                MainThreadDispatcher.Enqueue(() =>
                {
                    StartCoroutine(DisableSpirit(objectName));
                });
            }
            if (cmd.command == "ClickButton")
            {
                string objectName = cmd.parameters;
                MainThreadDispatcher.Enqueue(() =>
                {
                    StartCoroutine(ClickButton(objectName, clientStream));
                });
            }
        }

        tcpClient.Close();
    }



    private IEnumerator FocusSprite(string objectName)
    {
        // 这是一个协程，我们在这里做一些需要在主线程上执行的事情
        //GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        List<GameObject> rootObjects = new List<GameObject>();
        UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        activeScene.GetRootGameObjects(rootObjects);
        foreach (GameObject obj in rootObjects)
        {
            // 除了objectName 和 SocketServer 和 Main Camera
            if (obj.name != objectName && !NesscessaryGameObjects.Contains(obj.name))
            {
                obj.SetActive(false);
            }

        }

        yield return null; // 等待下一帧
    }

    private IEnumerator DisableSpirit(string objectName)
    {
        // 这是一个协程，我们在这里做一些需要在主线程上执行的事情
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == objectName)
            {
                obj.SetActive(false);
            }

        }

        yield return null; // 等待下一帧
    }

    private IEnumerator ClickButton(string objectName, NetworkStream clientStream)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == objectName)
            {
                Button myButton = GameObject.Find("Attack").GetComponent<Button>();
                myButton.onClick.Invoke();
                // Send a message back to the client
                string response = obj.name + " button clicked successfully!";
                // client might shut down already
                try
                {
                    byte[] message = Encoding.ASCII.GetBytes(response);
                    clientStream.Write(message, 0, message.Length);
                }
                catch (IOException ex)
                {
                    Debug.Log("Client has disconnected: " + ex.Message);
                }
            }

        }
        yield return null; // 等待下一帧
    }
}
