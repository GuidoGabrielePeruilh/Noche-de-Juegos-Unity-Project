using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ValuesToSaveInJson
{
    public int TotalScore;
    public List<TeamsInfo> TeamsInfo;

    public ValuesToSaveInJson()
    {
        TotalScore = 0;
        TeamsInfo = new List<TeamsInfo>();
    }

    public int FindItemIndex(string teamName)
    {
        return TeamsInfo.FindIndex(itemAux => itemAux.TeamName == teamName);
    }
}

[Serializable]
public class TeamsInfo
{
    public string TeamName;
    public int TeamScore;
    public Color TeamColor = Color.grey;
    public Sprite teamSprite;

    public TeamsInfo(string teamName, Color color, Sprite sprite)
    {
        TeamName = teamName;
        TeamScore = 0;
        TeamColor = color;
        teamSprite = sprite;
    }
}
