using UnityEngine;

public class CubeMove : MonoBehaviour {

    [SerializeField] float speed = 0.1f;
    [SerializeField] float minZ = -5.0f;
    [SerializeField] float maxZ = 5.0f;

    bool increasing = true; //増加させるかどうかのフラグ

    void Update() {
        //オブジェクトの位置を取得し、変数に代入
        Vector3 pos = transform.position;

        //z座標が最大値に達した場合，増加フラグをfalseに設定
        if (pos.z >= maxZ) {
            increasing = false;
        }
        //z座標が最小値に達した場合，増加フラグをtrueに設定
        else if (pos.z <= minZ) {
            increasing = true;
        }

        //minXに到達した場合，z座標を加算して移動
        if (increasing) {
            pos.z += speed;
        }
        //maxXに到達した場合，z座標を減算して移動
        else {
            pos.z -= speed;
        }

        //新しい位置を設定
        transform.position = pos;
    }
}
