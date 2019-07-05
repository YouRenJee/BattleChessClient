using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBtn : MonoBehaviour {

    public void OnSaveClick()
    {
        

    }

    public void OnReadyClick()
    {
        StateManager.Instance.ChangeState(StateManager.Instance.SF);
    }
}
