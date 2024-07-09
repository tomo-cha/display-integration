using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class SimplePublisher : MonoBehaviour
{
    [SerializeField] string topic;
    ROSConnection ros;
    //Twist
    Vector3Msg linear = new Vector3Msg(0f, 0f, 0f);
    Vector3Msg angular = new Vector3Msg(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topic);
    }

    // Update is called once per frame
    void Update()
    {
        if(topic == "/robot1/cmd_vel")
        {
            /*
              w
            a s d
              x
            */
            if (Input.GetKey(KeyCode.W))
            {
                linear.x += 0.01;
            }
            if (Input.GetKey(KeyCode.X))
            {
                linear.x -= 0.01;
            }
            if (Input.GetKey(KeyCode.A))
            {
                angular.z -= 0.1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                angular.z += 0.1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                linear.x = 0;
                angular.z = 0;
            }
            // Debug.Log("linea velocity " + linear.x + " angular velocity " + angular.z);
        }
        else if(topic == "/robot2/cmd_vel")
        {
            /*
              I
            J K L
              M
            */
            if (Input.GetKey(KeyCode.I))
            {
                linear.x += 0.01;
            }
            if (Input.GetKey(KeyCode.M))
            {
                linear.x -= 0.01;
            }
            if (Input.GetKey(KeyCode.J))
            {
                angular.z -= 0.1;
            }
            if (Input.GetKey(KeyCode.L))
            {
                angular.z += 0.1;
            }
            if (Input.GetKey(KeyCode.K))
            {
                linear.x = 0;
                angular.z = 0;
            }
            Debug.Log("linear velocity " + linear.x + " angular velocity " + angular.z);
        }
        //Send untiy_odom to turtlebot_control
        TwistMsg Twist = new TwistMsg(
               linear,
               angular
            );

        // Finally send the message to server_endpoint.py running in ROS
        ros.Publish(topic, Twist);
    }
}
