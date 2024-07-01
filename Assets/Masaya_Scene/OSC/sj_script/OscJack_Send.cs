using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

public class OscJack_Send : MonoBehaviour
{
	[SerializeField] string[] ipAddresses = { "127.0.0.1", "192.168.0.1", "192.168.0.1" }; // 複数のIPアドレス
	[SerializeField] int port = 12345;
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
		Vector3 mousePos = Input.mousePosition;

		foreach (OscClient client in clients)
		{
			client.Send("/MousePos", mousePos.x, mousePos.y);
		}
	}
}
