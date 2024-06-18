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

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        for(int i = 0; i < cameraInfoValues.Length; i ++)
        {
            string topicName = "/" + cameraInfoValues[i].rosNamespace + "/" + cameraTopicName;

            ros.RegisterPublisher<PoseMsg>(topicName);

            Debug.Log("register topic as:" + topicName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
