using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class Team
{

    public Chess[] chess = new Chess[25];
    public TeamColor Col
    {
        get { return _col;}
        set
        {
            _col = value;
            for (int i = 0; i < 25; i++)
            {
                
                chess[i].color = value;
            }
        }
    }
    private TeamColor _col;
    public bool IsUnknow;
    public Team(TeamColor col, bool isUnknow)
    {
        IsUnknow = isUnknow;
        Dictionary<int, string> TeamPrefix = new Dictionary<int, string>();
        TeamPrefix.Add(0, "GR");
        TeamPrefix.Add(1, "BA");
        TeamPrefix.Add(2, "BL");
        TeamPrefix.Add(3, "YE");
        _col = col;
        for (int i = 0; i < 25; i++)
        {
            chess[i] = new Chess();
            chess[i].color = col;
        }
        if (IsUnknow)
        {
            for (int i = 0; i < 25; i++)
            {
                chess[i].type = ChessType.Unknown;
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                chess[0 + i].type = ChessType.GongBing;
                chess[3 + i].type = ChessType.PaiZhang;
                chess[6 + i].type = ChessType.LianZhang;
                chess[19 + i].type = ChessType.DiLei;
            }
            for (int i = 0; i < 2; i++)
            {
                chess[9 + i].type = ChessType.YingZhang;
                chess[11 + i].type = ChessType.TuanZhang;
                chess[13 + i].type = ChessType.LvZhang;
                chess[15 + i].type = ChessType.ShiZhang;
                chess[17 + i].type = ChessType.ZhaDan;
            }
            chess[23].type = ChessType.JunQi;
            chess[22].type = ChessType.JunZhang;
            chess[24].type = ChessType.SiLing;
        }
        string tm = TeamPrefix[(int)col];
        for (int i = 0; i < 5; i++)
        {
            chess[i].Pos = tm+ 0 + "-" + i;
        }
        for (int i = 0; i < 3; i++)
        {
            chess[5+i].Pos = tm+ 1 + "-" + 2 * i; 
        }
        chess[8].Pos = tm + 2 + "-" + 0;
        chess[9].Pos = tm + 2 + "-" + 1;
        chess[10].Pos = tm + 2 + "-" + 3;
        chess[11].Pos = tm + 2 + "-" + 4;

        for (int i = 0; i < 3; i++)
        {
            chess[12+i].Pos = tm + 3 + "-" + 2 * i;
        }
        for (int i = 0; i < 5; i++)
        {
            chess[15+i].Pos = tm + 4 + "-" + i;
        }
        for (int i = 0; i < 5; i++)
        {
            chess[20+i].Pos = tm + 5 + "-" + i;
        }

    }
}
