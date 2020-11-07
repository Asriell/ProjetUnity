//Les informations de l'arme du joueur
using System.Runtime.CompilerServices;
using UnityEngine;
[System.Serializable]
public class PlayerWeapon
{
    //nom de l'arme
    [SerializeField]
    private string name = "MP-2055";
    [SerializeField]
    //ses dégâts
    private int damage = 5;
    //sa portée
    [SerializeField]
    private float range = 100;
    //sa cadence de tirs (0 = coup par coup)
    [SerializeField]
    private float fireRate = 14;

    private int magazineCapacity = 30;
    private int currentMagazineSize;
    private float reloadingTime = 2;

    //son modele graphique
    [SerializeField]
    private GameObject graphics;

    public string GetName()
    {
        return name;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetRange()
    {
        return range;
    }

    public float GetFireRate ()
    {
        return fireRate;
    }

    public GameObject GetGraphics ()
    {
        return graphics;
    }

    public int GetMagazineCapacity ()
    {
        return magazineCapacity;
    }

    public int GetCurrentMagazineSize()
    {
        return currentMagazineSize;
    }

    public float GetReloadingTime()
    {
        return reloadingTime;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public void SetDamage(int _damage)
    {
        damage = _damage;
    }

    public void SetRange(float _range)
    {
        range = _range;
    }

    public void SetFireRate (float _fireRate)
    {
        fireRate = _fireRate;
    }

    public void SetGraphics (GameObject _graphics)
    {
        graphics = _graphics;
    }

    public void SetMagazineCapacity(int _magazineCapacity)
    {
        magazineCapacity = _magazineCapacity;
    }

    public void SetCurrentMagazineSize(int _currentMagazineSize)
    {
        currentMagazineSize = _currentMagazineSize;
    }

    public void SetReloadingTime(float _reloadingTime)
    {
        reloadingTime = _reloadingTime;
    }

    public PlayerWeapon()
    {
        currentMagazineSize = magazineCapacity;
    }


}
