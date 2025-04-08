using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAttackState : BaseState
{
    public override void Awake(FSM_Attacks fsm)
    {
        Debug.Log("IDLE");
        fsm.inputCount = 0;
        fsm.anim.SetFloat("attackIndex", 0);
    }

    public override void Execute(FSM_Attacks fsm)
    {
        if (Input.GetMouseButtonDown(0) && fsm.canCombo)
            fsm.SwitchState(fsm.attack1);
    }

    public override void Sleep(FSM_Attacks fsm)
    {
        fsm.canCombo = false;
    }
}
