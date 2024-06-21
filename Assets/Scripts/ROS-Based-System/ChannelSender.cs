using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

public class ChannelSender : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField] string channelTopicName = "channel";
    [SerializeField] [Range(1,3)] int channel = 1;
    string topicName = "";
    // Start is called before the first frame update
    void Start()
    {
        topicName = "/" + channelTopicName;
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Int32Msg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Int32Msg msg = new Int32Msg();
            msg.data = channel;

            ros.Publish(topicName, msg);
        }
       
    }
}
