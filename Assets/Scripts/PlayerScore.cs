using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour
{
    private int lastKills = 0;
    private int lastDeaths = 0;
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
        if (player.kills == lastKills && player.deaths == lastDeaths)
        {
            return;
        }
        int killsSinceLastTime = player.kills - lastKills;
        int deathsSinceLastTime = player.deaths - lastDeaths;

        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);
        int newKills = killsSinceLastTime + kills;
        int newDeaths = deathsSinceLastTime + deaths;
        string newData = DataTranslator.ValuesToData(newKills, newDeaths);

        lastKills = player.kills;
        lastDeaths = player.deaths;

        Debug.Log("Sending : " + newData);

        UserAccountManager.instance.SendData(newData);
    }
}
