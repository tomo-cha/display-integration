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

    [SerializeField] float publishRate = 0.033f; // 60fps
    string[] topicNames;

    float lastTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        Debug.Log(cameraInfoValues.Length);

        if(cameraInfoValues.Length > 0)
        {
            topicNames = new string[cameraInfoValues.Length];
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

            ros.RegisterPublisher<PoseMsg>(topicNames[i]);

            Debug.Log("register topic as:" + topicNames[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > publishRate)
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

                ros.Publish(topicNames[i], poseMsg);
            }

            lastTime = Time.time;

        }
        
        
    }
}
