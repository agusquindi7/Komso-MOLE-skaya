using Fusion;
using System.Collections;
using UnityEngine;

public class PlayerNetworkShoot : NetworkBehaviour
{
    public Transform spawnerEmpty;
    public Transform spawnerBullet;
    public GameObject bullet;
    public float cdMax = 2f;
    private float cd;

    public KeyCode keyShoot = KeyCode.Mouse0;

    public override void FixedUpdateNetwork()
    {
        if (!HasInputAuthority) return; // Solo el dueño controla disparo y rotación

        RotateSpawner();

        if (Input.GetKeyDown(keyShoot))
        {
            if (cd >= cdMax)
            {
                // El spawn de la bala debe hacerlo el Runner
                RPC_SpawnBullet(spawnerBullet.position, spawnerBullet.rotation);
                cd = 0;
            }
        }

        cd += Mathf.Clamp(Time.deltaTime, 0, 3);
    }

    public void RotateSpawner()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -Camera.main.transform.position.z;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Vector2 dir = (mouseWorld - spawnerEmpty.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        spawnerEmpty.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SpawnBullet(Vector3 pos, Quaternion rot)
    {
        Runner.Spawn(bullet, pos, rot);
    }
}