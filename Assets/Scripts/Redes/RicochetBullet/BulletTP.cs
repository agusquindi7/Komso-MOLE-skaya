using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Fusion;
using Fusion.Addons.Physics;

[RequireComponent(typeof(Rigidbody))]
public class BulletTP : NetworkBehaviour
{
    public float speed = 32f;

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            transform.position += transform.forward * speed * Runner.DeltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return; //Solo la autoridad del server decide el teleporte
        Debug.Log("BalaTP choco con: " + other.gameObject.name);

        //Obtengo el NetworkObject del objeto que entro al trigger
        var otherNetObj = other.GetComponentInParent<NetworkObject>();
        Vector3 hitPoint = transform.position;

        //    //Podemos decidir teletransportar incluso si es pared u otro objeto
        //    RPC_TeleportOwner(hitPoint, Object.InputAuthority);         
        //    Runner.Despawn(Object); //Desaparece la bala de red, un destroy en redes
        //}

        if (other.gameObject.name == "BackGlass" || other.gameObject.name == "BackGlass (1)")
        {
            Debug.Log("Ignorando trigger con: " + other.gameObject.name);
            return;
        }

        //Si se encontro un NetworkObject y es el dueño de la bala, lo ignora
        if (otherNetObj != null && otherNetObj.InputAuthority == Object.InputAuthority)
        {
            Debug.Log($"Ignorando teleporte: es el dueño ({otherNetObj.name})");
            return;
        }

        //Si es un NetworkObject distinto, entonces se teletransporta el owner
        if (otherNetObj != null && otherNetObj.InputAuthority != Object.InputAuthority)
        {
            RPC_TeleportOwner(hitPoint, Object.InputAuthority);
            Runner.Despawn(Object);
            return;
        }

        //Si no es NetworkObject (como una pared), tambien se teletransporta, asi podria ahcer que choque con las balas
        RPC_TeleportOwner(hitPoint, Object.InputAuthority);
        Runner.Despawn(Object);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_TeleportOwner(Vector3 position, PlayerRef owner)
    {
        //Encuntra el objeto jugador/owner que tiro la bala
        var go = Runner.GetPlayerObject(owner);
        if (go != null)
        {
            //SE TEPEA FUERA DEL MAPA SI LO DEJO ASI
            //go.transform.position = position;

            //LO CLAMPEO COMO BOUNDS DEL SPAWN (-10, 10) EN X, (-6, 12) EN Y, 0 EN Z);
            Vector3 clampedPos = new Vector3(Mathf.Clamp(position.x, -10f, 10f), Mathf.Clamp(position.y, -6, 12), Mathf.Clamp(position.z, 0f, 0f));
            go.transform.position = clampedPos;

            var rb = go.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }


}
