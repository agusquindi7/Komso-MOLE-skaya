using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Fusion;
using Fusion.Addons.Physics;

[RequireComponent(typeof(Rigidbody))]
public class BulletRicochet : NetworkBehaviour
{
    [SerializeField] private float _speed = 20f;
    //[SerializeField] NetworkRigidbody3D _rb;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private string _newTag;
    [SerializeField] private int _maxWallCount = 3;
    [SerializeField] private int _wallCount;

    [SerializeField] NetworkObject networkBullet;
    public VisualEffect vfx;

    //[SerializeField] private Collision collision;
    //[SerializeField] bool hasToBounce;
    //public VisualEffect sparksPrefab;

    public override void Spawned()
    {
        //GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(transform.forward * _speed, ForceMode.VelocityChange);
        //rb.velocity = transform.forward * speed;
        _dir = transform.forward;

        if (Runner.IsClient && !HasStateAuthority)
        {
            Runner.SetIsSimulated(GetComponent<NetworkObject>(), false);
        }

        //if (Runner.IsClient && !HasStateAuthority)
        //    Runner.SetIsSimulated(Object, true);
    }

    //public override void FixedUpdateNetwork()
    //{
    //    if (hasToBounce)
    //        Ricochet();
    //}

    //void Ricochet()
    //{
    //    _dir = Vector3.Reflect(_dir, collision.GetContact(0).normal);
    //    hasToBounce = false;
    //}

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            transform.position += _dir * Runner.DeltaTime * _speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Object == null || !Object.HasStateAuthority) return;
        

        var hitObj = collision.gameObject;
        var otherNetObj = hitObj.GetComponentInParent<NetworkObject>();

        if (hitObj.CompareTag("Wall"))
        {
            Debug.LogWarning("CHOCO CON UNA PARED");


            //LOGICA VFX            
            RPC_PlayBounceVFX(hitObj.transform.position);





            MaxCount();

            Vector3 normal = collision.GetContact(0).normal;
            _dir = Vector3.Reflect(_dir, normal);
            return;
        }

        if (otherNetObj != null && otherNetObj != Object && otherNetObj != null)
        {
            //Al mandar a que spawnee la bala en el script PlayerShoot, le pase un owner. InputAuthority = el jugador que la disparo, por eso daña a otro jugador si otherNetObj es diferente a la autoridad de quien disparo
            if (otherNetObj.InputAuthority != Object.InputAuthority)
            {
                var lifeHandler = hitObj.GetComponentInParent<LifeHandler>();
                if (lifeHandler != null)
                {
                    lifeHandler.TakeDamage(10);
                    Runner.Despawn(Object);
                    return;
                }
            }
        }
    }

    //public override void FixedUpdateNetwork()
    //{
    //    _rb.velocity = _dir * _speed;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!HasStateAuthority)
    //        return;

    //    IDamageable damageable = other.GetComponent<IDamageable>(); // Get the IDamageable component
    //    if (damageable != null)
    //    {
    //        Debug.Log("bala daño a " + other);
    //        damageable.TakeDamage(10);
    //        Destroy(gameObject);
    //    }

    //    //ACA PUEDO HACER QUE SALGAN CHISPITAS O TOMAR A ENEMIGOS PARA BAJARLES LA VIDA, INCLUSO UN CONTADOR DE REBOTES, ASI AL 3ER REBOTE SE APAQUE LA BALA. CUANDO EL COLLIDER.COLLISION LLEGUE
    //    //PARA GRANADA CON GRAVITY TAMBIEN PODRIA SERVIR
    //    //O UNA MECANICA QUE SI LLEGA A UN ENEMIGO, LO INSTAKILLEE Y LE PASE LA DIRECCION DEL PROXIMO ENEMIGO Y QUE LO MANDE HACIA ESA DIRECCION. SE VIENE BALA RAYO

    //    if (other.gameObject.CompareTag("Wall"))
    //    {
    //        Debug.LogWarning("TRIGEREO CON UNA PARED");
    //        //sparksPrefab.Play();

    //        _wallCount++;
    //        MaxCount();
    //    }

    //    ////podria incluso hacer destruibles, o hacerlo por interface mejor
    //    //if (other.gameObject.CompareTag(_newTag))
    //    ////if (other.GetComponent<IDamageable>())
    //    //{
    //    //    Debug.LogWarning("TRIGEREO CON UN " + _newTag);        

    //    //}
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (Object == null || Runner == null)
    //        return;

    //    if (!Object.HasStateAuthority)
    //        return;

    //    if (other == null)
    //        return;

    //    var otherNetObj = other.GetComponentInParent<NetworkObject>();
    //    if (otherNetObj == null)
    //        return;

    //    var lifeHandler = other.GetComponentInParent<LifeHandler>();
    //    if (lifeHandler == null)
    //        return;

    //    if (otherNetObj.InputAuthority == Object.InputAuthority)
    //        return;

    //    Debug.Log($"Host hace daño de {Object.InputAuthority} a {otherNetObj.InputAuthority}");

    //    lifeHandler.TakeDamage(10);
    //    Runner.Despawn(Object);
    //}

    public void MaxCount()
    {
        _wallCount++;

        if (_wallCount >= _maxWallCount && gameObject != null)
        {
            if (Object != null && Object.IsValid && Runner != null)
            {
                Runner.Despawn(Object);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    //LOGICA VFX
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_PlayBounceVFX(Vector3 pos)
    {
        vfx.SendEvent("Bounce");
    }
}
