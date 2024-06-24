using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

public class ChannelControl : MonoBehaviour
{
    ROSConnection ros;
    [SerializeField] string channelTopicName = "channel";
    [SerializeField] [Range(1, 3)] int channel = 1;

    // Start is called before the first frame update
    void Start()
    {
        channelTopicName = "/" + channelTopicName;
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Int32Msg>(channelTopicName);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
        {
            
        }
    }

    void PressedAction(int ch)
    {
        switch(ch)
        {
            case 1:
                var channelMsg = new Int32Msg();
                channelMsg.data = ch;

                ros.Publish(channelTopicName, channelMsg);
            break;
        }

    }
}
