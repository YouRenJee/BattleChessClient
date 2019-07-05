using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum ChessType
{
    JunQi = 31,
    GongBing = 32,
    PaiZhang = 33,
    LianZhang = 34,
    YingZhang = 35,
    TuanZhang = 36,
    LvZhang = 37,
    ShiZhang = 38,
    JunZhang = 39,
    SiLing = 40,
    DiLei = 41,
    ZhaDan = 42,
    Unknown
}
[Serializable]
public enum TeamColor
{
    TeamGreen,
    TeamBlank,
    TeamBlue,
    TeamYellow
}

[Serializable]
public class Chess
{
    public ChessType type;
    public TeamColor color;
    public string Pos;
}

