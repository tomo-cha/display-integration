using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMotionController : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] float vel_x = 1.0f;
    private Vector3 sphereInitialPosition;
    void Start()
    {
        sphereInitialPosition = sphere.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float pos_x = 2.0f * Mathf.Cos(Time.time*vel_x);
        sphere.transform.position = new Vector3(pos_x, sphereInitialPosition.y, sphereInitialPosition.z);
    }
}
