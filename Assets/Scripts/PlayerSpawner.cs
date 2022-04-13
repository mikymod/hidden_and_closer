using UnityEngine;
using UnityEngine.SceneManagement;

namespace HNC
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private SaveSystem saveSystem;

        private void Start()
        {
            if (saveSystem.SaveData.Player.Scene != null &&
                saveSystem.SaveData.Player.Scene != "" &&
                saveSystem.SaveData.Player.Scene == SceneManager.GetActiveScene().name)
            {
                Spawn(saveSystem.SaveData.Player.Position, Quaternion.identity);
            }
            else
            {
                Spawn(transform.position, transform.rotation);
            }
        }

        private void Spawn(Vector3 position, Quaternion rotation)
        {
            Instantiate(playerPrefab, position, rotation);
            SaveSystem.PlayerSave?.Invoke(SceneManager.GetActiveScene(), transform);
            SaveSystem.CompanionSave?.Invoke(true);
            SaveSystem.LevelStarted?.Invoke(SceneManager.GetActiveScene(), transform);
        }
    }
}
