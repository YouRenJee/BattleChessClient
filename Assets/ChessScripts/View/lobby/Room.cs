using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;
using System;

public class Room : MonoBehaviour {

    public void EnterRoomReq()
    {

        
        EnterRoom req = new EnterRoom();
        string[] str = transform.Find("ID").GetComponent<Text>().text.Split(':');
        req.room_id = Convert.ToInt32(str[1]);
        NetWorkManagement.Instance.SendProtobufCmd((int)Stype.game_server, (int)Cmd.eEnterRoom, req);

    }
}
