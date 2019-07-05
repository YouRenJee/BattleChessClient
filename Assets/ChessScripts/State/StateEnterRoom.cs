using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnterRoom : BaseState
{
    public override void OnEnter(object[] obj = null)
    {
        ModuleManager.Instance.Invoke("CreateTeam", obj);
    }

    public override void OnLeave(object[] obj = null)
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
