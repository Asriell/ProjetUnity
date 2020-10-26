﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Camera cam;
    private Vector3 velocity;
    private Vector3 rotationX; //rotation selon l'axe X de la souris
    private Vector3 rotationY; //rotation selon l'axe Y de la souris
    private Vector3 thrusterForce;
    private Rigidbody rb;
    private float cameraRotateLimit;

    PlayerMotor()
    {
        velocity = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Rotate();
        Fly();
    }
    public void SetVelocity(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void SetRotationX(Vector3 _rotationX)
    {
        rotationX = _rotationX;
    }

    public void SetRotationY(Vector3 _rotationY)
    {
        rotationY = _rotationY;
    }

    public void SetThrusterForce(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    public void SetCameraRotateLimit(float _cameraRotateLimit)
    {
        cameraRotateLimit = _cameraRotateLimit;
    }

    private void Move()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void Rotate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationX));
        //cam.transform.Rotate(-rotationY);
        cam.transform.localEulerAngles = rotationY;
    }

    private void Fly()
    {
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration) ;
        }
    }
}
