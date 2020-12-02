//Classe de gestion des armes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    //Layer de l'arme (pour l'afficher dans la caméra dédiée)
    [SerializeField]
    private string weaponLayerName = "Weapon";
    //Emplacement de l'arme (= emplacement des "mains" du joueur)
    [SerializeField]
    private Transform weaponHolder;

    //Arme principale
    [SerializeField]
    private PlayerWeapon primaryWeapon;
    //Arme actuellement en mains
    private PlayerWeapon currentWeapon;
    //Effets de l'arme (flashs + impacts)
    private WeaponGraphics currentWeaponGraphics;

    public bool isReloading = false;

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
        EquipWeapon(primaryWeapon);//On commence la partie avec l'arme principale équipée
    }

    private void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;
        //clonage de l'arme à l'emplacement de la main du joueur
        GameObject weaponInstance = (GameObject)Instantiate(weapon.GetGraphics(), weaponHolder.position, weaponHolder.rotation);
        weaponInstance.transform.SetParent(weaponHolder);
        //Récupération du script graphique de l'arme
        currentWeaponGraphics = weaponInstance.GetComponent<WeaponGraphics>();

        if(currentWeaponGraphics == null)
        {
            Debug.Log("No \" WeaponGraphics \" script on the current weapon : ");
        }

        if (isLocalPlayer)
        {
            //L'arme ainsi que les effets auront le layer "Weapon", pour être vus dans la deuxieme caméra uniquement.
            Util.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public void Reload()
    {
        if (isReloading)
        {
            return;
        }
        StartCoroutine(ReloadThread());
    }
    public IEnumerator ReloadThread()
    {
        Debug.Log("<b>Reloading...</b>");
        isReloading = true;
        CmdOnReloadAnimation();
        yield return new WaitForSeconds(currentWeapon.GetReloadingTime());

        currentWeapon.SetCurrentMagazineSize(currentWeapon.GetMagazineCapacity());

        isReloading = false;

        Debug.Log("<b>Finished reloading</b>");
    }

    [Command]
    void CmdOnReloadAnimation()
    {
        RpcOnReloadAnimation();
    }

    [ClientRpc]
    void RpcOnReloadAnimation()
    {
        Animator anim = currentWeaponGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }
}
