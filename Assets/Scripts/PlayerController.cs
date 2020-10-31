using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof (PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField]
    private float cameraRotationLimit = 85;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lookSensitivityX;
    [SerializeField]
    private float lookSensitivityY;

    [Header("Thruster")]
    [SerializeField]
    private float thrusterForce = 1000;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1;
    [SerializeField]
    private float thrusterFuelRefillSpeed = 0.3f;
    private float thrusterFuelAmount = 1;

    [Header("Spring settings ")]
    [SerializeField]
    private float jointSpring = 20;
    [SerializeField]
    private float jointMaxForce = 40;
    private float currentRotationX = 0;
    [SerializeField]
    private LayerMask environmentMask;

    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

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
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit _hit;
        if(Physics.Raycast(transform.position,Vector3.down,out _hit,100,environmentMask))
        {
            joint.targetPosition = new Vector3(0, -_hit.point.y, 0);
        } else
        {
            joint.targetPosition = new Vector3(0, 0, 0);
        }


        float _xMove = Input.GetAxis("Horizontal");
        float _zMove = Input.GetAxis("Vertical");

        Vector3 _velocity = (transform.right * _xMove + transform.forward * _zMove) * speed;
        animator.SetFloat("ForwardVelocity", _zMove);

        motor.SetVelocity(_velocity);

        float _yRotation = Input.GetAxisRaw("Mouse X");
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        currentRotationX -= _xRotation * lookSensitivityY;
        currentRotationX = Mathf.Clamp(currentRotationX, -cameraRotationLimit, cameraRotationLimit);
        Vector3 _rotationX = new Vector3(0, _yRotation, 0) * lookSensitivityX;
        Vector3 _rotationY = new Vector3(currentRotationX, 0, 0);
        motor.SetRotationX(_rotationX);

        motor.SetRotationY(_rotationY);

        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
            if (thrusterFuelAmount >=0.01)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(JointDriveMode.Position, 0, jointMaxForce);
            }
        } else
        {
            thrusterFuelAmount += thrusterFuelRefillSpeed * Time.deltaTime;
            SetJointSettings(JointDriveMode.Position, jointSpring, jointMaxForce);
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0, 1);

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

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }
}
