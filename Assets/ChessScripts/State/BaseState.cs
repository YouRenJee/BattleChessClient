using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGF.Base;

public  class BaseState : MonoBehaviour
{
    public virtual void OnEnter(object[] obj = null) { }
    public virtual void OnUpdate() { }
    public virtual void OnLeave(object[] obj = null) { }
}
