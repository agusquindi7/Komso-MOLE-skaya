using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2State : BaseState
{
    public override void Awake(FSM_Attacks fsm)
    {
        Debug.Log("ATTACK 2");

        fsm.anim.SetTrigger("Attack2");
    }

    public override void Execute(FSM_Attacks fsm)
    {
        if (fsm.canCombo && Input.GetMouseButtonDown(0))
        {
            fsm.SwitchState(fsm.attack3);
        }
        else if (fsm.hasStopped)
        {
            fsm.SwitchState(fsm.idleState);
        }
    }

    public override void Sleep(FSM_Attacks fsm)
    {
        fsm.canCombo = false;
    }
}
