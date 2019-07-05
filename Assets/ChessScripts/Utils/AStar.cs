using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class AStar
{
    private static List<ChessPosLogic> openList;
    private static List<ChessPosLogic> clostList;

    private static int[] ConvertPos(string[] pos)
    {
        int[] xy = new int[2];
        xy[0] = Convert.ToInt32(pos[0]);
        xy[1] = Convert.ToInt32(pos[1]);
        return xy;
    }
    private static int HeuristicsFoo(ChessPosLogic start, ChessPosLogic end)
    {
        int[] posStart = ConvertPos(start.GlobalPos.Split('-'));
        int[] posEnd = ConvertPos(end.GlobalPos.Split('-'));
        return Math.Abs(posStart[0] - posEnd[0]) + Math.Abs(posStart[1] - posEnd[1]);

    }

    public static List<ChessPosLogic> AStarEx(ChessPosLogic start, ChessPosLogic end)
    {
        openList = new List<ChessPosLogic>();
        clostList = new List<ChessPosLogic>();
        List<ChessPosLogic> res = new List<ChessPosLogic>();
        openList.Add(start);

        while (openList.Count > 0 && !openList.Contains(end))
        {
            ChessPosLogic temp = null;
            int H = 100000;
            foreach (var item in openList)
            {
                    int dis = HeuristicsFoo(item, end);
                    if (H > dis)
                    {
                        H = dis;
                        temp = item;
                    }
            }
            openList.Remove(temp);
            clostList.Add(temp);

            foreach (var item in temp.GetNodes)
            {
                if ((!clostList.Contains(item) && item.CsType == ChessBoardPosType.RailWay && item.IsOccupied == false)|| item.GlobalPos == end.GlobalPos)
                {
                    if (openList.Contains(item))
                    {

                    }
                    else
                    {
                        openList.Add(item);
                        item.ParentNode = temp;
                    }
                }
            }
        }
        
        if (openList.Count > 0)
        {
            
            ChessPosLogic tp = end;
            while (tp != start)
            {

                res.Add(tp);
                tp = tp.ParentNode;
            } 
            res.Reverse();
            
        }
        return res;

    }

}



