using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : BaseState
{
    public override void Awake(FSM_Attacks fsm)
    {
        Debug.Log("ATTACK 1");
        //Aca los cooldowns son distintos de su maximo por lo que van a empezar a sumarse, cuando comboCounter llegue primero prendo canCombo para pasar al siguiente
        //Por otro lado si attackCounter llega a su maximo entonces vuelvo a IdleState
        fsm.anim.SetTrigger("Attack1");
    }
    
    public override void Execute(FSM_Attacks fsm)
    {
        if (fsm.canCombo && Input.GetMouseButtonDown(0))
        {
            fsm.SwitchState(fsm.attack2);
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
