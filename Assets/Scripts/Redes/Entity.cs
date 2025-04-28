using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;


//public class Entity : MonoBehaviour, IDamageable
public class Entity : NetworkBehaviour, IDamageable
{
    [SerializeField] protected NetworkRigidbody3D _rb;
    //[SerializeField] protected Rigidbody _rb;


    [Header("Life")]
    [SerializeField] protected float _maxLife = 100f;
    [SerializeField] protected float _life;



    public float MaxLife
    {
        get { return _maxLife; }
        set { _maxLife = value; }        
    }

    public float Life
    {
        get { return _life; }
        set
        {
            _life = Mathf.Clamp(value, 0, _maxLife); // el valor siempre esta entre 0 y maxLife
        }
    }

    protected virtual void Awake()
    {
        Life = _maxLife;
    }

    protected virtual void Start()
    {
        
    }

    public virtual void TakeDamage(float dmg) // metodo para restar daño a la vida
    {
        Life -= dmg; // se utiliza el setter de la propiedad Life para aplicar la reduccion
        Debug.Log("vida restante: " + Life);

        if (Life <= 0)
        {
            Debug.Log("entity murio");
        }
    }

}
