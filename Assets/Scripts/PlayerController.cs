//les événements et parametres du joueur
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
    {//Assignation des différents composants
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(JointDriveMode.Position, jointSpring, jointMaxForce);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
            }

            motor.SetVelocity(Vector3.zero);
            motor.SetRotationX(Vector3.zero);
            motor.SetThrusterForce(Vector3.zero);
            //motor.SetRotationY(Vector3.zero);

            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        //raycast sous le personnage, pour adapter sa physique en fonction d'où il retombe, pour un effet "rebond" sur plusieurs surfaces
        RaycastHit _hit;
        if(Physics.Raycast(transform.position,Vector3.down,out _hit,100,environmentMask))
        {
            joint.targetPosition = new Vector3(0, -_hit.point.y, 0);
        } else
        {
            joint.targetPosition = new Vector3(0, 0, 0);
        }

        //Avancer - Reculer et Gauche - Droite : animation + déplacement
        float _xMove = Input.GetAxis("Horizontal");
        float _zMove = Input.GetAxis("Vertical");

        Vector3 _velocity = (transform.right * _xMove + transform.forward * _zMove) * speed;
        animator.SetFloat("ForwardVelocity", _zMove);//animation

        motor.SetVelocity(_velocity);//envoi des accélérations au moteur -> déplacement
        //Orientations avec la souris : faire tourner le personage sur l'axe Y et la caméra sur l'axe X.
        float _yRotation = Input.GetAxisRaw("Mouse X");
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        currentRotationX -= _xRotation * lookSensitivityY;
        currentRotationX = Mathf.Clamp(currentRotationX, -cameraRotationLimit, cameraRotationLimit);
        Vector3 _rotationX = new Vector3(0, _yRotation, 0) * lookSensitivityX;
        Vector3 _rotationY = new Vector3(currentRotationX, 0, 0);
        motor.SetRotationX(_rotationX);

        motor.SetRotationY(_rotationY);

        Vector3 _thrusterForce = Vector3.zero;//pas de saut 
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0)//quand on saute et qu on a du carburant
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;//baisse du niveau de carburant
            if (thrusterFuelAmount >=0.01)//pour ne pas voler à 0
            {
                _thrusterForce = Vector3.up * thrusterForce;//envoi d un vecteur (0,TF,0) pour monter
                SetJointSettings(JointDriveMode.Position, 0, jointMaxForce);//pour ne pas que la gravité nous empêche de monter, on la désactive
            }
        } else//quand on ne saute pas
        {
            thrusterFuelAmount += thrusterFuelRefillSpeed * Time.deltaTime;//remplissage du carburant
            SetJointSettings(JointDriveMode.Position, jointSpring, jointMaxForce);//rétablissement de la gravité
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0, 1);//pour que la jauge reste entre 0% et 100%.

        motor.SetThrusterForce(_thrusterForce);//application des transformations pour voler
    }

    private void SetJointSettings (JointDriveMode _mode, float _jointSpring, float _maxForce)//changement de la physique de la scene
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
