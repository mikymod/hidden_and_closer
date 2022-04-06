using System;
using System.Collections;
using System.Collections.Generic;
using HNC.Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Events;

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

    public static UnityAction<bool, bool> GraphicSettingsSave;
    public static UnityAction<float, float, float> AudioSettingsSave;
    public static UnityAction<Scene, Transform, bool> PlayerSave;

    private void OnEnable()
    {
        PlayerSave += OnPlayerSave;
        GraphicSettingsSave += OnGraphicSettingsSave;
        AudioSettingsSave += OnAudioSettingsSave;
    }

    private void OnDisable()
    {
        PlayerSave -= OnPlayerSave;
        GraphicSettingsSave -= OnGraphicSettingsSave;
        AudioSettingsSave += OnAudioSettingsSave;
    }

    private void OnPlayerSave(Scene scene, Transform player, bool CompanionAvailable)
    {
        saveData.Player = new Player
        {
            Scene = scene.name,
            Position = player.position,
            CompanionAvailable = CompanionAvailable,
        };

        saveData.UpdatedAt = DateTime.Now;

        SaveGameDataToDisk();
    }

    private void OnGraphicSettingsSave(bool fullscreen, bool vsync)
    {
        saveData.Settings.Graphic = new GraphicSettings
        {
            Fullscreen = fullscreen,
            VerticalSync = vsync,
        };

        saveData.UpdatedAt = DateTime.Now;

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

        saveData.UpdatedAt = DateTime.Now;

        SaveGameDataToDisk();
    }

    // API
    public void CreateNewGameFile()
    {
        if (FileManager.FileExists(saveFilename))
        {
            DeleteGameData();
        }

        if (FileManager.WriteToFile(saveFilename, ""))
        {
            saveData = new SaveData();
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
            saveData.FromJson(json);
            return true;
        }

        return false;
    }
}

