using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using Unity.Robotics;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;

public class CameraTouchInteraction : MonoBehaviour
{
    ROSConnection ros;
    [SerializeField] GameObject markerObject;
    [SerializeField] float distanceThreshold = 0.2f;
    [SerializeField] GameObject UiGameObject;
    [SerializeField] string topicName = "waypoints_unity";

    Camera cam;
    Vector3 lastPosition;
    bool initialProcess = false;    
    GameObject parentObject;
    UIDocument _uiDocument;

    List<PoseMsg> waypoints = new List<PoseMsg>();
    void Start ()
    {
        this.cam = FindObjectOfType<Camera>();

        parentObject = new GameObject("robot1_waypoints");
        parentObject.transform.position = Vector3.zero;

        _uiDocument = UiGameObject.GetComponent<UIDocument>();
        var resetButtonElement = _uiDocument.rootVisualElement.Q<Label>("Reset");
        resetButtonElement.AddManipulator(new Clickable(ResetClicked));

        _uiDocument = UiGameObject.GetComponent<UIDocument>();
        var publishButtonElement = _uiDocument.rootVisualElement.Q<Label>("Publish");
        publishButtonElement.AddManipulator(new Clickable(PublishClicked));

        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseArrayMsg>(topicName);
    }
        
    void Update () {
        if(Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;

            if(mousePosition.y < Screen.height * 0.7)
            {
                mousePosition.z = this.cam.transform.position.y;
                var worldPoint = this.cam.ScreenToWorldPoint(mousePosition);

                worldPoint.y = 0.0f;

                if(!initialProcess)
                {
                    lastPosition = worldPoint;
                    initialProcess = true;
                }
                else
                {
                    if(Vector3.Distance(lastPosition, worldPoint) > distanceThreshold)
                    {
                        float a = (worldPoint.z - lastPosition.z) / (worldPoint.x - lastPosition.x);

                        float y_angular = Mathf.Atan(a);
                        float y_angular_rad = y_angular / Mathf.PI * 180f;

                        if((worldPoint.x - lastPosition.x) < 0) y_angular_rad += 180f;

                        Quaternion quat = Quaternion.Euler(0f, -y_angular_rad, 0f);

                        var go = GameObject.Instantiate(markerObject, lastPosition, quat);
                        go.transform.parent = parentObject.transform;

                        lastPosition = worldPoint;

                        var pose = ConvertTransformUnityToRos(lastPosition, -y_angular);
                        waypoints.Add(pose);
                    }
                }    
            }              
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Destroy(parentObject);
            
            parentObject = new GameObject("robot1_waypoints");
            initialProcess = false;
        }
    }

    void ResetClicked()
    {
        Debug.Log("Reset Button is Clicked");
        GameObject.Destroy(parentObject);
            
        parentObject = new GameObject("robot1_waypoints");
        initialProcess = false;
    }

    void PublishClicked()
    {
        Debug.Log("Reset Button is Clicked");

        if(waypoints.Count > 0)
        {
            PoseArrayMsg msg = new PoseArrayMsg();
        
            PoseMsg[] poses = new PoseMsg[waypoints.Count]; 

            for(int i = 0; i < waypoints.Count; i ++)
            {
                poses[i] = waypoints[i];
            }

            msg.poses = poses;
            msg.header.stamp.sec = Time.frameCount;

            ros.Publish(topicName, msg);

        }        
    }

    PoseMsg ConvertTransformUnityToRos(Vector3 unityPosition, float unityRotation)
    {
        PoseMsg pose = new PoseMsg();
        pose.position.x =  unityPosition.z;
        pose.position.y = -unityPosition.x;
        pose.position.z = 0f;

        Quaternion unity_quat = Quaternion.Euler(0f, 0f, unityRotation);
        pose.orientation.x = unity_quat.x;
        pose.orientation.y = unity_quat.y;
        pose.orientation.z = unity_quat.z;
        pose.orientation.w = unity_quat.w;

        return pose;
    }
}
