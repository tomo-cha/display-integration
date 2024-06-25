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
        SceneManager.GetActiveScene();
        if(Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
        {
            PressedAction(1);
        }

        if(Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2))
        {
            PressedAction(2);
        }
    }

    void PressedAction(int ch)
    {
        string nextChannelName = "Ch" + ch.ToString();
        string currentChannelName = SceneManager.GetActiveScene().name;

        if(nextChannelName != currentChannelName)
        {
            // var rosObject = GameObject.Find("ROSConnectionPrefab(Clone)");
            // DontDestroyOnLoad(rosObject);  

            var channelMsg = new Int32Msg();
            channelMsg.data = ch;

            ros.Publish(channelTopicName, channelMsg);

            SceneManager.LoadScene(nextChannelName);
        }
        

    }
}
