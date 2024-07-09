using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

public class OscJack_Receive : MonoBehaviour
{
	UnityEngine.Object sync = new UnityEngine.Object();
	
	[SerializeField] int port_Mouse = 12345;
	OscServer server_Mouse;
	
	Vector2 MousePos = new Vector2(0, 0);
	private string label = "";

	void OnEnable()
	{
		server_Mouse = new OscServer(port_Mouse);
		server_Mouse.MessageDispatcher.AddCallback(
			"/MousePos", // OSC address
			(string address, OscDataHandle data) => {
				lock(sync){
					MousePos.x = data.GetElementAsFloat(0);
					MousePos.y = data.GetElementAsFloat(1);
				}
			}
		);
	}
	
	void Update()
	{
		lock(sync){
			label = string.Format("({0}, {1})", MousePos.x, MousePos.y);
		}
	}
	
	void OnGUI()
	{
		GUI.color = Color.white;
		GUI.Label(new Rect(15, 100, 500, 30), label);
	}
	
	void OnDisable()
	{
		if (server_Mouse != null)
		{
			server_Mouse.Dispose();
			server_Mouse = null;
		}
	}
}
