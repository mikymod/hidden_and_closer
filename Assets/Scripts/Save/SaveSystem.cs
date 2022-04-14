using System;
using HNC.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SaveSystem", menuName = "HNC/Save System")]
public class SaveSystem : ScriptableObject
{
    [SerializeField] private string saveFilename = "save.hnc";
    private SaveData saveData = new SaveData();
    public SaveData SaveData { get => saveData; }
    // public bool CanLoadPlayer { get => saveData.Player.Scene != null && saveData.Player.Scene != ""; }

    public static UnityAction<Scene, Transform> PlayerSave;
    public static UnityAction<bool> CompanionSave;
    public static UnityAction<Scene, Transform> LevelStarted;
    public static UnityAction<Scene> LevelFinished;
    public static UnityAction<float, float, float> AudioSettingsSave;
    public static UnityAction<bool, bool> GraphicSettingsSave;

    private void OnEnable()
    {
        PlayerSave += OnPlayerSave;
        CompanionSave += OnCompanionSave;
        LevelStarted += OnLevelStarted;
        LevelFinished += OnLevelFinished;
        GraphicSettingsSave += OnGraphicSettingsSave;
        AudioSettingsSave += OnAudioSettingsSave;
    }

    private void OnDisable()
    {
        PlayerSave -= OnPlayerSave;
        CompanionSave -= OnCompanionSave;
        LevelStarted -= OnLevelStarted;
        LevelFinished -= OnLevelFinished;
        GraphicSettingsSave -= OnGraphicSettingsSave;
        AudioSettingsSave -= OnAudioSettingsSave;
    }

    private void OnPlayerSave(Scene scene, Transform player)
    {
        saveData.Player = new Player
        {
            Scene = scene.name,
            Position = player.position + Vector3.forward * 2,
        };

        SaveGameDataToDisk();
    }

    private void OnCompanionSave(bool companionAvailable) {
        saveData.Player.CompanionAvailable = companionAvailable;

        SaveGameDataToDisk();
    }

    private void OnLevelStarted(Scene scene, Transform spawnPoint)
    {
        if (saveData.Levels.ContainsKey(scene.name))
        {
            saveData.Levels[scene.name].Name = scene.name;
        }
        else
        {
            saveData.Levels.Add(scene.name, new Level
            {
                Name = scene.name,
                Completed = false,
            });
        }

        SaveGameDataToDisk();
    }

    private void OnLevelFinished(Scene scene)
    {
        if (!saveData.Levels.ContainsKey(scene.name))
        {
            throw new Exception("Level not found");
        }

        saveData.Levels[scene.name].Completed = true;

        SaveGameDataToDisk();
    }

    private void OnGraphicSettingsSave(bool fullscreen, bool vsync)
    {
        saveData.Settings.Graphic = new GraphicSettings
        {
            Fullscreen = fullscreen,
            VerticalSync = vsync,
        };

        SaveGameDataToDisk();
    }

    private void OnAudioSettingsSave(float master, float music, float sfx)
    {
        saveData.Settings.Audio = new HNC.Save.AudioSettings
        {
            Master = master,
            Music = music,
            SFX = sfx,
        };

        SaveGameDataToDisk();
    }

    // API
    public void CreateNewGameFile()
    {
        Settings previousSettings = new Settings();
        if (FileManager.FileExists(saveFilename))
        {
            previousSettings = saveData.Settings;
            DeleteGameData();
        }

        if (FileManager.WriteToFile(saveFilename, ""))
        {
            saveData = new SaveData();
            saveData.Settings = previousSettings;
            saveData.CreatedAt = DateTime.Now;
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

        saveData.UpdatedAt = DateTime.Now;
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
            saveData = SaveData.FromJson(json);
            return true;
        }

        return false;
    }
}

