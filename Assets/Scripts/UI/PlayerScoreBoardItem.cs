using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScoreBoardItem : MonoBehaviour
{
    [SerializeField]
    private Text userNameText;

    [SerializeField]
    private Text killsText;

    [SerializeField]
    private Text deathsText;


    public void Setup(string username, int kills, int deaths)
    {
        userNameText.text = username;
        killsText.text = "Kills : " + kills;
        deathsText.text = "Deaths : " + deaths;
    }
}
