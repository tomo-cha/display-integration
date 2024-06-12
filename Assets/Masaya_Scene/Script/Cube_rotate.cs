using UnityEngine;

public class Cube_rotate : MonoBehaviour {
    void Update () {
        Transform myTransform = this.transform;

　　// 現在の回転に加算する
        myTransform.Rotate (0.0f, 0.0f, 0.05f);
    }
}
