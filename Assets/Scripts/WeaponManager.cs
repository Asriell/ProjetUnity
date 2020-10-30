using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;


    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    private void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    private void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;

        GameObject weaponInstance = (GameObject)Instantiate(weapon.GetGraphics(), weaponHolder.position, weaponHolder.rotation);
        weaponInstance.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
        {
            weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }
}
