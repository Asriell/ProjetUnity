using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + "has now " + currentHealth + " HP, taken " + damage + " damages.");
    }

    private void Awake()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
