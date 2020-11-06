//Gestion des données de jeu.
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject SceneCamera;

    private void Awake()//pour n'instancier le gameManager qu'une fois : il doit etre unique.
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of GameManager in the scene !");
        } else
        {
            instance = this;
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (SceneCamera == null)
        {
            return;
        }
        SceneCamera.SetActive(isActive);
    }

    #region Player Managing
    //préfixe avant la numérotation des joeurs
    private const string PLAYER_TO_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();//tableau associatif des joueurs en fonction de leur identifiant

    public static void RegisterPlayer(string _netid,Player _player )//ajout d'un joueur dans le tableau
    {
        string id = PLAYER_TO_PREFIX + _netid;
        _player.transform.name = id;
        players.Add(id, _player);
    }

    public static void UnRegisterPlayer(string name)//retrait d'un joueur dans le tableau
    {
        players.Remove(name);
    }

    public static Player GetPlayer(string id)//le joueur du tableau avec l'id id.
    {
        return players[id];
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    /*private void OnGUI()//GUI de débogage
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

    #endregion
}
