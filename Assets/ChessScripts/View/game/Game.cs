using gprotocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject ChessBd;
    public GameObject BtnReady;
    public GameObject GMReady;
    private GameObject tp_Ready;
    private void Start()
    {

        InitChessBoard();

        ModuleManager.Instance.AddListener("OtherEnterRoom", SyncOthers);

        ModuleManager.Instance.AddListener("OtherPlayerReady", OnOtherPlayerReady);

        ModuleManager.Instance.AddListener("GameStart", OnGameStart);
        ModuleManager.Instance.AddListener("CouldMov", OnCouldMov);
        SyncOthers(null);
        CreateMySelf();
    }

    private object OnCouldMov(object[] arg)
    {
        StateManager.Instance.ChangeState(StateManager.Instance.SF);
        return null;
    }

    private void OnDestroy()
    {
        ModuleManager.Instance.RemoveListener("OtherEnterRoom", SyncOthers);

        ModuleManager.Instance.RemoveListener("OtherPlayerReady", OnOtherPlayerReady);

        ModuleManager.Instance.RemoveListener("GameStart", OnGameStart);

        ModuleManager.Instance.RemoveListener("CouldMov", OnCouldMov);
    }

    private object OnOtherPlayerReady(object[] arg)
    {

        Debug.Log(arg[0]);
        return null;
    }

    private object OnGameStart(object[] arg)
    {
        if (tp_Ready != null)
        {
            Destroy(tp_Ready);
        }
        
        StateManager.Instance.ChangeState(StateManager.Instance.SA);
        Debug.Log("game start");
        return null;
    }

    private void InitChessBoard()
    {
        GameObject obj = GameObject.Find("ChessBoard");
        foreach (var item in obj.GetComponentsInChildren<ChessPosViewBehaviour>())
        {
            item.Init();
        }
    }

    private object SyncOthers(object[] arg)
    {
        for (int i = 1; i <= 4; i++)
        {
            if (GameInfo.Instance.SeatId != i)
            {
                switch (i)
                {
                    case 1:
                        ChessGameManager.Instance.ClearTeam(TeamColor.TeamGreen);
                        break;
                    case 2:
                        ChessGameManager.Instance.ClearTeam(TeamColor.TeamBlank);
                        break;
                    case 3:
                        ChessGameManager.Instance.ClearTeam(TeamColor.TeamBlue);
                        break;
                    case 4:
                        ChessGameManager.Instance.ClearTeam(TeamColor.TeamYellow);
                        break;
                    default:
                        break;
                }
            }
        }
        foreach (var item in GameInfo.Instance.other)
        {
            if (item != null)
            {
                switch (item.seat_id)
                {
                    case 1:
                        ChessGameManager.Instance.CreateTeam(new Team(TeamColor.TeamGreen, true));
                        break;
                    case 2:
                        ChessGameManager.Instance.CreateTeam(new Team(TeamColor.TeamBlank, true));
                        break;
                    case 3:
                        ChessGameManager.Instance.CreateTeam(new Team(TeamColor.TeamBlue, true));
                        break;
                    case 4:
                        ChessGameManager.Instance.CreateTeam(new Team(TeamColor.TeamYellow, true));
                        break;
                    default:
                        break;
                }
            }
            
        }
        return null;
    }


    

    private void CreateMySelf()
    {
        Team tm;
        TeamColor cl = TeamColor.TeamGreen;
        float rot = 0f;
        switch (GameInfo.Instance.SeatId)
        {
            case 1:
                cl = TeamColor.TeamGreen;
                break;
            case 2:
                cl = TeamColor.TeamBlank;
                rot = 90f;
                break;
            case 3:
                cl = TeamColor.TeamBlue;
                rot = 180f;
                break;
            case 4:
                cl = TeamColor.TeamYellow;
                rot = -90f;
                break;

        }
        string fileName = PlayerPrefs.GetString("SAVE_BOARD");
        if (fileName != null && fileName != "")
        {
            Team temp = SaveReadFiles<Team>.ReadFile(fileName);
            if (temp != null)
            {
                tm = temp;
                tm.Col = cl;
            }
            else
            {
                tm = new Team(cl, false);
            }

        }
        else
        {
            tm = new Team(cl, false);
        }
        object[] obj = new object[1];
        obj[0] = tm;
        ChessBd.transform.Rotate(new Vector3(0, 0, rot));
        StateManager.Instance.ChangeState(StateManager.Instance.SER, null, obj);
        StateManager.Instance.ChangeState(StateManager.Instance.SE);
    }


    public void OnClickReady()
    {
        tp_Ready =Instantiate(GMReady);
        BtnReady.SetActive(false);
        Team team = ChessGameManager.Instance.GetTeamMyself();
        List<ChessInfo> list = new List<ChessInfo>();
        foreach (var item in team.chess)
        {
            ChessInfo ci = new ChessInfo();
            ci.chess = (int)item.type;
            ci.pos = item.Pos;
            list.Add(ci);
        }
        Ready ready = new Ready(list);
        NetWorkManagement.Instance.SendProtobufCmd((int)Stype.game_server, (int)Cmd.eReady, ready);
        tp_Ready.SetActive(true);
    }

    public void SaveChess()
    {
        SaveReadFiles<Team>.SaveFile("Team", ChessGameManager.Instance.GetTeamMyself());
        PlayerPrefs.SetString("SAVE_BOARD", "Team");
    }

    public void ExitRoom()
    {
        NetWorkManagement.Instance.SendProtobufCmd((int)Stype.game_server, (int)Cmd.eExitRoom, new ExitRoom());
        GameInfo.Instance.other.RemoveAll(data => { return true; });
        SceneManager.LoadScene(0);
    }

}
