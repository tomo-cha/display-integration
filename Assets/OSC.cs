using UnityEngine;

public class ServerExample : MonoBehaviour
{
   public void OnMessage(float value) {
       Debug.Log(value);
   }
}