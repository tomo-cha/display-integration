/************************************************************
************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

/************************************************************
************************************************************/
struct DATASET{
	public int key;
}

/************************************************************
■C#のQueueを使ってみる
	https://gametukurikata.com/csharp/queue
************************************************************/
public class OscJack_Receive : MonoBehaviour
{
	/****************************************
	****************************************/
	UnityEngine.Object sync = new UnityEngine.Object();
	
	[SerializeField] int port_Mouse = 12345;
	[SerializeField] int port_key = 12346;
	[SerializeField] int port_various = 12347;
	OscServer server_Mouse;
	OscServer server_key;
	OscServer server_various;
	
	Queue<DATASET> DataSet = new Queue <DATASET>();
	Vector2 MousePos = new Vector2(0, 0);
	private string label = "";
	
	/****************************************
	****************************************/
	/******************************
	******************************/
	void OnEnable()
	// void Start()
	{
		/********************
		********************/
		server_Mouse = new OscServer(port_Mouse);
		server_Mouse.MessageDispatcher.AddCallback(
			"/MousePos", // OSC address
			(string address, OscDataHandle data) => {
				lock(sync){
					MousePos.x = data.GetElementAsFloat(0);  // GetElementAsInt, GetElementAsFloat, GetElementAsString
					MousePos.y = data.GetElementAsFloat(1);
				}
			}
		);
		
		/********************
		********************/
		server_key = new OscServer(port_key);
		server_key.MessageDispatcher.AddCallback(
			"/key", // OSC address
			(string address, OscDataHandle data) => {
				DATASET Data = new DATASET();
				Data.key = data.GetElementAsInt(0); // GetElementAsInt, GetElementAsFloat, GetElementAsString
				
				lock(sync){
					DataSet.Enqueue(Data);
				}
			}
		);
		
		/********************
		********************/
		server_various = new OscServer(port_various);
		server_various.MessageDispatcher.AddCallback(
			"/various", // OSC address
			(string address, OscDataHandle data) => {
				lock(sync){
					Debug.Log(data.GetElementAsString(0));
					Debug.Log(data.GetElementAsFloat(1));
					Debug.Log(data.GetElementAsInt(2));
					Debug.Log(data.GetElementAsString(3));
					Debug.Log(data.GetElementAsFloat(4));
					Debug.Log(data.GetElementAsInt(5));
					Debug.Log(data.GetElementAsString(6));
					Debug.Log(data.GetElementAsFloat(7));
					Debug.Log(data.GetElementAsInt(8));
				}
			}
		);
	}
	
	/******************************
	******************************/
	void Update(){
		/********************
		********************/
		lock(sync){
			label = string.Format("({0}, {1})", MousePos.x, MousePos.y);
		}
		
		/********************
		********************/
		lock(sync){
			for(int i = 0; 0 < DataSet.Count; i++){
				DATASET Data = new DATASET();
				Data = DataSet.Dequeue();
				
				string message = string.Format("{0}th : key = {1}", i, Data.key);
				Debug.Log(message);
			}
		}
	}
	
	/******************************
	******************************/
	void OnGUI()
	{
		GUI.color = Color.white;
		
		/********************
		********************/
		GUI.Label(new Rect(15, 100, 500, 30), label);
	}
	
	/******************************
	OnDestroy で掃除をすると、本scriptをattachしたobjectがactiveでない場合でも、本処理が走る。
	その場合は、
		if(server_key != null)
	などが必要
	******************************/
	void OnDisable()
	// void OnDestroy()
	{
		server_key.Dispose();
		server_key = null;
		
		server_Mouse.Dispose();
		server_Mouse = null;
		
		server_various.Dispose();
		server_various = null;
	}
}
