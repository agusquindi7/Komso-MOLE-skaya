using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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





    //[Header("Bullet")]
    //public GameObject bulletPrefab;
    //public float dmg;

    [Header("Teclas")]
    public KeyCode Ability;
    public KeyCode codeJump;
    public KeyCode codeAttack;

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
       Grounded();
       if (Input.GetKeyDown(codeJump) && _isGrounded == true) Jump();        
    }

    private void FixedUpdate()
    {
        NewGravity();
        MovePlayer();   
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

    public void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Vector3 inputLocal = new Vector3(horizontal, 0, vertical);
        Vector3 inputLocal = new Vector3(horizontal, 0, 0);

        Vector3 movement = transform.TransformDirection(inputLocal);

        if (_cantMove == false) _rb.velocity = movement * _currentSpeed + new Vector3(0, _rb.velocity.y, 0);

        //Vector3 direccionMovimiento = new Vector3(horizontal, 0f, vertical).normalized;
        //rb.AddForce(direccionMovimiento * currentSpeed, ForceMode.Force);

        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
        }
    }

    public void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
        _rb.AddForce(Physics.gravity * _gravityMultiplier, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = -transform.up * _rayLenght;
        Gizmos.DrawRay(transform.position, dir);
    }
}
