using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;


public class SIM_CameraInfoReciever : MonoBehaviour
{
    // Start is called before the first frame update
    ROSConnection ros;
    [SerializeField] string rosNamespace;
    [SerializeField] string cameraTopicName = "/camera";
    [SerializeField] Camera mainCameraObject;
    [SerializeField] float screenHeight = 1.0f;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

       // topicName = "/" + rosNamespace + "/" + cameraTopicName;
        
        ros.Subscribe<PoseMsg>(cameraTopicName,CameraPoseSubscriber);

        mainCameraObject.orthographicSize = screenHeight * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CameraPoseSubscriber(PoseMsg msg)
    {
        mainCameraObject.transform.position = new Vector3(
            (float)msg.position.x,
            (float)msg.position.y,
            (float)msg.position.z
        );

        mainCameraObject.transform.rotation = new Quaternion(
            (float)msg.orientation.x,
            (float)msg.orientation.y,
            (float)msg.orientation.z,
            (float)msg.orientation.w
        );
    }
}
