using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SGF.Base;
using UnityEngine.Assertions;


public class ChessBoardLogicData : ServiceModule<ChessBoardLogicData>
{
    public List<ChessPosLogic> TeamGreen = new List<ChessPosLogic>();
    public List<ChessPosLogic> TeamBlank = new List<ChessPosLogic>();
    public List<ChessPosLogic> TeamBlue = new List<ChessPosLogic>();
    public List<ChessPosLogic> TeamYellow = new List<ChessPosLogic>();
    public List<ChessPosLogic> Mid = new List<ChessPosLogic>();
    public List<List<ChessPosLogic>> GlobalChessBorad = new List<List<ChessPosLogic>>();

    public void Init()
    {
        SetPosInfo();

        SetPosTypeInfo();

        SetPosPath();
    }

    public void ClearPath()
    {
        TeamGreen = new List<ChessPosLogic>();
        TeamBlank = new List<ChessPosLogic>();
        TeamBlue = new List<ChessPosLogic>();
        TeamYellow = new List<ChessPosLogic>();
        Mid = new List<ChessPosLogic>();
        GlobalChessBorad = new List<List<ChessPosLogic>>();
    }

    public ChessPosLogic FindPosWithPos(string Pos)
    {
        string[] str = Pos.Split('-');
        if (str.Length!=2)
        {
            return null;
        }
        bool IsLocal = false;
        List<ChessPosLogic> Tep = null;
        if (str[0].Length > 2)
        {
            string tp = str[0].Substring(0, 2);
            if (tp == "GR")
            {
                Tep = TeamGreen;
                IsLocal = true;
            }
            else if (tp == "BA")
            {
                Tep = TeamBlank;
                IsLocal = true;
            }
            else if (tp == "BL")
            {
                Tep = TeamBlue;
                IsLocal = true;
            }
            else if (tp == "YE")
            {
                Tep = TeamYellow;
                IsLocal = true;
            }
            else if (tp == "MD")
            {
                Tep = Mid;
                IsLocal = true;
            }
            else
            {
                int row = Convert.ToInt32(str[0]);
                int cow = Convert.ToInt32(str[1]);

                if (row >= 11)
                {
                    Tep = TeamGreen;
                }
                else if (row <= 5)
                {
                    Tep = TeamBlue;
                }
                else if (cow <= 5)
                {
                    Tep = TeamBlank;
                }
                else if (cow >= 11)
                {
                    Tep = TeamYellow;
                }
                else
                {
                    Tep = Mid;
                }
                IsLocal = false;
            }
        }
        else
        {
            int row = Convert.ToInt32(str[0]);
            int cow = Convert.ToInt32(str[1]);


           
            IsLocal = false;
            if (row >= 11)
            {
                Tep = TeamGreen;
            }
            else if (row <= 5)
            {
                Tep = TeamBlue;
            }
            else if (cow <= 5)
            {
                Tep = TeamBlank;
            }
            else if (cow >= 11)
            {
                Tep = TeamYellow;
            }
            else
            {
                Tep = Mid;
            }
        }

        if (IsLocal)
        {
            for (int i = 0; i < Tep.Count; i++)
            {
                if (Tep[i].LocalPos == Pos)
                {
                    return Tep[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < Tep.Count; i++)
            {
                if (Tep[i].GlobalPos == Pos)
                {
                    return Tep[i];
                }
            }
        }

        return null;
    }

    public ChessBoardPosType GetChessPosType(string Pos)
    {
        return FindPosWithPos(Pos).CsType;
    }

    private void SetPosType(ChessBoardPosType Type, string Pos)
    {
        ChessPosLogic tp = FindPosWithPos(Pos);

        tp.CsType = Type;

    }

    private void SetPosInfo()
    {
        for (int i = 0; i < 30; i++)
        {
            int row = i / 5;
            int cow = i % 5;
            TeamGreen.Add(new ChessPosLogic("GR" + row + "-" + cow, (row + 11) + "-" + (cow+6)));
            TeamBlank.Add(new ChessPosLogic("BA" + row + "-" + cow, (6 + cow) + "-" + (5 - row)));
            TeamBlue.Add(new ChessPosLogic("BL" + row + "-" + cow, (5 - row) + "-" + (10 - cow)));
            TeamYellow.Add(new ChessPosLogic("YE" + row + "-" + cow, (10 - cow) + "-" + (11 + row)));

        }
        int cnt = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                {
                    Mid.Add(new ChessPosLogic("MD" + (cnt/3) + "-" + (cnt%3), (6 + i) + "-" + (6 + j), false, ChessBoardPosType.RailWay));
                    cnt++;
                }

            }

        }

        for (int i = 0; i < 17; i++)
        {
            GlobalChessBorad.Add(new List<ChessPosLogic>());
            for (int j = 0; j < 17; j++)
            {
                ChessPosLogic tep = FindPosWithPos(i + "-" + j);
                if (tep != null)
                {
                    GlobalChessBorad[i].Add(tep);
                }
                else
                {
                    GlobalChessBorad[i].Add(null);
                }

            }
        }
    }

    private void SetPosTypeInfo()
    {
        string[] str = { "GR", "BA", "BL", "YE", "MD" };
        for (int i = 0; i < 4; i++)
        {
            SetPosType(ChessBoardPosType.XY, str[i] + 1 + "-" + 1);
            SetPosType(ChessBoardPosType.XY, str[i] + 1 + "-" + 3);
            SetPosType(ChessBoardPosType.XY, str[i] + 2 + "-" + 2);
            SetPosType(ChessBoardPosType.XY, str[i] + 3 + "-" + 1);
            SetPosType(ChessBoardPosType.XY, str[i] + 3 + "-" + 3);
        }
        for (int i = 0; i < 4; i++)
        {
            SetPosType(ChessBoardPosType.BY, str[i] + 5 + "-" + 1);
            SetPosType(ChessBoardPosType.BY, str[i] + 5 + "-" + 3);
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (j == 0 || j == 5)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        SetPosType(ChessBoardPosType.RailWay, str[i] + 0 + "-" + k);
                        SetPosType(ChessBoardPosType.RailWay, str[i] + 4 + "-" + k);
                    }
                }
                else
                {
                    SetPosType(ChessBoardPosType.RailWay, str[i] + j + "-" + 0);
                    SetPosType(ChessBoardPosType.RailWay, str[i] + j + "-" + 4);
                }

            }
            
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                SetPosType(ChessBoardPosType.RailWay, str[4] + i + "-" + j);
            }
        }
    }

    private void SetPosPath()
    {
        for (int i = 0; i < GlobalChessBorad.Count; i++)
        {
            for (int j = 0; j < GlobalChessBorad[i].Count; j++)
            {
                if (GlobalChessBorad[i][j] != null)
                {
                    string[] pos = GlobalChessBorad[i][j].GlobalPos.Split('-');
                    int tp = Convert.ToInt32(pos[0]);
                    int wb = Convert.ToInt32(pos[1]);
                    FindDir(GlobalChessBorad[i][j], tp, wb);
                }

            }
        }

        for (int i = 0; i < 9; i++)
        {
            string[] pos = Mid[i].LocalPos.Split('-');
            int tp = Convert.ToInt32(pos[0].Substring(2));
            int wb = Convert.ToInt32(pos[1]);
            FindDirInMid(Mid[i], tp, wb);
        }
        GlobalChessBorad[11][6].AddNodes(GlobalChessBorad[10][5]);
        GlobalChessBorad[10][5].AddNodes(GlobalChessBorad[11][6]);
        GlobalChessBorad[6][5].AddNodes(GlobalChessBorad[5][6]);
        GlobalChessBorad[5][6].AddNodes(GlobalChessBorad[6][5]);
        GlobalChessBorad[5][10].AddNodes(GlobalChessBorad[6][11]);
        GlobalChessBorad[6][11].AddNodes(GlobalChessBorad[5][10]);
        GlobalChessBorad[10][11].AddNodes(GlobalChessBorad[11][10]);
        GlobalChessBorad[11][10].AddNodes(GlobalChessBorad[10][11]);
    }

    private void FindDir(ChessPosLogic ogl, int tp, int wb)
    {
        if (ogl.CsType == ChessBoardPosType.XY)
        {
            ChessPosLogic tep = FindPosWithPos((tp - 1) + "-" + (wb - 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp - 1) + "-" + wb);
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp - 1) + "-" + (wb + 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos(tp + "-" + (wb + 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp + 1) + "-" + (wb + 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp + 1) + "-" + wb);
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp + 1) + "-" + (wb - 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos(tp + "-" + (wb - 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }

        }
        else
        {
            ChessPosLogic tep = FindPosWithPos((tp - 1) + "-" + (wb - 1));
            if (tep != null)
            {
                if (tep.CsType == ChessBoardPosType.XY)
                {
                    ogl.AddNodes(tep);
                }  
            }
            tep = FindPosWithPos((tp - 1) + "-" + wb);
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp - 1) + "-" + (wb + 1));
            if (tep != null)
            {
                if (tep.CsType == ChessBoardPosType.XY)
                {
                    ogl.AddNodes(tep);
                }
            }
            tep = FindPosWithPos(tp + "-" + (wb + 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp + 1) + "-" + (wb + 1));
            if (tep != null)
            {
                if (tep.CsType == ChessBoardPosType.XY)
                {
                    ogl.AddNodes(tep);
                }
            }
            tep = FindPosWithPos((tp + 1) + "-" + wb);
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
            tep = FindPosWithPos((tp + 1) + "-" + (wb - 1));
            if (tep != null)
            {
                if (tep.CsType == ChessBoardPosType.XY)
                {
                    ogl.AddNodes(tep);
                }
            }
            tep = FindPosWithPos(tp + "-" + (wb - 1));
            if (tep != null)
            {
                ogl.AddNodes(tep);
            }
        }
        
    }

    private void FindDirInMid(ChessPosLogic ogl, int tp, int wb)
    {
        ChessPosLogic tep = FindPosWithPos("MD" + (tp - 1) + "-" + wb);
        if (tep != null)
        {
            ogl.AddNodes(tep);
        }
        tep = FindPosWithPos("MD" +  tp + "-" + (wb + 1));
        if (tep != null)
        {
            ogl.AddNodes(tep);
        }
        tep = FindPosWithPos("MD" +  (tp + 1) + "-" + wb);
        if (tep != null)
        {
            ogl.AddNodes(tep);
        }
        tep = FindPosWithPos("MD" + tp + "-" + (wb - 1));
        if (tep != null)
        {
            ogl.AddNodes(tep);
        }
    }

    


}
