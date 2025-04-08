using System;
using UnityEngine;

public class ViewPlayer
{
    ModelPlayer _model;
    Animator _animator;
    Transform _transform;
    public ViewPlayer(ModelPlayer model, Animator animator, Transform transform)
    {
        _model = model;
        _animator = animator;
        _transform = transform;

        _model.EventIdle += Idle;
        _model.EventJump += Jump;
        _model.EventLand += Land;
        //_model.EventRoll += Roll;
        //_model.EventAttack += LightAttack;
        _model.EventWalk += Walk;
        _model.EventRun += Run;
        _model.EventTakeDamage += TakeDamage;
    }

    //private void LightAttack(int index)
    //{
    //    _animator.SetTrigger($"LightAttack{index}");
    //}

    public void Idle()
    {
        _animator.SetBool("Idle", true);
    }

    public void Walk(float horizontal, float vertical) //Los pongo pero no los uso
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("isRunning", false);
        //Cappeo los valores a .5 y -.5
        _animator.SetFloat("movX", Mathf.Clamp(Input.GetAxis("Horizontal"),-0.5f,0.5f));
        _animator.SetFloat("movY", Mathf.Clamp(Input.GetAxis("Vertical"), -0.5f, 0.5f));
    }

    public void Run(float horizontal, float vertical) //Los pongo pero no los uso
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("isRunning", true);
        //No es necesario cappear porque llegan a 1 y -1
        _animator.SetFloat("movX", Input.GetAxis("Horizontal"));
        _animator.SetFloat("movY", Input.GetAxis("Vertical"));
    }

    public void Jump()
    {
        _animator.SetBool("hasLanded", false);
        _animator.SetTrigger("Jump");
    }

    public void Land(bool hasLanded)
    {
        _animator.SetBool("hasLanded", hasLanded);
    }

    // la animacion del roll por ahora la sacamos hasta que se apruebe la otra idea
    //public void Roll(float rollY)
    //{
    //    _animator.SetFloat("rollY", rollY);
    //    _animator.SetTrigger("Roll");
    //}

    public void TakeDamage(float dmg, float maxLife)
    {
        Debug.Log("OUCH ESO DOLIO!");
    }
}
