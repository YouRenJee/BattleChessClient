using UnityEngine;

public class AppMain :UnitySingleton<AppMain>
{
    // Use this for initialization
    void Start()
    {
        ModuleManager.Instance.AddListener("Log", Log);
        ModuleManager.Instance.AddListener("ConnectSuccess", ConnectSuccess);
        GameServer.Instance.Init();
        NetWorkManagement.Instance.Init();
        StateManager.Instance.Init();
        ChessBoardLogicData.Instance.Init();
        ResManager.Instance.Init();
        ChessGameManager.Instance.Init();
        //StartCoroutine(st());
    }


    private object DisConnect(params object[] msg)
    {
        return null;
    }

    private object ConnectSuccess(params object[] str)
    {
        if (str != null)
        {
            Debug.Log(str[0]);
        }
        ModuleManager.Instance.Invoke("OnConnectSuccess", null);
        return null;
    }

    private object Log(params object[] str)
    {
        if (str != null)
        {
            Debug.Log(str[0]);
        }
        return null;
    }



    
}
