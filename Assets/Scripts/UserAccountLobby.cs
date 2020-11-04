using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserAccountLobby : MonoBehaviour
{
    public Text UserNameText;

    private void Start()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            UserNameText.text = UserAccountManager.LoggedIn_Username;
        }
        else
        {
            UserNameText.text = "Foobar";
        }
    }

    public void LogOut()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.LogOut();
        }
    }
}
