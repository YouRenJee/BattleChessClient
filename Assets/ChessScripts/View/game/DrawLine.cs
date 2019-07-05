using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    void OnDrawGizmos()
    {
        
        List<List<ChessPosLogic>> GlobalChessBorad = ChessBoardLogicData.Instance.GlobalChessBorad;
        for (int i = 0; i < GlobalChessBorad.Count; i++)
        {
            for (int j = 0; j < GlobalChessBorad[i].Count; j++)
            {
                if (GlobalChessBorad[i][j] != null)
                {
                    GameObject nowPos = GameObject.Find(GlobalChessBorad[i][j].LocalPos);
                    if (GlobalChessBorad[i][j].CsType == ChessBoardPosType.XY)
                    {
                        Gizmos.color = new Color(1, 0, 0, 1);
                        Gizmos.DrawSphere(nowPos.transform.position, 0.1f);
                    }
                    else if (GlobalChessBorad[i][j].CsType == ChessBoardPosType.BY)
                    {
                        Gizmos.color = new Color(0, 1, 0, 1);
                        Gizmos.DrawCube(nowPos.transform.position, new Vector3(0.1f, 0.1f));
                    }
                    else if (GlobalChessBorad[i][j].CsType == ChessBoardPosType.RailWay)    
                    {
                        Gizmos.color = new Color(0, 0, 1, 1);
                        Gizmos.DrawCube(nowPos.transform.position, new Vector3(0.1f, 0.1f));
                    }
                    if (nowPos != null)
                    {
                        Gizmos.color = new Color(1, 1, 0, 1);
                        for (int k = 0; k < GlobalChessBorad[i][j].GetNodes.Count; k++)
                        {
                            GameObject node = GameObject.Find(GlobalChessBorad[i][j].GetNodes[k].LocalPos).gameObject;
                            if (node != null)
                            {
                                Gizmos.DrawLine(nowPos.transform.position, node.transform.position);
                            }
                        }
                    }
                }
                
            }
        }
    }
}
