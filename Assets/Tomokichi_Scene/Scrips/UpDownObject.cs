using UnityEngine;
using System.Collections;
using System;
public class UpDownObject : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] GameObject cylinder;
    private struct PhysicsState
    {
        public float pos_x;
        public float pos_y;
        public float pos_z;
        public float vel_x;
        public float vel_y;
        public float vel_z;
    }
    private PhysicsState sphereState;
    private PhysicsState cylinderState;

    private Vector3 sphereInitialPosition;
    private const float GROUND_Y = 0.0f; // 水滴の落ちる位置
    [SerializeField] float radius_max = 1.0f; //radiusの最大値
    [SerializeField] float velocityFallingSurfaceConstant = 1.0f; //水滴の落下速度、水面の拡縮速度の比例定数
    private int SpacePushedCount=0;
    private bool Manual = false;
    private bool sphereIsActive = true; 

    void Start()
    {
        sphereInitialPosition = sphere.transform.position;
        sphereState.pos_x = sphereInitialPosition.x;
        sphereState.pos_y = sphereInitialPosition.y;
        sphereState.pos_z = sphereInitialPosition.z;
        sphereState.vel_x = 0.0f;
        sphereState.vel_y = 0.0f;
        sphereState.vel_z = 0.0f;

        // cylinderはsphereの真下にくる
        cylinderState.pos_x = sphereInitialPosition.x;
        cylinderState.pos_y = GROUND_Y;
        cylinderState.pos_z = sphereInitialPosition.z;
        cylinderState.vel_x = 0.0f;
        cylinderState.vel_y = 0.0f;
        cylinderState.vel_z = 0.0f;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            SpacePushedCount += 1;
        }
        if(SpacePushedCount % 2 == 0){
            Manual = false;
        }else{
            Manual = true;
        }
        
        if(Manual){ // 水滴の高さを手動で調整
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                sphereState.pos_y += 0.001f;
            }else if(Input.GetKeyDown(KeyCode.DownArrow)){
                sphereState.pos_y -= 0.001f;
            }   
        }else{
            //sphereの位置
            sphereState.pos_y = sphereInitialPosition.y * Mathf.Cos(Time.time*velocityFallingSurfaceConstant); // sphereの初期位置を最大値としたcosの動きをする
            if(sphereState.pos_y < GROUND_Y){
                sphereState.pos_y = GROUND_Y;
            }

            //cylinderのradiusの大きさ
            float cylinderRadius = -radius_max * Mathf.Cos(Time.time*velocityFallingSurfaceConstant); // cosの動きで拡縮する
            if(cylinderRadius < 0.0f){
                cylinderRadius = 0.0f;
            }
            Vector3 scale = cylinder.transform.localScale;
            scale.x = cylinderRadius;
            scale.y = 0.0f;
            scale.z = cylinderRadius;
            cylinder.transform.localScale = scale;

            if(sphereIsActive && sphereState.pos_y <= GROUND_Y + 0.015f){ //カメラが球体の中に侵入しないようにするために非アクティブにする距離
                sphereIsActive = false;
            }
            if(!sphereIsActive && sphereState.pos_y >= GROUND_Y + 0.015f){
                sphereIsActive = true;
            }
            sphere.SetActive(sphereIsActive);
        }
                    

        sphere.transform.position = new Vector3(sphereState.pos_x, sphereState.pos_y, sphereState.pos_z);
        cylinder.transform.position = new Vector3(cylinderState.pos_x, cylinderState.pos_y, cylinderState.pos_z);
    }
}