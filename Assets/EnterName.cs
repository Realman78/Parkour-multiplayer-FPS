using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class EnterName : NetworkBehaviour
{
    public NetworkManagerHUD hud;
    public static string nickname = "";
    public InputField inputField;

    public void Register() {
        nickname = inputField.text.Equals("") ? "Guest69" : inputField.text;

        hud.showGUI = true;

        transform.parent.gameObject.SetActive(false);
    }
}
