using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;

public class PoseStampedPublisher : MonoBehaviour
{
    ROSConnection ros;
    [SerializeField] string topicName;
    [SerializeField] Vector2 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseStampedMsg>(topicName);
        
        // PoseStampedMsg msg = new PoseStampedMsg();

        // msg.header.stamp.sec = 0;
        // msg.header.stamp.nanosec = 0;
        // msg.header.frame_id = "map";

        // msg.pose.position.x = 0f;
        // msg.pose.position.y = -0.5f;
        // msg.pose.position.z = 0f;

        // msg.pose.orientation.x = 0f;
        // msg.pose.orientation.y = 0f;
        // msg.pose.orientation.z = 0f;
        // msg.pose.orientation.w = -1f;

        // ros.Publish(topicName, msg);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            PoseStampedMsg msg = new PoseStampedMsg();

            msg.header.stamp.sec = 0;
            msg.header.stamp.nanosec = 0;
            msg.header.frame_id = "map";

            msg.pose.position.x = targetPosition.x;
            msg.pose.position.y = targetPosition.y;
            msg.pose.position.z = 0f;

            msg.pose.orientation.x = 0f;
            msg.pose.orientation.y = 0f;
            msg.pose.orientation.z = 0f;
            msg.pose.orientation.w = -1f;

            ros.Publish(topicName, msg);
        }
        
    }
}
