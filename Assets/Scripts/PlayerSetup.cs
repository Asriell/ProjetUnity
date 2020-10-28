using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();

        } else
        {
            sceneCamera = Camera.main;
            if (sceneCamera!=null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }

    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }


    public override void OnStartClient() //se lit quand un client se connecte au serveur
    {
        base.OnStartClient();

        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<Player>());
    }
    private void OnDisable() //se lit quand on se déconnecte
    {
        if(sceneCamera!=null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);
    }

}
