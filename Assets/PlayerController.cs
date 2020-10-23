﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float lookSensitivityX;

    [SerializeField]
    private float lookSensitivityY;

    private PlayerMotor motor;

    PlayerController()
    {
        speed = 5;
        lookSensitivityX = 3;
        lookSensitivityY = 3;
    }
    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");

        Vector3 _velocity = (transform.right * _xMove + transform.forward * _zMove) * speed;
        Console.WriteLine(speed);
        motor.SetVelocity(_velocity);

        float _yRotation = Input.GetAxisRaw("Mouse X");
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        Vector3 _rotationX = new Vector3(0, _yRotation, 0) * lookSensitivityX;
        Vector3 _rotationY = new Vector3(_xRotation, 0, 0) * lookSensitivityY;
        motor.SetRotationX(_rotationX);
        motor.SetRotationY(_rotationY);
    }
}
