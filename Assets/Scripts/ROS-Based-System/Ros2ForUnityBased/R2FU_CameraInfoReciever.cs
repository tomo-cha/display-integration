using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS2;

public class R2FU_CameraInfoReciever : MonoBehaviour
{
    ROS2UnityComponent ros2Unity;
    ROS2Node ros2Node;
    ISubscription<geometry_msgs.msg.Pose> subscriber;
    string rosNamespace = "";
    [SerializeField] string nodeName = "CameraInfoSub";
    [SerializeField] string cameraTopicName = "camera";
    [SerializeField] Camera mainCameraObject; 
    GameObject uiManagerRuntimeObject;
    GameObject informationObject;

    // Start is called before the first frame update
    void Start()
    {
        ros2Unity = GetComponent<ROS2UnityComponent>();

        informationObject = GameObject.Find("Information");

        if(informationObject != null)
        {
            rosNamespace = informationObject.GetComponent<ValueTransport>().rosNamespace;
        }

        if(rosNamespace != "")
        {
            nodeName = rosNamespace + "oF" + nodeName;
            cameraTopicName = "/" + rosNamespace + "/" + cameraTopicName;
        }
        else
        {
            cameraTopicName = "/" + cameraTopicName;
        }

        if (ros2Node == null && ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode(nodeName);
            subscriber = ros2Node.CreateSubscription<geometry_msgs.msg.Pose>(
              cameraTopicName, CameraInfoRecieveCallBack);

        }

        uiManagerRuntimeObject = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<UiActivator>().showGUI)
        {
            if(uiManagerRuntimeObject == null)
            {
                uiManagerRuntimeObject = GameObject.Find("UI");
            }
            else
            {
                if(uiManagerRuntimeObject.GetComponent<R2FU_UIManagerRuntime>().enableTransition)
                {
                    ros2Node.RemoveSubscription<geometry_msgs.msg.Pose>(subscriber);
                    ros2Unity.RemoveNode(ros2Node);
                }
            }          
        }
    }

    void CameraInfoRecieveCallBack(geometry_msgs.msg.Pose msg)
    {
        mainCameraObject.transform.position = new Vector3(
            (float)msg.Position.X,
            (float)msg.Position.Y,
            (float)msg.Position.Z
        );

        mainCameraObject.transform.rotation = new Quaternion(
            (float)msg.Orientation.X,
            (float)msg.Orientation.Y,
            (float)msg.Orientation.Z,
            (float)msg.Orientation.W
        );
    }
}
