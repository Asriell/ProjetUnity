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
    private WeaponGraphics currentWeaponGraphics;


    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetWeaponGraphics()
    {
        return currentWeaponGraphics;
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

        currentWeaponGraphics = weaponInstance.GetComponent<WeaponGraphics>();

        if(currentWeaponGraphics == null)
        {
            Debug.Log("No \" WeaponGraphics \" script on the current weapon : ");
        }

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(weaponLayerName));
        }
    }
}
