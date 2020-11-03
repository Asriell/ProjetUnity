using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Video;
using System.Text.RegularExpressions;

public class JoinGame : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();
    private NetworkManager networkManager;

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();

    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo,List<MatchInfoSnapshot>matchList)
    {
        status.text = "";
        if (matchList == null)
        {
            status.text = "Cannot get match list !";
            return;
        }

        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject roomListItemGO = Instantiate(roomListItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent);
            RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
            if (roomListItem != null)
            {
                roomListItem.Setup(match);
            }

            roomList.Add(roomListItemGO);
        }
        if (roomList.Count == 0)
        {
            status.text = "No room available !";
        }
    }

    private void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }
}
