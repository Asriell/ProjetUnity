using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;

    private bool isShooting = false;
    

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        if (cam == null)
        {
            Debug.LogError("camera not set !");
            this.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        if (currentWeapon.GetFireRate() <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        } else
        {
            if (Input.GetButton("Fire1") && !isShooting)
            {
                InvokeRepeating("Shoot", 0,1/currentWeapon.GetFireRate());
                isShooting = true;
            } else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
                isShooting = false;
            }
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.GetRange(), mask))
        {
            if (_hit.collider.tag == "Player")
            {
                CmdPlayerShot(_hit.collider.name,currentWeapon.GetDamage());
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string _playerId, int damage)
    {
        Debug.Log(_playerId + " a été touché ! " );
        GameManager.GetPlayer(_playerId).RpcTakeDamage(damage);
    }
}
