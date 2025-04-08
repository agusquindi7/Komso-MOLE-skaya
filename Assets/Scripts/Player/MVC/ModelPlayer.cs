using System;
using UnityEngine;

public class ModelPlayer
{
    Rigidbody _rigidbody;
    Transform _transform;
    Transform _pivotCamera;
    float _speed;
    float _jumpStrenght;
    float _rollStrenght;
    float _maxLife;
    float _life;
    bool _isGrounded, _canRoll;
    public int currentCombo, maxCombo;
    public float comboTimer, comboResetTime;
    float _counterRoll, _rollCD; 
    float _sensitivity, _clampViewY;
    Vector2 _turn;

    public event Action EventJump;
    public event Action<bool> EventLand;
    public event Action EventIdle;
    public event Action<float, float> EventWalk;
    public event Action<float, float> EventRun;
    public event Action<float, float> EventTakeDamage;
    public event Action EventDeath;
    //public event Action<float> EventRoll;
    public ModelPlayer(Rigidbody rigidbody, Transform transform, float speed,
                        float jumpStrenght, float rollStrenght, float maxLife,
                        bool isGrounded, bool canRoll, float counterRoll, float rollCD, 
                        float sensitivity, float clampViewY, Vector2 turn, Transform pivotCamera)
    {
        _rigidbody = rigidbody;
        _transform = transform;
        _speed = speed;
        _jumpStrenght = jumpStrenght;
        _rollStrenght = rollStrenght;
        _maxLife = maxLife;
        _life = maxLife;
        _isGrounded = isGrounded;
        _canRoll = canRoll;
        _counterRoll = counterRoll;
        _rollCD = rollCD;
        _sensitivity = sensitivity;
        _clampViewY = clampViewY;
        _turn = turn;
        _pivotCamera = pivotCamera;
    }

    public void Idle() { if (EventIdle != null) EventIdle(); }

    public void Walk(float horizontal, float vertical)
    {
        // Dirección en base a la orientación del personaje
        Vector3 dir = _transform.forward * vertical + _transform.right * horizontal;

        // Normalizamos para evitar velocidad extra en diagonal
        if (dir.magnitude > 1)
            dir.Normalize();

        // Calculamos la nueva posición
        Vector3 newPosition = _rigidbody.position + dir * _speed * Time.fixedDeltaTime;

        // Aplicamos el movimiento con MovePosition()
        _rigidbody.MovePosition(newPosition);

        // Disparamos el evento si hay suscriptores
        EventWalk?.Invoke(horizontal, vertical);
    }

    public void Run(float horizontal, float vertical)
    {
        Vector3 dir = _transform.forward * vertical + _transform.right * horizontal;

        if (dir.magnitude > 1)
            dir.Normalize();

        Vector3 newPosition = _rigidbody.position + dir * (_speed * 1.5f) * Time.fixedDeltaTime; // Multiplicamos por 1.5 para correr

        _rigidbody.MovePosition(newPosition);

        EventRun?.Invoke(horizontal, vertical);
    }

    public bool LandUpdate() //Para chequear si estoy en el piso
    {
        //LOGICA DEL SALTO
        if (Physics.Raycast(_transform.position, Vector3.down, .25f))
        {
            _isGrounded = true;
            EventLand(_isGrounded);
            return _isGrounded;
        }
        else
        {
            _isGrounded = false;
            EventLand(_isGrounded);
            return _isGrounded;
        }
    }

    public bool RollUpdate()
    {
        //LOGICA DEL ROLL
        _counterRoll += Time.deltaTime;
        _counterRoll = Mathf.Clamp(_counterRoll, 0, _rollCD);
        if (_counterRoll == _rollCD)
        {
            _canRoll = true;
            return _canRoll;
        }
        _canRoll = false;
        return _canRoll;
    }

    //public void LightAttack()
    //{
    //    if(Physics.Raycast(_transform.position, Vector3.down, .25f)) //IF GROUNDED
    //    {
    //        if (EventAttack != null)
    //            EventAttack(currentCombo);
    //    }
    //}

    //public bool CanContinueCombo() => currentCombo < maxCombo;
    //public void ResetCombo() => currentCombo = 0;

    public void Jump()
    {
        if (Physics.Raycast(_transform.position, Vector3.down, .25f)) //IF GROUNDED
        {
            //_isGrounded = true;
            _rigidbody.AddForce(Vector3.up * _jumpStrenght, ForceMode.Impulse);
        }

        if (EventJump != null)
            EventJump();
    }

    public void Roll(Vector3 dir)
    {
        if (Physics.Raycast(_transform.position, Vector3.down, .25f))
        {
            _counterRoll = 0;
            _rigidbody.AddRelativeForce(dir * _rollStrenght, ForceMode.Impulse);
            //PROXIMAMENTE TRAIL DE ALAS Y PUNTOS DE APARICION Y DESAPARICION
            //if(EventRoll != null)
            //    EventRoll(dir.y);
        }
    }

    public void RotateCharacter()
    {
        _turn.x += (Input.GetAxisRaw("Mouse X") * _sensitivity);
        _turn.y += (Input.GetAxisRaw("Mouse Y") * -_sensitivity);
        _turn.y = Mathf.Clamp(_turn.y, -_clampViewY, _clampViewY); //Clamps the rotation vertically

        _pivotCamera.transform.localRotation = Quaternion.Euler(_turn.y , 0, 0);
        _transform.localRotation = Quaternion.Euler(0, _turn.x, 0);
    }

    public void TakeDamage(float dmg)
    {
        _life -= dmg;

        if (_life <= 0)
        {
            if (EventDeath != null)
                EventDeath();
            Debug.Log("GAME OVER");
        }

        if (EventTakeDamage != null)
            EventTakeDamage(_life, _maxLife);
    }
}
