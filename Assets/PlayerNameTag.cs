using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;
public class PlayerNameTag : NetworkBehaviour
{
    [SerializeField] private TextMeshPro nameTag;
    [SyncVar(hook = "SetName")] public string playerName;
    public Transform player;

    private void Start() {
/*        if (isServer) {
            RpcAddToLog(EnterName.nickname);
        } else {
            CmdAddToLog(EnterName.nickname);
        }*/
    }

    private void Update() {
        try {
            nameTag.transform.LookAt(2 * transform.position - Camera.main.transform.position);
        }catch(Exception) {}
        
        if (!isLocalPlayer) return;
    }

    public override void OnStartLocalPlayer() {
        CmdSetPlayerName(EnterName.nickname);
        
    }
    [Command]
    private void CmdAddToLog(string _shooterName) {
        RpcAddToLog(_shooterName);
    }

    [ClientRpc]
    void RpcAddToLog(string _name) {
        LogScript.AddToLog(_name + " has joined the game" + "\n");
    }

    [Command]
    void CmdSetPlayerName(string newName) {
        playerName = newName;
    }

    void SetName(String oldValue, String newValue) {
        nameTag.text = newValue;
    }
}
