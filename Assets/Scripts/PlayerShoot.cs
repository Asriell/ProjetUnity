using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("camera not set !");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.GetRange(), mask))
        {
            if (_hit.collider.tag == "Player")
            {
                CmdPlayerShot(_hit.collider.name,weapon.GetDamage());
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string _playerId, int damage)
    {
        Debug.Log(_playerId + " a été touché ! " );
        GameManager.GetPlayer(_playerId).TakeDamage(damage);
    }
}
