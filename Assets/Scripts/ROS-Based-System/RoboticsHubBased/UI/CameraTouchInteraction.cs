using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using Unity.Robotics;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;

public class CameraTouchInteraction : MonoBehaviour
{
    ROSConnection ros;
    [Header("必要なゲームオブジェクト")] 
    [SerializeField] GameObject markerObject;
    [SerializeField] GameObject robotObject;
    [SerializeField] GameObject UiGameObject;

    [Header("しきい値やレートの調整")]
    [SerializeField] float distanceThreshold = 0.2f;
    [SerializeField] [Range(0, 1f)] float nearDistanceRatio = 0.5f;
    [SerializeField] float publishRate = 2f;   

    [Header("トピック名の設定")] 
    [SerializeField] string waypointTopicName = "waypoints_unity";
    [SerializeField] string cancelTopicName = "cancel_status";
    [SerializeField] string distanceTopicName = "distance_remain";
    Camera cam;
    Vector3 lastPosition;
    bool initialProcess = false;    
    GameObject parentObject;
    UIDocument _uiDocument;
    List<PoseStampedMsg> waypoints = new List<PoseStampedMsg>();

    bool enableNavigation = false;
    int currentWaypointID = 0;
    float lastTime = 0f;
    float distance_remain = 0f;

    void Start ()
    {
        cam = FindObjectOfType<Camera>();

        parentObject = new GameObject("robot1_waypoints");
        parentObject.transform.position = Vector3.zero;

        _uiDocument = UiGameObject.GetComponent<UIDocument>();
        var resetButtonElement = _uiDocument.rootVisualElement.Q<Label>("Reset");
        resetButtonElement.AddManipulator(new Clickable(ResetButtonPressed));

        _uiDocument = UiGameObject.GetComponent<UIDocument>();
        var publishButtonElement = _uiDocument.rootVisualElement.Q<Label>("Publish");
        publishButtonElement.AddManipulator(new Clickable(PublishButtonPressed));

        _uiDocument = UiGameObject.GetComponent<UIDocument>();
        var cancelButtonElement = _uiDocument.rootVisualElement.Q<Label>("Cancel");
        cancelButtonElement.AddManipulator(new Clickable(CancelButtonPressed));

        ros = ROSConnection.GetOrCreateInstance();

        ros.RegisterPublisher<PoseStampedMsg>(waypointTopicName);
        ros.RegisterPublisher<BoolMsg>(cancelTopicName);

        ros.Subscribe<Float32Msg>(distanceTopicName, DistanceInfoCallback);

        Debug.Log("Threshold Distance is " + distanceThreshold * nearDistanceRatio);
    }
        
    void Update () {

        if(enableNavigation)
        {            
            //ナビゲーションが開始されている際の処理
            if(Time.time - lastTime > publishRate)
            {
                ros.Publish(waypointTopicName, waypoints[currentWaypointID]);
                lastTime = Time.time;

                if(distance_remain < distanceThreshold * nearDistanceRatio)
                {
                    Debug.Log("near!!!");
                    currentWaypointID ++;
                    if(currentWaypointID > waypoints.Count - 1) enableNavigation = false;
                }
            }                         
        }
        else
        {
            //ナビゲーションが開始されていない際の処理＝通過点の設定
            currentWaypointID = 0;
            var mousePosition = Input.mousePosition;

            if(mousePosition.y < Screen.height * 0.7)
            {
                mousePosition.z = this.cam.transform.position.y;
                var worldPoint = this.cam.ScreenToWorldPoint(mousePosition);

                worldPoint.y = 0.0f;

                if(Input.GetMouseButton(0))
                {
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

                            PoseStampedMsg poseStamped = new PoseStampedMsg();

                            poseStamped.header.stamp.sec = 0;
                            poseStamped.header.stamp.nanosec = 0;
                            poseStamped.header.frame_id = "map";

                            poseStamped.pose = ConvertTransfromUnityToRos(lastPosition, -y_angular);
                            waypoints.Add(poseStamped);
                        }
                    } 
                }
            }  
        }
    }

    void ResetButtonPressed()
    {
        Debug.Log("Reset Button is Pressed");

        waypoints = new List<PoseStampedMsg>();

        GameObject.Destroy(parentObject);
            
        parentObject = new GameObject("robot1_waypoints");
        enableNavigation = false;
        initialProcess = false;
    }

    void PublishButtonPressed()
    {
        Debug.Log("Publish Button is Pressed");
        if(waypoints.Count > 0)
        {
            enableNavigation = true;
            Debug.Log("Navigation has been started!!!");
        }
        else
        {
            Debug.LogWarning("Navigation has not been started");
        }
       
    }

    void CancelButtonPressed()
    {
        Debug.Log("Cancel Button is Pressed");
        BoolMsg msg = new BoolMsg();
        msg.data = true;
        
        ros.Publish(cancelTopicName, msg);

        enableNavigation = false;
    }

    PoseMsg ConvertTransfromUnityToRos(Vector3 unityPosition, float unityRotation)
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

    void DistanceInfoCallback(Float32Msg msg)
    {
        Debug.Log("Disntance remain... " + msg.data + "/ Threshold..." + distanceThreshold * nearDistanceRatio);
        distance_remain = msg.data;
    }
}
