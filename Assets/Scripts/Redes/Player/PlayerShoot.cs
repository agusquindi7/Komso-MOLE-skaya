using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class PlayerShoot : NetworkBehaviour
{
    public Transform spawnerEmpty;
    public Transform spawnerBullet;
    public GameObject bullet;
    public float cdMax;
    public float cd;
    public KeyCode keyShoot;

    private bool isShooting;

    public void Update()
    {
        //if (!HasStateAuthority) return;
        //Agus ADDON
        if (!Object.HasInputAuthority) return;

        //RotateSpawner(); MUEVO EL CAÑON EN NETWORK PARA QUE EL ENEMIGO VEA A DONDE VOY A DISPARAR
        if (Input.GetMouseButtonDown(0))
        {
            if (cd >= cdMax)
            {
                isShooting = true;
                Debug.Log("Aprete Click");
                cd = 0;
            }
        }

        cd += Mathf.Clamp(Time.deltaTime, 0, cdMax);

    }

    public override void FixedUpdateNetwork()
    {
        if (isShooting)
        {
            Shoot();
            isShooting = false;
        }
    }

    private void Shoot()
    {
        RPC_RequestBullet(spawnerBullet.position, spawnerBullet.rotation, Object.InputAuthority);
    }

    //Agus ADDON
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestBullet(Vector3 position, Quaternion rotation, PlayerRef owner)
    {
        Runner.Spawn(bullet, position, rotation, owner); // Spawn la bala con autoridad del jugador que disparó
    }

    //[Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    //private void RPC_SpawnBullet(Vector3 position, Quaternion rotation)
    //{
    //    Runner.Spawn(bullet, position, rotation);
    //}
}
