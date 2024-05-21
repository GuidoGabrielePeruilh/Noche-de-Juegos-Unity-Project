using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeamsHandler : MonoBehaviour
{
    [SerializeField] private List<TeamsInfo> teamsInfo = new();

    private void Awake()
    {
        var instanciaClase = DataPersistance.Instance.LoadJson();
        if (instanciaClase.TeamsInfo.Count == 0)
        {
            foreach (var item in teamsInfo)
            {
                instanciaClase.TeamsInfo.Add(item);
            }

            DataPersistance.Instance.SaveData(instanciaClase);
        }


    }

    public TeamsInfo FindTeamInformation(string teamName)
    {
        return teamsInfo.Find(x => teamName == x.TeamName);
    }

    public void AddTeam(string teamName, Color color, Sprite sprite)
    {
        var instanciaClase = DataPersistance.Instance.LoadJson();
        var newTeam = new TeamsInfo(teamName, color, sprite);
        instanciaClase.TeamsInfo.Add(newTeam);
        DataPersistance.Instance.SaveData(instanciaClase);
    }

    public void DeleteTeam(string teamName)
    {
        var instanciaClase = DataPersistance.Instance.LoadJson();
        var indexofTeam = instanciaClase.FindItemIndex(teamName);
        instanciaClase.TeamsInfo.RemoveAt(indexofTeam);
        DataPersistance.Instance.SaveData(instanciaClase);
    }

    public void ModifyTeam(string oldName,string newName, Color newColor)
    {
        var instanciaClase = DataPersistance.Instance.LoadJson();
        var indexofTeam = instanciaClase.FindItemIndex(oldName);
        instanciaClase.TeamsInfo[indexofTeam].TeamName = newName;
        instanciaClase.TeamsInfo[indexofTeam].TeamColor = newColor;
        DataPersistance.Instance.SaveData(instanciaClase);
    }

}
