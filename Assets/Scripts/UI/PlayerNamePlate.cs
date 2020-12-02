using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePlate : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText;

    [SerializeField]
    private Player player;

    [SerializeField]
    private RectTransform HealthBarFill;

    private void Update()
    {
        playerNameText.text = player.userName;
        HealthBarFill.localScale = new Vector3(player.GetHealthPercentage(), 1, 1);
    }
}
