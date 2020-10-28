
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    [SerializeField]
    private string name = "Glock";
    [SerializeField]
    private float damage = 10;
    [SerializeField]
    private float range = 100;

    public string GetName()
    {
        return name;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetRange()
    {
        return range;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    public void SetRange(float _range)
    {
        range = _range;
    }

}
