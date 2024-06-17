using UnityEngine;

public class cube_move_sender : MonoBehaviour {

    float speed = 0.001f;
    float minZ = -3.0f;
    float maxZ = 3.0f;

    bool increasing = true; //増加させるかどうかのフラグ

    void Update() {
        //オブジェクトの位置を取得し、変数に代入
        Vector3 pos = transform.position;

        //X座標が最大値に達した場合，増加フラグをfalseに設定
        if (pos.z >= maxZ) {
            increasing = false;
        }
        //X座標が最小値に達した場合，増加フラグをtrueに設定
        else if (pos.z <= minZ) {
            increasing = true;
        }

        //minXに到達した場合，X座標を加算して移動
        if (increasing) {
            pos.z += speed;
        }
        //maxXに到達した場合，X座標を減算して移動
        else {
            pos.z -= speed;
        }

        //新しい位置を設定
        transform.position = pos;
    }
}
