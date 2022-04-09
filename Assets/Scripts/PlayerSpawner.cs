using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform defaultSpawnPoint;
        [SerializeField] private SaveSystem saveSystem;

        private void Start()
        {
            // Should be called at the start of the game
            saveSystem.LoadGameDataFromDisk();

            if (saveSystem.CanLoadPlayer)
            {
                Instantiate(playerPrefab, saveSystem.SaveData.Player.Position, Quaternion.identity);
            }
            else
            {
                Instantiate(playerPrefab, defaultSpawnPoint.position, defaultSpawnPoint.rotation);
            }
        }
    }
}
