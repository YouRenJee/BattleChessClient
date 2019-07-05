using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPosViewBehaviour : MonoBehaviour
{

	public ChessPosLogic PosLogic = null;
    

    public void Init()
    {
        PosLogic = ChessBoardLogicData.Instance.FindPosWithPos(name);
    }
}
