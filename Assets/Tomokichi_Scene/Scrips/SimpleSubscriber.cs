using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class SimpleSubscriber : MonoBehaviour
{
    ROSConnection ros;
    // [SerializeField] targetObject;
    [SerializeField] string topic = "";
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<TwistMsg>(topic, move);
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    void move(TwistMsg Msg)
     {
        this.transform.Translate((float)Msg.linear.x * Time.deltaTime,0,0);
        this.transform.Rotate(new Vector3(0,0,(float)Msg.angular.z));
        Debug.Log("linearx: "+ Msg.linear.x);
        Debug.Log("angularz: "+ Msg.angular.z);
     }
}
