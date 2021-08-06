using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class LaserGun : NetworkBehaviour
{
    [SerializeField] Transform lasergun;
    [SerializeField] Transform cam;
    [SerializeField] ParticleSystem shootFX;
    [SerializeField] ParticleSystem playerShotFX;
    [SerializeField] ParticleSystem muzzleFlashFX;
    [SerializeField] float shootDelay = 1f;
    [SerializeField] Transform gunPoint;
    [SyncVar] bool canShoot = true;
    [SyncVar] public bool isShooting = false;
    Animator animator;
    [SyncVar] bool isRunning = false;
    Canvas hitmarker;

    private void Awake() {
        hitmarker = GameObject.FindGameObjectWithTag("Hitmarker").GetComponent<Canvas>();
        animator = GetComponent<Animator>();
    }
    private void Start() {
        if (hitmarker) {
            if (isServer) {
                TargetShowHitmarker(false);
            } else {
                CmdShowHitmarker(false);
            }
        }
    }

    [Command]
    private void CmdShowHitmarker(bool _visible) {
        TargetShowHitmarker(_visible);
    }

    void Update()
    {
        if (isLocalPlayer && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.W))) {
            handleRunning(true);
        } else if(isLocalPlayer && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.W))) {
            handleRunning(false);
        }
        if (isLocalPlayer && Input.GetMouseButton(0) && canShoot) {
            canShoot = false;
            CmdShoot();
        } else if (isLocalPlayer && !Input.GetMouseButton(0)) {
            CmdStopShootAnim();
        }
    }

    [Command]
    void handleRunning(bool _isRunning) {
        isRunning = _isRunning;
    }

    [Command]
    public void CmdStopShootAnim() {
        StopShootingAnim();
        isShooting = false;
    }

    [Command]
    public void CmdShoot() {
        canShoot = false;
        isShooting = true;
        ShootAnim();
        //RpcMuzzleFlash();
        if (isRunning) {
            StartShoot();
        } else {
            StartCoroutine(Shoot());
        }
    }

    void StartShoot() {
        Invoke("StartShootInvoke", .5f);
    }

    void StartShootInvoke() {
        StartCoroutine(Shoot());
    }
    [Server]
    public IEnumerator Shoot() {
        if (!canShoot) yield return null;
        RaycastHit ray;
        
        if (Physics.Raycast(cam.position, cam.forward, out ray, 500f)) {
            if (ray.collider.tag == "Player") {
                TargetShowHitmarker(true);
                RPCShootEffectPlayer(ray.point, ray.normal);
                var playerHealth = ray.collider.gameObject.GetComponent<PlayerHealth>();
                playerHealth.Damage(gameObject.GetComponent<PlayerNameTag>().playerName,20);
            } else {
                RPCShootEffectFloor(ray.point, ray.normal);
            }
        }
        yield return new WaitForSeconds(shootDelay);
        TargetShowHitmarker(false);
        canShoot = true;
    }

    [TargetRpc]
    void TargetShowHitmarker(bool _visible) {
        Debug.Log("Client - TargetRpc");
        hitmarker.enabled = _visible;
    }
    
    [ClientRpc]
    void RpcMuzzleFlash() {
        //Instantiate(muzzleFlashFX, gunPoint.position, Quaternion.identity);
        muzzleFlashFX.Play();
    }

    [ClientRpc]
    void RPCShootEffectPlayer(Vector3 point, Vector3 rot) {
        Instantiate(playerShotFX, point, Quaternion.LookRotation(rot));
    }

    [ClientRpc]
    void RPCShootEffectFloor(Vector3 point, Vector3 rot) {
        Instantiate(shootFX, point, Quaternion.LookRotation(rot));
    }

    [ClientRpc]
    void ShootAnim() {
        if (!animator.GetBool("shoot")) {
            GetComponent<Animator>().SetBool("run", false);
            GetComponent<Animator>().SetBool("shoot", true);
        }
        
    }
    [ClientRpc]
    void StopShootingAnim() {
        if (animator.GetBool("shoot")) {
            GetComponent<Animator>().SetBool("shoot", false);
        }
    }
}
