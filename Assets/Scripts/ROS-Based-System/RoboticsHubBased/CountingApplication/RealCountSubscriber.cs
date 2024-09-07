using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using System;

public class RealCountSubscriber : MonoBehaviour
{
    ROSConnection ros;
    [SerializeField] string topicName;
    
    public int num_realCount = 0;

    bool dataIncoming = false;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<Int32Msg>(topicName, callback);
    }

    void Update()
    {
        if(dataIncoming) dataIncoming = false;
    }

    void callback(Int32Msg msg)
    {
        dataIncoming = true;
        num_realCount = msg.data;
    }
}
