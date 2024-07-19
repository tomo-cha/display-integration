using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CameraTouchInteraction : MonoBehaviour
{
    [SerializeField] GameObject markerObject;
    [SerializeField] float distanceThreshold = 0.2f;
    [SerializeField] GameObject UiGameObject;

    Camera cam;
    Vector3 lastPosition;
    bool initialProcess = false;

    GameObject parentObject;

    void Start ()
    {
        this.cam = FindObjectOfType<Camera>();

        parentObject = new GameObject("robot1_waypoints");
        parentObject.transform.position = Vector3.zero;
    }
        
    void Update () {
        if(Input.GetMouseButton(0) && !UiGameObject.activeSelf)
        {
            var mousePosition = Input.mousePosition;

            if(mousePosition.y < Screen.height * 0.8)
            {
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

                        var go = GameObject.Instantiate(markerObject, lastPosition, quat);
                        go.transform.parent = parentObject.transform;

                        lastPosition = worldPoint;
                    }
                }    
            }              
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Destroy(parentObject);
            
            parentObject = new GameObject("robot1_waypoints");
            initialProcess = false;
        }
    }
}
