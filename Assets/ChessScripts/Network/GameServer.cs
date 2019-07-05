using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGF.Base;
using System;
using gprotocol;
using UnityEngine.SceneManagement;

public class GameServer : ServiceModule<GameServer>
{
    public void Init()
    {
        NetWorkManagement.Instance.AddServiceListener((int)Stype.game_server, GameServerReturn);

    }

    private void GameServerReturn(CmdMsg msg)
    {
        switch (msg.cType)
        {
            case (int)Cmd.eLoginRes:
                OnLoginResReturn(msg);
                break;
            case (int)Cmd.eRoomList:
                OnRoomListReturn(msg);
                break;
            case (int)Cmd.eCreateRoomRes:
                OnCreateRoomResReturn(msg);
                break;
            case (int)Cmd.eOtherPlayerEnter:
                OnReciveOtherPlayerEnter(msg);
                break;
            case (int)Cmd.eEnterRoomRes:
                OnCreateRoomResReturn(msg);
                break;
            case (int)Cmd.eOtherPlayerExit:
                OnReciveOtherPlayerExit(msg);
                break;
            case (int)Cmd.eOtherPlayerReady:
                OnReciveOtherPlayerReady(msg);
                break;
            case (int)Cmd.eGameStart:
                OnReciveGameStart(msg);
                break;
            case (int)Cmd.eCouldMov:
                OnReciveCouldMov(msg);
                break;
            case (int)Cmd.eMovResReturn:
                OnReciveMovResReturn(msg);
                break;
            default:
                break;

        }
    }

    private void OnReciveMovResReturn(CmdMsg msg)
    {
        MovResReturn ms = ProtoMan.ProtobufDeserialize<MovResReturn>(msg.body);
        object[] obj = new object[1];
        obj[0] = ms;
        ModuleManager.Instance.Invoke("PlayChessMove", obj);

    }

    private void OnReciveCouldMov(CmdMsg msg)
    {
        ModuleManager.Instance.Invoke("CouldMov", null);
    }

    private void OnReciveGameStart(CmdMsg msg)
    {
        ModuleManager.Instance.Invoke("GameStart", null);
    }

    private void OnReciveOtherPlayerReady(CmdMsg msg)
    {
        OtherPlayerReady ot = ProtoMan.ProtobufDeserialize<OtherPlayerReady>(msg.body); ;

        object[] obj = new object[1];
        obj[0] = ot.seat_id;
        ModuleManager.Instance.Invoke("OtherPlayerReady", obj);
    }

    private void OnReciveOtherPlayerEnter(CmdMsg msg)
    {
        OtherPlayerEnter ot = ProtoMan.ProtobufDeserialize<OtherPlayerEnter>(msg.body);
        GameInfo.Instance.other.Add(ot);
        ModuleManager.Instance.Invoke("OtherEnterRoom", null);
    }

    private void OnReciveOtherPlayerExit(CmdMsg msg)
    {
        OtherPlayerExit ot = ProtoMan.ProtobufDeserialize<OtherPlayerExit>(msg.body);
        for (int i = 0; i < GameInfo.Instance.other.Count; i++)
        {
            if (GameInfo.Instance.other[i] != null)
            {
                if (GameInfo.Instance.other[i].seat_id == ot.seat_id)
                {
                    GameInfo.Instance.other[i] = null;
                }
            }

        }
        ModuleManager.Instance.Invoke("OtherEnterRoom", null);
    }

    private void OnCreateRoomResReturn(CmdMsg msg)
    {
        CreateRoomRes res = ProtoMan.ProtobufDeserialize<CreateRoomRes>(msg.body);

        GameInfo.Instance.RoomId = res.room_id;
        GameInfo.Instance.SeatId = res.seat_id;
        SceneManager.LoadScene(1);
    }

    private void OnRoomListReturn(CmdMsg msg)
    {
        RoomList res = ProtoMan.ProtobufDeserialize<RoomList>(msg.body);
        GameInfo.Instance.RoomListInfo = res.room_list;
        ModuleManager.Instance.Invoke("SyncRoomList", null);
    }

    private void OnLoginResReturn(CmdMsg msg)
    {
        LoginRes res = ProtoMan.ProtobufDeserialize<LoginRes>(msg.body);
        if (res.status == 1)
        {
            ModuleManager.Instance.Invoke("OnLoginReturn", null);
        }
    }

}
