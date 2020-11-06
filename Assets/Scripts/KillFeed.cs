using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class KillFeed : MonoBehaviour
{

    [SerializeField]
    GameObject killFeedItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onPlayerKilledCallback += OnKill;
    }

    public void OnKill(string deadPlayer, string source)
    {
        GameObject KFIGO = Instantiate(killFeedItemPrefab, transform);
        KFIGO.GetComponent<KillFeedItem>().Setup(deadPlayer, source);
        Destroy(KFIGO, 5);
    }
}
