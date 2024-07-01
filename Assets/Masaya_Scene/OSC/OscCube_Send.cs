using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

public class OscCube_Send : MonoBehaviour
{
	[SerializeField] string[] ipAddresses = { "127.0.0.1", "192.168.0.1", "192.168.0.1" }; // 複数のIPアドレス
	[SerializeField] int port = 10000;
	List<OscClient> clients = new List<OscClient>();

	void OnEnable()
	{
		foreach (string ip in ipAddresses)
		{
			clients.Add(new OscClient(ip, port));
		}
	}

	void OnDisable()
	{
		foreach (OscClient client in clients)
		{
			client.Dispose();
		}
		clients.Clear();
	}

	void Update()
{
    Vector3 objectPos = transform.position; // オブジェクトの位置を取得

    foreach (OscClient client in clients)
    {
        client.Send("/ObjectPos", objectPos.x, objectPos.y, objectPos.z); // OSCメッセージとして位置情報を送信
    }
}

}
