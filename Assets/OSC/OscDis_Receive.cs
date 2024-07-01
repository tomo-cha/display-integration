using UnityEngine;
using OscJack;

public class OscDis_Receive : MonoBehaviour
{
    UnityEngine.Object sync = new UnityEngine.Object();

    [SerializeField] int port_ObjectPos = 10001;
    OscServer server_ObjectPos;

    Vector3 ObjectPos = Vector3.zero;
    private string label = "";

    void OnEnable()
    {
        server_ObjectPos = new OscServer(port_ObjectPos);
        server_ObjectPos.MessageDispatcher.AddCallback(
            "/ObjectPos", // OSC address
            (string address, OscDataHandle data) => {
                lock (sync)
                {
					//受信したZ座標をX座標に適応させる
                    ObjectPos.x = data.GetElementAsFloat(2); // X座標の受信
                    ObjectPos.y = 0; // Y座標の受信
                    ObjectPos.z = -10; // Z座標の受信
                }
            }
        );
    }

    void Update()
    {
        lock (sync)
        {
            label = string.Format("Object Position: ({0}, {1}, {2})", ObjectPos.x, ObjectPos.y, ObjectPos.z);
        }
		//ここで受信したZ座標をX座標に適応させるためにxとz座標を入れ替えている
		Vector3 newPosition = new Vector3(ObjectPos.x, ObjectPos.y, ObjectPos.z);
		//新しく設定した座標を現在のオブジェクト座標として設定する
        transform.position = newPosition;
    }

	//OSCの受信を確認するために，受信された値をGUIとして表示する
    // void OnGUI()
    // {
    //     GUI.color = Color.white;
    //     GUI.Label(new Rect(15, 100, 500, 30), label);
    // }

    void OnDisable()
    {
        if (server_ObjectPos != null)
        {
            server_ObjectPos.Dispose();
            server_ObjectPos = null;
        }
    }
}


