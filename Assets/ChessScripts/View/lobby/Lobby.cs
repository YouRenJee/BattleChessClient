using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;
using System;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public InputField it;
    public GameObject NameDiaLog;
    public GameObject RoomList;
    public Text UName;
    public InputField RoomName;
    public GameObject RoomObj;

    private void Start()
    {
        ModuleManager.Instance.AddListener("OnConnectSuccess", OnConnectSuccess);
        ModuleManager.Instance.AddListener("OnLoginReturn", OnLoginReturn);
        ModuleManager.Instance.AddListener("SyncRoomList", SyncRoomList);
    }




    private object SyncRoomList(object[] arg)
    {
        Transform RoomList2  = GameObject.Find("RoomList").transform;
        List<RoomItem> list = GameInfo.Instance.RoomListInfo;
        if (list.Count == 0)
        {
            foreach (Transform item in RoomList2)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                Transform RoomPanel = RoomList2.GetChild(i);
                RoomPanel.Find("ID").GetComponent<Text>().text =  "房间ID:"+list[i].id.ToString();
                RoomPanel.Find("Name").GetComponent<Text>().text = "房间名:"+list[i].name.ToString();
                int status = list[i].status;
                if (status == 1)
                {
                    RoomPanel.Find("State").GetComponent<Text>().text = "等待玩家加入";
                }
                else
                {
                    RoomPanel.Find("State").GetComponent<Text>().text = "游戏中";
                }
                
                RoomPanel.Find("Num").GetComponent<Text>().text = "人数:"+list[i].num.ToString()+"/4";
                RoomPanel.gameObject.SetActive(true);
            }
        }
        
        return null;
    }

    private object OnLoginReturn(object[] arg)
    {
        NameDiaLog.SetActive(false);
        RoomList.SetActive(true);
        UName.text = GameInfo.Instance.UserName;
        StartCoroutine(GetRoomInfo());
        return null;
    }

    private IEnumerator GetRoomInfo()
    {
        while (true)
        {
            GetRoomList list = new GetRoomList();
            
            NetWorkManagement.Instance.SendProtobufCmd((int)Stype.game_server, (int)Cmd.eGetRoomList, list);
            yield return new WaitForSeconds(1f);
        }

        
    }

    private object OnConnectSuccess(object[] arg)
    {
        NameDiaLog.SetActive(true);
        return null;
    }






    public void ExitGame()
    {
        Application.Quit();
    }


    public void ShowCrateRoom()
    {
        RoomObj.SetActive(true);
    }

    public void CloseCrateRoomDia()
    {
        RoomObj.SetActive(false);
    }

    public void OnClick()
    {
        LoginReq req = new LoginReq();
        req.u_name = it.text;
        GameInfo.Instance.UserName = it.text;
        NetWorkManagement.Instance.SendProtobufCmd((int)Stype.game_server, (int)Cmd.eLoginReq, req);
        
    }

    public void OnClickCreateRoom()
    {
        CreateRoom ct = new CreateRoom();
        ct.name = RoomName.text;
        NetWorkManagement.Instance.SendProtobufCmd((int)Stype.game_server, (int)Cmd.eCreateRoom, ct);


    }

    private void OnDestroy()
    {
        ModuleManager.Instance.RemoveListener("OnConnectSuccess",OnConnectSuccess);
        ModuleManager.Instance.RemoveListener("OnLoginReturn", OnLoginReturn);
        ModuleManager.Instance.RemoveListener("SyncRoomList", SyncRoomList);
    }

}
