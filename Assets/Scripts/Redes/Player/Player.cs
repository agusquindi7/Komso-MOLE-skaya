using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Fusion;
using Fusion.Addons.Physics;

public class Player : Entity
{    
    [SerializeField] protected Slider mySlider;
    //[SerializeField] protected Gradient myGradient;

    /*
    [Header("Componentes")]
    //[SerializeField] protected Rigidbody _rb;
    [SerializeField] protected TrailRenderer _trail;

    [Header("Movement")]
    [SerializeField] protected float _currentSpeed;
    [SerializeField] protected float _baseSpeed = 5f;
    [SerializeField] protected float _maxSpeed = 30f;
    [SerializeField] protected float _rayLenght = 1.1f;
    [SerializeField] protected float _jumpForce = 5f;
    [SerializeField] protected float _gravityMultiplier = 2f;
    [SerializeField] protected RaycastHit _hit;
    [SerializeField] protected bool _isGrounded;
    [SerializeField] protected bool _cantMove;
    */

    //DEL NETWORK
    //private bool _isJumpingPressed;
    //private bool _isShootingPressed;
    //private float _horizontalInput;

    //public Action OnShoot;
    //public Action<float> OnMove;

    /*
    [Header("Teclas")]
    public KeyCode Ability;
    public KeyCode codeJump;
    public KeyCode codeAttack;
    */

    public override void Spawned()
    {
        //_rb = GetComponent<NetworkRigidbody3D>();
        _life = _maxLife;                
    }

    /*
    protected override void Awake()
    {
        base.Awake();
        //rb.useGravity = false;
    }

    protected override void Start()
    {
        _currentSpeed = _baseSpeed;
        UpdateUI();
    }

    void Update()
    {
        if (!HasStateAuthority) return;

        _horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W)) _isJumpingPressed = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;

        Movement(_horizontalInput);
        if (_isJumpingPressed)
        {
            Jump();
            _isJumpingPressed = false;
        }
    }
    */

    #region LifeMethods
    public override void TakeDamage(float dmg)
    {
        Life -= dmg; // se utiliza el setter de la propiedad Life para aplicar la reduccion
        Debug.Log($"vida restante {gameObject} ({HasStateAuthority}): " + Life);

        if (Life <= 0)
        {
            Debug.Log("Player murio");
            //AGUS ADD-ON
            Death();
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (mySlider != null)
        {
            mySlider.maxValue = MaxLife;
            mySlider.value = Life;
        }
    }
    #endregion

    /*
    void Movement(float xAxis)
    {
        // Código de movimiento comentado
    }

    public void MovePlayer(float xAxis)
    {
        // Código de movimiento comentado
    }

    public void Jump()
    {
        //_rb.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }
    */

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(int dmg)
    {
        Local_TakeDamage(dmg);
    }

    void Local_TakeDamage(int dmg)
    {
        _life -= dmg;
        if (_life <= 0)
            Death();
    }

    //OLD DEATH
    private void Death()
    {
        Debug.Log($"Mori :(");

        //GameManager.Instance.RPC_Defeat(Runner.LocalPlayer);

        Runner.Despawn(Object);
    }

    /*
    public void Grounded()
    {
        // Código comentado
    }

    public void NewGravity()
    {
        // Código comentado
    }

    private void OnDrawGizmos()
    {
        // Código comentado
    }
    */
}
