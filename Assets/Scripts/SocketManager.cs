using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketManager : MonoBehaviour
{
    private Thread clientSocketThread;
    private TcpClient socketConnection;
    private static NetworkStream stream;
    private bool running = true;

    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ConnectToServer() {
        try {
            clientSocketThread = new Thread(new ThreadStart(ListenForData));
            clientSocketThread.IsBackground = true;
            clientSocketThread.Start();
        } catch (Exception e) {
            Debug.Log("On client connect exception " + e);
        }
    }

    private void ListenForData() {
        try {
            socketConnection = new TcpClient("localhost", 25565);
            stream = socketConnection.GetStream();
            byte[] bytes = new byte[1024];
            while (running == true) {
                int length;
                while (stream.DataAvailable && (length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                    byte[] incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);
                    SocketDataManager.GiveData(incommingData);
                }
            }

            try {
                Debug.Log("closing socket");
                socketConnection.GetStream().Close();
                socketConnection.Close();
                socketConnection = null;
                Debug.Log("closed socket");
            } catch (Exception e) {
                Debug.Log(e.Message);
            }
        } catch (SocketException socketException) {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public static void SendData(byte[] data) {
        if (stream == null  && !stream.CanWrite) {
            return;
        }

        try {
            stream.Write(data, 0, data.Length);
        } catch (SocketException socketException) {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    void OnApplicationQuit() {
        try {
            stream.Close();
            running = false;
            socketConnection.Close();
            Debug.Log("Closed Application");
        } catch (Exception e) {
            Debug.LogError(e.Message);
        }
    }
}
