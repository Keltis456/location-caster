using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class ShowCurrentLocalIP : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    void Start()
    {
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            _text.text = "Local IP: " + endPoint.Address;
        }
    }
}