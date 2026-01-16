using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class PlayerShoot : NetworkBehaviour
{
    public Transform spawnerBullet;
    public GameObject bullet;
    public TickTimer cd;
    public KeyCode keyShoot;

    private bool wantsToShoot;

    public void Update()
    {
        //if (!Object.HasInputAuthority) return;

        // Detecta el input local
        if (Input.GetMouseButtonDown(0))
        {
            if (cd.ExpiredOrNotRunning(Runner))
            {
                wantsToShoot = true;
                cd = TickTimer.CreateFromSeconds(Runner, .6f);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority) return;

        if (wantsToShoot)
        {
            RPC_RequestBullet(spawnerBullet.position, spawnerBullet.rotation, Object.InputAuthority);
            wantsToShoot = false;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestBullet(Vector3 position, Quaternion rotation, PlayerRef owner)
    {
        Runner.Spawn(bullet, position, rotation, owner);
    }
}
