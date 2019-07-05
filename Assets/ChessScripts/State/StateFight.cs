using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFight : BaseState
{
    public GameObject PrevSeletedChess = null;
    public GameObject PrevEnemy = null;
    public GameObject Bz = null;
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
                GameObject gmObj = hit.collider.gameObject;
                ChessViewBehaviour tp = hit.collider.gameObject.GetComponent<ChessViewBehaviour>();
                if (tp != null) // 点到有棋子的位置
                {
                    if (tp.GetChessType() != ChessType.Unknown) // 自己的棋子
                    {

                        if (tp.OnFightClick() == 0)//没有点到地雷或本营里的棋
                        {
                            PrevSeletedChess = tp.gameObject;
                        }
                        else if (tp.OnFightClick() == -1)
                        {
                            PrevSeletedChess = null;
                        }

                    }
                    else if (PrevSeletedChess != null) //别人的棋子
                    {
                        if (gmObj.tag == PrevSeletedChess.tag)
                        {

                        }
                        else if(gmObj.transform.parent.GetComponent<ChessPosViewBehaviour>().PosLogic.CsType == ChessBoardPosType.XY)
                        {

                        }
                        else
                        {
                            if (PrevSeletedChess.GetComponent<ChessViewBehaviour>().OnFightClick(tp.GetGlobalPos()) == 1)
                            {
                                PrevSeletedChess = null;
                            }
                        }

                    }
                    else
                    {
                        Bz.SetActive(true);
                        Bz.transform.position = new Vector3(gmObj.transform.position.x, gmObj.transform.position.y, gmObj.transform.position.z-0.3f);
                        PrevEnemy = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.tag == "Pos")//点到没有棋子的位置
                {
                    if (PrevSeletedChess != null)
                    {
                        if (PrevSeletedChess.GetComponent<ChessViewBehaviour>().OnFightClick(hit.collider.gameObject.GetComponent<ChessPosViewBehaviour>().PosLogic.GlobalPos) == 1)
                        {
                            PrevSeletedChess = null;
                        }
                    }
                }
                else
                {
                    hit.collider.GetComponent<Mark>().OnClick(PrevEnemy);
                    Bz.SetActive(false);
                    PrevEnemy = null;
                }

            }
        }
    }

    private void ShowBZ()
    {
        throw new NotImplementedException();
    }
}

