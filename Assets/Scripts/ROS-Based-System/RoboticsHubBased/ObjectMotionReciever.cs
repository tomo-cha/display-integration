using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;

public class ObjectMotionReciever : MonoBehaviour
{
    ROSConnection ros;
    string topicName;
    [SerializeField] string objectTopicName = "object";
    [SerializeField] GameObject targetGameobject;
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        topicName = "/" + objectTopicName;

        ros.Subscribe<PoseMsg>(topicName,ObjectPoseSubscriber);
    }

     void ObjectPoseSubscriber(PoseMsg msg)
    {
        targetGameobject.transform.position = new Vector3(
            (float)msg.position.x,
            (float)msg.position.y,
            (float)msg.position.z
        );

        targetGameobject.transform.rotation = new Quaternion(
            (float)msg.orientation.x,
            (float)msg.orientation.y,
            (float)msg.orientation.z,
            (float)msg.orientation.w
        );
    }
}
