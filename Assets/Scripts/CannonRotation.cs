using System;
using UnityEngine;
using Fusion;

public class CannonRotation : NetworkBehaviour
{
    [SerializeField] private Transform spawnerEmpty;

    public override void Spawned()
    {
        if (!HasInputAuthority) {
            // ¡Esto es clave! El cliente ve los cambios aunque no tenga autoridad
            Runner.SetIsSimulated(Object, true);
        }
    }

    void Update()
    {
        if (!HasInputAuthority) return;

        var mouseScreen = Input.mousePosition;
        if (mouseScreen.magnitude != 0)
        {
            mouseScreen.z = -Camera.main.transform.position.z;

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            Vector2 dir = (mouseWorld - spawnerEmpty.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            RPC_RotateCannon(angle);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_RotateCannon(float angle)
    {
        spawnerEmpty.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}