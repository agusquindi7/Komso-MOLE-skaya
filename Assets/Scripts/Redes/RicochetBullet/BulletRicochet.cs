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

    //[SerializeField] private Collision collision;
    //[SerializeField] bool hasToBounce;
    //public VisualEffect sparksPrefab;

    public override void Spawned()
    {
        //GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(transform.forward * _speed, ForceMode.VelocityChange);
        //rb.velocity = transform.forward * speed;
        _dir = transform.forward;
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
        //if (!Object.HasStateAuthority)
        //    return;

        if (collision.contactCount > 0 && collision.gameObject.CompareTag("Wall"))
        {
            Debug.LogWarning("CHOCO CON UNA PARED");

            Vector3 normal = collision.GetContact(0).normal;
            _dir = Vector3.Reflect(_dir, normal);
        }

        // solo rebota contra objetos etiquetados como "Wall", puedo hacerlo con mas pero no creo. a lo sumo sumo podria hacer con una habilidad en cadena para que vaya al siguiente enemigo y eso seria con el trigger
        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //hasToBounce = true;
        //Debug.LogWarning("CHOCO CON UNA PARED");
        //this.collision = collision;

        //// Tomamos el primer punto de contacto
        ////ContactPoint contact = collision.contacts[0];

        //// Obtener directamente el primer ContactPoint sin asignar arreglo
        //ContactPoint contact = collision.GetContact(0);

        //Vector3 incomingDir = rb.velocity.normalized;
        //Vector3 reflectDir = Vector3.Reflect(incomingDir, contact.normal);

        //rb.velocity = reflectDir * speed;

        //// opcional o por si acaso: alinear la rotación de la bala con su nueva dirección
        ////transform.forward = reflectDir;

        //---------------------------------------------------------------

        //ANTES PEGABA CON ALGUNAS SUPERFICIES Y SE IBA A OTRA LADO O COPIABA SU NORMAL EN VEZ DE REBOTAR. YA NO

        //Vector3 normal = collision.GetContact(0).normal;
        //_dir = Vector3.Reflect(_dir, normal);

        //Agus ADDON
        //Vector3 normal = collision.GetContact(0).normal;
        //_dir = Vector3.Reflect(_dir, normal);


        //_dir = Vector3.Reflect(_dir, collision.GetContact(0).normal);

        //sparksPrefab.SendEvent("Burst");
    
    }

    //public override void FixedUpdateNetwork()
    //{
    //    _rb.velocity = _dir * _speed;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority)
            return;

        IDamageable damageable = other.GetComponent<IDamageable>(); // Get the IDamageable component
        if (damageable != null)
        {
            Debug.Log("bala daño a " + other);
            damageable.TakeDamage(10);
            Destroy(gameObject);
        }

        //ACA PUEDO HACER QUE SALGAN CHISPITAS O TOMAR A ENEMIGOS PARA BAJARLES LA VIDA, INCLUSO UN CONTADOR DE REBOTES, ASI AL 3ER REBOTE SE APAQUE LA BALA. CUANDO EL COLLIDER.COLLISION LLEGUE
        //PARA GRANADA CON GRAVITY TAMBIEN PODRIA SERVIR
        //O UNA MECANICA QUE SI LLEGA A UN ENEMIGO, LO INSTAKILLEE Y LE PASE LA DIRECCION DEL PROXIMO ENEMIGO Y QUE LO MANDE HACIA ESA DIRECCION. SE VIENE BALA RAYO

        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.LogWarning("TRIGEREO CON UNA PARED");
            //sparksPrefab.Play();

            _wallCount++;
            MaxCount();
        }

        ////podria incluso hacer destruibles, o hacerlo por interface mejor
        //if (other.gameObject.CompareTag(_newTag))
        ////if (other.GetComponent<IDamageable>())
        //{
        //    Debug.LogWarning("TRIGEREO CON UN " + _newTag);        
            
        //}
    }

    public void MaxCount()
    {
        if (_wallCount >= _maxWallCount)
        {
            //gameObject.SetActive(false);
            //Agus ADDON
            Runner.Despawn(Object);
        }
    }
}
