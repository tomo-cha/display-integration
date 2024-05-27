using UnityEngine;

public class Cube_move : MonoBehaviour {

    float speed = 0.001f;
    float minX = -3.0f;
    float maxX = 2.0f;

    bool increasing = true; //増加させるかどうかのフラグ

    void Update() {
        //オブジェクトの位置を取得し、変数に代入
        Vector3 pos = transform.position;

        //X座標が最大値に達した場合，増加フラグをfalseに設定
        if (pos.x >= maxX) {
            increasing = false;
        }
        //X座標が最小値に達した場合，増加フラグをtrueに設定
        else if (pos.x <= minX) {
            increasing = true;
        }

        //minXに到達した場合，X座標を加算して移動
        if (increasing) {
            pos.x += speed;
        }
        //maxXに到達した場合，X座標を減算して移動
        else {
            pos.x -= speed;
        }

        //新しい位置を設定
        transform.position = pos;
    }
}
