using UnityEngine;

public class CubeRotate : MonoBehaviour {
    [SerializeField] float rotate_speed = 0.50f;
    void Update () {
        Transform myTransform = this.transform;

　　// 現在の回転に加算する
        myTransform.Rotate (rotate_speed, 0.0f, 0.0f);
    }
}
