
using System.Runtime.CompilerServices;
using UnityEngine;
[System.Serializable]
public class PlayerWeapon
{
    [SerializeField]
    private string name = "MP-2055";
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float range = 100;
    [SerializeField]
    private float fireRate = 0;
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

}
