using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataPersistance : Singleton<DataPersistance>
{
    private string _path;
    protected override void Awake()
    {
        base.Awake();
        _path = Application.persistentDataPath + "/ScoreJson.json";

        if (File.Exists(_path))
            LoadJson();
        else
            SaveData(new ValuesToSaveInJson());
       
    }


    public int UpdateTotalScore(ValuesToSaveInJson data)
    {
        var instanciaClase = data;


        if (instanciaClase.TeamsInfo.Count > 0)
        {
            data.TotalScore = 0;
            foreach (var item in instanciaClase.TeamsInfo)
                data.TotalScore += item.TeamScore;
        }

        return data.TotalScore;
    }

    public void SaveData(ValuesToSaveInJson data)
    {
        data.TotalScore = UpdateTotalScore(data);
        WriteJson(data);
    }

    private void WriteJson(ValuesToSaveInJson data)
    {
        string dataToSave = JsonUtility.ToJson(data, true);
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(dataToSave);
        string encodedText = Convert.ToBase64String(bytesToEncode);

        File.WriteAllText(_path, encodedText);
    }

    public ValuesToSaveInJson LoadJson()
    {

        var instanciaClase = new ValuesToSaveInJson();
        if (File.Exists(_path))
        {
            string dataToLoad = File.ReadAllText(_path);
            byte[] bytesToDecode = Convert.FromBase64String(dataToLoad);
            string decodedText = Encoding.UTF8.GetString(bytesToDecode);
            JsonUtility.FromJsonOverwrite(decodedText, instanciaClase);
        }
        return instanciaClase;
    }

    [ContextMenu("DeleteData")]
    public void DeleteData()
    {
        if (File.Exists(_path))
            File.Delete(_path);


    }
}
