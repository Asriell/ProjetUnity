//classe pour gérer le systeme de tir du joueur
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    //informations sur l'arme du joueur
    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;

    private bool isShooting = false;
    
    //camera de l'arme
    [SerializeField]
    private Camera cam;
    //layers que le joueur a le droit de toucher
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
        if (PauseMenu.isOn)
        {
            return;
        }
        currentWeapon = weaponManager.GetCurrentWeapon();

        if(Input.GetKeyDown(KeyCode.R) && currentWeapon.GetCurrentMagazineSize() < currentWeapon.GetMagazineCapacity())
        {
            weaponManager.Reload();
            return;
        }

        if (currentWeapon.GetFireRate() <= 0)//Si c est une arme au coup par coup
        {
            if (Input.GetButtonDown("Fire1"))//Seulement quand le bouton de feu est appuyé
            {
                Shoot();
            }
        } else
        {
            if (Input.GetButton("Fire1") && !isShooting)//Quand le bouton de feu est maintenu
            {
                InvokeRepeating("Shoot", 0,1/currentWeapon.GetFireRate());//répète la fonction de tir 1/cadence fois par seconde.
                isShooting = true;
            } else if (Input.GetButtonUp("Fire1"))//quand le bouton est relâché
            {
                CancelInvoke("Shoot");//arrête InvokeRepeating
                isShooting = false;
            }
        }
    }

    [Command]//dire au serveur que quelqu'un a tiré
    private void CmdOnShoot ()
    {
        RpcDoShootEffect();
    }

    [ClientRpc]//application de l'effet de tir sur tous les clients à la fois
    private void RpcDoShootEffect()
    {
        weaponManager.GetWeaponGraphics().muzzleFlash.Play();
    }

    [Command]//dire au serveur que quelqu'un a tiré sur une surface
    private void CmdOnHit(Vector3 pos,Vector3 normal)
    {
        RpcDoHitEffect(pos,normal);
    }

    [ClientRpc]//Instancier un impact sur tous les clients à la fois
    private void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        //créer un objet pour l'impact qui disparait deux secondes apres.
        GameObject hitEffect = Instantiate(weaponManager.GetWeaponGraphics().hitEffectPrefab,pos,Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2);
    }

    [Client]//action de tirer sur un seul client (celui du joueur qui a tiré)
    private void Shoot()
    {
        if (!isLocalPlayer)//empêche le controle de l'arme de tous les joueurs.
        {
            return;
        }

        if (weaponManager.isReloading)
        {
            return;
        }

        if (currentWeapon.GetCurrentMagazineSize() <= 0)//empêche le controle de l'arme de tous les joueurs.
        {
            Debug.Log("No ammo !");
            weaponManager.Reload();
            return;
        }
        currentWeapon.SetCurrentMagazineSize(currentWeapon.GetCurrentMagazineSize() - 1);
        Debug.Log(currentWeapon.GetCurrentMagazineSize() + "remaining");
        CmdOnShoot();//dire au serveur qu on a tiré
        RaycastHit _hit;//objet touché par le ray cast

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.GetRange(), mask))//si le raycast a touché un game object
        {
            if (_hit.collider.tag == "Player")//si c est un joueur
            {
                CmdPlayerShot(_hit.collider.name,currentWeapon.GetDamage(),transform.name);//indique au serveur qu'un joueur a été touché
            }
            CmdOnHit(_hit.point, _hit.normal);//indique au serveur où instancier le point d'impact + la rotation.
        }
        if(currentWeapon.GetCurrentMagazineSize()<= 0)
        {
            weaponManager.Reload();
        }
    }

    [Command]//applique les dégats au joueur dans la liste des joueurs de GameManager.
    private void CmdPlayerShot(string _playerId, int damage,string sourceId)
    {
        Debug.Log(_playerId + " a été touché ! " );
        GameManager.GetPlayer(_playerId).RpcTakeDamage(damage,sourceId);
    }
}
