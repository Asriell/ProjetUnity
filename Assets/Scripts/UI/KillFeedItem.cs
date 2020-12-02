using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField]
    private Text KillFeedItemText;

    public void Setup(string deadPlayer,string source)
    {
        KillFeedItemText.text = "<b>" + source + "</b>" + " killed " + "<b>" + deadPlayer + "</b>";
    }
}
