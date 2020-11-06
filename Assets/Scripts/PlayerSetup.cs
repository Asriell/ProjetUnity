//préparations à l'arrivée de chaque joueur sur le serveur
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    //Les composants à désactiver (script de controle des autres joueurs par ex.)
    [SerializeField]
    private Behaviour[] componentsToDisable;
    //Layer des ennemis
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";
    //Layer des objets à ne pas afficher
    [SerializeField]
    private string dontDrawLayerName = "DontDraw";
    //apparence du joueur
    [SerializeField]
    private GameObject playerGraphics;
    //Interface Utilisateur du joueur
    [SerializeField]
    private GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;

    private void Start()
    {
        if (!isLocalPlayer)//si ça n'est pas le joueur local, on désactive des élements en trop, et on assigne le layer remote aux ennemis.
        {
            DisableComponents();
            AssignRemoteLayer();

        } else
        {

            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));//ne pas afficher le modele du personnage local (pour ne pas gêner la vue)

            playerUIInstance = Instantiate(playerUIPrefab);//instanciation du UI du joueur. 
            playerUIInstance.name = playerUIPrefab.name;

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No PlayerUI component on PlayerUIInstance !");
            } else
            {
                ui.SetController(GetComponent<PlayerController>());//fixer le controller au UI (pour la modification des jauges en fct des données)
            }
            GetComponent<Player>().SetupPlayer();//préparation du joueur

            string username = "Loading...";
            if (UserAccountManager.IsLoggedIn)
            {
                username = UserAccountManager.LoggedIn_Username;
            } else
            {
                username = transform.name;
            }

            CmdSetUsername(transform.name, username);
        }

    }

    [Command]
    private void CmdSetUsername(string playerId,string userName)
    {
        Player player = GameManager.GetPlayer(playerId);
        if (player != null)
        {
            player.userName = userName;
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)//a supprimer, pareil quedans Util.
    {
        obj.layer = newLayer;
        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private void DisableComponents()//désactivation des composants en trop
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void AssignRemoteLayer()//le joueur courant devient un ennemi par rapport au joueur local.
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }


    public override void OnStartClient() //se lit quand un client se connecte au serveur
    {
        base.OnStartClient();
        //ajout du joueur dans la base de joueurs du serveur.
        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<Player>());
    }
    private void OnDisable() //se lit quand on se déconnecte 
    {
        Destroy(playerUIInstance);//suppression du joueur qui se déconnecte de la base de joueurs et de la scene

        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }

}
