
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    [SerializeField]
    private string name = "Glock";
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float range = 100;

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

}
