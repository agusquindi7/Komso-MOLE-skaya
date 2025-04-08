using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public abstract void Awake(FSM_Attacks fsm);

    public abstract void Execute(FSM_Attacks fsm);

    public abstract void Sleep(FSM_Attacks fsm);
}
