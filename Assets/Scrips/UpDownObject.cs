using UnityEngine;
using System.Collections;

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

    private Vector3 initialPosition;
    private const float G=9.8f * 0.25f; // 落下速度調整
    private const float GROUND_Y = 0.0f; // 水滴の落ちる位置
    private float gravity = G;
    private float radius_coef = 1.0f; //radius = -sphereState.pos_y * radius_coef;
    private int SpacePushedCount=0;
    private bool Manual = false;
    private bool sphereIsActive = true; 

    void Start()
    {
        initialPosition = sphere.transform.position;
        sphereState.pos_x = initialPosition.x;
        sphereState.pos_y = initialPosition.y;
        sphereState.pos_z = initialPosition.z;
        sphereState.vel_x = 0.0f;
        sphereState.vel_y = 0.0f;
        sphereState.vel_z = 0.0f;

        // cylinderはsphereの真下にくる
        cylinderState.pos_x = initialPosition.x;
        cylinderState.pos_y = GROUND_Y;
        cylinderState.pos_z = initialPosition.z;
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
            if(sphereState.pos_y > GROUND_Y){
                gravity = G;
            }else{
                gravity = -G;
            }
            sphereState.vel_y += gravity*Time.deltaTime;
            sphereState.pos_y -= sphereState.vel_y*Time.deltaTime;

            if(sphereIsActive && sphereState.pos_y <= GROUND_Y + 0.015f){ //カメラが球体の中に侵入しないようにするために非アクティブにする距離
                sphereIsActive = false;
            }
            if(!sphereIsActive && sphereState.pos_y >= GROUND_Y + 0.015f){
                sphereIsActive = true;
            }
            sphere.SetActive(sphereIsActive);

            // cylinderのサイズ調整
            Vector3 scale = cylinder.transform.localScale;
            float radius = -sphereState.pos_y * radius_coef;
            if(radius < 0.0f){
                radius = 0.0f;
            }
            scale.x = radius;
            scale.y = 0.0f;
            scale.z = radius;
            cylinder.transform.localScale = scale;

        }
                    

        sphere.transform.position = new Vector3(sphereState.pos_x, sphereState.pos_y, sphereState.pos_z);
        cylinder.transform.position = new Vector3(cylinderState.pos_x, cylinderState.pos_y, cylinderState.pos_z);
    }
}