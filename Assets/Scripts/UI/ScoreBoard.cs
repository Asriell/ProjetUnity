using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{

    [SerializeField]
    GameObject playerScoreBoardItem;

    [SerializeField]
    Transform playerScoreBoardList;
    private void OnEnable()
    {
        foreach ( Player player in GameManager.GetAllPlayers())
        {
            //Debug.Log(player.userName + " | " + player.kills + " | " + player.deaths);
            GameObject ItemGO = Instantiate(playerScoreBoardItem,playerScoreBoardList);
            PlayerScoreBoardItem item = ItemGO.GetComponent<PlayerScoreBoardItem>();
            if (item != null)
            {
                item.Setup(player.userName, player.kills, player.deaths);
            }
        }
    }
    private void OnDisable()
    {
        foreach (Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }
    }
}
