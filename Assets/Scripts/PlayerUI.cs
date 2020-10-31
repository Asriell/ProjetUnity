using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController playerController;

    public void SetFuelAmount(float amount)
    {
        thrusterFuelFill.localScale = new Vector3(1, amount, 1);
    }

    public void SetController(PlayerController controller)
    {
        playerController = controller;
    }
    private void Update()
    {
        SetFuelAmount(playerController.GetThrusterFuelAmount());
    }
}
