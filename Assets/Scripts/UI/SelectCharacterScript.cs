using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent (typeof(ToggleGroup))]
public class SelectCharacterScript : MonoBehaviour
{
    private ToggleGroup toggleGroup;
    private NetworkManager networkManager;
    private Toggle PreviousSelectedToggle;
    // Start is called before the first frame update
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        networkManager = NetworkManager.singleton;
        PreviousSelectedToggle = toggleGroup.ActiveToggles().ElementAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        Toggle CurrentSelectedToggle = toggleGroup.ActiveToggles().ElementAt(0);
        if (PreviousSelectedToggle != CurrentSelectedToggle)
        {
            OnToggleChange();
            PreviousSelectedToggle = CurrentSelectedToggle;
        }
    }

    private void OnToggleChange()
    {
        Debug.Log(toggleGroup.ActiveToggles().ElementAt(0).name);
        networkManager.playerPrefab = toggleGroup.ActiveToggles().ElementAt(0).GetComponent<DataToggle>().playerPrefab;
    }
}
