using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 _turn;
    public bool isGrounded;
    Rigidbody playerRB;
    [Header("Public Values")]
    //[SerializeField] float _sensitivity;
    [SerializeField] float _speed;
    float _maxSpeed = 5;
    [SerializeField] float _jumpStrenght;
    [SerializeField] float _runningSpeed;
    [SerializeField] KeyCode jumpKey, runningKey;

    private void Start()
    {
        PauseManager.instance.Subscribe(ArtificialUpdate);

        if (playerRB == null) playerRB = GetComponent<Rigidbody>();

        _speed = _maxSpeed;
    }

    private void ArtificialUpdate()
    {
        if(Input.GetKey(runningKey))
        {
            _speed = _runningSpeed;
        }
        else
        {
            _speed = _maxSpeed;
        }

        Vector3 mov = new (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        mov = transform.TransformDirection(mov) * (_speed * Time.deltaTime);

        //transform.rotation = Quaternion.Euler(0, _turn.x ,0);
        transform.position += new Vector3 (mov.x , 0 , mov.z);

        if(Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Debug.Log("GROUNDED");
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Debug.Log("NOT GROUNDED");
            isGrounded = false;
        }
    }

    public void Jump()
    {
        playerRB.AddForce(new Vector3 (0 , _jumpStrenght , 0) ,ForceMode.Impulse); 
    }

    private void OnDisable()
    {
        PauseManager.instance.Unsubscribe(ArtificialUpdate);
    }
}
