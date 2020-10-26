using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof (PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lookSensitivityX;
    [SerializeField]
    private float lookSensitivityY;
    [SerializeField]
    private float thrusterForce = 1000;

    [Header("Spring settings ")]
    [SerializeField]
    private float jointSpring = 20;
    [SerializeField]
    private float jointMaxForce = 40;



    private PlayerMotor motor;
    private ConfigurableJoint joint;

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
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(JointDriveMode.Position, jointSpring, jointMaxForce);
    }

    // Update is called once per frame
    void Update()
    {
        
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");

        Vector3 _velocity = (transform.right * _xMove + transform.forward * _zMove) * speed;
        motor.SetVelocity(_velocity);

        float _yRotation = Input.GetAxisRaw("Mouse X");
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        Vector3 _rotationX = new Vector3(0, _yRotation, 0) * lookSensitivityX;
        Vector3 _rotationY = new Vector3(_xRotation, 0, 0) * lookSensitivityY;
        motor.SetRotationX(_rotationX);
        motor.SetRotationY(_rotationY);

        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(JointDriveMode.Position, 0, jointMaxForce);
        } else
        {
            SetJointSettings(JointDriveMode.Position, jointSpring, jointMaxForce);
        }
        motor.SetThrusterForce(_thrusterForce);
    }

    private void SetJointSettings (JointDriveMode _mode, float _jointSpring, float _maxForce)
    {
        joint.yDrive = new JointDrive {
            mode = _mode,
            positionSpring = _jointSpring,
            maximumForce = _maxForce
        };

    }
}
