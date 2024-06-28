using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;

public class ObjectMotionSender : MonoBehaviour
{
    ROSConnection ros;
    [SerializeField] string objectTopicName = "object";
    [SerializeField] GameObject targetGameobject;
    [SerializeField] float publishRate = 0.033f; // 60fps

    float lastTime = 0;

    string topicName = "";

    // Start is called before the first frame update
    void Start()
    {
        topicName = "/" + objectTopicName;
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseMsg>(topicName);

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > publishRate)
        {
            PoseMsg poseMsg = new PoseMsg();

            poseMsg.position = new PointMsg(
                (double)targetGameobject.transform.position.x,
                (double)targetGameobject.transform.position.y,
                (double)targetGameobject.transform.position.z
            );

            poseMsg.orientation = new QuaternionMsg(
                (double)targetGameobject.transform.rotation.x,
                (double)targetGameobject.transform.rotation.y,
                (double)targetGameobject.transform.rotation.z,
                (double)targetGameobject.transform.rotation.w
            );

            ros.Publish(topicName, poseMsg);

            lastTime = Time.time;
        }
        
    }
}
