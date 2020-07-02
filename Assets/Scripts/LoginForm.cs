using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginForm : MonoBehaviour
{
    private string ip;
    private int port;
    private string username;
    private int gameID;

    public SocketManager socketManager;
    public TMP_InputField ipInput;
    public TMP_InputField portInput;
    public TMP_InputField nameInput;
    public TMP_InputField gameIDInput;

    public void Submit() {
        socketManager.ConnectToServer(ipInput.text, Int32.Parse(portInput.text), username, Int32.Parse(gameIDInput.text));
    }


}
