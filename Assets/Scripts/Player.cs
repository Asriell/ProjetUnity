//Données du joueur
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
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

    [SyncVar]
    public string userName = "Loading...";

    public int deaths;
    public int kills;
    //PV actuels
    [SyncVar]
    private int currentHealth;
    //les objets à désactiver à la mort du joueur en fonction de ce qui était activé.
    [SerializeField]
    private Behaviour[] disableOnDeath;//pour les scripts

    private GameObject[] disableGameObjectsOnDeath = new GameObject[2];//pour les game objects

    private bool[] wasEnabled;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    private void Start()
    {
        disableGameObjectsOnDeath[0] = transform.GetChild(0).gameObject;
        disableGameObjectsOnDeath[1] = transform.GetChild(1).gameObject;
    }


    [ClientRpc]//si le joueur est touché
    public void RpcTakeDamage(int damage, string sourceId)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log(transform.name + "has now " + currentHealth + " HP, taken " + damage + " damages.");

        if(currentHealth <= 0)
        {

            Die(sourceId);
        }
    }
    //Le joueur est mort
    private void Die(string sourceId = null)
    {
 
        isDead = true;
        if (sourceId != null)
        {
            Player sourcePlayer = GameManager.GetPlayer(sourceId);
            if (sourcePlayer != null)
            {
                sourcePlayer.kills++;
                GameManager.instance.onPlayerKilledCallback(userName, sourcePlayer.userName);
            }
        }

        deaths++;
        for (int i = 0; i < disableOnDeath.Length; i++)//le joueur ne peut plus tirer ou se déplacer
        {
            disableOnDeath[i].enabled = false;
        }
        
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)//le joueur ne peut plus tirer ou se déplacer
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        Collider coll = GetComponent<Collider>();//désactivation des collisions
        if (coll != null)
        {
            coll.enabled = false;
        }

        Debug.Log(transform.name + " is dead. ");
        GameObject explosion = Instantiate(deathEffect, transform.position,Quaternion.identity);
        Destroy(explosion, 3);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        StartCoroutine(Respawn());
    }
    //Réapparition en fonction des points de spawn définis dans la scene
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.spawnTime);

        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        yield return new WaitForSeconds(0.1f);
        SetupPlayer();

        GameObject spawnEffectObject = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(spawnEffectObject, 2);


        Debug.Log(transform.name + " has respawned! ");
    }

    public void SetupPlayer()
    {//parametres initiaux du joueur
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(firstSetup)
        {
            disableGameObjectsOnDeath[0] = transform.GetChild(0).gameObject;
            disableGameObjectsOnDeath[1] = transform.GetChild(1).gameObject;
            wasEnabled = new bool[disableOnDeath.Length];

            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
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
        
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)//le joueur ne peut plus tirer ou se déplacer
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }
        Collider coll = GetComponent<Collider>();
        if(coll != null)
        {
            coll.enabled = true;
        }

        

    }
}
