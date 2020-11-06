//l'Interface Utilisateur du joueur (jauges, réticule,...)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //jauge de l'état des propulseurs
    [SerializeField]
    private RectTransform thrusterFuelFill;
    //les différentes données du joueur
    private PlayerController playerController;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject scoreBoard;

    public void SetFuelAmount(float amount)
    {
        thrusterFuelFill.localScale = new Vector3(1, amount, 1);//étire ou rétracte la jauge en fonction d'une quantité
    }

    public void SetController(PlayerController controller)
    {
        playerController = controller;
    }
    private void Update()
    {
        SetFuelAmount(playerController.GetThrusterFuelAmount());//affichage à chaque frame

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleScoreBoard();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    public void ToggleScoreBoard()
    {
        scoreBoard.SetActive(!scoreBoard.activeSelf);
    }

    private void Start()
    {
        PauseMenu.isOn = false;
    }
}
