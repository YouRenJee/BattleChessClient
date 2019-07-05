using gprotocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChessViewBehaviour : MonoBehaviour
{

    private Chess _chs;
    public AudioClip Selected;
    public AudioClip Confirm;
    public AudioClip Err;

    private SpriteRenderer rd;

    public TeamColor GetColor()
    {
        return _chs.color;
    }



    public string GetPos()
    {

        _chs.Pos = transform.parent.name;
        return _chs.Pos;
    }

    public string GetGlobalPos()
    {
        return transform.parent.GetComponent<ChessPosViewBehaviour>().PosLogic.GlobalPos;
    }


    public ChessType GetChessType()
    {
        return _chs.type;
    }

    public void SetChs(Chess Chs)
    {
        _chs = Chs;
    }

    public void Moveto(string Pos)
    {
        GameObject gm = GameObject.Find(Pos);
        gameObject.transform.position = new Vector3(gm.transform.position.x, gm.transform.position.y, gm.transform.position.z - 0.1f);
        gameObject.transform.SetParent(gm.transform);
        _chs.Pos = Pos;
    }

    private void Start()
    {
        rd = GetComponent<SpriteRenderer>();
    }

    public int OnEditClick()
    {
        StateEdit se = StateManager.Instance.GetState() as StateEdit;

        if (se.PrevSeletedChess == gameObject)
        {
            StopAllCoroutines();
            rd.enabled = true;
            AudioManager.Instance.PlayEffect(Confirm);
            return 0;
        }
        else
        {

            if (se.PrevSeletedChess != null)
            {
                if (ChangePos(se.PrevSeletedChess))
                {
                    se.PrevSeletedChess.GetComponent<ChessViewBehaviour>().StopAllCoroutines();
                    se.PrevSeletedChess.GetComponent<SpriteRenderer>().enabled = true;
                    AudioManager.Instance.PlayEffect(Confirm);
                    return 0;
                }
                else
                {
                    AudioManager.Instance.PlayEffect(Err);
                    return 2;
                }

            }
            else
            {
                StartCoroutine(Flash());
                AudioManager.Instance.PlayEffect(Selected);
                return 1;
            }

        }

    }

    public int OnFightClick(string pos = "")
    {
        StateFight se = StateManager.Instance.GetState() as StateFight;
        if (pos == "")
        { 
            if (GetChessType() == ChessType.DiLei || transform.parent.GetComponent<ChessPosViewBehaviour>().PosLogic.CsType == ChessBoardPosType.BY)
            {
                if (se.PrevSeletedChess != null)
                {
                    se.PrevSeletedChess.GetComponent<ChessViewBehaviour>().StopAllCoroutines();
                    se.PrevSeletedChess.GetComponent<SpriteRenderer>().enabled = true;
                }
                return -1;
            }
            if (se.PrevSeletedChess != null)
            {
                se.PrevSeletedChess.GetComponent<ChessViewBehaviour>().StopAllCoroutines();
                se.PrevSeletedChess.GetComponent<SpriteRenderer>().enabled = true;
            }
            StartCoroutine(Flash());
            AudioManager.Instance.PlayEffect(Selected);
            return 0;
        }
        else  //棋子移动
        {
            ChessPosLogic mData = transform.parent.GetComponent<ChessPosViewBehaviour>().PosLogic;
            ChessPosLogic tarData = ChessBoardLogicData.Instance.FindPosWithPos(pos);
            List<ChessPosLogic> path = FindPath(mData, tarData);
            List<Path> pathString = new List<Path>();
            foreach (var item in path)
            {
                Path pt = new Path();
                pt.pt = item.LocalPos;
                pathString.Add(pt);
            }
            if (path.Count <= 0)
            {
                return 0;
            }
            if (path[path.Count-1] != tarData)
            {
                path.RemoveAll(data => { return true; });
                return 0;
            }
            StopAllCoroutines();
            GetComponent<SpriteRenderer>().enabled = true;
            //object[] obj = new object[1];
            //AnimParameter am = new AnimParameter();
            //am.Path = path;
            //am.Start = gameObject;
            //obj[0] = am;
            //ModuleManager.Instance.Invoke("PlayChessMove", obj);
            //tarData.IsOccupied = true;
            MovResReq req = new MovResReq(pathString);
            req.start = mData.LocalPos;
            req.targetpos = tarData.LocalPos;
            NetWorkManagement.Instance.SendProtobufCmd((int)Stype.game_server, (int)Cmd.eMovResReq, req);
            StateManager.Instance.ChangeState(StateManager.Instance.SA);
            return 1;
        }

    }

    private int[] GetGlobalTB(string Pos)
    {
        string[] pos1 = Pos.Split('-');
        int[] tb = new int[2];
        tb[0] = Convert.ToInt32(pos1[0]);
        tb[1] = Convert.ToInt32(pos1[1]);
        return tb;
    }

    private List<ChessPosLogic> FindPath(ChessPosLogic mData, ChessPosLogic tarData)
    {
        string tarPos = tarData.GlobalPos;
        string mPos = mData.GlobalPos;
        List<ChessPosLogic> path = new List<ChessPosLogic>();
        if (mData.CsType == ChessBoardPosType.RailWay && tarData.CsType == ChessBoardPosType.RailWay)
        {
            if (GetChessType() == ChessType.GongBing)
            {
                path = AStar.AStarEx(mData, tarData);
            }
            else
            {
                int[] myIPos = GetGlobalTB(mPos);
                int[] tarIPos = GetGlobalTB(tarPos);
                if (IsNeedTurn(mData, tarData))//需要转弯
                {
                    FindPathRecWithTurn(mData, tarData, path);
                }
                else if (myIPos[0] != tarIPos[0] && myIPos[1] != tarIPos[1])//不能到达
                {
                }
                else  //一般寻路
                {
                    FindPathRec(mData, tarData, path);
                }
            }

        }
        else
        {
            foreach (var item in mData.GetNodes)
            {
                if (item == tarData)
                {
                    if (tarData.CsType == ChessBoardPosType.XY && tarData.IsOccupied)
                    {
                        return path;
                    }
                    path.Add(item);
                }
            }
        }
        return path;
    }

    private bool IsNeedTurn(ChessPosLogic nowPos, ChessPosLogic tarPos)
    {
        int[] myIPos = GetGlobalTB(nowPos.GlobalPos);
        int[] tarIPos = GetGlobalTB(tarPos.GlobalPos);
        if (myIPos[1] == 6 && (myIPos[0] >= 11 && myIPos[0] <= 15))
        {
            if (tarIPos[0] == 10 && (tarIPos[1] >= 1 && tarIPos[1] <= 5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (myIPos[0] == 10 && (myIPos[1] >= 1 && myIPos[1] <= 5))
        {
            if (tarIPos[1] == 6 && (tarIPos[0] >= 11 && tarIPos[0] <= 15))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (myIPos[0] == 6 && (myIPos[1] >= 1 && myIPos[1] <= 5))
        {
            if (tarIPos[1] == 6 && (tarIPos[0] >= 1 && tarIPos[0] <= 5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (myIPos[1] == 6 && (myIPos[0] >= 1 && myIPos[0] <= 5))
        {
            if (tarIPos[0] == 6 && (tarIPos[1] >= 1 && tarIPos[1] <= 5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (myIPos[0] == 6 && (myIPos[1] >= 11 && myIPos[1] <= 15))
        {
            if (tarIPos[1] == 10 && (tarIPos[0] >= 1 && tarIPos[0] <= 5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (myIPos[1] == 10 && (myIPos[0] >= 1 && myIPos[0] <= 5))
        {
            if (tarIPos[0] == 6 && (tarIPos[1] >= 11 && tarIPos[1] <= 15))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (myIPos[0] == 10 && (myIPos[1] >= 11 && myIPos[1] <= 15))
        {
            if (tarIPos[1] == 10 && (tarIPos[0] >= 11 && tarIPos[0] <= 15))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (myIPos[1] == 10 && (myIPos[0] >= 11 && myIPos[0] <= 15))
        {
            if (tarIPos[0] ==10 && (tarIPos[1] >= 11 && tarIPos[1] <= 15))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    private void FindPathRec(ChessPosLogic mData, ChessPosLogic tarData, List<ChessPosLogic> path)
    {
        int[] myIPos = GetGlobalTB(mData.GlobalPos);
        int[] tarIPos = GetGlobalTB(tarData.GlobalPos);

        if (myIPos[0] == tarIPos[0] && myIPos[1] == tarIPos[1])
        {
            return;
        }
        if (mData.IsOccupied && mData!= transform.parent.GetComponent<ChessPosViewBehaviour>().PosLogic)
        {
            return;
        }

        foreach (var item in mData.GetNodes)
        {
            int[] posInfo = GetGlobalTB(item.GlobalPos);
            if (myIPos[0] < tarIPos[0] && posInfo[1] == myIPos[1] && myIPos[0]<posInfo[0])
            {

                path.Add(item);
                FindPathRec(item, tarData, path);

            }
            else if (myIPos[0] > tarIPos[0] && posInfo[1] == myIPos[1] && myIPos[0] > posInfo[0])
            {

                path.Add(item);
                FindPathRec(item, tarData, path);
            }
            else if (myIPos[1] < tarIPos[1] && posInfo[0] == myIPos[0] && myIPos[1] < posInfo[1])
            {

                path.Add(item);
                FindPathRec(item, tarData, path);

            }
            else if (myIPos[1] > tarIPos[1] && posInfo[0] == myIPos[0] && myIPos[1] > posInfo[1])
            {
                path.Add(item);
                FindPathRec(item, tarData, path);
            }
        }
    }

    private void FindPathRecWithTurn(ChessPosLogic mData, ChessPosLogic tarData, List<ChessPosLogic> path)
    {
        int[] myIPos = GetGlobalTB(mData.GlobalPos);
        int[] tarIPos = GetGlobalTB(tarData.GlobalPos);

        if (myIPos[0] == tarIPos[0] && myIPos[1] == tarIPos[1])
        {
            return;
        }
        ChessPosLogic ps = transform.parent.GetComponent<ChessPosViewBehaviour>().PosLogic;
        if (mData.IsOccupied && mData != ps)
        {
            return;
        }
        if (InTurnPos(mData.GlobalPos) && InNotTurned(myIPos, tarIPos))
        {
            string pos = GetTurnPoint(mData.GlobalPos);
            foreach (var item in mData.GetNodes)
            {
                if (item.GlobalPos == pos)
                {
                    path.Add(item);
                    FindPathRecWithTurn(item, tarData, path);
                }
            }
        }
        else
        {
            foreach (var item in mData.GetNodes)
            {
                int[] posInfo = GetGlobalTB(item.GlobalPos);

                if (myIPos[0] < tarIPos[0] && posInfo[1] == myIPos[1] && myIPos[0] < posInfo[0])
                {

                    path.Add(item);
                    FindPathRecWithTurn(item, tarData, path);

                }
                else if (myIPos[0] > tarIPos[0] && posInfo[1] == myIPos[1] && myIPos[0] > posInfo[0])
                {

                    path.Add(item);
                    FindPathRecWithTurn(item, tarData, path);
                }
                else if (myIPos[1] < tarIPos[1] && posInfo[0] == myIPos[0] && myIPos[1] < posInfo[1])
                {

                    path.Add(item);
                    FindPathRecWithTurn(item, tarData, path);

                }
                else if (myIPos[1] > tarIPos[1] && posInfo[0] == myIPos[0] && myIPos[1] > posInfo[1])
                {
                    path.Add(item);
                    FindPathRecWithTurn(item, tarData, path);
                }
            }
        }
       
    }

    private string GetTurnPoint(string posInfo)
    {
        if (posInfo == "10-5")
        {
            return "11-6";
        }
        else if (posInfo == "11-6")
        {
            return "10-5";
        }
        else if (posInfo == "11-10")
        {
            return "10-11";
        }
        else if (posInfo == "10-11")
        {
            return "11-10";
        }
        else if (posInfo == "6-11")
        {
            return "5-10";
        }
        else if (posInfo == "5-10")
        {
            return "6-11";
        }
        else if (posInfo == "5-6")
        {
            return "6-5";
        }
        else if (posInfo == "6-5")
        {
            return "5-6";
        }
        return null;
    }

    private bool InNotTurned(int[] myIPos, int[] tarIPos)
    {
        if (myIPos[0] != tarIPos[0] && myIPos[1] != tarIPos[1] )
        {
            return true;
        }
        return false;
    }

    private bool InTurnPos(string posInfo)
    {
        if (posInfo == "10-5" || posInfo =="11-6" || posInfo =="11-10" || posInfo =="10-11" || posInfo =="6-11" || posInfo =="5-10" || posInfo =="5-6" || posInfo =="6-5")
        {
            return true;
        }

        return false;
    }







    public bool IsCouldToPos(string pos)
    {
        string[] tp = pos.Split('-');
        int row = Convert.ToInt32(tp[0].Substring(2));
        ChessBoardPosType psTP = ChessBoardLogicData.Instance.GetChessPosType(pos);
        if (GetChessType() == ChessType.JunQi)
        {
            if (psTP != ChessBoardPosType.BY)
            {
                return false;
            }
        }
        else if (GetChessType() == ChessType.ZhaDan)
        {
            if (row == 0)
            {
                return false;
            }
        }
        else if (GetChessType() == ChessType.DiLei)
        {
            if (row < 4)
            {
                return false;
            }
        }
        return true;
    }

    private bool ChangePos(GameObject gm)
    {
        ChessViewBehaviour cs = gm.GetComponent<ChessViewBehaviour>();
        if (IsCouldToPos(cs.GetPos()) && cs.IsCouldToPos(GetPos()))
        {
            string pos = GetPos();
            Moveto(cs.GetPos());
            cs.Moveto(pos);
            return true;
        }

        return false;
    }

    IEnumerator Flash()
    {
        while (true)
        {
            rd.enabled = !rd.enabled;
            yield return new WaitForSeconds(0.3f);
        }

    }
}
