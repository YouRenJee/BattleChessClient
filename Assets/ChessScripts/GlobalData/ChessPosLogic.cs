using System.Collections;
using System.Collections.Generic;
using System;

public enum ChessBoardPosType
{
    Nomal,
    RailWay,
    XY,
    BY,
    InvalidPos
}

public class ChessPosLogic
{
    public ChessBoardPosType CsType;  //保存位置的类型

    private List<ChessPosLogic> nodes; //保存相邻的顶点

    public List<ChessPosLogic> GetNodes
    {
        get
        {
            return nodes;
        }
    }

    private bool _isOccupied;  //该位置上是否有棋子
    public bool IsOccupied { get { return _isOccupied; } set { _isOccupied = value; } }

    private string _localPos; //该位置的坐标
    public string LocalPos { get { return _localPos; } set { _localPos = value; } }

    private string _globalPos;
    public string GlobalPos { get { return _globalPos; } set { _globalPos = value; } }

    public ChessPosLogic(string localPos = "", string globalPos ="", bool isOccupied = false, ChessBoardPosType csType = ChessBoardPosType.Nomal)
    {
        nodes = new List<ChessPosLogic>();
        _localPos = localPos;
        _globalPos = globalPos;
        _isOccupied = isOccupied;
    }
    public ChessPosLogic ParentNode = null;
    public void AddNodes(ChessPosLogic pos)
    {
        nodes.Add(pos);
    }
}
