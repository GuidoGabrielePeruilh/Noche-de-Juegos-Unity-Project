using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public void AddScore(string teamName, int scoreToAdd)
    {
        var instanciaClase = DataPersistance.Instance.LoadJson();
        var teamIndex = instanciaClase.FindItemIndex(teamName);
        instanciaClase.TeamsInfo[teamIndex].TeamScore += scoreToAdd;
        instanciaClase.TeamsInfo[teamIndex].TeamScore = Mathf.Max(0, instanciaClase.TeamsInfo[teamIndex].TeamScore);
        DataPersistance.Instance.SaveData(instanciaClase);
    }
}
