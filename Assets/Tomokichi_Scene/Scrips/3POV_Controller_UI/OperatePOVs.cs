using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class OperatePOVs : MonoBehaviour
{
    private int OperateId = 1; //1,2,3
    private string topic = "/pov1/cmd_vel";
    ROSConnection ros;
    //Twist
    // Vector3Msg linear = new Vector3Msg(0f, 0f, 0f);
    // Vector3Msg angular = new Vector3Msg(0f, 0f, 0f);
    Vector3Msg[] linears = new Vector3Msg[3]; //３つのロボットの速度を保存しておきたいため配列に格納
    Vector3Msg[] angulars = new Vector3Msg[3];
    [SerializeField] TextMeshProUGUI linearText;
    [SerializeField] TextMeshProUGUI angularText;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(topic);

        // 速度の初期化
        for (int i = 0; i < linears.Length; i++)
        {
            linears[i] = new Vector3Msg(0f, 0f, 0f);
        }
        for (int i = 0; i < angulars.Length; i++)
        {
            angulars[i] = new Vector3Msg(0f, 0f, 0f);
        }

        debug();
    }
    void Update()
    {
        if (linears[OperateId - 1].x >= 0.22)
        {
            linears[OperateId - 1].x = 0.22;
        }
        if (angulars[OperateId - 1].z >= 2.84)
        {
            angulars[OperateId - 1].z = 2.84;
        }
        // Send untiy_odom to turtlebot_control
        TwistMsg Twist = new TwistMsg(
               linears[OperateId - 1],
               angulars[OperateId - 1]
            );

        // Finally send the message to server_endpoint.py running in ROS
        ros.Publish(topic, Twist);
    }

    public void SelectId(int id)
    {

        Image[] POVs = new Image[3];
        POVs[0] = GameObject.Find("POV1").GetComponent<Image>();
        POVs[1] = GameObject.Find("POV2").GetComponent<Image>();
        POVs[2] = GameObject.Find("POV3").GetComponent<Image>();

        // すべてのPOVの色を白にリセット
        foreach (var pov in POVs)
        {
            pov.color = Color.white;
        }

        OperateId = id;

        // 選択されたPOVの色を赤に設定
        POVs[OperateId - 1].color = Color.red;
        // トピックを設定
        topic = $"/pov{OperateId}/cmd_vel";
        debug();
    }
    public void Up()
    {
        linears[OperateId - 1].x += 0.01;
        debug();
    }

    public void Down()
    {
        linears[OperateId - 1].x -= 0.01;
        debug();
    }
    public void Left()
    {
        angulars[OperateId - 1].z -= 0.1;
        debug();
    }
    public void Right()
    {
        angulars[OperateId - 1].z += 0.1;
        debug();
    }
    public void Center()
    {
        linears[OperateId - 1].x = 0.0;
        angulars[OperateId - 1].z = 0.0;
        debug();
    }
    
    private void debug()
    {
        Debug.Log("now operating: " + OperateId);
        Debug.Log("send to topic: " + topic);
        Debug.Log("linea velocity " + linears[OperateId - 1].x + " angular velocity " + angulars[OperateId - 1].z);
        linearText.text = "linear.x = " + linears[OperateId - 1].x;
        angularText.text = "angular.z = " + angulars[OperateId - 1].z;
    }
}
