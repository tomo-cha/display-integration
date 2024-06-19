using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

public class RosConnectionManager : MonoBehaviour
{
    ROSConnection ros;
    // Start is called before the first frame update
    void Start()
    {
      //  ros = new ROSConnection();
        ros = ROSConnection.GetOrCreateInstance();
        ros.ConnectOnStart = false;
        ros.RosIPAddress = "127.0.0.1";
        ros.RosPort = 10000;
        ros.ShowHud = true;

        ros.NetworkTimeoutSeconds = 1f;
        
        //ros.Connect();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) ros.Connect();
       // if (ros.isActiveAndEnabled) Debug.Log("Connected!!!!");
    }

    private void OnApplicationQuit()
    {
        //Debug.Log("exit!!!");
        //ros.Disconnect();
    }

}
