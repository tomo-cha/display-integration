using UnityEngine;
using System.Collections;

public class UpDownObject : MonoBehaviour
{

    private Vector3 initialPosition;
    private const float G=9.8f * 0.25f;
    private const float GROUND_Y = 0.018f;
    private float gravity = G;
    private float x;
    private float y;
    private float z;
    private float y_vel=0;
    private int SpacePushedCount=0;
    private bool Manual = false;

    void Start()
    {
        initialPosition = transform.position;
        x = initialPosition.x;
        y = initialPosition.y;
        z = initialPosition.z;
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
        
        if(Manual){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                y += 0.001f;
            }else if(Input.GetKeyDown(KeyCode.DownArrow)){
                y -= 0.001f;
            }   
        }else{
            if(y > GROUND_Y){
                gravity = G;
            }else{
                gravity = -G;
            }
            y_vel += gravity*Time.deltaTime;
            y -= y_vel*Time.deltaTime;
        }
        x = x;
        float y_=y;
        if(y<GROUND_Y){
            y_ = GROUND_Y;
        }
        z = z;

        transform.position = new Vector3(x, y_, z);
    }
}