//Données du joueur par rapport à la scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    //caméra de tête du joueur
    private Camera cam;
    private Vector3 velocity;//accélération du joueur -> déplacement
    private Vector3 rotationX; //rotation selon l'axe X de la souris
    private Vector3 rotationY; //rotation selon l'axe Y de la souris
    private Vector3 thrusterForce;//Force des propulseurs
    private Rigidbody rb;//le rigidBody du joueur
    private float cameraRotateLimit;//empêcher la rotation de caméra du joueur

    PlayerMotor()
    {
        velocity = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {//Assignation des différents composants
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {//les différents événements
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
    {//bouge le joueur avec son rigidbody
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void Rotate()
    {//fait tourner le modele du joueur et sa caméra
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationX));
        //cam.transform.Rotate(-rotationY);
        cam.transform.localEulerAngles = rotationY;
    }

    private void Fly()
    {//fait monter le personnage
        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration) ;
        }
    }
}
