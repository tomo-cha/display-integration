using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

public class R2FU_CameraInfoSender : MonoBehaviour
{
    ROS2UnityComponent ros2Unity;
    ROS2Node ros2Node;
    IPublisher<geometry_msgs.msg.Pose>[] publisher;

     [System.Serializable]
    public struct cameraInfoValue{
        public string rosNamespace;
        public GameObject targetGameobject;
    }

    [SerializeField] string cameraTopicName = "camera";

    [SerializeField] cameraInfoValue[] cameraInfoValues;

    [SerializeField] float publishRate = 0.033f; // 60fps
    float lastTime = 0;
    string[] topicNames;
    string nodeName = "cameraInfoSenderNode";

    // Start is called before the first frame update
    void Start()
    {
        ros2Unity = GetComponent<ROS2UnityComponent>();

        if (ros2Node == null && ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode(nodeName);

            if(cameraInfoValues.Length > 0)
            {
                topicNames = new string[cameraInfoValues.Length];
                publisher = new IPublisher<geometry_msgs.msg.Pose>[cameraInfoValues.Length];
            }

            for(int i = 0; i < cameraInfoValues.Length; i ++)
            {
                if(cameraInfoValues[i].rosNamespace != "")
                {
                    topicNames[i] = "/" + cameraInfoValues[i].rosNamespace + "/" + cameraTopicName;
                }
                else
                {
                    topicNames[i] = "/" + cameraTopicName;
                }
                
                publisher[i] = ros2Node.CreatePublisher<geometry_msgs.msg.Pose>(topicNames[i]);
                Debug.Log("register topic as:" + topicNames[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > publishRate)
        {
            for(int i = 0; i < cameraInfoValues.Length; i ++)
            {
                geometry_msgs.msg.Pose poseMsg = new geometry_msgs.msg.Pose();

                poseMsg.Position.X = cameraInfoValues[i].targetGameobject.transform.position.x;
                poseMsg.Position.Y = cameraInfoValues[i].targetGameobject.transform.position.y;
                poseMsg.Position.Z = cameraInfoValues[i].targetGameobject.transform.position.z;

                poseMsg.Orientation.X = cameraInfoValues[i].targetGameobject.transform.rotation.x;
                poseMsg.Orientation.Y = cameraInfoValues[i].targetGameobject.transform.rotation.y;
                poseMsg.Orientation.Z = cameraInfoValues[i].targetGameobject.transform.rotation.z;
                poseMsg.Orientation.W = cameraInfoValues[i].targetGameobject.transform.rotation.w;

                publisher[i].Publish(poseMsg);
            }

            lastTime = Time.time;
        }
    }
}
