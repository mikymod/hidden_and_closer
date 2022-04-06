using System;
using System.Collections;
using System.Collections.Generic;
using HNC.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[CreateAssetMenu(fileName = "SaveSystem", menuName = "HNC/Save System")]
public class SaveSystem : ScriptableObject
{
    [SerializeField] private string saveFilename = "save.hnc";
    private SaveData saveData = new SaveData();

    // Events
    // 
    // Level Started Event
    // Level Completed Event
    // Checkpoint Reached Event
    // Companion Used Event
    // Settings Changed Event

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelStarted;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelStarted;
    }

    private void LevelStarted(Scene scene, LoadSceneMode sceneMode)
    {
        var level = saveData.Levels.FirstOrDefault((level) => scene.name == level.Name);
        if (level != null)
        {
            // stub
        }
        else
        {
            var lvl = new Level { Name = scene.name, Completed = false };
            saveData.Levels.Add(lvl);
            SaveGameDataToDisk();
        }
    }

    // API
    public void CreateNewGameFile()
    {
        if (FileManager.FileExists(saveFilename))
        {
            // Delete
        }

        if (FileManager.WriteToFile(saveFilename, ""))
        {
            saveData = new SaveData();
        }
    }

    public bool DeleteGameData()
    {
        if (FileManager.MoveFile(saveFilename, $"deleted{saveFilename}"))
        {
            return true;
        }

        return false;
    }

    public bool SaveGameDataToDisk()
    {
        if (!FileManager.FileExists(saveFilename))
        {
            // FIXME: this should be called inside main menu
            CreateNewGameFile();

            // // Currently disable 
            // return false;
        }

        var json = saveData.ToJson();
        if (FileManager.WriteToFile(saveFilename, json))
        {
            return true;
        }

        return false;
    }

    public bool LoadGameDataFromDisk()
    {
        if (!FileManager.FileExists(saveFilename))
        {
            return false;
        }

        if (FileManager.LoadFromFile(saveFilename, out var json))
        {
            saveData.FromJson(json);
            return true;
        }

        return false;
    }
}

