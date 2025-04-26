using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody rb;
    public TrailRenderer trail;

    [Header("Movimiento")]
    public float currentSpeed;
    public float baseSpeed = 5f;
    public float maxSpeed = 30f;
    public float rayLenght = 1.1f;
    public float jumpForce = 5f;
    public float gravityMultiplier = 2f;
    private RaycastHit hit;
    public bool isGrounded;
    public bool cantMove;


    [Header("Teclas")]
    public KeyCode Ability;
    public KeyCode codeJump;
    public KeyCode codeAttack;

    private void Awake()
    {
        //rb.useGravity = false;
    }

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
       Grounded();
       if (Input.GetKeyDown(codeJump) && isGrounded == true) Jump();        
    }

    private void FixedUpdate()
    {
        NewGravity();
        MovePlayer();   
    }

    public void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Vector3 inputLocal = new Vector3(horizontal, 0, vertical);
        Vector3 inputLocal = new Vector3(horizontal, 0, 0);

        Vector3 movement = transform.TransformDirection(inputLocal);

        if (cantMove == false) rb.velocity = movement * currentSpeed + new Vector3(0, rb.velocity.y, 0);

        //Vector3 direccionMovimiento = new Vector3(horizontal, 0f, vertical).normalized;
        //rb.AddForce(direccionMovimiento * currentSpeed, ForceMode.Force);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void Grounded()
    {

        //if (isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayLenght))
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayLenght);

        //{
        // Usando el collider
        //if (hit.collider.CompareTag("Wall"))
        if (isGrounded && hit.collider.CompareTag("Wall"))
        {
            Debug.Log("RAYO PEGO PARED");
        }

        // O usando el transform (equivalente)
        //if (hit.transform.CompareTag("Wall"))
        if (isGrounded && hit.transform.CompareTag("Wall"))
        {
            Debug.Log("RAYO TRIGEREO PARED");
        }
        //cantMove = true;
        if (isGrounded == false) cantMove = true;
        else if (isGrounded == true) cantMove = false;
        //}

        //return isGrounded == false ? cantMove = true : cantMove = false;
    }

    public void NewGravity()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = -transform.up * rayLenght;
        Gizmos.DrawRay(transform.position, dir);
    }
}
