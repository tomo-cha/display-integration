using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;

public class CameraInfoReciever : MonoBehaviour
{
    ROSConnection ros;
    string rosNamespace;
    string topicName;
    [SerializeField] string cameraTopicName = "camera";
    [SerializeField] GameObject targetGameobject;
    
    // Start is called before the first frame update
    void Start()
    {
        var rosObject = GameObject.Find("ROSConnectionPrefab(Clone)");
        rosNamespace = rosObject.GetComponent<RosNamespaceManager>().rosNamespace;

        ros = ROSConnection.GetOrCreateInstance();

        topicName = "/" + rosNamespace + "/" + cameraTopicName;

        ros.Subscribe<PoseMsg>(topicName,CameraPoseSubscriber);
    }

    void Update()
    {
        
    }

    void CameraPoseSubscriber(PoseMsg msg)
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
