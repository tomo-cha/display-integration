using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHolizontal : MonoBehaviour
{
    [SerializeField] float vel_z = 1.0f;
    private Vector3 InitialPosition;
    void Start()
    {
        InitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float pos_z = 3.0f * Mathf.Cos(Time.time*vel_z) + InitialPosition.x;
        transform.position = new Vector3(InitialPosition.x, InitialPosition.y, pos_z);
    }
}
