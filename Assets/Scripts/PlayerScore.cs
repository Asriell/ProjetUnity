using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = GetComponent<Player>();
        StartCoroutine(SyncScoreLoop());
    }

    private void OnDestroy()
    {
        if(player != null)
        {
            SyncNow();
        }
    }

    private IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            SyncNow();
        }
        
    }

    private void SyncNow()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.GetData(OnDataReceived);
        }
    }

    private void OnDataReceived(string data)
    {
        if (player.kills == 0 && player.deaths == 0)
        {
            return;
        }
        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);
        int newKills = player.kills + kills;
        int newDeaths = player.deaths + deaths;
        string newData = DataTranslator.ValuesToData(newKills, newDeaths);

        player.kills = 0;
        player.deaths = 0;

        Debug.Log("Sending : " + newData);

        UserAccountManager.instance.SendData(newData);
    }
}
