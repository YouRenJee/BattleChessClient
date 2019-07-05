using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetEditorView : Editor
{
    [MenuItem(@"SetEditMap/SetName")]
    public static void SetName()
    {
        GameObject teamGreen = GameObject.Find("TeamGreenPos");
        int cnt = 0;
        foreach (Transform item in teamGreen.transform)
        {
            item.name = "GR" + cnt / 5 + "-" + cnt%5;
            cnt++;
        }
        GameObject teamBlank = GameObject.Find("TeamBlankPos");
        cnt = 0;
        foreach (Transform item in teamBlank.transform)
        {
            item.name = "BA" + cnt / 5 + "-" + cnt%5;
            cnt++;
        }
        GameObject teamBlue = GameObject.Find("TeamBluePos");
        cnt = 0;
        foreach (Transform item in teamBlue.transform)
        {
            item.name = "BL" + cnt / 5 + "-" + cnt%5;
            cnt++;
        }
        GameObject teamYellow = GameObject.Find("TeamYellowPos");
        cnt = 0;
        foreach (Transform item in teamYellow.transform)
        {
            item.name = "YE" + cnt / 5 + "-" + cnt%5;
            cnt++;
        }
        
    }

    [MenuItem(@"SetEditMap/SetPath")]
    public static void SetPath()
    {
        ChessBoardLogicData.Instance.Init();
    }

    [MenuItem(@"SetEditMap/RemovePath")]
    public static void RemovePath()
    {
        ChessBoardLogicData.Instance.ClearPath();
    }

}
