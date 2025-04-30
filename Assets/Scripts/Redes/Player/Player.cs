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


    //DEL NERTWORK
    private bool _isJumpingPressed;
    private bool _isShootingPressed;
    private float _horizontalInput;

    public Action OnShoot;
    public Action<float> OnMove;



    //[Header("Bullet")]
    //public GameObject bulletPrefab;
    //public float dmg;

    [Header("Teclas")]
    public KeyCode Ability;
    public KeyCode codeJump;
    public KeyCode codeAttack;


    public override void Spawned()
    {
        _rb = GetComponent<NetworkRigidbody3D>();
        _life = _maxLife;                
        GameManager.Instance.AddToList(this);
    }

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

        //if (Input.GetKeyDown(KeyCode.Mouse0)) _isShootingPressed = true;


        //CODIGO QUE ANDABA BIEN SIN NETWORK
        // Grounded();
        //if (Input.GetKeyDown(codeJump) && _isGrounded == true) Jump();        
    }

    //protected void FixedUpdate()
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;

        //MOVE QUE ANDABA BIEN SIN NEWTWORK
        //NewGravity();
        //MovePlayer();

        //var _horizontalInput = Input.GetAxisRaw("Horizontal");
        //var _verticalInput = Input.GetAxisRaw("Vertical");
        //MovePlayer(_horizontalInput);
        
        Movement(_horizontalInput);
        if (_isJumpingPressed)
        {
            Jump();
            _isJumpingPressed = false;
        }

    }

    #region LifeMethods
    public override void TakeDamage(float dmg)
    {
        Life -= dmg; // se utiliza el setter de la propiedad Life para aplicar la reduccion
        Debug.Log("vida restante Player: " + Life);

        if (Life <= 0)
        {
            Debug.Log("Player murio");
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (mySlider != null)
        {
            mySlider.maxValue = MaxLife;
            mySlider.value = Life;

            //if (fillImage != null && lifeGradient != null)
            //{
            //    float t = Life / MaxLife;
            //    fillImage.color = lifeGradient.Evaluate(t);
            //}
        }
    }
    #endregion

    void Movement(float xAxis)
    {

        if (xAxis != 0)
        {
            transform.forward = Vector3.right * Mathf.Sign(xAxis);

            _rb.Rigidbody.velocity += Vector3.right * (xAxis * _currentSpeed * Runner.DeltaTime);

            if (Mathf.Abs(_rb.Rigidbody.velocity.z) > _currentSpeed)
            {
                var velocity = Vector3.ClampMagnitude(_rb.Rigidbody.velocity, _currentSpeed);

                velocity.y = _rb.Rigidbody.velocity.y;
                _rb.Rigidbody.velocity = velocity;
            }

            OnMove?.Invoke(xAxis);
        }
        else
        {
            var velocity = _rb.Rigidbody.velocity;
            velocity.z = 0;

            _rb.Rigidbody.velocity = velocity;

            OnMove?.Invoke(0);
        }
    }

    //ESTE ES EL MOVEMENTE SIN NETWORK QUE NADA BIEN
    //public void MovePlayer()
    public void MovePlayer(float xAxis)
    {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        /*
        //ESTO NO FUNCIONA ASI
        float horizontal = INetworkInput.GetAxisRaw("Horizontal");
        float vertical = INetworkInput.GetAxisRaw("Vertical");
        
        */

        //TENGO QUE ALTERAR TODO EL TEMA DEL MOVIMIENTO PORQUE NO FUNCA
        //Vector3 inputLocal = new Vector3(horizontal, 0, vertical);
        Vector3 inputLocal = new Vector3(xAxis, 0, 0);

        Vector3 movement = transform.TransformDirection(inputLocal);

        //if (_cantMove == false) _rb.velocity = movement * _currentSpeed + new Vector3(0, _rb.velocity.y, 0);
        if (_cantMove == false) _rb.Rigidbody.velocity = movement * _currentSpeed + new Vector3(0, _rb.Rigidbody.velocity.y, 0);

        
        //Vector3 direccionMovimiento = new Vector3(horizontal, 0f, vertical).normalized;
        //rb.AddForce(direccionMovimiento * currentSpeed, ForceMode.Force);

        if (_rb.Rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rb.Rigidbody.velocity = _rb.Rigidbody.velocity.normalized * _maxSpeed;
        }        
    }

    public void Jump()
    {
        //_rb.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _rb.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }

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

    private void Death()
    {
        Debug.Log($"Mori :(");

        GameManager.Instance.RPC_Defeat(Runner.LocalPlayer);

        Runner.Despawn(Object);
    }

    public void Grounded()
    {

        //if (isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayLenght))
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out _hit, _rayLenght);

        //{
        // Usando el collider
        //if (hit.collider.CompareTag("Wall"))
        if (_isGrounded && _hit.collider.CompareTag("Wall"))
        {
            Debug.Log("RAYO PEGA PARED");
        }

        // O usando el transform (equivalente)
        //if (hit.transform.CompareTag("Wall"))
        if (_isGrounded && _hit.transform.CompareTag("Wall"))
        {
            Debug.Log("RAYO TRIGEREA PARED");
        }
        //cantMove = true;
        if (_isGrounded == false) _cantMove = true;
        else if (_isGrounded == true) _cantMove = false;
        //}

        //return isGrounded == false ? cantMove = true : cantMove = false;
    }

    public void NewGravity()
    {
        _rb.Rigidbody.AddForce(Physics.gravity * _gravityMultiplier, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = -transform.up * _rayLenght;
        Gizmos.DrawRay(transform.position, dir);
    }
}
