//Données du joueur
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    //Si il est vivant ou mort
    [SyncVar]
    private bool _isDead = false;

    public bool isDead
    {
        get { return _isDead; }

        protected set { _isDead = value; }
    }
    //PV max
    [SerializeField]
    private int maxHealth = 100;
    //PV actuels
    [SyncVar]
    private int currentHealth;
    //les objets à désactiver à la mort du joueur en fonction de ce qui était activé.
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [ClientRpc]//si le joueur est touché
    public void RpcTakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log(transform.name + "has now " + currentHealth + " HP, taken " + damage + " damages.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    //Le joueur est mort
    private void Die()
    {
        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++)//le joueur ne peut plus tirer ou se déplacer
        {
            disableOnDeath[i].enabled = false;
        }
        Collider coll = GetComponent<Collider>();//désactivation des collisions
        if (coll != null)
        {
            coll.enabled = false;
        }

        Debug.Log(transform.name + " is dead. ");

        StartCoroutine(Respawn());
    }
    //Réapparition en fonction des points de spawn définis dans la scene
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.spawnTime);
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(transform.name + " has respawned! ");
    }

    public void Setup()
    {//parametres initiaux du joueur
        wasEnabled = new bool[disableOnDeath.Length];

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }
    /*
    public void Update()
    {
        
        if (!isLocalPlayer)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.K))//mort si on appuie sur K
        {
            RpcTakeDamage(999);
        }
    }*/

    public void SetDefaults()
    {//tous les parametres par défaut du joueur
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        Collider coll = GetComponent<Collider>();
        if(coll != null)
        {
            coll.enabled = true;
        }
    }
}
