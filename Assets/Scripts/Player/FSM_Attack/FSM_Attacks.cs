using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Attacks : MonoBehaviour
{
    BaseState _currentState;

    public IdleAttackState idleState = new IdleAttackState();
    public Attack1State attack1 = new Attack1State();
    public Attack2State attack2 = new Attack2State();
    public Attack3State attack3 = new Attack3State();
    [Header("References")]
    public Animator anim;
    [Header("public Values")]
    //Hago 2 contadores, uno para seguir el combo y otro para reiniciar (PONEMOS DE EJEMPLO 1SEG PARA VOLVER A ATACAR Y 2SEG PARA REINICIAR TODO EL ATAQUE)
    public bool canCombo = true;
    public bool hasStopped = true;
    public int inputCount;
    private void Awake()
    {
        //PauseManager.instance.Subscribe(ArtificialUpdate, false);

        if (anim == null) anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentState = idleState;

        _currentState.Awake(this);
    }
    #region CanCombo & HasStopped
    public void CanComboTrue()
    {
        canCombo = true;
    }

    public void CanComboFalse()
    {
        canCombo = false;
    }

    public void StoppedAttackingTrue()
    {
        hasStopped = true;
    }
    public void StoppedAttackingFalse()
    {
        hasStopped = false;
    }
    #endregion
    public void Update()
    {
        _currentState.Execute(this);
    }

    public void SwitchState(BaseState state)
    {
        _currentState.Sleep(this);
        _currentState = state;
        _currentState.Awake(this);
    }

    //private void OnDestroy()
    //{
    //    PauseManager.instance.Unsubscribe(ArtificialUpdate, false);
    //}
}
