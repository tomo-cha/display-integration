using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;

public class CameraInfoSender : MonoBehaviour
{
    ROSConnection ros;

    [System.Serializable]
    public struct cameraInfoValue{
        public string rosNamespace;
        public GameObject targetGameobject;
    }

    [SerializeField] string cameraTopicName = "camera";

    [SerializeField] cameraInfoValue[] cameraInfoValues;

    string topicName = "";

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        Debug.Log(cameraInfoValues.Length);

        for(int i = 0; i < cameraInfoValues.Length; i ++)
        {
            topicName = "/" + cameraInfoValues[i].rosNamespace + "/" + cameraTopicName;

            ros.RegisterPublisher<PoseMsg>(topicName);

            Debug.Log("register topic as:" + topicName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < cameraInfoValues.Length; i ++)
        {
            PoseMsg poseMsg = new PoseMsg();

            poseMsg.position = new PointMsg(
               (double)cameraInfoValues[i].targetGameobject.transform.position.x,
               (double)cameraInfoValues[i].targetGameobject.transform.position.y,
               (double)cameraInfoValues[i].targetGameobject.transform.position.z
            );

            poseMsg.orientation = new QuaternionMsg(
                (double)cameraInfoValues[i].targetGameobject.transform.rotation.x,
                (double)cameraInfoValues[i].targetGameobject.transform.rotation.y,
                (double)cameraInfoValues[i].targetGameobject.transform.rotation.z,
                (double)cameraInfoValues[i].targetGameobject.transform.rotation.w
            );

            ros.Publish(topicName, poseMsg);
        }
        
    }
}
