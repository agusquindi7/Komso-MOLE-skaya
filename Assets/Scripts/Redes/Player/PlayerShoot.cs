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

        RotateSpawner();
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
        //Runner.Spawn(bullet, spawnerBullet.position, spawnerBullet.rotation, Object.InputAuthority);
        //Agus ADDON
        if (Object.HasStateAuthority)
        {
            // Si soy el host, spawneo directamente
            Runner.Spawn(bullet, spawnerBullet.position, spawnerBullet.rotation, Object.InputAuthority);
        }
        else
        {
            // Si soy un cliente, pido al host que spawnee por mí
            RPC_RequestBullet(spawnerBullet.position, spawnerBullet.rotation, Object.InputAuthority);
        }
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

    public void RotateSpawner()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector2 dir = new Vector2(mousePos.x, mousePos.y);

        //transform.rotation = Quaternion.Euler(0f, 0f, angle);

        //Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, spawnerEmpty.position.z));

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (plane.Raycast(ray, out float enter))
        //{
        //    Vector3 worldPos = ray.GetPoint(enter);

        //    Vector3 dir = worldPos - spawnerEmpty.position;
        //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //    spawnerEmpty.rotation = Quaternion.Euler(0f, 0f, angle);
        //}

        //spawnerEmpty.rotation = spawnerEmpty.LookAt(mousePos);



        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -Camera.main.transform.position.z; //anulo el eje z
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);//transformo a sistema de coordenada

        Vector2 dir = (mouseWorld - spawnerEmpty.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        spawnerEmpty.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
