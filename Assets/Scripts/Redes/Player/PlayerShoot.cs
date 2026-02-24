using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class PlayerShoot : NetworkBehaviour
{
    //Variables bala normal
    public Transform spawnerBullet;
    public GameObject bullet;
    public TickTimer cd;
    public KeyCode keyShoot;
    private bool wantsToShoot;

    //Variables bala TP
    public Transform _spawnerTPBullet;
    public GameObject _tpBullet;
    public TickTimer _tpcd;
    public KeyCode _tpKeyShoot;
    private bool _tpWantsToShoot;

    public AudioSource audioSource;
    public AudioClip shootSound;

    public void Update()
    {
        //if (!Object.HasInputAuthority) return;

        //Bala normal
        // Detecta el input local
        if (Input.GetMouseButtonDown(0))
        {
            if (cd.ExpiredOrNotRunning(Runner))
            {
                wantsToShoot = true;
                cd = TickTimer.CreateFromSeconds(Runner, .6f);
            }
        }

        //Bala TP
        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetMouseButtonDown(1))
        {
            if (_tpcd.ExpiredOrNotRunning(Runner))
            {
                _tpWantsToShoot = true;
                _tpcd = TickTimer.CreateFromSeconds(Runner, 2f);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority) return;

        //Bala normal
        if (wantsToShoot)
        {
            RPC_RequestBullet(spawnerBullet.position, spawnerBullet.rotation, Object.InputAuthority);
            wantsToShoot = false;
        }

        //Bala TP
        if (_tpWantsToShoot)
        {
            RPC_RequestTPBullet(_spawnerTPBullet.position, _spawnerTPBullet.rotation, Object.InputAuthority);
            _tpWantsToShoot = false;
        }
    }

    //Bala normal
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //PlayerRef owner: Le paso un OWNER a la bala, InputAuthority = el jugador que la disparo
    private void RPC_RequestBullet(Vector3 position, Quaternion rotation, PlayerRef owner)
    {
        Runner.Spawn(bullet, position, rotation, owner);
        RPCAudioShootSound();
    }

    //Bala TP
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestTPBullet(Vector3 position, Quaternion rotation, PlayerRef owner)
    {
        Runner.Spawn(_tpBullet, position, rotation, owner);
        RPCAudioShootSound();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPCAudioShootSound()
    {
        audioSource.PlayOneShot(shootSound, 1f);
    }

    //SPAWNEAR BALA TP
    //NetworkCharacterController TIENE DE POR SI UN TELEPORT, PODRIA OVERRIDEARLO PARA DEJARLO MEJOR
    //public void Teleport(Vector3? position = null, Quaternion? rotation = null)
    //{
    //    _controller.enabled = false;
    //    NetworkTRSP.Teleport(this, transform, position, rotation);
    //    _controller.enabled = true;
    //}


}
