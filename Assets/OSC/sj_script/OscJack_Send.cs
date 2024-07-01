/************************************************************
************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

/************************************************************
************************************************************/
public class OscJack_Send : MonoBehaviour
{
	/****************************************
	****************************************/
	[SerializeField] string ipAddress = "127.0.0.1";
	[SerializeField] int port = 12345;
	OscClient client;
	bool b_Send = false;

	/****************************************
	****************************************/
	/******************************
	******************************/
	// void Start()
	void OnEnable()
	{
		client = new OscClient(ipAddress, port);
	}

	/******************************
	******************************/
	void OnDisable()
	// void OnDestroy()
	{
		client.Dispose();
	}

	/******************************
	******************************/
	void Update()
	{
		/********************
		********************/
		if (Input.GetKeyDown(KeyCode.S)) {
			b_Send = !b_Send;
			if(b_Send)	Debug.Log("StartSending");
			else		Debug.Log("StopSending");
		}
		
		/********************
		public void Send(string address);
		public void Send(string address, int data);
		public void Send(string address, int element1, int element2);
		public void Send(string address, int element1, int element2, int element3);
		public void Send(string address, float data);
		public void Send(string address, float element1, float element2);
		public void Send(string address, float element1, float element2, float element3);
		public void Send(string address, float element1, float element2, float element3, float element4);
		public void Send(string address, string data);
		********************/
		Vector3 mousePos = Input.mousePosition;
		if(b_Send){
			client.Send("/MousePos", mousePos.x, mousePos.y);
		}
	}
}

