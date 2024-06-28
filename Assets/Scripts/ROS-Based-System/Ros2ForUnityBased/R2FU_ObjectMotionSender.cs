using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

public class R2FU_ObjectMotionSender : MonoBehaviour
{
    ROS2UnityComponent ros2Unity;
    ROS2Node ros2Node;
    IPublisher<geometry_msgs.msg.Pose> publisher;

    [SerializeField] string objectTopicName = "object";
    [SerializeField] GameObject targetGameobject;
    string nodeName = "objectMotionSenderNode";

    [SerializeField] float publishRate = 0.033f; // 60fps
    float lastTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        ros2Unity = GetComponent<ROS2UnityComponent>();

        objectTopicName = "/" + objectTopicName;

        if (ros2Node == null && ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode(nodeName);
            publisher = ros2Node.CreatePublisher<geometry_msgs.msg.Pose>(objectTopicName);
            Debug.Log("register topic as:" + objectTopicName);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > publishRate)
        {
            geometry_msgs.msg.Pose poseMsg = new geometry_msgs.msg.Pose();

            poseMsg.Position.X = targetGameobject.transform.position.x;
            poseMsg.Position.Y = targetGameobject.transform.position.y;
            poseMsg.Position.Z = targetGameobject.transform.position.z;

            poseMsg.Orientation.X = targetGameobject.transform.rotation.x;
            poseMsg.Orientation.Y = targetGameobject.transform.rotation.y;
            poseMsg.Orientation.Z = targetGameobject.transform.rotation.z;
            poseMsg.Orientation.W = targetGameobject.transform.rotation.w;

            publisher.Publish(poseMsg);

            lastTime = Time.time;
        }
    }
}
