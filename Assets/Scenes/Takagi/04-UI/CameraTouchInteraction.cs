using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchInteraction : MonoBehaviour
{
    [SerializeField] GameObject markerObject;
    [SerializeField] float distanceThreshold = 0.2f;

    Camera cam;

    Vector3 lastPosition;

    bool initialProcess = false;

    void Start ()
    {
        this.cam = FindObjectOfType<Camera>();
    }
        
    void Update () {
        if(Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = this.cam.transform.position.y;
            var worldPoint = this.cam.ScreenToWorldPoint(mousePosition);

            worldPoint.y = 0.0f;

            if(!initialProcess)
            {
                lastPosition = worldPoint;
                initialProcess = true;
            }
            else
            {
                if(Vector3.Distance(lastPosition, worldPoint) > distanceThreshold)
                {
                    float a = (worldPoint.z - lastPosition.z) / (worldPoint.x - lastPosition.x);

                    float y_angular = Mathf.Atan(a);
                    float y_angular_rad = y_angular / Mathf.PI * 180f;

                    if((worldPoint.x - lastPosition.x) < 0) y_angular_rad += 180f;

                    Quaternion quat = Quaternion.Euler(0f, -y_angular_rad, 0f);

                    GameObject.Instantiate(markerObject, lastPosition, quat);

                    lastPosition = worldPoint;
                }

            }      
            
        }
    }
}
