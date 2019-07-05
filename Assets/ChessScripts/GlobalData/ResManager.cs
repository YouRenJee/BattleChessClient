using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGF.Base;

public class ResManager : ServiceModule<ResManager>
{
    public List<Dictionary<ChessType, Sprite>> ChessSprite = new List<Dictionary<ChessType, Sprite>>();

    public void Init()
    {
        Sprite[] All = Resources.LoadAll<Sprite>("img/qizi");
        for (int i = 0; i < 4; i++)
        {
            ChessSprite.Add(new Dictionary<ChessType, Sprite>());
            ChessSprite[i].Add(ChessType.Unknown, All[0+13*i]);
            ChessSprite[i].Add(ChessType.JunQi, All[1 + 13 * i]);
            ChessSprite[i].Add(ChessType.DiLei, All[2 + 13 * i]);
            ChessSprite[i].Add(ChessType.ZhaDan, All[3 + 13 * i]);
            ChessSprite[i].Add(ChessType.GongBing, All[4 + 13 * i]);
            ChessSprite[i].Add(ChessType.PaiZhang, All[5 + 13 * i]);
            ChessSprite[i].Add(ChessType.LianZhang, All[6 + 13 * i]);
            ChessSprite[i].Add(ChessType.YingZhang, All[7 + 13 * i]);
            ChessSprite[i].Add(ChessType.TuanZhang, All[8 + 13 * i]);
            ChessSprite[i].Add(ChessType.LvZhang, All[9 + 13 * i]);
            ChessSprite[i].Add(ChessType.ShiZhang, All[10 + 13 * i]);
            ChessSprite[i].Add(ChessType.JunZhang, All[11 + 13 * i]);
            ChessSprite[i].Add(ChessType.SiLing, All[12 + 13 * i]);
        }
    }
}
