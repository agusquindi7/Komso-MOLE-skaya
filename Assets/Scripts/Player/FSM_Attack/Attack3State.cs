using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3State : BaseState
{
    public override void Awake(FSM_Attacks fsm)
    {
        Debug.Log("ATTACK 3");

        fsm.anim.SetTrigger("Attack3");
    }

    public override void Execute(FSM_Attacks fsm)
    {
        Debug.Log("Termine de atacar");

        if (fsm.hasStopped)
        {
            fsm.SwitchState(fsm.idleState);
        }
    }

    public override void Sleep(FSM_Attacks fsm)
    {
        fsm.canCombo = true;
        fsm.hasStopped = true;
    }
}
