using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGF.Base;
using gprotocol;

public class ChessGameManager : UnitySingleton<ChessGameManager>
{
    public override void Awake()
    {
        
    }

    private Team _team = null;


    public Team GetTeamMyself()
    {
        return _team;
    }


    public void CreateTeam(Team team)
    {
        if (team.IsUnknow == false)
        {
            _team = team;
        }
        Transform gm = null;
        string teamID = "Team1";
        GameObject obj = Resources.Load<GameObject>("prefabs/qizi");
        switch (team.Col)
        {
            case TeamColor.TeamGreen:
                gm = GameObject.Find("TeamGreenPos").transform;
                teamID = "Team1";
                break;
            case TeamColor.TeamBlank:
                gm = GameObject.Find("TeamBlankPos").transform;
                teamID = "Team2";
                break;
            case TeamColor.TeamBlue:
                gm = GameObject.Find("TeamBluePos").transform;
                teamID = "Team1";
                break;
            case TeamColor.TeamYellow:
                gm = GameObject.Find("TeamYellowPos").transform;
                teamID = "Team2";
                break;
        }
        for (int i = 0; i < 25; i++)
        {

            GameObject newObj = Instantiate(obj, gm, false);
            newObj.tag = teamID;
            newObj.GetComponent<SpriteRenderer>().sprite = ResManager.Instance.ChessSprite[(int)team.Col][team.chess[i].type];
            Transform GM2 = gm.Find(team.chess[i].Pos);
            GM2.GetComponent<ChessPosViewBehaviour>().PosLogic.IsOccupied = true;
            newObj.transform.position = new Vector3(GM2.position.x, GM2.position.y, GM2.position.z - 0.1f);
            newObj.GetComponent<ChessViewBehaviour>().SetChs(team.chess[i]);
            newObj.transform.SetParent(GM2);
        }
    } 

    public AudioClip Confirm;
    public AudioClip DuiZi;
    public AudioClip ZhuangSi;
    public AudioClip Chi;

    public GameObject Jt;
    private List<GameObject> JtList = new List<GameObject>();

    private void ClearJtList()
    {
        foreach (var item in JtList)
        {
            Destroy(item);
        }
        JtList.RemoveAll(data => { return true; });
    }

    private float VectorAngle(Vector2 from, Vector2 to)
    {
        float angle;

        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector2.Angle(from, to);
        return cross.z > 0 ? angle : -angle;
    }

    IEnumerator MoveTOPosAnime(MovResReturn am)
    {
        ClearJtList();
        List<Path> path = am.path;
        int res = am.res;
        GameObject start = GameObject.Find(am.start).transform.GetChild(0).gameObject;
        ChessBoardLogicData.Instance.FindPosWithPos(am.start).IsOccupied = false;
        for (int i = 0; i < path.Count; i++)
        {
            Transform ts = null;
            AudioManager.Instance.PlayEffect(Confirm);
            ts = GameObject.Find(path[i].pt).transform;
            Vector2 direct = (ts.position - start.transform.position).normalized;
            float ang = VectorAngle(Vector2.up, direct);
            Vector3 pos = new Vector3((start.transform.position.x + ts.position.x) / 2, (start.transform.position.y + ts.position.y) / 2, ts.position.z - 0.1f);
            GameObject obj = Instantiate(Jt);
            obj.transform.position = pos;
            obj.transform.Rotate(new Vector3(0, 0, ang));
            JtList.Add(obj);


            start.transform.position = new Vector3(ts.position.x, ts.position.y, ts.position.z - 0.1f);
            if (i == path.Count - 1)
            {

                if (res == 1)
                {
                    AudioManager.Instance.PlayEffect(Confirm);
                    start.transform.SetParent(ts);
                }
                else if (res == 2)
                {
                    Destroy(GameObject.Find(am.targetpos).transform.GetChild(0).gameObject);
                    AudioManager.Instance.PlayEffect(Chi);
                    start.transform.SetParent(ts);
                }
                else if (res == 3)
                {
                    Destroy(start);
                    AudioManager.Instance.PlayEffect(ZhuangSi);
                }
                else
                {
                    ts.GetComponent<ChessPosViewBehaviour>().PosLogic.IsOccupied = false;
                    Destroy(GameObject.Find(am.targetpos).transform.GetChild(0).gameObject);
                    Destroy(start);
                    AudioManager.Instance.PlayEffect(DuiZi);
                }
                
            }

            yield return new WaitForSeconds(0.1f);
        }

    }

    private object MoveTOPos(object[] obj)
    {
        StartCoroutine("MoveTOPosAnime", obj[0]);
        return null;
    }

    public void Init()
    {
        ModuleManager.Instance.AddListener("CreateTeam", OnCreateTeam);
        ModuleManager.Instance.AddListener("PlayChessMove", MoveTOPos);

    }

    public void ClearTeam(TeamColor cl)
    {
        GameObject cs = GameObject.Find("ChessBoard");
        foreach (var item in cs.GetComponentsInChildren<ChessViewBehaviour>())
        {
            if (item.GetColor() == cl)
            {
                item.gameObject.GetComponentInParent<ChessPosViewBehaviour>().PosLogic.IsOccupied = false;
                Destroy(item.gameObject);
            }
        }
    }

    private object OnCreateTeam(object[] arg)
    {
        CreateTeam((Team)arg[0]);
        return null;
    }
}
