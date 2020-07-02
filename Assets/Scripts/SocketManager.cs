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
    public string ip;
    public int port;
    public int gameID;
    public string username;

    // Start is called before the first frame update
    void Start()
    {
        byte number = 3;
        number = (byte) (number << 2);
        number += 2;
        Debug.Log(number);
        // ConnectToServer();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ConnectToServer(string ip, int port, string name, int gameID) {
        try {
            this.ip = ip;
            this.port = port;
            this.name = name;
            this.gameID = gameID;
            clientSocketThread = new Thread(new ThreadStart(ListenForData));
            clientSocketThread.IsBackground = true;
            clientSocketThread.Start();
        } catch (Exception e) {
            // Debug.Log("On client connect exception " + e);
        }
    }

    private void ListenForData() {
        try {
            socketConnection = new TcpClient(ip, port);
            stream = socketConnection.GetStream();
            JoinGame();
            
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
                // Debug.Log(e.Message);
            }
        } catch (SocketException socketException) {
            // Debug.Log("Socket exception: " + socketException);
        }
    }

    public static void SendData(byte[] data) {
        if (stream == null  && !stream.CanWrite) {
            return;
        }

        try {
            stream.Write(data, 0, data.Length);
        } catch (SocketException socketException) {
            // Debug.Log("Socket exception: " + socketException);
        }
    }

    void OnApplicationQuit() {
        try {
            stream.Close();
            running = false;
            socketConnection.Close();
            Debug.Log("Closed Application");
        } catch (Exception e) {
            // Debug.LogError(e.Message);
        }
    }

    private void JoinGame() {
        List<byte> data = new List<byte>();
        int tmpGameID = gameID;

        // SendData(new byte[] {(byte)SocketDataManager.Commands.Join, 1, (byte)gameID});
        

        while (tmpGameID > 0) {
            Debug.Log(tmpGameID);
            byte b = 0;
            b += (byte)(tmpGameID % 10);
            b = (byte)(b << 4);
            tmpGameID /= 10;
            b += (byte)(tmpGameID % 10);
            tmpGameID /= 10;

            data.Add(b);
        }

        if (data.Count == 0) {
            data.Add(0);
        }

        String x = "";
        foreach (byte b in data) {
            x += b + ", ";
        }
        Debug.Log(x);

        
        List<byte> dataSend = new List<byte>();
        dataSend.Add((byte)SocketDataManager.Commands.Join);
        dataSend.Add((byte)data.Count);
        dataSend.AddRange(data);

        x = "";
        foreach (byte b in dataSend) {
            x += b + ", ";
        }
        Debug.Log(x);

        SendData(dataSend.ToArray());
    }
}
