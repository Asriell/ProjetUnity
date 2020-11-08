//l'Interface Utilisateur du joueur (jauges, réticule,...)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //jauge de l'état des propulseurs
    [SerializeField]
    private RectTransform thrusterFuelFill;

    [SerializeField]
    private RectTransform HealthFill;

    //les différentes données du joueur
    private PlayerController playerController;
    private Player player;
    private WeaponManager weaponManager;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject scoreBoard;

    [SerializeField]
    private Text ammoText; 


    public void SetFuelAmount(float amount)
    {
        thrusterFuelFill.localScale = new Vector3(1, amount, 1);//étire ou rétracte la jauge en fonction d'une quantité
    }

    public void SetHealthAmount(float amount)
    {
        HealthFill.localScale = new Vector3(1, amount, 1);//étire ou rétracte la jauge en fonction d'une quantité
    }

    public void SetAmmoAmount(int amount)
    {
        ammoText.text = amount.ToString();
    }

    public void SetPlayer(Player _player)
    {
        player = _player;
        playerController = _player.GetComponent<PlayerController>();
        weaponManager = _player.GetComponent<WeaponManager>();
    }
    private void Update()
    {
        SetFuelAmount(playerController.GetThrusterFuelAmount());//affichage à chaque frame
        SetHealthAmount(player.GetHealthPercentage());
        SetAmmoAmount(weaponManager.GetCurrentWeapon().GetCurrentMagazineSize());

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
