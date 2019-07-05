using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StateManager : UnitySingleton<StateManager>
{
    private BaseState _nowState = null;
    public StateEdit SE;
    public StateFight SF;
    public StateEnterRoom SER;
    public StateAwait SA;


    public void Init()
    {
        SE = GetComponent<StateEdit>();
        SF = GetComponent<StateFight>();
        SER = GetComponent<StateEnterRoom>();
        SA = GetComponent<StateAwait>();
        _nowState = SER;
    }

    public BaseState GetState()
    {
        return _nowState;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_nowState != null)
        {
            _nowState.OnUpdate();
        }
    }

    public void ChangeState(BaseState bs,object[] leav = null , object[] ent = null)
    {

        _nowState.OnLeave(leav);
        _nowState = bs;
        _nowState.OnEnter(ent);
    }
}
