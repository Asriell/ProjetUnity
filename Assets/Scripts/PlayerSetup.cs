using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;
    private void Start()
    {
        RegisterPlayer("Player " + GetComponent<NetworkIdentity>().netId);
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

    private void RegisterPlayer(string _name)
    {
        transform.name = _name;
    } 

    private void OnDisable()
    {
        if(sceneCamera!=null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
