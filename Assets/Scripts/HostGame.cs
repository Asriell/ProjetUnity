using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 6;

    private string roomName;
    private string roomPassword;

    private NetworkManager netWorkManager;

    private void Start()
    {
        netWorkManager = NetworkManager.singleton; 
        if(netWorkManager.matchMaker == null)
        {
            netWorkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    public void SetRoomSize(uint size)
    {
        roomSize = size;
    }

    public void SetRoomPassword(uint size)
    {
        roomSize = size;
    }

    public void CreateRoom()
    {
        if (roomName != "" && roomName != null)
        {
            Debug.Log("room " + roomName + " created - " + roomSize + " slots !");
            netWorkManager.matchMaker.CreateMatch(roomName, roomSize, false, "","","",0,0,netWorkManager.OnMatchCreate);
        }
    }
}
