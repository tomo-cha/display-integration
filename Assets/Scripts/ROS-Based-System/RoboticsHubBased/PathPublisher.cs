using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;
using UnityEngine.AI;

public class PathPublisher : MonoBehaviour
{
    ROSConnection ros;
    [SerializeField] string topicName;
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PathMsg>(topicName);

        PathMsg msg = new PathMsg();

        HeaderMsg header = new HeaderMsg();
        header.frame_id = "map";
        header.stamp.sec = 0;
        header.stamp.nanosec = 0;
        msg.header = header;

        PoseStampedMsg[] poses = new PoseStampedMsg[10];
        for(int i = 0; i < poses.Length; i ++)
        {
            poses[i] = new PoseStampedMsg();
            poses[i].header.stamp.sec = 0;
            poses[i].header.stamp.nanosec = 0;
            poses[i].header.frame_id = "";

            poses[i].pose.position.x = 0f + 0.5f;
            poses[i].pose.position.y = 0.5f;
            poses[i].pose.position.z = 0f;
            
            poses[i].pose.orientation.x = 0f;
            poses[i].pose.orientation.y = 0f;
            poses[i].pose.orientation.z = 0f;
            poses[i].pose.orientation.w = -1f;
        }

        msg.poses = poses;

        ros.Publish(topicName, msg);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
