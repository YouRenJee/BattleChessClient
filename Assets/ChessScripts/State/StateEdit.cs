using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEdit : BaseState
{
    public GameObject PrevSeletedChess = null;

    public override void OnEnter(object[] obj = null)
    {
        PrevSeletedChess = null;
    }

    public override void OnLeave(object[] obj = null)
    {
        PrevSeletedChess = null;
    }

    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                ChessViewBehaviour tp = hit.collider.gameObject.GetComponent<ChessViewBehaviour>();
                if (tp != null)
                {
                    
                    if (tp.GetChessType() == ChessType.Unknown)
                    {
                        return;
                    }
                    int res = tp.OnEditClick();

                    if (res == 0)
                    {
                        PrevSeletedChess = null;
                    }
                    else if (res == 1)
                    {
                        PrevSeletedChess = tp.gameObject;
                    }
                    else if (res == 2)
                    {
                        
                    }
                }
                else
                {
                    Debug.Log(hit.collider.name);
                }
            }
        }
    }

}
