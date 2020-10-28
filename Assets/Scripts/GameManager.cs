using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private const string PLAYER_TO_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netid,Player _player )
    {
        string id = PLAYER_TO_PREFIX + _netid;
        _player.transform.name = id;
        players.Add(id, _player);
    }

    public static void UnRegisterPlayer(string name)
    {
        players.Remove(name);
    }

    public static Player GetPlayer(string id)
    {
        return players[id];
    }

    /*private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();
        foreach(string id in players.Keys)
        {
            GUILayout.Label(id + "   -   " + players[id].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/
}
