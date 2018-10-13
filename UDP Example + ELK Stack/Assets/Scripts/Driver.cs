﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    private UDPThreaded connection;

    void Start()
    {
        string sendIp = "127.0.0.1";
        int sendPort = 8088;
        int receivePort = 10001;

        connection = new UDPThreaded();
        connection.StartConnection(sendIp, sendPort, receivePort);
    }

    void Update()
    {
        //foreach (var message in connection.getMessages()) Debug.Log(message);

        connection.Send("Hi!");
    }

    void OnDestroy()
    {
        connection.Stop();
    }
}
