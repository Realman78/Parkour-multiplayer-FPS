using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] public float health = 100f;
    Canvas deathCanvas;

    private void Start() {
        deathCanvas = GameObject.FindGameObjectWithTag("DeathCanvasTag").GetComponent<Canvas>();
        deathCanvas.enabled = false;
    }

    public void Damage(string shooterName, float damage) {
        health -= damage;
        if (health <= 0) {
            health = 100;

            RpcAddLog(shooterName);
            ShowDeathScreen(shooterName);
            Respawn(gameObject, GetComponent<NetworkIdentity>().connectionToClient);
            
            }
    }

    [ClientRpc]
    void RpcAddLog(string shooter_name) {
        LogScript.AddToLog(shooter_name + " Killed " + GetComponent<PlayerNameTag>().playerName + "\n");
    }

    [TargetRpc]
    public void ShowDeathScreen(string sn) {
        deathCanvas.enabled = true;
        deathCanvas.GetComponentInChildren<Text>().text = sn + " Killed You";
    }

    [TargetRpc]
    public void RemoveDeathScreen() {
        deathCanvas.enabled = false;
    }

    [Server]
    void Respawn(GameObject go, NetworkConnection conn) {
        StartCoroutine(_Respawn(go, conn));
    }

    IEnumerator _Respawn(GameObject go, NetworkConnection conn) {
        NetworkServer.RemovePlayerForConnection(conn, false);
        Transform newPosition = NetworkManager.singleton.GetStartPosition();
        go.transform.position = newPosition.position;
        go.transform.rotation = newPosition.rotation;
        yield return new WaitForSeconds(1.5f);
        NetworkServer.AddPlayerForConnection(conn, go);
    }
}
