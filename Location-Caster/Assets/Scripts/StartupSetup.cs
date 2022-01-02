using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StartupSetup : MonoBehaviour
{
    [SerializeField] private TMP_Text _logText;
    [SerializeField] private string _clientIP;

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 60;

#if !UNITY_EDITOR
        StartListener();
#endif
    }

    void Log(string msg)
    {
        _logText.text = _logText.text + "\n" + msg;
    }
    
    [ContextMenu("StartClient")]
    private void StartClient()
    {
        byte[] bytes = new byte[1024];

        IPAddress ipAddress = IPAddress.Parse(_clientIP);
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

        Socket sender = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        sender.Connect(remoteEP);

        Log("Socket connected to " + sender.RemoteEndPoint);

        byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

        int bytesSent = sender.Send(msg);

        int bytesRec = sender.Receive(bytes);
        Log("Echoed test = " + Encoding.ASCII.GetString(bytes, 0, bytesRec));

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }
    
    public void StartListener()
    {
        try
        {
            IPAddress ipAddress = IPAddress.Parse("192.168.0.114");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Log("Waiting for a connection... " + listener.LocalEndPoint);
            Socket handler = listener.Accept();
            StartCoroutine(ReceiveCoroutine(handler));
        }
        catch (Exception e)
        {
            Log(e.Message);
        }
    }

    IEnumerator ReceiveCoroutine(Socket handler)
    {
        string data = null;
        byte[] bytes = null;

        while (true)
        {
            bytes = new byte[1024];
            int bytesRec = handler.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            if (data.IndexOf("<EOF>") > -1)
            {
                break;
            }
            yield break;
        }
        Log("Text received : " + data);

        byte[] msg = Encoding.ASCII.GetBytes(data);
        handler.Send(msg);
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }
}