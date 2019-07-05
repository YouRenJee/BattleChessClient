using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGF.Base;
using gprotocol;

public class GameInfo :  UnitySingleton<GameInfo>
{
    public string UserName;
    public List<RoomItem> RoomListInfo = new List<RoomItem>();
    internal int RoomId = -1;
    internal int SeatId = -1;
    internal List<OtherPlayerEnter> other = new List<OtherPlayerEnter>();
}
